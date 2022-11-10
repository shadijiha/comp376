using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pistol : PlayerWeapon
{
    public Pistol()
    {
        weaponType              = WeaponType.Sidearm;
        damage                  = 12;
        currentLoadedAmmo       = 10;
        magazineSize            = 10;
        currentSpareAmmo        = -1;
        maxAmmo                 = -1;
        shotCount               = 1;
        burstCount              = 1;
        burstIndex              = 0;
        burstFireRate           = 1;
        critMultiplier          = 2.0f;
        falloffStart            = 20f;
        falloffMax              = 40f;
        falloffDamage           = 8;
        maxRange                = 200f;
        fireRate                = 10.0f;
        currentSpread           = 0.005f;
        minSpread               = 0.005f;
        maxSpread               = 0.10f;
        spreadIncrease          = 0.03f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.5f;
        reloadTime              = 1.0f;
        drawTime                = 0.3f;
        stowTime                = 0.4f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        //burstInfo               = new BurstInfo();
        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed = 20f,
                                        rotationSpeed = 10f,
                                        recoilRotation = new Vector3(7f, 1f, 0f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

    public new GameObject GetModel()
    {
        return model;
    }
}
