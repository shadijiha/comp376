using System;
using UnityEngine;
using TextMeshPro = TMPro.TextMeshProUGUI;

public class PlayerHeaderGUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro currentColourText;
    [SerializeField] private TextMeshPro nextColourTimer;
    [SerializeField] private TextMeshPro currentRoundTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var colorTimer = GameManagerServer.GetTimerDisplay();
        var roundTimer = GameManagerServer.GetMatchRoundDisplay();
        TimeSpan colorTimerTimeSpan = TimeSpan.FromSeconds(colorTimer);
        TimeSpan roundTimerTimeSpan = TimeSpan.FromSeconds(roundTimer);
        nextColourTimer.text = $"{colorTimerTimeSpan.Minutes.ToString().PadLeft(2, '0')}:{colorTimerTimeSpan.Seconds.ToString().PadLeft(2, '0')}";
        currentRoundTimer.text = $"{roundTimerTimeSpan.Minutes.ToString().PadLeft(2, '0')}:{roundTimerTimeSpan.Seconds.ToString().PadLeft(2, '0')}";
        currentColourText.text = GameManagerServer.GetRoundColour().ToString("g");

        // Do a fade-in fade-out animation when the time reaches at 6 seconds
        if (colorTimerTimeSpan.TotalSeconds <= 6)
            nextColourTimer.alpha = Math.Abs((float)Math.Cos(Math.PI * colorTimerTimeSpan.TotalMilliseconds / 1000));
        else
            nextColourTimer.alpha = 1;
    }
}
