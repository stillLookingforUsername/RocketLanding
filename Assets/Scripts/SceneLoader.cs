using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        //name should match exactly as the name of the Scenes
        MainMenuScene,
        GameScene,
    }

    
    //technically we r using string not directly
    public static void LoadScene(Scene scene)
    {
        //trick is we will use string but not by ourself, we'll define enum
        SceneManager.LoadScene(scene.ToString()); //we didn't use string or index int (magic numbers)
    }
}