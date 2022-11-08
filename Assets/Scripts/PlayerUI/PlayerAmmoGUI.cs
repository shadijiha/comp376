using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPText = TMPro.TMP_Text; 

public class PlayerAmmoGUI : MonoBehaviour
{
    [Header("Current Ammo")]
    [SerializeField] private TMPText currentAmmo;
    [SerializeField] private TMPText magazineSize;
    [Header("Current Spare")]
    [SerializeField] private TMPText currentSpare;
    [SerializeField] private TMPText maxSpare;

    [SerializeField] private TMPText ammoText;
    [SerializeField] private TMPText spareText;

    private WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        Player localPlayer = GetComponentInParent<PlayerUISetup>().localPlayer;
        weaponManager = localPlayer.GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWeapon weapon = weaponManager.GetCurrentWeapon();
        ammoText.text = $"{weapon.currentLoadedAmmo}/{weapon.magazineSize}";

        UpdateSpareTexts(weapon);
    }

    private void UpdateSpareTexts(PlayerWeapon weapon) 
    {
        spareText.text = weapon.maxAmmo > -1 ? $"{weapon.currentSpareAmmo}/{weapon.maxAmmo}" : "∞";
    }
}
