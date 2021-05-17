using UnityEngine;
using UnityEngine.UI;

public class PingText : MonoBehaviour
{
        public GameObject clientProperty;
        public InputField serverIP;
        public Text pingText;


        string ip;
        GameSocketClient gameSocketClient;
        Ping ping;
        int delayTime;
        bool isInitialized;

        // Start is called before the first frame update
        void Awake()
        {
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
        }

        private void OnGUI()
        {
                if (gameSocketClient.IsSocketConnected() && !isInitialized)
                {
                        isInitialized = true;
                        ip = serverIP.text;
                        ShowPing();
                }
                else if (!gameSocketClient.IsSocketConnected())
                {
                        isInitialized = false;
                        pingText.text = "Ping: (N/A)ms";
                }
                if (isInitialized && ping != null && ping.isDone)
                {
                        delayTime = ping.time;
                        if (delayTime <= 1)
                        {
                                pingText.text = "Ping: ≤1ms";
                        }
                        else
                        {
                                pingText.text = "Ping: " + delayTime + "ms";
                        }
                        ping.DestroyPing();
                        ping = null;
                        Invoke(nameof(ShowPing), 2.0f); // "ping" server per 2 seconds
                }
        }

        void ShowPing()
        {
                ping = new Ping(ip);
        }
}
