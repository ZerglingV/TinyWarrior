using UnityEngine;
using UnityEngine.UI;

public class ButtonTransform : MonoBehaviour
{
        public Sprite normalImage;
        public Sprite pressedImage;
        public float offesetY = 10f;

        Button currentButton;
        Text buttonText;
        Image buttonImage;
        Vector2 normalPosition;
        Vector2 pressedPosition;

        void Awake()
        {
                currentButton = GetComponent<Button>();
                buttonImage = currentButton.GetComponent<Image>();
                buttonText = currentButton.GetComponentInChildren<Text>();
                RefreshTextPosition();
        }

        void OnGUI()
        {
                if (buttonText.transform.position.x != normalPosition.x)
                {
                        RefreshTextPosition();
                }
        }

        public void TextShiftDown()
        {
                buttonImage.sprite = pressedImage;
                buttonText.transform.position = pressedPosition;
        }

        public void TextShiftUp()
        {
                buttonImage.sprite = normalImage;
                buttonText.transform.position = normalPosition;
        }

        void RefreshTextPosition()
        {
                normalPosition = buttonText.transform.position;
                pressedPosition = new Vector2(normalPosition.x, normalPosition.y - offesetY);
        }
}
