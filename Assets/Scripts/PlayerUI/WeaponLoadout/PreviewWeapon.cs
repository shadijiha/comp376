using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerWeapon;
using TMProText = TMPro.TextMeshProUGUI;

public class PreviewWeapon : MonoBehaviour
{
    [SerializeField] private TMProText nameText;
    [SerializeField] private TMProText description;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] weaponSprites = new Sprite[18];
    private GameObject[] weaponModels = new GameObject[18];

    public WeaponType currentWeaponType;

    public void UpdateGunPreview(PlayerWeapon playerWeapon)
    {
        currentWeaponType = playerWeapon.weaponType;
        nameText.text = playerWeapon.name;
        description.text = playerWeapon.description;

        if (weaponModels[(int)currentWeaponType] == null)
        {
            image.sprite = weaponSprites[(int)currentWeaponType];
        }
    }

}
