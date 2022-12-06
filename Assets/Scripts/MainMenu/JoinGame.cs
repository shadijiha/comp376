using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using TMPro;

public class JoinGame : MonoBehaviour
{
    private List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;

    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private GameObject matchListElementPrefab;
    [SerializeField] private Transform roomListScrollView;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker is null)
            networkManager.StartMatchMaker();

        RefreshMatchList();
    }

    public void RefreshMatchList()
    {
        ClearMatchList();
        status.enabled = true;
        status.text = "Loading...";
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, HostGame.RequestDomain, OnMatchList);        
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        if (!success || responseData is null) {
            status.text = "Failed to fetch matches!";
            Debug.LogWarning("Failed to load matches " + extendedInfo);
            return;
        }

        status.enabled = false;
        

        foreach (MatchInfoSnapshot match in responseData) {
            var roomListItem = Instantiate(matchListElementPrefab);
            roomListItem.transform.SetParent(roomListScrollView);

            var display = roomListItem.GetComponent<RoomListElementDisplay>();
            if (display != null)
            {
                display.Setup(match, e =>
                {
                    ClearMatchList();
                    status.enabled = true;
                    status.text = "Joining...";

                    networkManager.matchMaker.JoinMatch(e.networkId, "",
                        HostGame.publicClientAddress,
                        HostGame.privateClientAddress,
                        0,
                        HostGame.RequestDomain, networkManager.OnMatchJoined);
                });
            }

            roomList.Add(roomListItem);
        }

        if (responseData.Count == 0) {
            status.enabled = true;
            status.text = "No rooms to display!";
        }
    }

    private void ClearMatchList() {
        for (int i = 0; i < roomList.Count; i++) {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }
}
