using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    public const string WEAPON_LAYER = "Weapon";

    [SerializeField] private PlayerWeapon primary;
    [SerializeField] private PlayerWeapon secondary;
    [SerializeField] private PlayerWeapon super;
    [SerializeField] private Transform weaponHolder;

    private PlayerWeapon current;
    private WeaponGraphics currentGraphics;

    // Awake is called before Start, allowing this to equip the weapon before Start() in PlayerShoot requires it.
    void Awake()
    {
        // Equip the primary weapon
        Equip(primary);
    }

    // Update is called once per frame
    void Update()
    {
        // Weapon Switching
        if (Input.GetKey(KeyCode.Alpha1) && primary != current)
        {
            Equip(primary);
        }
        if (Input.GetKey(KeyCode.Alpha2) && secondary != current)
        {
            Equip(secondary);
        }
        if (Input.GetKey(KeyCode.Alpha3) && super != current)
        {
            Equip(super);
        }
    }

    public PlayerWeapon GetCurrentWeapon() {
        return current;
    }

    public WeaponGraphics GetCurrentWeaponGraphics() {
        return currentGraphics;
    }

    private void Equip(PlayerWeapon weapon) {
        this.current = weapon;

        GameObject weaponInstance = (GameObject)Instantiate(weapon.model, weaponHolder.position, weaponHolder.rotation);
        weaponInstance.transform.SetParent(weaponHolder);

        currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
            throw new Exception("No weapon graphics component on " + weaponInstance.name);

        if (isLocalPlayer) {
            Util.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(WEAPON_LAYER));
        }
    }
}
