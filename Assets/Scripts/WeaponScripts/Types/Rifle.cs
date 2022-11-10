using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rifle : PlayerWeapon
{
    public Rifle()
    {
        weaponType              = WeaponType.Rifle;
        damage                  = 15;
        currentLoadedAmmo       = 18;
        magazineSize            = 18;
        currentSpareAmmo        = 120;
        maxAmmo                 = 120;
        shotCount               = 1;
        burstCount              = 3;
        burstIndex              = 0;
        burstFireRate           = 15;
        critMultiplier          = 1.75f;
        falloffStart            = 50f;
        falloffMax              = 100f;
        falloffDamage           = 10;
        maxRange                = 200f;
        fireRate                = 3.0f;
        currentSpread           = 0.005f;
        minSpread               = 0.005f;
        maxSpread               = 0.10f;
        spreadIncrease          = 0.01f;
        spreadRecovery          = 0.01f;
        movementSpread          = 1.75f;
        reloadTime              = 1.75f;
        drawTime                = 0.8f;
        stowTime                = 0.9f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed = 20f,
                                        rotationSpeed = 10f,
                                        recoilRotation = new Vector3(10f, 2f, 2f)
                                    };
        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

    public new GameObject GetModel()
    {
        return model;
    }
}
