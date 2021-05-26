using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
        Dropdown resolutionDropdown;
        Toggle fullScreenToggle;

        void Start()
        {
                resolutionDropdown = transform.Find("PauseBoard/ResolutionDropdown").GetComponent<Dropdown>();
                switch (Screen.width)
                {
                        case 1920:
                                resolutionDropdown.value = 0;
                                break;
                        case 1366:
                                resolutionDropdown.value = 1;
                                break;
                        case 1280:
                                resolutionDropdown.value = 2;
                                break;
                        case 1024:
                                resolutionDropdown.value = 3;
                                break;
                }
                fullScreenToggle = transform.Find("PauseBoard/FullScreenToggle").GetComponent<Toggle>();
                fullScreenToggle.isOn = Screen.fullScreen;
        }
}
