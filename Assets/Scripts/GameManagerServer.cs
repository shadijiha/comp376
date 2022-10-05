using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System;
using Assets.Scripts;
using Random = UnityEngine.Random;

public class GameManagerServer : NetworkBehaviour
{
    private static GameManagerServer instance;
    public const double ChangeColourTimer = 15;   // 15 seconds

    [SyncVar]
    private Colour currentRoundColor = Colour.Green;
    
    [SyncVar]
    private double changeColourTimerDisplay = ChangeColourTimer; // in seconds

    [SyncVar]
    private PlayerStatsList playersStats = new PlayerStatsList();

    void FixedUpdate() {
        if (isServer) { 
            if (changeColourTimerDisplay < 1)
            {
                changeColourTimerDisplay = ChangeColourTimer;
                currentRoundColor = RandomColour();
            }
            changeColourTimerDisplay -= Time.deltaTime;
        }
    }

    public void Awake() {
        if (instance != null)
        {
            throw new Exception("More than 1 Game Manager in scene");
        }
        instance = this;
    }

    #region static functions that modify game state on server
    public static Colour GetRoundColour()
    {
        return instance.currentRoundColor;
    }
    
    public static double GetTimerDisplay() { 
        return instance?.changeColourTimerDisplay ?? 0.0;
    }

    public static void RegisterStat(Player killer, Player victim) {
        instance.playersStats.IncrementKills(killer);
        instance.playersStats.IncrementDeath(victim);
    }

    public static void LogStats() {
        Debug.Log(instance.playersStats.ToString());
    }

    public static PlayerStatsList GetPlayerStatsList()
    {
        return instance.playersStats;
    }

    #endregion

    private Colour RandomColour()
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
