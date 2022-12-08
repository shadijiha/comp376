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
    void FixedUpdate()
    {
        if (GameManagerServer.IsGameOver())
        {
            playerUISetup.FreezePlayer(false);
            Cursor.lockState = CursorLockMode.None;

            var playerStats = GameManagerServer.GetPlayerStatsList();
            print(playerStats);
            var sortedPlayerStats = playerStats.OrderByDescending(x => x.kills).ToList();

            int maxKills = sortedPlayerStats[0].kills;

            foreach (var p in sortedPlayerStats)
            {
                var player = Instantiate(playerTemplatePrefab, playerList.transform);
                player.GetComponent<PlayerTemplate>().SetText(p.player.name, p.kills, p.death, maxKills == p.kills);
            }

            gameOverObj.SetActive(true);
            playerHeaderGUI.enabled = false;
            enabled = false;
        }
    }
}
