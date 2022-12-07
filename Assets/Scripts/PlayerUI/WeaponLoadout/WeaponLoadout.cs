using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class WeaponLoadout : MonoBehaviour
{
    [SerializeField] private GameObject weaponButtonPrefab;
    [SerializeField] private GameObject primaryWeaponList;
    [SerializeField] private GameObject secondaryWeaponList;

    [SerializeField] private PreviewWeapon previewWeapon;

    [HideInInspector]
    public bool firstTime = true;

    private List<PlayerWeapon> primaryWeapons = new List<PlayerWeapon>()
    {
        new LightSMG(),
        new BurstSMG(),
        new HeavySMG(),
        new TacticalShotgun(),
        new AutoShotgun(),
        new Scattergun(),
        new AutoRifle(),
        new BurstRifle(),
        new DoubleRifle(),
        new HeavySniper(),
        new ScoutRifle(),
        new ShotRifle(),
    };

    private List<PlayerWeapon> secondaryWeapons = new List<PlayerWeapon>()
    {
        new MarksmanPistol(),
        new BurstPistol(),
        new MachinePistol(),
    };

    private List<GameObject> primaryWeaponButtons = new List<GameObject>();
    private List<GameObject> secondaryWeaponButtons = new List<GameObject>();

    private WeaponButton selectedPrimaryWeapon;
    private WeaponButton selectedSecondaryWeapon;

    private PlayerUISetup playerUISetup;
    private WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        playerUISetup = GetComponentInParent<PlayerUISetup>();

        foreach (var playerWeapon in primaryWeapons)
        {
            var button = Instantiate(weaponButtonPrefab, primaryWeaponList.transform);
            WeaponButton weaponButton = button.GetComponent<WeaponButton>();
            weaponButton.playerWeapon = playerWeapon;
            weaponButton.isPrimaryWeapon = true;
            weaponButton.weaponLoadout = this;
            primaryWeaponButtons.Add(button);
        }

        foreach (var playerWeapon in secondaryWeapons)
        {
            var button = Instantiate(weaponButtonPrefab, secondaryWeaponList.transform);
            WeaponButton weaponButton = button.GetComponent<WeaponButton>();
            weaponButton.playerWeapon = playerWeapon;
            weaponButton.weaponLoadout = this;
            secondaryWeaponButtons.Add(button);
        }

        selectedPrimaryWeapon = primaryWeaponButtons[0].GetComponent<WeaponButton>();
        selectedSecondaryWeapon = secondaryWeaponButtons[0].GetComponent<WeaponButton>();

        //selectedPrimaryWeapon.SelectWeapon();
        //selectedSecondaryWeapon.SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponManager == null)
        {
            weaponManager = GetComponentInParent<PlayerUISetup>().weaponManager;
            weaponManager.SwitchWeaponsFromLoadout(selectedPrimaryWeapon.playerWeapon, selectedSecondaryWeapon.playerWeapon);
            ShowPreview(selectedPrimaryWeapon.playerWeapon);
        }

        playerUISetup.FreezePlayer(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void SelectWeapon(WeaponButton weaponButton)
    {
        if (weaponButton != selectedPrimaryWeapon && weaponButton.isPrimaryWeapon)
        {
            selectedPrimaryWeapon.ResetSelection();
            selectedPrimaryWeapon = weaponButton;
        }
        else if (weaponButton != selectedSecondaryWeapon && !weaponButton.isPrimaryWeapon)
        {
            selectedSecondaryWeapon.ResetSelection();
            selectedSecondaryWeapon = weaponButton;
        }

        if (weaponManager != null)
        {
            weaponManager.SwitchWeaponsFromLoadout(selectedPrimaryWeapon.playerWeapon, selectedSecondaryWeapon.playerWeapon);
        }
    }

    public void ShowPreview(PlayerWeapon playerWeapon)
    {
        previewWeapon.UpdateGunPreview(playerWeapon);
    }

    public void PlayGame()
    {
        firstTime = false;
        playerUISetup.FreezePlayer(true);
        gameObject.SetActive(false);
    }
}
