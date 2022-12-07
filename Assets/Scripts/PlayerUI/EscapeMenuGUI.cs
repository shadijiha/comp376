using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EscapeMenuGUI : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject weaponLoadoutButton;

    [SerializeField] private GameObject weaponLoadoutObj;

    private bool menuEnabled;
    private PlayerUISetup playerUISetup;
    private WeaponLoadout weaponLoadout;

    private NetworkManager manager;

    // Start is called before the first frame update
    void Start()
    {
        menuEnabled = false;
        escapeMenuPanel.SetActive(menuEnabled);
        playerUISetup = GetComponentInParent<PlayerUISetup>();
        weaponLoadout = weaponLoadoutObj.GetComponent<WeaponLoadout>();
        print("EL: " + weaponLoadout);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            weaponLoadoutButton.GetComponent<Button>().enabled = !weaponLoadout.firstTime;

            menuEnabled = !menuEnabled;
            playerUISetup.FreezePlayer(!menuEnabled);
            escapeMenuPanel.SetActive(menuEnabled);
        }
    }

    public void Resume()
    {
        CloseMenu();
        playerUISetup.FreezePlayer(true);
    }

    public void Loadout()
    {
        CloseMenu();
        playerUISetup.FreezePlayer(false);
        weaponLoadoutObj.SetActive(true);
    }

    public void CloseMenu()
    {
        menuEnabled = false;
        escapeMenuPanel.SetActive(menuEnabled);
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
