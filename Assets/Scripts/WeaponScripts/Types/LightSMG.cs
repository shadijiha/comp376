using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightSMG : PlayerWeapon
{
    public LightSMG()
    {
        weaponType              = WeaponType.LightSMG;
        description             = "A high-recoil, short-range, fully-automatic smg with an extremely rapid rate of fire. Spread remains controlled while moving. Has a low critical hit modifier.";
        damage                  = 8;
        currentLoadedAmmo       = 30;
        magazineSize            = 30;
        currentSpareAmmo        = 180;
        maxAmmo                 = 180;
        shotCount               = 1;
        critMultiplier          = 1.25f;
        falloffStart            = 15f;
        falloffMax              = 30f;
        falloffDamage           = 6;
        maxRange                = 200f;
        fireRate                = 18.0f;
        currentSpread           = 0.05f;
        minSpread               = 0.05f;
        maxSpread               = 0.25f;
        spreadIncrease          = 0.005f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.2f;
        reloadTime              = 1.0f;
        drawTime                = 0.4f;
        stowTime                = 0.5f;
        allowContinuousFire     = true;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        rotationSpeed       = 10f,
                                        returnSpeed         = 20f,
                                        recoilRotation      = new Vector3(10f,       2f,         2f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 20f,
                                        positionReturnSpeed = 30f,
                                        rotationReturnSpeed = 40f,

                                        RecoilRotation      = new Vector3(-15,      5,          7),
                                        MinRecoilRotation   = new Vector3(-7.5f,    2.5f,       3.5f),
                                        RecoilKick          = new Vector3(0.025f,   0.01f,      -0.1f),
                                        MinRecoilKick       = new Vector3(0.0125f,  0.005f,     -0.05f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

}
