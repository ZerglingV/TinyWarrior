using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionPanel : MonoBehaviour
{
        public PanelManager panelManager;
        public GameObject clientProperty;
        public InputField serverIP;
        public InputField serverPort;
        public InputField inputName;

        GameSocketClient gameSocketClient;
        SucceedingCanvas succeedingCanvas;
        bool isConnecting;
        Thread connectThread;
        Button connectionButton;
        Text buttonText;

        void Start()
        {
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
                connectionButton = transform.Find("ConnectButton").GetComponent<Button>();
                buttonText = connectionButton.GetComponentInChildren<Text>();
                succeedingCanvas = panelManager.succeedingCanvas.GetComponent<SucceedingCanvas>();
        }

        void OnGUI()
        {
                if (!isConnecting)
                {
                        buttonText.text = "连入服务器";
                        connectionButton.interactable = true;
                }
                if (gameSocketClient.IsSocketConnected())
                {
                        clientProperty.GetComponent<PlayerProperty>().playerName = inputName.text;
                        panelManager.ShowRoomsPanel();
                }
                else
                {
                        panelManager.HideRoomEnteredPanel();
                        panelManager.HideRoomSettingPanel();
                        panelManager.HideRoomsPanel();
                        gameSocketClient.roomsInfo.Clear();
                }
        }

        public void ClickToConnect()
        {
                if (serverIP.text == "")
                {
                        succeedingCanvas.ShowTipsPanel("请输入服务器地址！");
                        return;
                }
                if (serverPort.text == "")
                {
                        succeedingCanvas.ShowTipsPanel("请输入服务器端口！");
                        return;
                }
                if (inputName.text == "")
                {
                        succeedingCanvas.ShowTipsPanel("请输入角色名！");
                        return;
                }
                buttonText.text = "正在连接...";
                connectionButton.interactable = false;
                isConnecting = true;

                // use thread to connect to server, for forbidding UI block
                connectThread = new Thread(ConnectToServer);
                connectThread.Start();

        }

        public void ConfirmExitGame()
        {
                //UnityEditor.EditorApplication.isPlaying = false; // work in progress
                Application.Quit();
        }

        // Connect to the server via socket
        void ConnectToServer()
        {
                if (gameSocketClient.ConnectServer(serverIP.text, serverPort.text))
                {
                        // set a UI to show connect successfully
                        succeedingCanvas.ShowTipsPanel("连接服务器成功！");
                        gameSocketClient.SendMsgToServer("name=" + inputName.text);
                }
                else
                {
                        // set a UI to show connect unsuccessfully
                        succeedingCanvas.ShowTipsPanel("无法连接至服务器！");
                }
                isConnecting = false;
        }
}
