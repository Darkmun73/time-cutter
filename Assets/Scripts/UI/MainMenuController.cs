using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void NewGame()
    {
        SceneLoader.LoadLevel1();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
