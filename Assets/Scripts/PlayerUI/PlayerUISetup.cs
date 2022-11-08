using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUISetup : MonoBehaviour
{
    public Player localPlayer { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        localPlayer = GameManager.GetLocalPlayer();
    }
}
