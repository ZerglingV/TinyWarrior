using UnityEngine;
using UnityEngine.UI;

public class InputFieldFilter : MonoBehaviour
{
        InputField inputField;

        void Awake()
        {
                inputField = this.gameObject.GetComponent<InputField>();
        }

        public void ChineseEnglishNumber()
        {
                char ch;
                for (int i = 0; i < inputField.text.Length; i++)
                {
                        ch = inputField.text[i];
                        if ((int)ch <= 127 && !('a' <= ch && ch <= 'z') && !('A' <= ch && ch <= 'Z') && !('0' <= ch && ch <= '9')) // if the character is not chinese & not english & not number, then filter it
                        {
                                inputField.text = inputField.text.Replace(ch.ToString(), "");
                        }
                }
        }

        public void IP()
        {
                char ch;
                for (int i = 0; i < inputField.text.Length; i++)
                {
                        ch = inputField.text[i];
                        if (ch != '.' && !('0' <= ch && ch <= '9')) // if the character is not chinese & not english & not number, then filter it
                        {
                                inputField.text = inputField.text.Replace(ch.ToString(), "");
                        }
                }
        }
}
