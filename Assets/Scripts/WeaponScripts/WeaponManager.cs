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
    [SerializeField]    public          PlayerWeapon    mSuper;
    [SerializeField]    private         Transform       weaponHolder;
    [SerializeField]    private         GameObject[]    mWeaponArr          = new GameObject[18];
    [SerializeField]    private         CameraRecoil    mCameraRecoil;
    [SerializeField]    private         ModelRecoil     mModelRecoil;
                        private         WallUIEffect    mWallUIEffect;
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
        mPrimary = new BurstSMG
        {
            readyToShoot = true
        };

        Equip(mPrimary);
        mCurrent = mPrimary;
        mCameraRecoil.UpdateRecoilInfo(mCurrent.cameraRecoilInfo);
        mModelRecoil.UpdateRecoilInfo(mCurrent.modelRecoilInfo);
        mCurrent.model.SetActive(true);

        // Create Sidearm
        mSecondary = new MachinePistol
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

    public void SwitchWeaponsFromLoadout(PlayerWeapon primary, PlayerWeapon secondary)
    {
        UnequipPrimary();
        UnequipSecondary();

        mPrimary = primary;
        mSecondary = secondary;

        mNextWeapon = mPrimary;

        Equip(mPrimary);
        Equip(mSecondary);

        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        if (mWallUIEffect == null)
        {
            mWallUIEffect = GetComponent<PlayerSetup>().playerUIInstance.GetComponentInChildren<WallUIEffect>();
        }

        // Weapon Switching
        if (Input.GetButtonDown("Primary") && !mCurrent.altFire && mPrimary != mCurrent && mPrimary != null)
        { 
            mNextWeapon                 = mPrimary;
            mNextWeapon.switchingWeapon = true;
            mCurrent.reloading          = false;
            mCurrent.switchingWeapon    = true;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
        if (Input.GetButtonDown("Secondary") && !mCurrent.altFire && mSecondary != mCurrent && mSecondary != null)
        {
            mNextWeapon                 = mSecondary;
            mNextWeapon.switchingWeapon = true;
            mCurrent.reloading          = false;
            mCurrent.switchingWeapon    = true;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }
        if (Input.GetButtonDown("Super") && !mCurrent.altFire && mSuper != mCurrent && mSuper != null)
        {
            mNextWeapon                 = mSuper;
            mNextWeapon.switchingWeapon = true;
            mCurrent.reloading          = false;
            mCurrent.switchingWeapon    = true;
            Invoke(nameof(Stow), mCurrent.stowTime);
        }

        if (mAmplified)
        {
            mAmplifyTime    += Time.deltaTime;

            if (mAmplifyTime > AMPLIFY_DURATION)
            {
                mWallUIEffect.AmpDeactivate();

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
                mWallUIEffect.HasteDeactivate();

                mHasteTime = 0f;

                mHasted = false;

                mPrimary.hasted             = false;
                mPrimary.speedMultiplier    = 1.0f; 
                mSecondary.hasted           = false;
                mSecondary.speedMultiplier  = 1.0f;

                if (mSuper != null)
                {
                    mSuper.hasted           = false;
                    mSuper.speedMultiplier  = 1.0f;
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

    public void Equip(PlayerWeapon weapon)
    {
        //weaponHolder.position, weaponHolder.rotation
        weapon.model = (GameObject)Instantiate(
            msWeaponArr[(int)weapon.weaponType]);
        weapon.model.transform.SetParent(weaponHolder);
        Vector3 holdPosition = weapon.model.GetComponentInChildren<HoldPt>().gameObject.transform.position;
        weapon.model.transform.position = weaponHolder.transform.position - holdPosition;
        Vector3 rotation = this.transform.rotation.eulerAngles + weapon.model.transform.rotation.eulerAngles;
        weapon.model.transform.rotation = Quaternion.Euler(rotation);

        mCurrentGraphics = weapon.model.GetComponentInChildren<WeaponGraphics>();
        if (mCurrentGraphics == null)
        {
            throw new Exception("No weapon graphics component on " + weapon.weaponType);
        }

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weapon.model, LayerMask.NameToLayer(WEAPON_LAYER));
        }

        mCameraRecoil.UpdateRecoilInfo(weapon.cameraRecoilInfo);
        mModelRecoil.UpdateRecoilInfo(weapon.modelRecoilInfo);

        // Hide model initially.
        weapon.model.SetActive(false);
    }

    /// <summary>Call before equipping a overwriting mPrimary with a new weapon.</summary>
    public void UnequipPrimary()
    {
        Destroy(mPrimary.model);
    }

    
    /// <summary>Call before equipping a overwriting mSecondary with a new weapon.</summary>
    public void UnequipSecondary()
    {
        Destroy(mSecondary.model);
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
        mCurrent.switchingWeapon    = false;
        mCurrent.readyToShoot       = true;
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
            mWallUIEffect.AmpActivate();
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
            mWallUIEffect.HasteActivate();
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
