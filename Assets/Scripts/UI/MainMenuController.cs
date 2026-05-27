using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void NewGame()
    {
        SceneLoader.LoadTutorial();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
