using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool Playing => !Paused && GameStarted && !GameFinished;
    public Action<int> OnCountdown;
    public PlayerManager PlayerManager => playerManager;
    public TimeSpan currentTime => DateTime.Now - roundStartTime;
    public TimeSpan roundTime => roundTimeSpan;
    public Colors ColorsInUse => colors;
    public Camera GameplayCamera => gameplayCamera;

    public bool Paused { get; private set; }
    public bool GameStarted { get; private set; }
    public bool GameFinished { get; private set; }
    public int CountdownTime
    {
        get => countdownTime;
        private set
        {
            countdownTime = value;
            OnCountdown?.Invoke(countdownTime);
        }
    }

    [SerializeField] private Camera gameplayCamera; //For the player car movement
    [SerializeField] private Colors colors; // Scriptable object that holds all the information on what colors are available 
    [SerializeField] private int gameTimeSecond = 30;  // Time for each round in seconds
    [SerializeField] private PlayerManager playerManager; //The script in charge of spawning and managing the cars

    private TimeSpan roundTimeSpan;
    private DateTime roundStartTime;
    private List<Action> onGameStart = new List<Action>();
    private List<Action> onGameEnd = new List<Action>();
    private List<Action> onReset = new List<Action>();
    private int countdownTime = 3;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        playerManager.SetColors(colors);
        roundTimeSpan = new TimeSpan(0, 0, gameTimeSecond);
        GameStarted = false;
    }

    public void AddActionOnGameStart(Action action)
    {
        onGameStart.Add(action);
    }

    public void AddActionOnGameEnd(Action action)
    {
        onGameEnd.Add(action);
    }

    public void AddActionOnReset(Action action)
    {
        onReset.Add(action);
    }

    [Sirenix.OdinInspector.Button]
    public void StartGame()
    {
        colors.ResetColors();
        Paused = true; //Make sure things don't move
        Time.timeScale = 1; //Need to make sure time still moves for this to work
        StartCoroutine(GameLoop());
    }

    //Set this here to show the start screen again on a restart
    public void ResetGame()
    {
        SetPauseState(true);
        foreach (Action action in onReset)
        {
            action?.Invoke();
        }
    }

    public void SetPauseState(bool pausedState)
    {
        Paused = pausedState;
        Time.timeScale = Paused ? 0 : 1;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());

        yield return new WaitUntil(() => currentTime > roundTimeSpan);

        RoundEnding();
    }

    //Actions on round Starting
    private IEnumerator RoundStarting()
    {
        playerManager.OnGameStart();
        foreach (var action in onGameStart)
        {
            action?.Invoke();
        }


        CountdownTime = 3;
        for (int i = CountdownTime; i >= 0; i--)
        {
            CountdownTime = i;
            yield return new WaitForSeconds(0.5f);
        }

        GameStarted = true;
        GameFinished = false;
        Paused = false;

        roundStartTime = DateTime.Now;
    }

    //Actions on round finished
    private void RoundEnding()
    {
        playerManager.SetCarState(false);
        GameStarted = false;
        GameFinished = true;
        foreach (Action action in onGameEnd)
        {
            action?.Invoke();
        }
    }
}





