using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SMG : PlayerWeapon
{
    public SMG()
    {
        weaponType              = WeaponType.SMG;
        damage                  = 8;
        currentLoadedAmmo       = 30;
        magazineSize            = 30;
        currentSpareAmmo        = 180;
        maxAmmo                 = 180;
        shotCount               = 1;
        burstCount              = 1;
        burstIndex              = 0;
        burstFireRate           = 1;
        critMultiplier          = 1.5f;
        falloffStart            = 25f;
        falloffMax              = 60f;
        falloffDamage           = 5;
        maxRange                = 200f;
        fireRate                = 15.0f;
        currentSpread           = 0.025f;
        minSpread               = 0.025f;
        maxSpread               = 0.50f;
        spreadIncrease          = 0.02f;
        spreadRecovery          = 0.01f;
        movementSpread          = 1.25f;
        reloadTime              = 1.0f;
        allowContinuousFire     = true;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;
        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

    public new GameObject GetModel()
    {
        return model;
    }
}
