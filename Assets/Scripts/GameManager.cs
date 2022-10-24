using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings MATCH_SETTINGS;

    private void Awake() {
        if (instance != null) {
            throw new Exception("More than 1 Game Manager in scene");
        }

        instance = this;
    }

    // Need this to update UI
    void Update() 
    {
        
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";
    private static IDictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player) {
        string playerID = PLAYER_ID_PREFIX + netID;

        players.Add(playerID, player);
        player.transform.name = playerID;
        // Initialize PlayerStats for new player
        GameManagerServer.InitPlayerStats(player);
    }

    public static void UnRegisterPlayer(string id) {
        players.Remove(id);
    }

    public static Player GetPlayer(string id) {
        return players[id];
    }

    public static ICollection<Player> GetPlayers() {
        return players.Values;
    }

    #endregion

}
