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
    public const double MatchRoundTimer = 3;   // 3 minutes

    [SyncVar]
    private Colour currentRoundColor = Colour.Yellow;
    
    [SyncVar]
    private double changeColourTimerDisplay = ChangeColourTimer; // in seconds

    [SyncVar]
    private double matchRoundTimerDisplay = MatchRoundTimer; // in seconds

    private PlayerStatsList playersStats = new PlayerStatsList();

    void Start()
    {
        if (isServer)
        {
            currentRoundColor = RandomColour();
        }
    }

    void FixedUpdate() {
        if (isServer) { 
            if (changeColourTimerDisplay < 1)
            {
                changeColourTimerDisplay = ChangeColourTimer;
                currentRoundColor = RandomColour();
            }
            changeColourTimerDisplay -= Time.deltaTime;
            matchRoundTimerDisplay -= Time.deltaTime;
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

    public static double GetMatchRoundDisplay()
    {
        return instance?.matchRoundTimerDisplay ?? 0.0;
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

    public static void InitPlayerStats(Player p)
    {
        instance.playersStats.InitPlayerStats(p);
    }

    public static bool IsGameOver()
    {
        return instance.matchRoundTimerDisplay <= 0.0;
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
        //Green = 0,
        Yellow = 0,
        //Orange,
        Red,
        //Black,
        Blue,
        Violet,
        Gold
    }

}
