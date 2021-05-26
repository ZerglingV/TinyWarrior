using UnityEngine;
using UnityEngine.UI;

public class SucceedingCanvas : MonoBehaviour
{
        public GameObject tipsPanel;
        public Text pingText;

        GameObject clientProperty;
        string ip;
        GameSocketClient gameSocketClient;
        Ping ping;
        int delayTime = -1;
        bool canPing = true;

        bool showTipsPanel;
        string tips;
        Text tipsText;
        Color redColor;
        Color yellowColor;
        Color greenColor;

        // Start is called before the first frame update
        void Awake()
        {
                clientProperty = GameObject.Find("ClientProperty");
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();

                tipsText = tipsPanel.transform.Find("TipsBoard").GetComponentInChildren<Text>();
                redColor = new Color(0.7254902f, 0.01568629f, 0.0864033f);
                yellowColor = new Color(0.990566f, 0.8719044f, 0.1806248f);
                greenColor = new Color(0.2276166f, 0.8773585f, 0.2276166f);
        }

        void OnGUI()
        {
                if (canPing)
                {
                        canPing = false;
                        Invoke(nameof(ShowPing), 1.0f); // ping per second
                }
                // ping text
                if (delayTime == -1)
                {
                        pingText.color = Color.white;
                        pingText.text = "Ping: (N/A)ms";
                }
                else if (delayTime <= 1)
                {
                        pingText.color = greenColor;
                        pingText.text = "Ping: ≤1ms";
                }
                else
                {
                        if (delayTime >= 200)
                        {
                                pingText.color = redColor;
                        }
                        else if (delayTime >= 100)
                        {
                                pingText.color = yellowColor;
                        }
                        else
                        {
                                pingText.color = greenColor;
                        }
                        pingText.text = "Ping: " + delayTime + "ms";
                }
                // tips panel
                if (showTipsPanel)
                {
                        tipsText.text = tips;
                        tipsPanel.SetActive(true);
                }
                else
                {
                        tipsPanel.SetActive(false);
                }
        }

        void ShowPing()
        {
                if (!gameSocketClient.IsSocketConnected())
                {
                        delayTime = -1;
                }
                else
                {
                        if (ping == null)
                        {
                                ip = gameSocketClient.clientSocket.RemoteEndPoint.ToString().Split(':')[0];
                        }
                        else if (ping.isDone)
                        {
                                delayTime = ping.time;
                        }
                        ping = new Ping(ip);
                }
                canPing = true;
        }

        #region -- Tips Panel Function --

        public void ShowTipsPanel(string tips)
        {
                this.tips = tips;
                showTipsPanel = true;
        }

        public void HideTipsPanel()
        {
                showTipsPanel = false;
        }

        #endregion
}
