using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class HostGame : MonoBehaviour
{
    public const int RequestDomain = 0; // Need to be same for Create and join game
    public const string publicClientAddress = "";
    public const string privateClientAddress = "";

    [SerializeField]    private uint roomSize = 10;
    
    private string roomName;
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker is null)
            networkManager.StartMatchMaker();

        roomName = "Room" + Random.Range(0, 10000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom() {
        if (roomName != "" && roomName != null) {
            Debug.Log("Creating room: " + roomName);

            networkManager.matchMaker.CreateMatch(roomName, roomSize, true,
                "",
                publicClientAddress,
                privateClientAddress,
                0, RequestDomain, networkManager.OnMatchCreate);
        }
    }

    public void SetRoomName(string name) { 
        roomName = name;
    }
}
