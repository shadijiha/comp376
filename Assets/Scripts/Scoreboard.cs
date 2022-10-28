using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Scoreboard : NetworkBehaviour
{
    public Canvas ScoreboardCanvas;
    public Canvas PlayersCanvas;
    public Canvas PlayerTemplateCanvas;

    private ICollection<Player> players;
    private PlayerStatsList playerStatsList;
    private Dictionary<Player, Canvas> playerStatsCanvas = new Dictionary<Player, Canvas>();

    // Start is called before the first frame update
    void Start()
    {
        ScoreboardCanvas.gameObject.SetActive(true);
        ScoreboardCanvas.enabled = false;
        players = GameManager.GetPlayers();
        playerStatsList = GameManagerServer.GetPlayerStatsList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            // Reset the scoreboard whenever players are entering/leaving the game
            if (playerStatsCanvas.Count != players.Count)
            {
                UnloadPlayerCanvases();
                LoadPlayerScoreboard();
                PositionPlayerCanvases();
            }
            // Update the deaths/kills of the players
            RefreshScoreboard();
        } 

        ScoreboardCanvas.enabled = Input.GetKey(KeyCode.Tab);
    }

    // Create the player canvases
    private void LoadPlayerScoreboard()
    {
        playerStatsCanvas = new Dictionary<Player, Canvas>();
        PlayerTemplateCanvas.enabled = true;
        // Create a canvas from the template for each current player
        foreach (var player in players)
        {
            var newPlayerCanvas = Instantiate(PlayerTemplateCanvas, PlayersCanvas.transform);
            var playerNameText = newPlayerCanvas.transform.Find("PlayerName").GetComponent<TMPro.TextMeshProUGUI>();
            playerNameText.text = player.name;

            playerStatsCanvas.Add(player, newPlayerCanvas);
        }

        PlayerTemplateCanvas.enabled = false;
    }

    // Update the kills/deaths of the players.
    private void RefreshScoreboard()
    {
        foreach (var playerStats in playerStatsList)
        {
            var currentPlayerStatsCanvas = playerStatsCanvas[playerStats.player];

            var playerKill = currentPlayerStatsCanvas.transform.Find("Kills").GetComponent<TMPro.TextMeshProUGUI>();
            var playerDeath = currentPlayerStatsCanvas.transform.Find("Deaths").GetComponent<TMPro.TextMeshProUGUI>();

            playerKill.text = playerStats.kills.ToString();
            playerDeath.text = playerStats.death.ToString();
        }
    }

    // Reset the player names from the scoreboard by destroying the canvas
    // whenever players leave the game or new players are entering.
    private void UnloadPlayerCanvases()
    {
        foreach (var canvas in playerStatsCanvas.Values)
        {
            Destroy(canvas.gameObject);
        }
    }

    // Helper method to position the player canvases in the scoreboard.
    private void PositionPlayerCanvases()
    {
        int i = 0; 
        foreach (var canvas in playerStatsCanvas.Values)
        {
            // Place canvas 
            canvas.transform.position += Vector3.down * PlayerTemplateCanvas.GetComponent<RectTransform>().rect.height * i;
            i++;
        }
    }
}
