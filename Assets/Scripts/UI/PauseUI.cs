using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;


    private void Awake()
    {
        _soundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSound();
            soundVolumeTextMesh.text = "SOUDN : " + SoundManager.Instance.GetSoundVolume();
        });
        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusic();
            musicVolumeTextMesh.text = "MUSIC : " + MusicManager.Instance.GetMusicVolume(); 
        });
        _resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnpauseGame();
        });
        _mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

        soundVolumeTextMesh.text = "SOUND : " + SoundManager.Instance.GetSoundVolume();
        soundVolumeTextMesh.text = "MUSIC : " + MusicManager.Instance.GetMusicVolume();
        _resumeButton.Select();
    }

    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}