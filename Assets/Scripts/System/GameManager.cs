using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
        public GameObject pausePanel;
        public GameObject playerPrefab;
        public GameObject playersCollection;
        public GameObject[] playerSpawn;
        public CinemachineVirtualCamera virtualCamera;

        GameSocketClient gameSocketClient;
        GameObject clientProperty;
        PlayerController playerController;
        GameObject[] players;
        Text playerNameText;
        GameObject youTitle;
        bool showPausePanel;

        void Awake()
        {
                clientProperty = GameObject.Find("ClientProperty");
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
                playerNameText = playerPrefab.transform.Find("Canvas/NameText").GetComponent<Text>();
                youTitle = playerPrefab.transform.Find("Canvas/YouTitle").gameObject;
                playerController = playerPrefab.transform.Find("Player").GetComponent<PlayerController>();
        }

        void Start()
        {
                string playerIndex = gameSocketClient.playerIndex;
                string[] spawnMessage = gameSocketClient.startMessage.Split(';');
                players = new GameObject[10]; // max players' number is 10
                for (int i = 0; i < spawnMessage.Length; i++)
                {
                        print(spawnMessage[i]);
                        playerNameText.text = spawnMessage[i].Split('=')[0];// player's name
                        playerPrefab.transform.position = playerSpawn[int.Parse(spawnMessage[i].Split('=')[1])].transform.position; // player's position
                        if (i.ToString() != playerIndex) // if player who is got from message is not current client player
                        {
                                youTitle.SetActive(false);
                                playerController.enabled = false; // it should not be controlled by current client's keyboard
                                players[i] = Instantiate(playerPrefab);
                                players[i].transform.SetParent(playersCollection.transform); // set parent to forbid UI layer higher than Canvas
                        }
                        else
                        {
                                youTitle.SetActive(true);
                                playerController.enabled = true;
                                GameObject currentPlayer = Instantiate(playerPrefab);  // initialize player
                                currentPlayer.transform.SetParent(playersCollection.transform); // set parent to forbid UI layer higher than Canvas
                                virtualCamera.Follow = currentPlayer.transform.Find("Player").transform; // camera follow player
                        }
                }
        }

        void Update()
        {
                if (!gameSocketClient.IsSocketConnected())
                {
                        BackToMenuScene();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                        showPausePanel = !showPausePanel;
                        if (showPausePanel)
                        {
                                pausePanel.SetActive(true);
                        }
                        else
                        {
                                pausePanel.SetActive(false);
                        }
                }
        }

        public void HidePausePanel()
        {
                showPausePanel = false;
                pausePanel.SetActive(false);
        }

        public void BackToMenuScene()
        {
                if (gameSocketClient.clientSocket != null)
                {
                        gameSocketClient.SendMsgToServer("exit");
                        gameSocketClient.clientSocket.Close();
                }
                gameSocketClient.clientSocket = null;
                Destroy(clientProperty.gameObject);
                SceneManager.LoadScene(0);
        }
}