using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;


    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene); //eg: here if we miss type the scene name we will get error
        });
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
         });
    }
}
