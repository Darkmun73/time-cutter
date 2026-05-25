using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTutorial() {}

    public static void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}