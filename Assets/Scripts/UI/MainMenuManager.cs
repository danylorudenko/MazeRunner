using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    public static MainMenuManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public GameObject recordsPanel;
    public GameObject controlsPanel;
    public GameObject exitButton;

    public void Awake()
    {
        Instance = this;
        XmlManager.TryCreateXmlRecrodsFile();
    }

    public void OpenRecordsPanel()
    {
        recordsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    } 
}