using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;
using TMPro;
public class RoomListElementDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameUI;

    private MatchInfoSnapshot matchInfo;
    private Action<MatchInfoSnapshot> onJoin;

    // Start is called before the first frame update
    public void Setup(MatchInfoSnapshot info, Action<MatchInfoSnapshot> then)
    {
        onJoin = then;
        matchInfo = info;
        roomNameUI.text = $"{matchInfo.name} ({matchInfo.currentSize}/{matchInfo.maxSize})";
    }

    public void JoinMatch() {
        onJoin(matchInfo);
    }
}
