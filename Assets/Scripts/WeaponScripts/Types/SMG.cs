using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SMG : PlayerWeapon
{
    public SMG()
    {
        weaponType              = WeaponType.SMG;
        damage                  = 10;
        currentLoadedAmmo       = 30;
        magazineSize            = 30;
        currentSpareAmmo        = 180;
        maxAmmo                 = 180;
        shotCount               = 1;
        critMultiplier          = 1.5f;
        falloffStart            = 15f;
        falloffMax              = 30f;
        falloffDamage           = 7;
        maxRange                = 200f;
        fireRate                = 15.0f;
        currentSpread           = 0.02f;
        minSpread               = 0.02f;
        maxSpread               = 0.075f;
        spreadIncrease          = 0.003f;
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
                                        rotationSpeed       = 8f,
                                        returnSpeed         = 20f,
                                        recoilRotation      = new Vector3(8f,       2f,         2f)
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
