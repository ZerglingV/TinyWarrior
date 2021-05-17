using UnityEngine;
using UnityEngine.EventSystems;

public class RoomButton : MonoBehaviour
{
        public string roomAddress;

        GameSocketClient gameSocketClient;

        void Start()
        {
                gameSocketClient = GameObject.Find("/ClientProperty").GetComponent<GameSocketClient>();
        }

        public void EnterRoom()
        {
                GameObject currentRoomButton = EventSystem.current.currentSelectedGameObject;
                gameSocketClient.SendMsgToServer("enter=" + currentRoomButton.GetComponent<RoomButton>().roomAddress);
        }
}
