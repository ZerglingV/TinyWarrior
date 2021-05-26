using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
        public string roomAddress;

        ButtonClickSound buttonClickSound;
        GameSocketClient gameSocketClient;

        void Start()
        {
                buttonClickSound = transform.Find("/Canvas").GetComponent<ButtonClickSound>();
                GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(buttonClickSound.PlayButtonClickAudio)); // roomInfoPrefab binds audio to play sound effect
                gameSocketClient = GameObject.Find("/ClientProperty").GetComponent<GameSocketClient>();
        }

        public void EnterRoom()
        {
                GameObject currentRoomButton = EventSystem.current.currentSelectedGameObject;
                gameSocketClient.SendMsgToServer("enter=" + currentRoomButton.GetComponent<RoomButton>().roomAddress);
        }
}
