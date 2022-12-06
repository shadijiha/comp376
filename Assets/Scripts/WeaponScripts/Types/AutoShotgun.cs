using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoShotgun : PlayerWeapon
{
    public AutoShotgun()
    {
        weaponType              = WeaponType.AutoShotgun;
        damage                  = 5;
        currentLoadedAmmo       = 10;
        magazineSize            = 10;
        currentSpareAmmo        = 60;
        maxAmmo                 = 60;
        shotCount               = 8;
        critMultiplier          = 1.25f;
        falloffStart            = 10f;
        falloffMax              = 30f;
        falloffDamage           = 3;
        maxRange                = 200f;
        fireRate                = 4.0f;
        currentSpread           = 0.1f;
        minSpread               = 0.1f;
        maxSpread               = 0.2f;
        spreadIncrease          = 0.025f;
        spreadRecovery          = 0.01f;
        movementSpread          = 0.2f;
        reloadTime              = 1.5f;
        drawTime                = 0.6f;
        stowTime                = 0.7f;
        allowContinuousFire     = true;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed         = 5f,
                                        rotationSpeed       = 15f,
                                        recoilRotation      = new Vector3(15f,      2.5f,       2.5f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 10f,
                                        positionReturnSpeed = 5f,
                                        rotationReturnSpeed = 5f,

                                        RecoilRotation      = new Vector3(-10f,         5f,         5f),
                                        MinRecoilRotation   = new Vector3(-5f,          2.5f,       2.5f),
                                        RecoilKick          = new Vector3(0.1f,         0.1f,       -0.3f),
                                        MinRecoilKick       = new Vector3(0.05f,        0.05f,      -0.15f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

}
