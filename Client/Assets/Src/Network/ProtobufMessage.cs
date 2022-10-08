using UnityEngine;
using System;
using System.Collections.Generic;
//using Google.Protobuf;

public class IMessage
{
    private string m_data = string.Empty;
    public string data
    {
        get
        {
            return m_data;
        }
        set
        {
            m_data = value;
        }
    }

    public void MergeFrom(byte[] bytes)
    {
        m_data = System.Text.Encoding.UTF8.GetString(bytes);
    }

    public byte[] ToByteArray()
    {
        return System.Text.Encoding.UTF8.GetBytes(m_data);
    }
}

public class ProtobufMessage : MessageBase
{
    abstract class Msg
    {
        public abstract void Execute(int msgCode, byte[] bytes);
    }
    sealed class Msg<T> : Msg where T : IMessage, new()
    {
        #region Variable
        /// <summary>
        /// 代理事件
        /// </summary>
        public event Action<int, T> callback = null;
        #endregion
        
        #region Function
        /// <summary>
        /// 执行消息
        /// </summary>
        /// <param name="bytes">真实消息字节</param>
        public override void Execute(int msgCode, byte[] bytes)
        {
            T t = new T();
            t.MergeFrom(bytes);
            if (callback != null)
            {
                callback(msgCode, t);
            }
        }
        #endregion
    }
    #region Variable
    /// <summary>
    /// 私钥
    /// </summary>
    private byte[] m_privateKey = { 0x68, 0x86 };

    /// <summary>
    /// 公钥(随机生成)
    /// </summary>
    private byte[] m_publicKey = { 0x68, 0x86 };
    
    /// <summary>
    /// 消息表
    /// </summary>
    private Dictionary<int, Msg> m_msg = null;

    /// <summary>
    /// 接收到消息
    /// </summary>
    private Action<int, string> m_receive = null;
    #endregion

    #region Function
    /// <summary>
    /// 构造
    /// </summary>
    public ProtobufMessage()
        : base()
    {
        m_msg = new Dictionary<int, Msg>();
    }

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msgCode"></param>
    /// <param name="callback"></param>
    public void Register<T>(int msgCode, Action<int, T> callback) where T : IMessage, new()
    {
        if (m_msg.ContainsKey(msgCode))
        {
            Msg<T> msg = (Msg<T>)(m_msg[msgCode]);
            msg.callback += callback;
        }
        else
        {
            Msg<T> msg = new Msg<T>();
            msg.callback += callback;
            m_msg.Add(msgCode, msg);
        }
    }

    /// <summary>
    /// 监听接收消息
    /// </summary>
    /// <param name="callback"></param>
    public void ListenRecieve(Action<int, string> callback)
    {
        m_receive = callback;
    }

    /// <summary>
    /// 注销消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msgCode"></param>
    /// <param name="callback"></param>
    public void UnRegister<T>(int msgCode, Action<int, T> callback) where T : IMessage, new()
    {
        if (m_msg.ContainsKey(msgCode))
        {
            Msg<T> msg = (Msg<T>)(m_msg[msgCode]);
            msg.callback -= callback;
        }
    }

    /// <summary>
    /// 注册所有监听
    /// </summary>
    public void UnRegisterAll()
    {
        m_msg.Clear();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msgCode"></param>
    /// <param name="msg"></param>
    public void Send<T>(int msgCode, T msg) where T : IMessage, new()
    {
        // 真实消息
        byte[] msgBytes = msg.ToByteArray();
        // 消息包字节大小
        short len = (short)(6 + msgBytes.Length);
        byte[] lenBytes = BitConverter.GetBytes(len);
        // 消息Code大小
        byte[] codeBytes = BitConverter.GetBytes((short)msgCode);
        // 生成公钥，并根据公钥、私钥加密Code和消息
        short publicKey = (short)(UnityEngine.Random.Range(short.MinValue, short.MaxValue));
        m_publicKey = BitConverter.GetBytes(publicKey);
        codeBytes[0] ^= m_publicKey[0];
        codeBytes[1] ^= m_publicKey[1];
        for (int i = 0; i < msgBytes.Length; ++i)
        {
            msgBytes[i] ^= m_publicKey[0];
            msgBytes[i] ^= m_publicKey[1];
        }
        m_publicKey[0] ^= m_privateKey[0];
        m_publicKey[1] ^= m_privateKey[1];

        // 组装数据
        byte[] bytes = new byte[len];
        Array.Copy(lenBytes, 0, bytes, 0, lenBytes.Length);
        Array.Copy(codeBytes, 0, bytes, 2, codeBytes.Length);
        Array.Copy(m_publicKey, 0, bytes, 4, m_publicKey.Length);
        Array.Copy(msgBytes, 0, bytes, 6, msgBytes.Length);
        // 发送消息
        this.Send(bytes);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="bytes"></param>
    public void Receive()
    {
        this.ReceiveAsync();
        lock (m_lock)
        {
            if (m_receiveLength >= 6)
            {
                // 消息总长度
                short len = BitConverter.ToInt16(m_receiveByte, 0);
                if (m_receiveLength >= len)
                {
                    // 消息Code
                    byte[] codeBytes = new byte[2];
                    Array.Copy(m_receiveByte, 2, codeBytes, 0, codeBytes.Length);
                    // 真实消息
                    byte[] msgBytes = new byte[len - 6];
                    Array.Copy(m_receiveByte, 6, msgBytes, 0, msgBytes.Length);
                    // 公钥
                    Array.Copy(m_receiveByte, 4, m_publicKey, 0, m_publicKey.Length);
                    m_publicKey[0] ^= m_privateKey[0];
                    m_publicKey[1] ^= m_privateKey[1];
                    for (int i = 0; i < msgBytes.Length; ++i)
                    {
                        msgBytes[i] ^= m_publicKey[0];
                        msgBytes[i] ^= m_publicKey[1];
                    }
                    codeBytes[0] ^= m_publicKey[0];
                    codeBytes[1] ^= m_publicKey[1];
                    // 处理缓存用于下一次使用
                    m_receiveLength -= len;
                    //UnityEngine.Debug.Log("len: " + len + ", m_receiveLength: " + m_receiveLength);

                    if (m_receiveLength > 0)
                    {
                        Array.Copy(m_receiveByte, len, m_byteTemp, 0, m_receiveLength);
                        Array.Copy(m_byteTemp, 0, m_receiveByte, 0, m_receiveLength);
                    }

                    // 准备处理消息
                    int msgCode = BitConverter.ToInt16(codeBytes, 0);

                    // 在监听层处理消息
                    try
                    {
                        if (null != m_receive)
                        {
                            IMessage t = new IMessage();
                            t.MergeFrom(msgBytes);
                            m_receive(msgCode, t.data);
                        }
                        else
                        {
                            if (m_msg.ContainsKey(msgCode))
                            {
                                m_msg[msgCode].Execute(msgCode, msgBytes);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
    }
    #endregion
}
