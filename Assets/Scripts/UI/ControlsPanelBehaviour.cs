using UnityEngine;

public class ControlsPanelBehaviour : MonoBehaviour {

    /// <summary>
    /// Closing controls panel, opening main menu
    /// </summary>
    public void CloseControlsPanel()
    {
        gameObject.SetActive(false);
        MainMenuManager.Instance.gameObject.SetActive(true);
    }
}
