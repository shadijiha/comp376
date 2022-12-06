using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TacticalShotgun : PlayerWeapon
{
    public TacticalShotgun()
    {
        weaponType              = WeaponType.TacticalShotgun;
        description             = "A moderate fire-rate shotgun with moderate damage and moderate spread. Spread does not change when moving. Has an moderate critical hit modifier.";
        damage                  = 5;
        currentLoadedAmmo       = 6;
        magazineSize            = 6;
        currentSpareAmmo        = 42;
        maxAmmo                 = 42;
        shotCount               = 15;
        critMultiplier          = 1.5f;
        falloffStart            = 10f;
        falloffMax              = 30f;
        falloffDamage           = 3;
        maxRange                = 200f;
        fireRate                = 2.0f;
        currentSpread           = 0.15f;
        minSpread               = 0.15f;
        maxSpread               = 0.25f;
        spreadIncrease          = 0.05f;
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
                                        returnSpeed         = 5f,
                                        rotationSpeed       = 20f,
                                        recoilRotation      = new Vector3(20f,      2.5f,       2.5f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 30f,
                                        rotationRecoilSpeed = 15f,
                                        positionReturnSpeed = 5f,
                                        rotationReturnSpeed = 5f,

                                        RecoilRotation      = new Vector3(-60f,         5f,         5f),
                                        MinRecoilRotation   = new Vector3(-30f,         2.5f,       2.5f),
                                        RecoilKick          = new Vector3(0.1f,         0.1f,       -0.4f),
                                        MinRecoilKick       = new Vector3(0.05f,        0.05f,      -0.2f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

}
