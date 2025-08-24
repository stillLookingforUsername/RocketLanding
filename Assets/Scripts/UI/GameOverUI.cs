using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBotton;
    [SerializeField] private TextMeshProUGUI _scoreTextMesh;

    private void Awake()
    {
        _mainMenuBotton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });

    }
    private void Start()
    {
        _scoreTextMesh.text = "FINAL SCORE : " + GameManager.Instance.GetTotalScore().ToString();
        _mainMenuBotton.Select();
    }

}
