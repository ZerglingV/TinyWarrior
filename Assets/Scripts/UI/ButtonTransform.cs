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

        private void Awake()
        {
                currentButton = GetComponent<Button>();
                buttonImage = currentButton.GetComponent<Image>();
                buttonText = currentButton.GetComponentInChildren<Text>();
                normalPosition = buttonText.transform.position;
                pressedPosition = new Vector2(normalPosition.x, normalPosition.y - offesetY);
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

}
