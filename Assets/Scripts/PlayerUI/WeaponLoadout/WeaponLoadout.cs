using System.Collections.Generic;
using UnityEngine;
using static PlayerWeapon;

public class WeaponLoadout : MonoBehaviour
{
    [SerializeField] private List<WeaponType> primaryWeaponTypes;
    [SerializeField] private List<WeaponType> secondaryWeaponTypes;

    private List<PlayerWeapon> primaryWeapons;
    private List<PlayerWeapon> secondaryWeapons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
