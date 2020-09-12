using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Button pauseButton;
    public Text clockText;

    public Map map;
    public Player player;
    public RaidSpawner raidSpawner;

    private int currHour = 0;
    private float interval = 1f;
    private bool isGoing;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FormatCurrentTime();
        isGoing = true;
        StartCoroutine(UpdateClock());
    }

    public void ToggleTimer()
    {
        isGoing = !isGoing;
        Text pauseButtonText = pauseButton.GetComponentInChildren<Text>();
        pauseButtonText.text = isGoing ? "Pause" : "Play";
        if (isGoing)
        {
            StartCoroutine(UpdateClock());
        } else
        {
            StopAllCoroutines();
        }
    }

    public void Pause()
    {
        isGoing = false;
        Text pauseButtonText = pauseButton.GetComponentInChildren<Text>();
        pauseButtonText.text = "Play";
        StopAllCoroutines();
    }

    public void Play()
    {
        isGoing = true;
        Text pauseButtonText = pauseButton.GetComponentInChildren<Text>();
        pauseButtonText.text = "Pause";
        StartCoroutine(UpdateClock());
    }

    public bool IsTimeMoving()
    {
        return isGoing;
    }

    public IEnumerator UpdateClock()
    {
        while (isGoing)
        {
            yield return new WaitForSeconds(interval);
            currHour = (currHour + 1) % 24;
            FormatCurrentTime();
            OnClockTick();
        }
    }

    void FormatCurrentTime()
    {
        clockText.text = "Time: " + (currHour < 10 ? "0" : "") + currHour + ":00";
    }

    void OnClockTick()
    {
        player.Tick();
        raidSpawner.Tick();
    }
}
