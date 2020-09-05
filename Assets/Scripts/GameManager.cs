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

    private int currHour = 0;
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

    public IEnumerator UpdateClock()
    {
        while (isGoing)
        {
            yield return new WaitForSeconds(5f);
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
        Debug.Log("Did some map updates - curr time: " + currHour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
