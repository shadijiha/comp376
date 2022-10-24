using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthGUI : MonoBehaviour
{
    public TMP_Text text;
    public Slider healthBar;
    private Player localPlayer;

    // How many players in the game
    // This variable is basically here so if there's any change in the match make sure
    // To update so you can load the localPlayer correctly
    private int playersCount;

    // Start is called before the first frame update
    void Start() {
        UpdateGUI();
        playersCount = GameManager.GetPlayers().Count;
    }

    // Update is called once per frame
    void Update() {

        // If the number of player has changed
        int tempCount = GameManager.GetPlayers().Count;
        if (playersCount != tempCount) {
            UpdateGUI();
            playersCount = tempCount;
        }

        if (text != null && localPlayer != null)
        {
            text.text = localPlayer.GetHealth() + "";
            healthBar.value = localPlayer.getHealth() / 100.0f;
        }
    }

    public void UpdateGUI() {
        // Display the health of the localPlayer
        foreach (Player p in GameManager.GetPlayers()) {
            if (p.isLocalPlayer) {
                localPlayer = p;
            }
        }
    }
}
