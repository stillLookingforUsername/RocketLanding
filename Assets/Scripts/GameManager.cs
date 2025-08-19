using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int _levelNumber;
    [SerializeField] private List<GameLevel> _gameLevelList; //to keep track of levels

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
    }
    private void LoadCurrentLevel()
    {
        foreach (GameLevel gameLevel in _gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == _levelNumber)
            {
                GameLevel spawnGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnGameLevel.GetLevelStartPosition();
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
}