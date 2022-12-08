using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameOverUI : NetworkBehaviour
{
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject playerList;
    [SerializeField] private GameObject playerTemplatePrefab;

    private PlayerHeaderGUI playerHeaderGUI;
    private PlayerUISetup playerUISetup;

    // Start is called before the first frame update
    void Start()
    {
        playerUISetup = GetComponent<PlayerUISetup>();
        playerHeaderGUI = GetComponent<PlayerHeaderGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerServer.IsGameOver())
        {
            playerUISetup.FreezePlayer(false);
            Cursor.lockState = CursorLockMode.None;

            var playerStats = GameManagerServer.GetPlayerStatsList();
            var sortedPlayerStats = playerStats.OrderByDescending(x => x.kills).ToList();

            int maxKills = sortedPlayerStats[0].kills;
            var stats = new Dictionary<string, PlayerStats>();

            foreach (var p in sortedPlayerStats)
            {
                // BUG: Somehow, there are duplicate PlayerStats in playerStats to players other than the host.
                // HACK: A dictionary is used to keep track of player stats in order to skip the duplicates.
                if (!stats.TryGetValue(p.name, out PlayerStats stat))
                {
                    var player = Instantiate(playerTemplatePrefab, playerList.transform);
                    player.GetComponent<PlayerTemplate>().SetText(p.name, p.kills, p.death, maxKills == p.kills);
                    stats[p.name] = stat;
                }
            }

            gameOverObj.SetActive(true);
            playerHeaderGUI.enabled = false;
            enabled = false;
        }
    }
}
