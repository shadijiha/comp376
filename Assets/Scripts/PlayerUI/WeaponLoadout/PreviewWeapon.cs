using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerWeapon;
using TMProText = TMPro.TextMeshProUGUI;

public class PreviewWeapon : MonoBehaviour
{
    [SerializeField] private TMProText nameText;
    [SerializeField] private TMProText description;
    [SerializeField] private GameObject gunModelParent;

    private GameObject[] weaponModels = new GameObject[18];
    public WeaponType currentWeaponType;
    private WeaponType previousWeaponType;

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
            weaponModels[(int)currentWeaponType] = Instantiate(WeaponManager.msWeaponArr[(int)currentWeaponType], gunModelParent.transform);
            weaponModels[(int)currentWeaponType].transform.localScale = new Vector3(1000, 1000, 1000);
            weaponModels[(int)currentWeaponType].transform.rotation = Quaternion.Euler(-90, 90, 0);
        }

        weaponModels[(int)currentWeaponType].SetActive(true);

        if (weaponModels[(int)previousWeaponType] != null)
            weaponModels[(int)previousWeaponType].SetActive(false);

        previousWeaponType = currentWeaponType;
    }

}
