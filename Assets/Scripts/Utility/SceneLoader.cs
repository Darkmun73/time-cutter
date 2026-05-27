using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public static void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}