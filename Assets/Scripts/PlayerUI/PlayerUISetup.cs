using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUISetup : MonoBehaviour
{
    public Player localPlayer { get; private set; }
    public WeaponManager weaponManager { get; private set; }
    public PlayerControler playerController { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = GameManager.GetLocalPlayer();
        weaponManager = localPlayer.GetComponent<WeaponManager>();
        playerController = localPlayer.GetComponent<PlayerControler>();
    }

    public void FreezePlayer(bool enabled)
    {
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;

        localPlayer.GetComponent<PlayerMotor>().enabled = enabled;
        localPlayer.GetComponent<PlayerControler>().enabled = enabled;
        localPlayer.GetComponent<PlayerShoot>().enabled = enabled;
    }
}
