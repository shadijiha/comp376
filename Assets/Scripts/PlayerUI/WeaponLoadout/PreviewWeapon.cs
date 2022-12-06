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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateGunPreview(PlayerWeapon playerWeapon)
    {
        currentWeaponType = playerWeapon.weaponType;
        nameText.text = playerWeapon.name;

        if (weaponModels[(int)currentWeaponType] == null)
        {
            image.sprite = weaponSprites[(int)currentWeaponType];
        }
    }

}
