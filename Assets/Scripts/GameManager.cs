using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TextMeshPro = TMPro.TextMeshProUGUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings MATCH_SETTINGS;

    // UI
    private TextMeshPro currentColourText;
    private TextMeshPro nextColourTimer;

    private void Awake() {
        if (instance != null) {
            throw new Exception("More than 1 Game Manager in scene");
        }

        instance = this;

        // Ui Text fields
        currentColourText = GameObject.Find("CurrentColourText").GetComponent<TextMeshPro>();
        nextColourTimer = GameObject.Find("NextColourTimer").GetComponent<TextMeshPro>();
    }

    // Need this to update UI
    void Update() {
        // Update UI
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
        return players[id];
    }

    public static ICollection<Player> GetPlayers() {
        return players.Values;
    }

    #endregion

}
