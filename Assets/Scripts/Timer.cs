using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private bool _timerActive = true; //timer starts when the scene is loaded
    private float _currentTime;
    [SerializeField] private TMP_Text _text;

    void Start()
    {
        _currentTime = 0; //initialises the timer
    }

    void Update()
    {
        if (_timerActive)
        {
            _currentTime += Time.deltaTime; //increments the timer
            UpdateTimerText(); //then displays the updated text
        }
    }

    private void UpdateTimerText()
    {
        //formats the timer to be minutes and seconds
        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        _text.text = time.ToString(@"mm\:ss");
    }

    public void StopTimer()
    {
        _timerActive = false; //stops the timer
    }
}

