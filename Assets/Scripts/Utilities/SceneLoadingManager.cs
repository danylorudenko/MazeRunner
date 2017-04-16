using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour {

    [SerializeField]
    private string menuSceneName;

    [SerializeField]
    private string mainSceneName;

    private void Awake()
    {
        References.sceneLoadingManager = this;
        if (!SceneManager.GetSceneByName(menuSceneName).isLoaded) {
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        }
    }

    private void Update()
    {
        if (SceneManager.GetSceneByName(mainSceneName).isLoaded) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                EndGame();
            }
        }
    }

    /// <summary>
    /// General start game logic
    /// </summary>
    public void StartGame()
    {
        if (!SceneManager.GetSceneByName(mainSceneName).isLoaded) {
            SetMenuObjectsState(false);
            SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// General gameOver method
    /// </summary>
    public void EndGame()
    {
        SceneManager.UnloadSceneAsync(mainSceneName);
        SetMenuObjectsState(true);
    }

    /// <summary>
    /// Setting all menu gameobjects state
    /// </summary>
    /// <param name="state">active state to set</param>
    private void SetMenuObjectsState(bool state)
    {
        GameObject[] menuGameObjects = SceneManager.GetSceneByName(menuSceneName).GetRootGameObjects();
        int gameObjectsCount = menuGameObjects.Length;
        for (int i = 0; i < gameObjectsCount; i++) {
            menuGameObjects[i].SetActive(state);
        }
    }
}
