using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunGUI : MonoBehaviour
{
    [SerializeField] private Image gunIcon;
    [SerializeField] private List<Sprite> weaponIconsSprites;

    private WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponentInParent<PlayerUISetup>().weaponManager;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWeapon currentWeapon = weaponManager.GetCurrentWeapon();
        try
        {
            gunIcon.sprite = weaponIconsSprites[(int)currentWeapon.weaponType];
        }
        catch
        {
            Debug.Log($"Sprite Not Found for {currentWeapon.weaponType}");
        }
    }
}
