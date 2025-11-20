using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    public void ToggleSettings()
    {
        settingsMenu?.SetActive(!settingsMenu.activeSelf);
    }
}
