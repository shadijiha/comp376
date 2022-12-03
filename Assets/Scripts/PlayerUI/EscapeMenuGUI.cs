using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class EscapeMenuGUI : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenuPanel;

    private bool menuEnabled;
    private PlayerControler playerController;
    private PlayerShoot playerShoot;

    private NetworkManager manager;

    // Start is called before the first frame update
    void Start()
    {
        menuEnabled = false;
        escapeMenuPanel.SetActive(menuEnabled);

        Player localPlayer = GetComponentInParent<PlayerUISetup>().localPlayer;
        playerController = localPlayer.GetComponent<PlayerControler>();
        playerShoot = localPlayer.GetComponent<PlayerShoot>();

        manager = NetworkManager.singleton;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            menuEnabled = !menuEnabled;

        Cursor.lockState = menuEnabled ? CursorLockMode.None : CursorLockMode.Locked;

        playerController.EnableMovement(!menuEnabled);
        playerShoot.enabled = !menuEnabled;
        escapeMenuPanel.SetActive(menuEnabled);
    }

    public void Resume()
    {
        menuEnabled = false;
    }

    public void Quit()
    {
        var match = manager.matchInfo;
        manager.matchMaker.DropConnection(match.networkId, match.nodeId, HostGame.RequestDomain, manager.OnDropConnection);
        manager.StopHost(); // TODO implement Migration

        Application.Quit();
    }
}
