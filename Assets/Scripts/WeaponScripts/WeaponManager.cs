using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    public const string WEAPON_LAYER = "Weapon";

                        public  static  GameObject[]    msWeaponArr         = new GameObject[18];
    [SerializeField]    public          PlayerWeapon    mPrimary;
    [SerializeField]    public          PlayerWeapon    mSecondary;
    [SerializeField]    private         PlayerWeapon    mSuper;
    [SerializeField]    private         Transform       weaponHolder;
    [SerializeField]    private         GameObject[]    mWeaponArr          = new GameObject[18];
    [SerializeField]    private         CameraRecoil    mCameraRecoil;
    [SerializeField]    private         ModelRecoil     mModelRecoil;
                        private         PlayerWeapon    mCurrent;
                        private         PlayerWeapon    mNextWeapon;
                        private         WeaponGraphics  mCurrentGraphics;
                        private         bool            mAmplified          = false;
                        private         bool            mHasted             = false;
                        private         float           mAmplifyTime        = 0f;
                        private         float           mHasteTime          = 0f;
                        private         float           AMPLIFY_DURATION    = 5f;
                        private         float           HASTE_DURATION      = 5f;

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
        mPrimary = new ShotRifle
        {
            readyToShoot = true
        };

        Equip(mPrimary);
        mCurrent = mPrimary;
        mCameraRecoil.UpdateRecoilInfo(mCurrent.cameraRecoilInfo);
        mModelRecoil.UpdateRecoilInfo(mCurrent.modelRecoilInfo);
        mCurrent.model.SetActive(true);

        // Create Sidearm
        mSecondary = new BurstPistol
        {
            readyToShoot = false
        };
        Equip(mSecondary);

        mSuper      = new RocketLauncher
        {
            readyToShoot = false
        };
        Equip(mSuper);
    }

    // Update is called once per frame
    void Update()
    {
        // Weapon Switching
        if (Input.GetButtonDown("Primary") && !mCurrent.altFire && mPrimary != mCurrent && mPrimary != null)
        { 
            mNextWeapon = mPrimary;
            mCurrent.reloading = false;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
        if (Input.GetButtonDown("Secondary") && !mCurrent.altFire && mSecondary != mCurrent && mSecondary != null)
        {
            mNextWeapon = mSecondary;
            mCurrent.reloading = false;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
        if (Input.GetButtonDown("Super") && !mCurrent.altFire && mSuper != mCurrent && mSuper != null)
        {
            mNextWeapon = mSuper;
            mCurrent.reloading = false;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }

        if (mAmplified)
        {
            mAmplifyTime    += Time.deltaTime;

            if (mAmplifyTime > AMPLIFY_DURATION)
            {
                mAmplifyTime = 0f;

                mAmplified = false;

                mPrimary.amplified      = false;
                mSecondary.amplified    = false;

                if (mSuper != null)
                {
                    mSuper.amplified    = false;
                }
            }
        }

        if (mHasted)
        {
            mHasteTime      += Time.deltaTime;

            if (mHasteTime > HASTE_DURATION)
            {
                mHasteTime = 0f;

                mHasted = false;

                mPrimary.hasted             = false;
                mPrimary.speedMultiplier    = 1.5f; 
                mSecondary.hasted           = false;
                mSecondary.speedMultiplier  = 1.5f;

                if (mSuper != null)
                {
                    mSuper.hasted           = false;
                    mSuper.speedMultiplier  = 1.5f;
                }
            }
        }
    }

    public PlayerWeapon GetCurrentWeapon() {
        return mCurrent;
    }

    public WeaponGraphics GetCurrentWeaponGraphics() {
        return mCurrentGraphics;
    }

    public ModelRecoil GetModelRecoil() {
        return mWeaponArr[(int)mCurrent.weaponType].GetComponent<ModelRecoil>();
    }

    private void Equip(PlayerWeapon weapon)
    {
        //weaponHolder.position, weaponHolder.rotation
        weapon.model = (GameObject)Instantiate(
            msWeaponArr[(int)weapon.weaponType]);
        weapon.model.transform.SetParent(weaponHolder);
        Vector3 holdPosition = weapon.model.GetComponentInChildren<HoldPt>().gameObject.transform.position;
        //weapon.model.GetComponentInChildren<HoldPt>().gameObject.transform.position = weaponHolder.transform.position;
        weapon.model.transform.position = weaponHolder.transform.position - holdPosition;
        //weapon.model.transform.position = weaponHolder.transform.position;
        //weapon.model.GetComponentInChildren<HoldPoint>().gameObject.transform.position

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
        mCameraRecoil.UpdateRecoilInfo(mCurrent.cameraRecoilInfo);
        mModelRecoil.UpdateRecoilInfo(mCurrent.modelRecoilInfo);

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

    public void Respawn()
    {
        mPrimary.Reset();
        mSecondary.Reset();
        Destroy(mSuper.model);
        mSuper                                                      = null;
        mSecondary.model.SetActive(false);
        mCurrent                                                    = mPrimary;
        mCurrent.model.SetActive(true);
        mCameraRecoil.UpdateRecoilInfo(mCurrent.cameraRecoilInfo);
        mModelRecoil.UpdateRecoilInfo(mCurrent.modelRecoilInfo);
        mCurrent.readyToShoot                                       = true;
    }

    public void AmplifyDamage()
    {
        if (mAmplified)
        {
            mAmplifyTime = 0f;
        }
        else
        {
            mPrimary.amplified      = true;
            mSecondary.amplified    = true;
            if (mSuper != null)
            {
                mSuper.amplified    = true;
            }
            mAmplified = true;
        }
    }

    public void Haste()
    {
        if (mHasted)
        {
            mHasteTime = 0f;
        }
        else
        {
            mPrimary.hasted             = true;
            mPrimary.speedMultiplier    = 1.5f;
            mSecondary.hasted           = true;
            mSecondary.speedMultiplier  = 1.5f;

            if (mSuper != null)
            {
                mSuper.hasted           = true;
                mSuper.speedMultiplier  = 1.5f;
            }

            mHasted = true;
        }
    }
}
