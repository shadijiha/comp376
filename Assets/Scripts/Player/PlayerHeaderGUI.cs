using System;
using UnityEngine;
using TextMeshPro = TMPro.TextMeshProUGUI;

public class PlayerHeaderGUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro currentColourText;
    [SerializeField] private TextMeshPro nextColourTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var timer = GameManagerServer.GetTimerDisplay();
        TimeSpan ts = TimeSpan.FromSeconds(timer);
        nextColourTimer.text = $"{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.Seconds.ToString().PadLeft(2, '0')}";
        currentColourText.text = GameManagerServer.GetRoundColour().ToString("g");

        // Do a fade-in fade-out animation when the time reaches at 6 seconds
        if (ts.TotalSeconds <= 6)
            nextColourTimer.alpha = Math.Abs((float)Math.Cos(Math.PI * ts.TotalMilliseconds / 1000));
        else
            nextColourTimer.alpha = 1;
    }
}
