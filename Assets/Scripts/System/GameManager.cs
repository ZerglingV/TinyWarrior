using Cinemachine;
using System.Threading;
using TinyWarriorInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
        public GameObject ghost;
        public GameObject pausePanel;
        public GameObject victoryPanel;
        public GameObject battleSituationPanel;
        public GameObject playerPrefab;
        public GameObject playersCollection;
        public GameObject[] playerSpawn;
        public CinemachineVirtualCamera virtualCamera;
        public AudioSource[] audioCollection;
        public Timer sendTimer;
        public int sendPeriod = 20;

        GameObject clientProperty;
        GameSocketClient gameSocketClient;

        Dropdown resolutionDropdown;
        Toggle fullScreenToggle;
        AudioSource musicAudio;
        Slider musicSlider;
        Slider soundSlider;

        GameObject yourTitle;
        GameObject yourPlayer;
        Animator yourAnimator;
        PlayerAction yourAction;
        string yourIndex;

        Text battleSituationText;
        bool battleMessageUpdated;
        string deadIndex;
        string leaverIndex;

        Text playerNameText;
        PlayerProperty playerProperty;
        PlayerController playerController;
        GameObject[] players;
        PlayerAction playerAction;

        bool showPausePanel;
        bool showVictoryPanel;
        Text winnerText;
        string winnerIndex;

        void Awake()
        {
                // add events listener to tipsPanel's confirmButton, back to menu scene when disconnection
                GameObject.Find("SucceedingCanvas/TipsPanel/ConfirmButton").GetComponent<Button>().onClick.AddListener(BackToMenuScene);

                clientProperty = GameObject.Find("ClientProperty");
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
                gameSocketClient.gameManager = this;

                resolutionDropdown = pausePanel.transform.Find("PauseBoard/ResolutionDropdown").GetComponent<Dropdown>();
                fullScreenToggle = pausePanel.transform.Find("PauseBoard/FullScreenToggle").GetComponent<Toggle>();
                musicAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
                musicSlider = pausePanel.transform.Find("PauseBoard/MusicSlider").GetComponent<Slider>();
                soundSlider = pausePanel.transform.Find("PauseBoard/SoundSlider").GetComponent<Slider>();

                yourTitle = playerPrefab.transform.Find("Canvas/YourTitle").gameObject;
                yourIndex = gameSocketClient.yourIndex;

                battleSituationText = battleSituationPanel.transform.Find("BattleText").GetComponent<Text>();

                playerNameText = playerPrefab.transform.Find("Canvas/NameText").GetComponent<Text>();
                playerProperty = playerPrefab.GetComponent<PlayerProperty>();
                playerController = playerPrefab.GetComponent<PlayerController>();

                winnerText = victoryPanel.transform.Find("NameText").GetComponent<Text>();
        }

        void Start()
        {
                string[] spawnMessage = gameSocketClient.startMessage.Split(';');
                players = new GameObject[10]; // max players' number is 10
                for (int i = 0; i < spawnMessage.Length; i++)
                {
                        playerProperty.playerName = spawnMessage[i].Split('=')[0];
                        playerNameText.text = playerProperty.playerName; // player's name
                        playerPrefab.transform.position = playerSpawn[int.Parse(spawnMessage[i].Split('=')[1])].transform.position; // player's position
                        if (i.ToString() != yourIndex) // if player who is got from message is not current client player
                        {
                                yourTitle.SetActive(false);
                                playerController.canControl = false; // it should not be controlled by current client's keyboard
                                players[i] = Instantiate(playerPrefab);
                                players[i].name = "Player" + i.ToString();
                                players[i].transform.SetParent(playersCollection.transform); // set parent to forbid UI layer higher than Canvas
                        }
                        else
                        {
                                yourTitle.SetActive(true);
                                playerController.canControl = true;
                                yourPlayer = Instantiate(playerPrefab);  // it is you
                                yourPlayer.name = "Player" + i.ToString();
                                yourAnimator = yourPlayer.GetComponent<Animator>();
                                yourPlayer.transform.SetParent(playersCollection.transform); // set parent to forbid UI layer higher than Canvas
                                virtualCamera.Follow = yourPlayer.transform; // camera follow player
                        }
                }
                sendTimer = new Timer(SendPlayerAction, null, 0, sendPeriod);
        }

        void Update()
        {
                playerAction = gameSocketClient.playerAction;
                if (playerAction != null)
                {
                        try
                        {
                                ReceivePlayerAction(int.Parse(playerAction.PlayerIndex));
                        }
                        catch
                        {
                                playerAction = null;
                        }
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

        void OnGUI()
        {
                if (showVictoryPanel)
                {
                        victoryPanel.SetActive(true);
                        winnerText.text = FindPlayerNameByIndex(winnerIndex);
                }
                else
                {
                        victoryPanel.SetActive(false);
                }
                if (battleMessageUpdated)
                {
                        battleMessageUpdated = false;
                        for (int i = battleSituationText.text.Split('\n').Length; i > 4; i--)
                        {
                                battleSituationText.text = battleSituationText.text.Remove(0, battleSituationText.text.Split('\n')[0].Length + 1); // plus 1 for adding new line character
                        }
                        if (deadIndex != "" && deadIndex != null)
                        {
                                battleSituationText.text += "<" + FindPlayerNameByIndex(deadIndex) + "> 被淘汰了！\n";
                                playersCollection.transform.Find("Player" + leaverIndex).GetComponent<Animator>().Play("Death");
                                deadIndex = "";
                        }
                        else if (leaverIndex != "" && leaverIndex != null)
                        {
                                battleSituationText.text += "<" + FindPlayerNameByIndex(leaverIndex) + "> 离开了游戏！\n";
                                playersCollection.transform.Find("Player" + leaverIndex).GetComponent<Animator>().Play("Death");
                                leaverIndex = "";
                        }
                }
        }

        string FindPlayerNameByIndex(string index)
        {
                return playersCollection.transform.Find("Player" + index.ToString()).GetComponent<PlayerProperty>().playerName;
        }

        void ReceivePlayerAction(int index)
        {
                Animator playerAnimator = players[index].GetComponent<Animator>();
                players[index].transform.position = new Vector2(playerAction.PositionX, playerAction.PositionY);
                playerAnimator.SetFloat("Horizontal", playerAction.Horizontal);
                playerAnimator.SetFloat("Vertical", playerAction.Vertical);
                playerAnimator.SetFloat("Speed", playerAction.Speed);
                playerAnimator.SetFloat("Health", playerAction.Health);
                playerAnimator.SetBool("MeleeAttack", playerAction.IsMeleeAttack);
                playerAnimator.SetBool("RangedAttack", playerAction.IsRangedAttack);
        }

        void SendPlayerAction(object obj)
        {
                yourAction = new PlayerAction
                {
                        PlayerIndex = yourIndex,
                        PositionX = yourPlayer.transform.position.x,
                        PositionY = yourPlayer.transform.position.y,
                        Horizontal = yourAnimator.GetFloat("Horizontal"),
                        Vertical = yourAnimator.GetFloat("Vertical"),
                        Speed = yourAnimator.GetFloat("Speed"),
                        Health = yourAnimator.GetFloat("Health"),
                        IsMeleeAttack = yourAnimator.GetBool("MeleeAttack"),
                        IsRangedAttack = yourAnimator.GetBool("RangedAttack")
                };
                gameSocketClient.SendObjectToServer(yourAction);
        }

        public void YourPlayerDead()
        {
                gameSocketClient.SendMsgToServer("dead=" + gameSocketClient.yourIndex);
                sendTimer.Dispose(); // stop sending player action to the server
                ghost.SetActive(true);
                ghost.transform.position = playersCollection.transform.GetChild(0).transform.position;
                virtualCamera.Follow = ghost.transform;
                if (playersCollection.transform.childCount - 1 <= 1) // the player himself has not yet destroyed this time, so must minus one
                {
                        gameSocketClient.SendMsgToServer("winner=" + playersCollection.transform.GetChild(0).name.Replace("Player", ""));
                }
        }

        public void ShowPausePanel()
        {
                showPausePanel = true;
                pausePanel.SetActive(true);
        }

        public void HidePausePanel()
        {
                showPausePanel = false;
                pausePanel.SetActive(false);
        }

        public void ShowVictoryPanel(string winnerIndex)
        {
                showVictoryPanel = true;
                this.winnerIndex = winnerIndex;
        }

        public void ChangeResolution()
        {
                switch (resolutionDropdown.value)
                {
                        case 0:
                                Screen.SetResolution(1920, 1080, fullScreenToggle.isOn);
                                break;
                        case 1:
                                Screen.SetResolution(1366, 768, fullScreenToggle.isOn);
                                break;
                        case 2:
                                Screen.SetResolution(1280, 720, fullScreenToggle.isOn);
                                break;
                        case 3:
                                Screen.SetResolution(1024, 768, fullScreenToggle.isOn);
                                break;
                }
        }

        public void ChangeFullScreen()
        {
                if (fullScreenToggle.isOn)
                {
                        Screen.fullScreen = true;
                }
                else
                {
                        Screen.fullScreen = false;
                }
        }

        public void ChangeMusicVolume()
        {
                musicAudio.volume = musicSlider.value;
        }

        public void ChangeSoundVolume()
        {
                foreach (AudioSource audioSource in audioCollection)
                {
                        audioSource.volume = soundSlider.value;
                }
        }

        public void UpdatePlayerDead(string deadIndex)
        {
                battleMessageUpdated = true;
                this.deadIndex = deadIndex;
        }

        public void UpdatePlayerLeaver(string leaverIndex)
        {
                battleMessageUpdated = true;
                this.leaverIndex = leaverIndex;
        }

        public void BackToMenuScene()
        {
                if (gameSocketClient.clientSocket != null)
                {
                        gameSocketClient.SendMsgToServer("leaver=" + yourIndex);
                        sendTimer.Dispose(); // stop sending player action to the server
                        if (playersCollection.transform.childCount - 1 <= 1) // the player himself has not yet destroyed this time, so must minus one
                        {
                                gameSocketClient.SendMsgToServer("winner=" + playersCollection.transform.GetChild(0).name.Replace("Player", ""));
                        }
                        gameSocketClient.clientSocket.Close();
                }
                Destroy(clientProperty.gameObject);
                SceneManager.LoadScene(0);
        }
}