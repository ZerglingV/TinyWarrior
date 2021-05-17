using System.Collections;
using TinyWarriorInfo;
using UnityEngine;
using UnityEngine.UI;

public class RoomEnteredPanel : MonoBehaviour
{
        public GameObject playerInfoPrefab;
        public GameObject clientProperty;
        public GameObject detailBoard;

        Text playerNameText;
        GameObject ownerImage;
        GameObject ownerText;
        GameObject youText;
        GameObject startButton;
        GameSocketClient gameSocketClient;
        RoomInfo roomEnteredInfo;
        bool isInitialized;
        bool getRoomEnteredData;

        void Awake()
        {
                playerNameText = playerInfoPrefab.transform.Find("PlayerName").GetComponent<Text>();
                ownerImage = playerInfoPrefab.transform.Find("OwnerImage").gameObject;
                ownerText = playerInfoPrefab.transform.Find("OwnerText").gameObject;
                youText = playerInfoPrefab.transform.Find("YouText").gameObject;
                startButton = transform.Find("StartButton").gameObject;
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
                isInitialized = true;
        }

        void OnEnable()
        {
                if (isInitialized)
                {
                        RefreshRoomInfo();
                }
        }

        void OnGUI()
        {
                if (getRoomEnteredData)
                {
                        RefreshRoomInfo();
                }
        }

        // function of enter room is written in the RoomButton script
        public void StartCurrentRoom()
        {
                gameSocketClient.SendMsgToServer("start=" + roomEnteredInfo.OwnerAddress);
        }

        public void LeaveCurrentRoom()
        {
                gameSocketClient.SendMsgToServer("leave=" + roomEnteredInfo.OwnerAddress);
        }

        public void AlreadyGetData()
        {
                getRoomEnteredData = true;
        }

        void RefreshRoomInfo()
        {
                for (int i = 0; i < detailBoard.transform.childCount; i++)
                {
                        Destroy(detailBoard.transform.GetChild(i).gameObject);
                }
                roomEnteredInfo = gameSocketClient.roomEnteredInfo;
                if (roomEnteredInfo != null)
                {
                        if (roomEnteredInfo.OwnerAddress != gameSocketClient.clientSocket.LocalEndPoint.ToString())
                        {
                                startButton.SetActive(false);
                        }
                        else
                        {
                                startButton.SetActive(true);
                        }
                        foreach (DictionaryEntry guestInfo in roomEnteredInfo.GuestsAddressAndName)
                        {
                                // set playerInfoPrefab's property
                                playerNameText.text = (string)guestInfo.Value;
                                if ((string)guestInfo.Key == roomEnteredInfo.OwnerAddress)
                                {
                                        ownerImage.SetActive(true);
                                        ownerText.SetActive(true);
                                }
                                else
                                {
                                        ownerText.SetActive(false);
                                        ownerImage.SetActive(false);
                                }
                                if ((string)guestInfo.Key == gameSocketClient.clientSocket.LocalEndPoint.ToString())
                                {
                                        youText.SetActive(true);
                                }
                                else
                                {
                                        youText.SetActive(false);
                                }

                                // instantiate a playerInfoPrefab
                                GameObject player = Instantiate(playerInfoPrefab);
                                player.transform.SetParent(detailBoard.transform);
                                player.transform.localScale = Vector3.one;
                        }
                }
                getRoomEnteredData = false;
        }
}
