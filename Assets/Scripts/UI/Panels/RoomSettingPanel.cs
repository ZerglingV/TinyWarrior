using System.Collections;
using TinyWarriorInfo;
using UnityEngine;
using UnityEngine.UI;

public class RoomSettingPanel : MonoBehaviour
{
        public InputField nameText;
        public Slider numberSlider;
        public Text numberText;
        public Image numberImage;
        public GameObject clientProperty;
        public PanelManager panelManager;

        GameSocketClient gameSocketClient;

        void Awake()
        {
                gameSocketClient = clientProperty.GetComponent<GameSocketClient>();
        }

        public void SetNumberImage()
        {
                numberImage.fillAmount = numberSlider.value / 10;
        }

        public void SetNumberText()
        {
                numberText.text = numberSlider.value + "人";
        }

        public void ConfirmCreateRoom()
        {
                if (nameText.text == "")
                {
                        panelManager.ShowConnectionPanel("请输入房间名！");
                        return;
                }
                RoomInfo newRoom = new RoomInfo();
                newRoom.OwnerAddress = gameSocketClient.clientSocket.LocalEndPoint.ToString();
                newRoom.GuestsAddressAndName = new Hashtable();
                newRoom.GuestsAddressAndName.Add(newRoom.OwnerAddress, clientProperty.GetComponent<PlayerProperty>().playerName);
                newRoom.MaxPlayer = (int)numberSlider.value; // work in progress
                newRoom.RoomName = nameText.text; // work in progress
                gameSocketClient.SendObjectToServer(newRoom);
        }
}
