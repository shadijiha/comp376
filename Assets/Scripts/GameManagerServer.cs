using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System;

public class GameManagerServer : NetworkBehaviour
{
    public static GameManagerServer instance;

    private volatile Colour currentRoundColor = RandomColour();
    private Thread colorChanger;
    private const double ChangeColourTimer = 15;   // 15 seconds
    private double changeColourTimerDisplay; // in seconds

    private bool serverStarted = false;

    ~GameManagerServer()
    {
        if (colorChanger != null)
        {
            colorChanger.Abort();
        }
    }

    public void Awake() {
        if (instance != null)
        {
            throw new Exception("More than 1 Game Manager in scene");
        }
        instance = this;

        if (colorChanger is null)
        {
            colorChanger = new Thread(() => {

                double counter = ChangeColourTimer;
                while (true)
                {
                    if (changeColourTimerDisplay < 1)
                    {
                        counter = ChangeColourTimer;
                        currentRoundColor = RandomColour();
                    }

                    // Update time
                    counter -= 1.0f;
                    changeColourTimerDisplay = counter;
                    Thread.Sleep(1000);
                }

            });
            colorChanger.Start();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static Colour GetRoundColour()
    {
        return instance.currentRoundColor;
    }
    
    public static double GetTimerDisplay() { 
        return instance?.changeColourTimerDisplay ?? 0.0;
    }

    private static System.Random random = new System.Random();
    private static Colour RandomColour()
    {
        Array values = Enum.GetValues(typeof(Colour));
        int index = random.Next(0, values.Length);
        return (Colour)values.GetValue(index);
    }

    public enum Colour
    {
        Green = 0,
        Yellow,
        Orange,
        Red,
        Black,
        Blue,
        Violet
    }
}
