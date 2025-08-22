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
    //private static int _levelNumber; //this is one of the problem why the level was not loading cuz we didn't initialize the value and it take random number which is not valid

    //public static int _levelNumber;
    [SerializeField] private List<GameLevel> _gameLevelList; //to keep track of levels
    [SerializeField] private CinemachineCamera _cinemachineCamera; //to keep track of levels

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

        LoadCurrentLevel();
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
        foreach (GameLevel gameLevel in _gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == _levelNumber)
            {
                GameLevel spawnGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnGameLevel.GetLevelStartPosition();
                _cinemachineCamera.Target.TrackingTarget = spawnGameLevel.GetCameraStartTargetTransform();
                CinemachineCameraZoom2D.Instance.SetOrthographicSize(spawnGameLevel.GetZoomOutOrthographicSize());
            }
        }

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
    public void GoToNextLevel()
    {
        _levelNumber++;
        SceneManager.LoadScene(0);
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }
    public int GetLevelNumber()
    {
        return _levelNumber;
    }
}