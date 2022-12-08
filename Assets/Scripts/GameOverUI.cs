using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameOverUI : NetworkBehaviour
{
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private GameObject playerList;
    [SerializeField] private GameObject playerTemplatePrefab;

    private bool isGameOver = false;
    private NetworkManager manager;

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas = GetComponent<Canvas>();
        gameOverCanvas.enabled = false;
        manager = NetworkManager.singleton;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isGameOver && GameManagerServer.IsGameOver())
        {
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;

            var playerStats = GameManagerServer.GetPlayerStatsList();
            var sortedPlayerStats = playerStats.OrderByDescending(x => x.kills).ToList();

            int maxKills = sortedPlayerStats[0].kills;

            foreach (var p in sortedPlayerStats)
            {
                var player = Instantiate(playerTemplatePrefab, playerList.transform);
                player.GetComponent<PlayerTemplate>().SetText(p.player.name, p.kills, p.death, maxKills == p.kills);
            }

            gameOverCanvas.enabled = true;
            isGameOver = true;
        }
    }

    public void Quit()
    {
        Cursor.lockState = CursorLockMode.None;

        var match = manager.matchInfo;
        manager.matchMaker.DropConnection(match.networkId, match.nodeId, HostGame.RequestDomain, manager.OnDropConnection);
        manager.StopHost();
    }
}
