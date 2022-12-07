using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUISetup : MonoBehaviour
{
    public Player localPlayer { get; private set; }
    public WeaponManager weaponManager { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = GameManager.GetLocalPlayer();
        weaponManager = localPlayer.GetComponent<WeaponManager>();
    }

    public void FreezePlayer(bool enabled)
    {
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;

        print("FreezePlayer: " + localPlayer.name);

        localPlayer.GetComponent<PlayerMotor>().enabled = enabled;
        localPlayer.GetComponent<PlayerControler>().enabled = enabled;
        localPlayer.GetComponent<PlayerShoot>().enabled = enabled;
    }
}
