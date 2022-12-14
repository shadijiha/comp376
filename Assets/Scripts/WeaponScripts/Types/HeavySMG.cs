using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeavySMG : PlayerWeapon
{
    public HeavySMG()
    {
        weaponType              = WeaponType.HeavySMG;
        description             = "A moderate-recoil, precision, fully-automatic smg with a rapid rate of fire. Spread remains controlled while moving. Has a moderate critical hit modifier.";
        damage                  = 12;
        currentLoadedAmmo       = 25;
        magazineSize            = 25;
        currentSpareAmmo        = 150;
        maxAmmo                 = 150;
        shotCount               = 1;
        critMultiplier          = 1.5f;
        falloffStart            = 25f;
        falloffMax              = 50f;
        falloffDamage           = 8;
        maxRange                = 200f;
        fireRate                = 10.0f;
        currentSpread           = 0.03f;
        minSpread               = 0.03f;
        maxSpread               = 0.15f;
        spreadIncrease          = 0.002f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.5f;
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
                                        returnSpeed         = 15f,
                                        recoilRotation      = new Vector3(8f,       3f,         3f)
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
        name                    = "Heavy SMG";
    }

}
