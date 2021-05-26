using System.Collections.Generic;
using TinyWarriorInfo;
using UnityEngine;
using UnityEngine.UI;

public class RoomsPanel : MonoBehaviour
{
        public PanelManager panelManager;
        public GameObject clientProperty;
        public GameObject content;
        public GameObject roomInfoPrefab;
        public bool getRoomsData;

        Text roomNameText;
        Text roomStateText;
        Text numberInfoText;
        RoomButton roomButton;
        GameSocketClient gameSocketClient;
        List<RoomInfo> roomsInfo;
        bool isInitialized;

        Color redColor;
        Color greenColor;

        void Awake()
        {
                roomNameText = roomInfoPrefab.transform.Find("RoomName").GetComponent<Text>();
                roomStateText = roomInfoPrefab.transform.Find("RoomState").GetComponent<Text>();
                numberInfoText = roomInfoPrefab.transform.Find("NumberInfo").GetComponent<Text>();
                roomButton = roomInfoPrefab.transform.GetComponent<RoomButton>();
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
                isInitialized = true;

                redColor = new Color(0.7254902f, 0.01568629f, 0.0864033f);
                greenColor = new Color(0.01713243f, 0.7264151f, 0.1045782f);
        }

        void OnEnable()
        {
                if (isInitialized)
                {
                        RequestRoomsData();
                        RefreshRooms();
                }
        }

        void OnGUI()
        {
                if (getRoomsData)
                {
                        RefreshRooms();
                }
        }

        public void CreateRoom()
        {
                panelManager.ShowRoomSettingPanel();
        }

        public void DisconnectServer()
        {
                gameSocketClient.clientSocket.Close();
        }

        public void RequestRoomsData()
        {
                gameSocketClient.SendMsgToServer("getrooms");
        }

        public void AlreadyGetData()
        {
                getRoomsData = true;
        }

        private void RefreshRooms()
        {
                for (int i = 0; i < content.transform.childCount; i++)
                {
                        Destroy(content.transform.GetChild(i).gameObject);
                }
                roomsInfo = gameSocketClient.roomsInfo;
                if (roomsInfo != null)
                {
                        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 75 * roomsInfo.Count);
                        foreach (RoomInfo roomInfo in roomsInfo)
                        {
                                // set roomInfoPrefab's property
                                if (roomInfo.IsStart)
                                {
                                        roomStateText.text = "游玩中...";
                                        roomStateText.color = redColor;
                                }
                                else
                                {
                                        roomStateText.text = "等待中...";
                                        roomStateText.color = greenColor;
                                }
                                numberInfoText.text = roomInfo.GuestsAddressAndName.Count + "/" + roomInfo.MaxPlayerNumber;
                                roomNameText.text = roomInfo.RoomName;
                                roomButton.roomAddress = roomInfo.OwnerAddress;

                                // instantiate a roomInfoPrefab
                                GameObject room = Instantiate(roomInfoPrefab);
                                room.transform.SetParent(content.transform);
                                room.transform.localScale = Vector3.one;
                        }
                }
                getRoomsData = false;
        }
}
