using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPText = TMPro.TMP_Text;

public class PlayerGunGUI : MonoBehaviour
{
    [SerializeField] private Image gunIcon;
    [SerializeField] private List<Sprite> weaponIconsSprites;

    private WeaponManager weaponManager;
    private Dictionary<string, Sprite> weaponSprites;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponentInParent<PlayerUISetup>().weaponManager;

        // Dictionary for easy access
        weaponSprites = new Dictionary<string, Sprite>();
        foreach (var weapon in weaponIconsSprites)
            weaponSprites.Add(weapon.name, weapon);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWeapon currentWeapon = weaponManager.GetCurrentWeapon();
        gunIcon.sprite = weaponSprites[currentWeapon.GetType().Name];
    }
}
