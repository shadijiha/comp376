using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;
    private string remoteLayerName = "RemotePlayerLayer";
    private Camera sceneCamera;

    // These are here because we only want to show the crosshair and HP when we are in game
    [SerializeField] private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        }
        else {
            sceneCamera = Camera.main;

            // Disable The global scene camera on join
            if (sceneCamera != null) {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        // Setup the player
        GetComponent<Player>().Setup();

        // Create Player UI
        playerUIInstance = Instantiate(playerUIPrefab);
        playerUIInstance.name = playerUIPrefab.name;
    }

    public override void OnStartClient() {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netID, player);
    }

    void DisableComponents() {
        foreach (Behaviour temp in componentsToDisable) {
            temp.enabled = false;
        }
    }

    void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void OnDisable() {
        // Destroy Player UI
        Destroy(playerUIInstance);


        // Enable the scene camera when player object get destroyed
        if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}