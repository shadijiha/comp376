using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;

public class PlayerTemplate : MonoBehaviour
{
    [SerializeField] private TMProText playerName;
    [SerializeField] private TMProText kills;
    [SerializeField] private TMProText deaths;
    [SerializeField] private TMProText winner;

    public void SetText(string playerName, int kills, int deaths, bool isWinner)
    {
        this.playerName.text = playerName;
        this.kills.text = $"{kills}";
        this.deaths.text = $"{deaths}";
        this.winner.gameObject.SetActive(isWinner);
    }
}
