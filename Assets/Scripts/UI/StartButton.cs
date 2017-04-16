using UnityEngine;

public class StartButton : MonoBehaviour {

	public void StartButtonBehaviour()
    {
        References.sceneLoadingManager.StartGame();
    }
}
