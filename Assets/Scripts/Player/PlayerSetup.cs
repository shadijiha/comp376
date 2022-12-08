using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Behaviour[] componentsToDisable;
    private string remoteLayerName = "RemotePlayerLayer";
    private Camera sceneCamera;

    // These are here because we only want to show the crosshair and HP when we are in game
    [SerializeField] private GameObject playerUIPrefab;
    public GameObject playerUIInstance;

    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer(transform);
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

        // Create Player UI for local player only
        if (isLocalPlayer)
        {
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }
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

    void AssignRemoteLayer(Transform root) {
        root.gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        foreach(Transform child in root)
        {
            AssignRemoteLayer(child);
        }
            
        //gameObject.layer = LayerMask.NameToLayer(remoteLayerName);

        //playerModel.layer = LayerMask.NameToLayer(remoteLayerName);
        //foreach (Transform child in playerModel.transform)
        //{
        //    GameObject obj = child.gameObject;
        //    obj.layer = LayerMask.NameToLayer(remoteLayerName);
        //}
    }

    //recursive calls
    void MoveToLayer(Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach(Transform child in root)
            MoveToLayer(child, layer);
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