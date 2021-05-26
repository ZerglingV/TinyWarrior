using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
        public GameObject connectionPanel;
        public GameObject exitPanel;
        public GameObject roomsPanel;
        public GameObject roomSettingPanel;
        public GameObject roomEnteredPanel;
        public GameObject clientProperty;
        public GameObject succeedingCanvasPrefab;
        public GameObject succeedingCanvas;

        RoomEnteredPanel roomEnteredPanelScript;
        RoomsPanel roomsPanelScript;
        bool gameStart;
        bool showConnectionPanel = true;
        bool showExitPanel;
        bool showRoomsPanel;
        bool showRoomSettingPanel;
        bool showRoomEnteredPanel;

        void Awake()
        {
                succeedingCanvas = GetSucceedingCanvas();
                roomEnteredPanelScript = roomEnteredPanel.GetComponent<RoomEnteredPanel>();
                roomsPanelScript = roomsPanel.GetComponent<RoomsPanel>();
        }

        // update GUI
        private void OnGUI()
        {
                if (gameStart)
                {
                        DontDestroyOnLoad(clientProperty);
                        DontDestroyOnLoad(succeedingCanvas);
                        SceneManager.LoadScene(1);
                }

                // connect panel
                if (showConnectionPanel)
                {
                        connectionPanel.SetActive(true);
                }
                else
                {
                        connectionPanel.SetActive(false);
                }

                // exit panel
                if (showExitPanel)
                {
                        exitPanel.SetActive(true);
                }
                else
                {
                        exitPanel.SetActive(false);
                }

                // rooms panel
                if (showRoomsPanel)
                {
                        roomsPanel.SetActive(true);
                }
                else
                {
                        roomsPanel.SetActive(false);
                }

                // room setting panel
                if (showRoomSettingPanel)
                {
                        roomSettingPanel.SetActive(true);
                }
                else
                {
                        roomSettingPanel.SetActive(false);
                }

                // room entered panel
                if (showRoomEnteredPanel)
                {
                        roomEnteredPanel.SetActive(true);
                }
                else
                {
                        roomEnteredPanel.SetActive(false);
                }
        }

        // try to get a succeedingCanvas instance
        public GameObject GetSucceedingCanvas()
        {
                GameObject canvas;
                if ((canvas = GameObject.Find("SucceedingCanvas")) != null)
                {
                        return canvas;
                }
                else
                {
                        canvas = Instantiate(succeedingCanvasPrefab);
                        canvas.name = "SucceedingCanvas";
                        return canvas;
                }
        }

        // game start function
        public void StartGame()
        {
                gameStart = true;
        }

        #region -- Connect Panel Function --

        public void HideConnectionPanel()
        {
                showConnectionPanel = false;
        }

        #endregion

        #region -- Exit Panel Function --

        public void ShowExitPanel()
        {
                showExitPanel = true;
        }

        public void HideExitPanel()
        {
                showExitPanel = false;
        }
        #endregion

        #region -- Rooms Panel Function --

        public void ShowRoomsPanel()
        {
                showRoomsPanel = true;
        }

        public void HideRoomsPanel()
        {
                showRoomsPanel = false;
        }

        public void RefreshRoomsPanel()
        {
                roomsPanelScript.AlreadyGetData();
        }

        #endregion

        #region -- Room Setting Panel Function --

        public void ShowRoomSettingPanel()
        {
                showRoomSettingPanel = true;
        }

        public void HideRoomSettingPanel()
        {
                showRoomSettingPanel = false;
        }


        #endregion

        #region -- Room Entered Panel Function --

        public void ShowRoomEnteredPanel()
        {
                showRoomEnteredPanel = true;
        }

        public void HideRoomEnteredPanel()
        {
                showRoomEnteredPanel = false;
        }

        public bool IsShowingRoomEnteredPanelState()
        {
                return showRoomEnteredPanel;
        }

        public void RefreshRoomEnteredPanel()
        {
                roomEnteredPanelScript.AlreadyGetData();
        }

        #endregion

}
