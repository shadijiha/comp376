using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scattergun : PlayerWeapon
{
    public Scattergun()
    {
        weaponType              = WeaponType.Scattergun;
        damage                  = 4;
        currentLoadedAmmo       = 2;
        magazineSize            = 2;
        currentSpareAmmo        = 30;
        maxAmmo                 = 30;
        shotCount               = 40;
        critMultiplier          = 1.25f;
        falloffStart            = 5f;
        falloffMax              = 15f;
        falloffDamage           = 2;
        maxRange                = 200f;
        fireRate                = 1.0f;
        currentSpread           = 0.4f;
        minSpread               = 0.4f;
        maxSpread               = 0.4f;
        spreadIncrease          = 0.0f;
        spreadRecovery          = 0.1f;
        movementSpread          = 0.0f;
        reloadTime              = 1.0f;
        drawTime                = 0.7f;
        stowTime                = 0.8f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed         = 5f,
                                        rotationSpeed       = 30f,
                                        recoilRotation      = new Vector3(30f,      2.5f,       2.5f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 50f,
                                        rotationRecoilSpeed = 25f,
                                        positionReturnSpeed = 5f,
                                        rotationReturnSpeed = 5f,

                                        RecoilRotation      = new Vector3(-120f,        5f,         5f),
                                        MinRecoilRotation   = new Vector3(-60f,         2.5f,       2.5f),
                                        RecoilKick          = new Vector3(0.1f,         0.1f,       -0.5f),
                                        MinRecoilKick       = new Vector3(0.05f,        0.05f,      -0.25f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

}
