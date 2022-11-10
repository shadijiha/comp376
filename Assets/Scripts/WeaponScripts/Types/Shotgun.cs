using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shotgun : PlayerWeapon
{
    public Shotgun()
    {
        weaponType              = WeaponType.Shotgun;
        damage                  = 6;
        currentLoadedAmmo       = 6;
        magazineSize            = 6;
        currentSpareAmmo        = 42;
        maxAmmo                 = 42;
        shotCount               = 15;
        burstCount              = 1;
        burstIndex              = 0;
        burstFireRate           = 1;
        critMultiplier          = 1.25f;
        falloffStart            = 10f;
        falloffMax              = 30f;
        falloffDamage           = 3;
        maxRange                = 200f;
        fireRate                = 1.0f;
        currentSpread           = 0.25f;
        minSpread               = 0.25f;
        maxSpread               = 0.50f;
        spreadIncrease          = 0.25f;
        spreadRecovery          = 0.01f;
        movementSpread          = 0.0f;
        reloadTime              = 1.75f;
        drawTime                = 0.7f;
        stowTime                = 0.8f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed = 10f,
                                        rotationSpeed = 50f,
                                        recoilRotation = new Vector3(25f, 5f, 5f)
                                    };
        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

    public new GameObject GetModel()
    {
        return model;
    }
}
