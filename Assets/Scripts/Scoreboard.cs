using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Scoreboard : NetworkBehaviour
{
    public Canvas ScoreboardCanvas;
    public Canvas PlayersCanvas;
    public Canvas PlayerTemplateCanvas;

    private int previousNumOfPlayers = 0;
    private ICollection<Player> players;
    private PlayerStatsList playerStatsList;
    private Dictionary<Player, Canvas> playerStatsCanvas = new Dictionary<Player, Canvas>();

    // Start is called before the first frame update
    void Start()
    {
        ScoreboardCanvas.enabled = false;
        PlayerTemplateCanvas.enabled = false;
        UpdateScoreboard();
    }

    public override void OnStartClient()
    {
        players = GameManager.GetPlayers();
        previousNumOfPlayers = players.Count;
        playerStatsList = GameManagerServer.GetPlayerStatsList();
        playerStatsList.Callback = PlayerStatsChanged;
    }

    /// Event callback when PlayerStatsList gets updated, and it updates the scoreboard.
    void PlayerStatsChanged(PlayerStatsList.Operation op, int index)
    {
        UpdateScoreboard();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousNumOfPlayers != players.Count)
        {
            // Reset the scoreboard whenever players are entering/leaving the game
            UpdateScoreboard();
            previousNumOfPlayers = players.Count;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            // Update the deaths/kills of the players
            UpdateScoreboard();
        }

        ScoreboardCanvas.enabled = Input.GetKey(KeyCode.Tab);
    }

    private void UpdateScoreboard()
    {
        UnloadPlayerCanvases();

        // Double loop to keep the player list order
        foreach (var p in players)
        {
            foreach (var playerStats in playerStatsList)
            {
                if (!(playerStats.player != null && playerStats.player == p))
                    continue;

                // Create the player text canvas
                Canvas playerScoreCanvas = Instantiate(PlayerTemplateCanvas, PlayersCanvas.transform);

                // Update name text
                var playerNameText = playerScoreCanvas.transform.Find("PlayerName").GetComponent<TMPro.TextMeshProUGUI>();
                playerNameText.text = playerStats.player.name;

                // Update kills and deaths
                var playerKill = playerScoreCanvas.transform.Find("Kills").GetComponent<TMPro.TextMeshProUGUI>();
                var playerDeath = playerScoreCanvas.transform.Find("Deaths").GetComponent<TMPro.TextMeshProUGUI>();
                playerKill.text = playerStats.kills.ToString();
                playerDeath.text = playerStats.death.ToString();

                // Show canvas
                playerScoreCanvas.enabled = true;

                playerStatsCanvas.Add(playerStats.player, playerScoreCanvas);

                // Stop searching for the player's stats
                break;
            }
        }

        PositionPlayerCanvases();
    }

    // Reset the player names from the scoreboard by destroying the canvas
    // whenever players leave the game or new players are entering.
    private void UnloadPlayerCanvases()
    {
        foreach (var canvas in playerStatsCanvas.Values)
        {
            Destroy(canvas.gameObject);
        }
        playerStatsCanvas = new Dictionary<Player, Canvas>();
    }

    // Helper method to position the player canvases in the scoreboard.
    private void PositionPlayerCanvases()
    {
        int i = 0; 
        foreach (var canvas in playerStatsCanvas.Values)
        {
            canvas.transform.position = PlayerTemplateCanvas.transform.position;

            // Place canvas 
            canvas.transform.position += Vector3.down * PlayerTemplateCanvas.GetComponent<RectTransform>().rect.height * i;
            i++;
        }
    }
}
