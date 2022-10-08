using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Framework.Singleton;

public class NetMessage : MonoBehaviourSingleton<NetMessage>
{
    /// <summary>
    /// 消息协议
    /// </summary>
    private ProtobufMessage m_msg = null;

    /// <summary>
    /// 是否连接
    /// </summary>
    public bool connected
    {
        get
        {
            return null != m_msg && m_msg.connected;
        }
    }

    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="callback"></param>
    public void Connect(string host, int port, Action<bool> callback)
    {
        DisConnect();
        m_msg = new ProtobufMessage();
        m_msg.ConnectAsync(host, port, callback);
    }


    /// <summary>
    /// 断开连接
    /// </summary>
    public void DisConnect()
    {
        if (null != m_msg)
        {
            m_msg.Disconnect();
            m_msg = null;
        }
    }

    /// <summary>
    /// 注册一个消息
    /// </summary>
    /// <param name="msgCode"></param>
    /// <param name="callback"></param>
    private void Register(int msgCode, Action<int, string> callback)
    {
        if (null != m_msg)
        {
            m_msg.Register<IMessage>(msgCode, (msgId, msg)=> {
                callback?.Invoke(msgId, msg.data);
            });
        }
    }

    /// <summary>
    /// 监听接收消息
    /// </summary>
    /// <param name="callback"></param>
    public void ListenRecieve(Action<int, string> callback)
    {
        if (null != m_msg)
        {
            m_msg.ListenRecieve(callback);
        }
    }

    /// <summary>
    /// 注销所有消息
    /// </summary>
    private void UnRegisterAll()
    {
        m_msg?.UnRegisterAll();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msgCode"></param>
    /// <param name="data"></param>
    public void Send(int msgCode, string data)
    {
        if (!string.IsNullOrWhiteSpace(data) && null != m_msg)
        {
            IMessage msg = new IMessage();
            msg.data = data;
            m_msg.Send<IMessage>(msgCode, msg);
        }
    }

    /// <summary>
    /// 设置异常事件
    /// </summary>
    /// <param name="action"></param>
    public void SetExceptionAction(Action action)
    {
        if (null != m_msg)
        {
            m_msg.SetExceptionAction(action);
        }
    }

    public void Update()
    {
        if (connected)
        {
            m_msg.Receive();
        }
    }

    void OnDestroy()
    {
        DisConnect();
    }
}
