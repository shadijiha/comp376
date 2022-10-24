using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings MATCH_SETTINGS;

    // UI
    private Text currentColourText;
    private Text nextColourTimer;

    private void Awake() {
        if (instance != null) {
            throw new Exception("More than 1 Game Manager in scene");
        }

        instance = this;

        // Ui Text fields
        currentColourText = GameObject.Find("CurrentColourText").GetComponent<Text>();
        nextColourTimer = GameObject.Find("NextColourTimer").GetComponent<Text>();
    }

    // Need this to update UI
    void Update() {
        // Update UI
        {
            var timer = GameManagerServer.GetTimerDisplay();
            TimeSpan ts = TimeSpan.FromSeconds(timer);
            nextColourTimer.text = $"{ts.Minutes}:{ts.Seconds}";
            currentColourText.text = "Colour: " + GameManagerServer.GetRoundColour().ToString("g");
        }
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";
    private static IDictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player) {
        string playerID = PLAYER_ID_PREFIX + netID;

        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnRegisterPlayer(string id) {
        players.Remove(id);
    }

    public static Player GetPlayer(string id) {
        try {
            return players[id];
        }
        catch (Exception) {
            return null;
        }       
    }

    public static ICollection<Player> GetPlayers() {
        return players.Values;
    }

    #endregion

}
