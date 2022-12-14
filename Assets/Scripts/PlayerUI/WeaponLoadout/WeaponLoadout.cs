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
    [SerializeField] private GameObject playButton;

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

    private bool isPrimaryWeaponSelected = false;
    private bool isSecondaryWeaponSelected = false;

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

        playButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponManager == null)
        {
            weaponManager = GetComponentInParent<PlayerUISetup>().weaponManager;
        }

        playerUISetup.FreezePlayer(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void SelectWeapon(WeaponButton weaponButton)
    {
        if (weaponButton.isPrimaryWeapon && weaponButton != selectedPrimaryWeapon)
        {
            if (selectedPrimaryWeapon != null)
                selectedPrimaryWeapon.ResetSelection();

            selectedPrimaryWeapon = weaponButton;
            isPrimaryWeaponSelected = true;
        }
        else if (!weaponButton.isPrimaryWeapon && weaponButton != selectedSecondaryWeapon)
        {
            if (selectedSecondaryWeapon != null)
                selectedSecondaryWeapon.ResetSelection();

            selectedSecondaryWeapon = weaponButton;
            isSecondaryWeaponSelected = true;
        }

        if (weaponManager != null && selectedPrimaryWeapon != null && selectedSecondaryWeapon != null)
        {
            weaponManager.SwitchWeaponsFromLoadout(selectedPrimaryWeapon.playerWeapon, selectedSecondaryWeapon.playerWeapon);
        }

        playButton.SetActive(isPrimaryWeaponSelected && isSecondaryWeaponSelected);
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
