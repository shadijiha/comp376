using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    public const string WEAPON_LAYER = "Weapon";

                        public  static  GameObject[]    msWeaponArr     = new GameObject[10];
    [SerializeField]    public          PlayerWeapon    mPrimary;
    [SerializeField]    public          PlayerWeapon    mSecondary;
    [SerializeField]    private         PlayerWeapon    mSuper;
    [SerializeField]    private         Transform       weaponHolder;
    [SerializeField]    private         GameObject[]    mWeaponArr      = new GameObject[10];
    [SerializeField]    private         CameraRecoil    mCameraRecoil;
  //[SerializeField]    private         ModelRecoil     mModelRecoil;
                        private         PlayerWeapon    mCurrent;
                        private         PlayerWeapon    mNextWeapon;
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

        // Equip Each weapon.
        mPrimary = new SMG
        {
            readyToShoot = true
        };

        Equip(mPrimary);
        mCurrent = mPrimary;
        mCameraRecoil.UpdateRotationInfo(mCurrent.cameraRecoilInfo);
        mCurrent.model.SetActive(true);

        // Create Sidearm
        mSecondary = new Pistol
        {
            readyToShoot = false
        };
        Equip(mSecondary);
    }

    // Update is called once per frame
    void Update()
    {
        // Weapon Switching
        if (Input.GetButtonDown("Primary") && mPrimary != mCurrent && mPrimary != null)
        { 
            mNextWeapon = mPrimary;
            mCurrent.reloading = false;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
        if (Input.GetButtonDown("Secondary") && mSecondary != mCurrent && mSecondary != null)
        {
            mNextWeapon = mSecondary;
            mCurrent.reloading = false;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
        if (Input.GetButtonDown("Super") && mSuper != mCurrent && mSuper != null)
        {
            mNextWeapon = mSuper;
            mCurrent.reloading = false;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
    }

    public PlayerWeapon GetCurrentWeapon() {
        return mCurrent;
    }

    public WeaponGraphics GetCurrentWeaponGraphics() {
        return mCurrentGraphics;
    }

    private void Equip(PlayerWeapon weapon)
    {
        //weaponHolder.position, weaponHolder.rotation
        weapon.model = (GameObject)Instantiate(
            msWeaponArr[(int)weapon.weaponType]);
        weapon.model.transform.position = weaponHolder.transform.position;
        weapon.model.transform.SetParent(weaponHolder);

        mCurrentGraphics = weapon.model.GetComponentInChildren<WeaponGraphics>();
        if (mCurrentGraphics == null)
        {
            throw new Exception("No weapon graphics component on " + weapon.weaponType);
        }

        if (isLocalPlayer) {
            Util.SetLayerRecursively(weapon.model, LayerMask.NameToLayer(WEAPON_LAYER));
        }

        // Hide model initially.
         weapon.model.SetActive(false);
    }

    private void Stow()
    {
        mCurrent.readyToShoot = false;
        Invoke(nameof(Draw), this.mCurrent.stowTime);

        // Todo: Animate Stowing here
        //
        //
    }

    private void Draw() 
    {
        // Hide the model
        mCurrent.model.SetActive(false);

        // Switch active weapon
        mCurrent = mNextWeapon;
        mCurrent.model.SetActive(true);
        mCameraRecoil.UpdateRotationInfo(mCurrent.cameraRecoilInfo);
        //mCurrent.model.transform.position = weaponHolder.transform.position;

        // Todo: Trigger drawing animation here.
        //
        //

        // Allow player to shoot after the drawing completes (alternatively can invoke via an animation event).
        Invoke(nameof(ReadyToShoot), mCurrent.drawTime);
    }

    private void ReadyToShoot()
    {
        mCurrent.readyToShoot = true;
    }
}
