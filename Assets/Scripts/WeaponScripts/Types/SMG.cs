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
        burstCount              = 1;
        burstIndex              = 0;
        burstFireRate           = 1;
        critMultiplier          = 1.5f;
        falloffStart            = 15f;
        falloffMax              = 30f;
        falloffDamage           = 7;
        maxRange                = 200f;
        fireRate                = 15.0f;
        currentSpread           = 0.01f;
        minSpread               = 0.01f;
        maxSpread               = 0.10f;
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
                                        returnSpeed = 20f,
                                        rotationSpeed = 8f,
                                        recoilRotation = new Vector3(8f, 2f, 2f)
                                    };
        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

    public new GameObject GetModel()
    {
        return model;
    }
}
