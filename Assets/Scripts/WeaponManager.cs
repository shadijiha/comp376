using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    public const string WEAPON_LAYER = "Weapon";

    [SerializeField]    private         PlayerWeapon    mPrimary;
    [SerializeField]    private         PlayerWeapon    mSecondary;
    [SerializeField]    private         PlayerWeapon    mSuper;
    [SerializeField]    private         Transform       weaponHolder;
    [SerializeField]    private         GameObject[]    mWeaponArr      = new GameObject[10];
                        public  static  GameObject[]    msWeaponArr     = new GameObject[10];
                        private         PlayerWeapon    mCurrent;
                        private         WeaponGraphics  mCurrentGraphics;

    private void Start()
    {
        for (int i = 0; i < mWeaponArr.Length; ++i)
        {
            if (mWeaponArr[i] != null && msWeaponArr[i] == null)
            {
                msWeaponArr[i] = mWeaponArr[i];
            }
        }

        // Equip the primary weapon
        mPrimary = new SMG();
        Equip(mPrimary);

        // Create Sidearm
        mSecondary = new Pistol();
    }

    // Awake is called before Start, allowing this to equip the weapon before Start() in PlayerShoot requires it.
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Weapon Switching
        if (Input.GetButtonDown("Primary") && mPrimary != mCurrent && mPrimary != null)
        {
            Equip(mPrimary);
        }
        if (Input.GetButtonDown("Secondary") && mSecondary != mCurrent && mSecondary != null)
        {
            Equip(mSecondary);
        }
        if (Input.GetButtonDown("Super") && mSuper != mCurrent && mSuper != null)
        {
            Equip(mSuper);
        }
    }

    public PlayerWeapon GetCurrentWeapon() {
        return mCurrent;
    }

    public WeaponGraphics GetCurrentWeaponGraphics() {
        return mCurrentGraphics;
    }

    private void Equip(PlayerWeapon weapon) {
        this.mCurrent = weapon;

        GameObject weaponInstance = (GameObject)Instantiate(weapon.model, weaponHolder.position, weaponHolder.rotation);
        weaponInstance.transform.SetParent(weaponHolder);

        mCurrentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
        if (mCurrentGraphics == null)
            throw new Exception("No weapon graphics component on " + weaponInstance.name);

        if (isLocalPlayer) {
            Util.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(WEAPON_LAYER));
        }
    }
}
