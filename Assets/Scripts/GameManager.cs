using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //[SerializeField]private int _levelNumber;
    private static int _levelNumber = 1;
    private static int _totalScore = 0;
    //private static int _levelNumber; //this is one of the problem why the level was not loading cuz we didn't initialize the value and it take random number which is not valid

    //public static int _levelNumber;

    public static void ResetStaticData()
    {
        _levelNumber = 1;
        _totalScore = 0;
    }
    [SerializeField] private List<GameLevel> _gameLevelList; //to keep track of levels
    [SerializeField] private CinemachineCamera _cinemachineCamera; //to keep track of levels

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;


    private int score;
    private float time;
    private bool _isTimerActive;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //using both singleton and Event&Listeners
        Lander.Instance.OnCoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        GameInputs.Instance.OnMenuButtonPressed += GameInputs_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    private void GameInputs_OnMenuButtonPressed(object sender, EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        _isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal)
        {
            _cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
        }
    }
    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnGameLevel.GetLevelStartPosition();
        _cinemachineCamera.Target.TrackingTarget = spawnGameLevel.GetCameraStartTargetTransform();
        CinemachineCameraZoom2D.Instance.SetOrthographicSize(spawnGameLevel.GetZoomOutOrthographicSize());
    }
    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in _gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == _levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void Update()
    {
        if (_isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickUp(object sender, EventArgs e)
    {
        AddScore(500);
    }


    public void AddScore(int scoreAmt)
    {
        score += scoreAmt;
        Debug.Log("Score : " + score);
    }

    public int GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return time;
    }
    public int GetTotalScore()
    {
        return _totalScore;
    }
    public void GoToNextLevel()
    {
        _levelNumber++;
        _totalScore += score;
        if (GetGameLevel() == null)
        {
            //No more Level
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            //we still have more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }
    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }
    public int GetLevelNumber()
    {
        return _levelNumber;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnPaused?.Invoke(this, EventArgs.Empty);
    }
    public void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }
}