using UnityEngine.UI;
using UnityEngine;

public class GameOverPanelBehaviour : MonoBehaviour {

    public InputField userNameInputField;

    public void SavePreviousGame()
    {
        XmlManager.WriteNewRecord(userNameInputField.text, References.labyrinthManager.player.GetCoinsCollectedCount());
        References.sceneLoadingManager.EndGame();
    }
}
