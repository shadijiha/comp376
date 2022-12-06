using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUISetup : MonoBehaviour
{
    public Player localPlayer { get; private set; }
    public WeaponManager weaponManager { get; private set; }

    [SerializeField] private WeaponLoadout weaponLoadout;

    private bool waitUntilPlayerRespawn;

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = GameManager.GetLocalPlayer();
        weaponManager = localPlayer.GetComponent<WeaponManager>();
    }

    void Update()
    {
        if (!waitUntilPlayerRespawn && localPlayer.IsDead()) 
        {
            waitUntilPlayerRespawn = true;
        }
        else if (waitUntilPlayerRespawn && !localPlayer.IsDead())
        {
            weaponLoadout.gameObject.SetActive(true);
            waitUntilPlayerRespawn = false;
        }
    }

    public void FreezePlayer(bool enabled)
    {
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;

        localPlayer.GetComponent<PlayerMotor>().enabled = enabled;
        localPlayer.GetComponent<PlayerControler>().enabled = enabled;
        localPlayer.GetComponent<PlayerShoot>().enabled = enabled;
    }
}
