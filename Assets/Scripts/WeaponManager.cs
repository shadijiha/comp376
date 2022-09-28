using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    public const string WEAPON_LAYER = "Weapon";

    [SerializeField] private PlayerWeapon primary;
    [SerializeField] private Transform weaponHolder;

    private PlayerWeapon current;
    private WeaponGraphics currentGraphics;

    // Start is called before the first frame update
    void Start()
    {
        // Equipe the weapon
        Equipe(primary);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerWeapon GetCurrentWeapon() {
        return current;
    }

    public WeaponGraphics GetCurrentWeaponGraphics() {
        return currentGraphics;
    }

    private void Equipe(PlayerWeapon weapon) {
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
