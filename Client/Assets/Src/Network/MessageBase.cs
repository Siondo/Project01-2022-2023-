using System;
using System.Net;
using System.Net.Sockets;

public abstract class MessageBase
{
    #region Variable
    public const int SOCKET_SEND_BUFFER_LEN = 20480;
    public const int SOCKET_RECV_BUFFER_LEN = 40960;
    /// <summary>
    /// Socket
    /// </summary>
    private Socket m_socket = null;

    /// <summary>
    /// IP
    /// </summary>
    private string m_ip = string.Empty;

    /// <summary>
    /// 端口
    /// </summary>
    private int m_port = 65535;

    /// <summary>
    /// 目标连接点
    /// </summary>
    private EndPoint m_endPoint = null;

    /// <summary>
    /// 接收消息参数
    /// </summary>
    private SocketAsyncEventArgs m_receiveArgs = null;

    /// <summary>
    /// 接收的字节
    /// </summary>
    protected byte[] m_receiveByte = new byte[SOCKET_RECV_BUFFER_LEN];

    /// <summary>
    /// 字节缓存
    /// </summary>
    protected byte[] m_byteTemp = new byte[SOCKET_RECV_BUFFER_LEN];

    /// <summary>
    /// 当前接收到的消息总长度
    /// </summary>
    protected int m_receiveLength = 0;

    /// <summary>
    /// 上锁
    /// </summary>
    protected object m_lock = new object();

    /// <summary>
    /// 异常事件
    /// </summary>
    protected Action m_exceptionAction = null;
    #endregion

    #region Property
    /// <summary>
    /// Socket是否连接
    /// </summary>
    public bool connected
    {
        get
        {
            return m_socket != null && m_socket.Connected;
        }
    }

    #endregion

    #region Function
    /// <summary>
    /// 设置异常事件
    /// </summary>
    /// <param name="action"></param>
    public void SetExceptionAction(Action action)
    {
        m_exceptionAction = action;
    }

    /// <summary>
    /// 异步连接
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="connectResult"></param>
    public void ConnectAsync(string ip, int port, Action<bool> connectResult)
    {
        try
        {
            // 关闭原有的连接
            this.Disconnect();
            // 记录IP和端口
            this.m_ip = ip;
            this.m_port = port;
            m_endPoint = new IPEndPoint(IPAddress.Parse(this.m_ip), this.m_port);
            // 创建Socket
            IPAddress[] iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            if (iPAddresses.Length > 0)
            {
                UnityEngine.Debug.Log("IPAddress: " + iPAddresses[0].ToString());
            }
            if (iPAddresses.Length > 0 && iPAddresses[0].ToString().Split(':').Length == 5)
            {
                m_socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            m_socket.SendBufferSize = SOCKET_SEND_BUFFER_LEN;
            m_socket.ReceiveBufferSize = SOCKET_RECV_BUFFER_LEN;
            // 连接Socket
            using (SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs())
            {
                connectArgs.RemoteEndPoint = m_endPoint;
                connectArgs.UserToken = m_socket;
                connectArgs.Completed += (sender, args) => {
                    bool bResult = args.SocketError == SocketError.Success && connected;
                    if (connectResult != null)
                    {
                        connectResult(bResult);
                    }
                };
                this.m_socket.ConnectAsync(connectArgs);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
            connectResult?.Invoke(false);
        }
    }

    /// <summary>
    /// 异步接收消息
    /// </summary>
    public void ReceiveAsync(Action<byte[], int> receiveResult = null)
    {
        try
        {
            if (this.connected && this.m_socket.Available > 0)
            {
                while (this.m_socket.Available > 0)
                {
                    int len = this.m_socket.Receive(m_receiveByte, m_receiveLength, this.m_socket.Available, SocketFlags.None);
                    m_receiveLength += len;
                    //UnityEngine.Debug.Log("BytesTransferred: " + len + ", m_receiveLength: " + m_receiveLength);
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
            m_exceptionAction?.Invoke();
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void Send(byte[] msg)
    {
        try
        {
            if (this.connected)
            {
                this.m_socket.Send(msg);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
            m_exceptionAction?.Invoke();
        }
    }

    /// <summary>
    /// 关闭Socket
    /// </summary>
    public void Close()
    {
        if (m_receiveArgs != null)
        {
            m_receiveArgs.Dispose();
            m_receiveArgs = null;
        }
        if (m_socket != null)
        {
            if (this.connected)
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Disconnect(true);
            }
            m_socket.Close();
            m_socket = null;
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        if (m_socket != null)
        {
            UnityEngine.Debug.Log("Disconnect");
        }
        this.Close();
    }
    #endregion
}
