using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System;
using Unity.Jobs;
using Random = UnityEngine.Random;

public class GameManagerServer : NetworkBehaviour
{
    private static GameManagerServer instance;
    public const double ChangeColourTimer = 15;   // 15 seconds

    [SyncVar]
    private Colour currentRoundColor = Colour.Green;
    
    [SyncVar]
    private double changeColourTimerDisplay = ChangeColourTimer; // in seconds

    void FixedUpdate() {
        if (isServer) { 

            Debug.Log("Server!!!");
            if (changeColourTimerDisplay < 1)
            {
                changeColourTimerDisplay = ChangeColourTimer;
                currentRoundColor = RandomColour();
            }
            changeColourTimerDisplay -= Time.deltaTime;
            // Update time
            // counter -= 1.0f;
            //changeColourTimerDisplay = counter;
            //RpcDecrementTimer();
        }
    }

    public void Awake() {
        if (instance != null)
        {
            throw new Exception("More than 1 Game Manager in scene");
        }
        instance = this;
    }

    public static Colour GetRoundColour()
    {
        return instance.currentRoundColor;
    }
    
    public static double GetTimerDisplay() { 
        return instance?.changeColourTimerDisplay ?? 0.0;
    }

    public Colour RandomColour()
    {
        Array values = Enum.GetValues(typeof(Colour));
        int index = Random.Range(0, values.Length);
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
