using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System;
using TinyWarriorInfo;

public class GameSocketClient : MonoBehaviour
{
        public PanelManager panelManager;
        public Socket clientSocket;
        public List<RoomInfo> roomsInfo;
        public RoomInfo roomEnteredInfo;
        public string startMessage;
        public string playerIndex;

        Thread receiveThread;
        private static byte[] result = new byte[2048]; // decide how many rooms can be received

        #region -- Public Function --

        public bool ConnectServer(string ip, string port)
        {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
                        clientSocket.Connect(ipEndPoint);
                }
                catch
                {
                        return false;
                }
                receiveThread = new Thread(ReceiveMsgFromServer);
                receiveThread.Start(clientSocket);
                return true;
        }

        // judge socket connection status
        public bool IsSocketConnected()
        {
                try
                {
                        if (clientSocket.RemoteEndPoint != null && (!clientSocket.Poll(3000, SelectMode.SelectRead) || clientSocket.Available > 0))
                        {
                                return true;
                        }
                        return false;
                }
                catch
                {
                        return false;
                }
        }

        // send message to server
        public void SendMsgToServer(string message)
        {
                if (!IsSocketConnected(clientSocket))
                {
                        return;
                }
                try
                {
                        ByteBuffer buffer = new ByteBuffer(message);
                        clientSocket.Send(buffer.ToBytes());
                }
                catch
                {
                        clientSocket.Close();
                }
        }

        public void SendObjectToServer(object obj)
        {
                ByteBuffer buffer = new ByteBuffer();
                byte[] result = buffer.ToBytes(obj);
                clientSocket.Send(result);
        }


        #endregion

        #region -- Private Static Function --

        private bool IsSocketConnected(Socket clientSocket)
        {
                try
                {
                        if (clientSocket.RemoteEndPoint != null && (!clientSocket.Poll(3000, SelectMode.SelectRead) || clientSocket.Available > 0))
                        {
                                return true;
                        }
                        return false;
                }
                catch
                {
                        return false;
                }
        }

        // recieve message from server
        private void ReceiveMsgFromServer(object clientSocket)
        {
                Socket mServerSocket = (Socket)clientSocket;

                while (true)
                {
                        try
                        {
                                mServerSocket.Receive(result); // receive message to result
                                object obj = new ByteBuffer().GetObject(result);
                                if (obj != null)
                                {
                                        try
                                        {
                                                roomEnteredInfo = (RoomInfo)obj;
                                                // Here shows panel and set get data and differs from getting List<roomInfo> that show rooms panel by connect button
                                                if (!panelManager.IsShowingRoomEnteredPanelState())
                                                {
                                                        panelManager.ShowRoomEnteredPanel();
                                                }
                                                else
                                                {
                                                        panelManager.RefreshRoomEnteredPanel();
                                                }
                                        }
                                        catch
                                        {
                                                try
                                                {
                                                        roomsInfo = (List<RoomInfo>)obj;
                                                        panelManager.RefreshRoomsPanel();
                                                }
                                                catch (Exception ex)
                                                {
                                                        panelManager.ShowConnectionPanel("获取对象有误. " + ex.Message);
                                                }
                                        }
                                        continue;
                                }
                                else
                                {
                                        string message = new ByteBuffer(result).GetString().Trim().ToLower(); // analysis data content to string from result
                                        MessageHandle(message);
                                }
                        }
                        catch
                        {
                                panelManager.ShowConnectionPanel("已断开与服务器的连接！");
                                mServerSocket.Close();
                                break;
                        }
                }
        }

        private void MessageHandle(string message)
        {
                if (message.StartsWith("start="))
                {
                        panelManager.StartGame();
                        startMessage = message.Remove(0, 7);
                        return;
                }
                if (message.StartsWith("index="))
                {
                        playerIndex = message.Remove(0, 6);
                        return;
                }
                switch (message)
                {
                        case "ownerleft":
                                panelManager.ShowConnectionPanel("房主离开了当前房间！");
                                panelManager.HideRoomEnteredPanel();
                                SendMsgToServer("getrooms");
                                break;
                        default:
                                panelManager.ShowConnectionPanel("服务器消息：" + message);
                                break;
                }
        }

        #endregion
}