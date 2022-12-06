using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class EscapeMenuGUI : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenuPanel;

    private bool menuEnabled;
    private PlayerUISetup playerUISetup;

    private NetworkManager manager;

    // Start is called before the first frame update
    void Start()
    {
        menuEnabled = false;
        escapeMenuPanel.SetActive(menuEnabled);
        playerUISetup = GetComponentInParent<PlayerUISetup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuEnabled = !menuEnabled;
            playerUISetup.FreezePlayer(!menuEnabled);
            escapeMenuPanel.SetActive(menuEnabled);
        }
    }

    public void Resume()
    {
        menuEnabled = false;
        escapeMenuPanel.SetActive(menuEnabled);
        playerUISetup.FreezePlayer(true);
    }

    public void Quit()
    {
        Cursor.lockState = CursorLockMode.None;

        // This function is suppose to migrate the host in case the host has disconnected
        manager.migrationManager.hostMigration = true;

        // Check if the current player is host
        /*if (manager.isNetworkActive) {            
            manager.migrationManager.FindNewHost(out var newHostInfo, out var isNewHost);
            manager.migrationManager.newHostAddress = newHostInfo.address;

            if (isNewHost) {
                manager.migrationManager.BecomeNewHost(manager.networkPort);
            }

            // Get the list of clients in the game
            var clients = NetworkClient.allClients;

            // Iterate through the list of clients
            foreach (var client in clients)
            {
                // Reconnect the client to the new host
                client.ReconnectToNewHost(newHostInfo.address, newHostInfo.port);
            }
        }*/

        var match = manager.matchInfo;
        manager.matchMaker.DropConnection(match.networkId, match.nodeId, HostGame.RequestDomain, manager.OnDropConnection);
        manager.StopHost();
        //manager.StopClient();
        
        //Application.Quit();
    }
}
