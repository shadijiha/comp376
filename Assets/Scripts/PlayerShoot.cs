using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * NOTE: Ya de l'asti de sauce qui marche pas avec les layers!!!!
 */
[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";


    private PlayerWeapon m_CurrentWeapon;
    private WeaponManager m_WeaponManager;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null) {
            this.enabled = false;
            throw new Exception("PlayerShoot: No camera attached");
        }

        // Make the gun in the gun layer for the purpose of the weapon camera
        // The weapon camera is responsible of preventing weapon cliping

        m_WeaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update() {
        m_CurrentWeapon = m_WeaponManager.GetCurrentWeapon();

        if (m_CurrentWeapon.fireRate <= 0.0f) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        }
        else {
            if (Input.GetButtonDown("Fire1")) {
                InvokeRepeating("Shoot", 0.0f, 1.0f / m_CurrentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1")) {
                CancelInvoke("Shoot");
            }
        }
    }

    /// <summary>
    /// Called on the server when the player shoots
    /// </summary>
    [Command]
    void CmdOnShoot() {
        RpcDoShootEffect();
    }

    /// <summary>
    /// Called on the server when we hit something
    /// </summary>
    /// <param name="pos">The hit point</param>
    /// <param name="normal">The hit normal</param>
    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal) {
        RpcDoHitEffect(pos, normal);
    }

    /// <summary>
    /// Called on all clients when we need to do a
    /// shoot effect
    /// </summary>
    [ClientRpc]
    void RpcDoShootEffect() {
        m_WeaponManager.GetCurrentWeaponGraphics().muzzelFlash.Play();
    }

    /// <summary>
    /// Shows the hit effect on all Clients
    /// </summary>
    /// <param name="pos">The hit point</param>
    /// <param name="normal">The surface normal</param>
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal) {
        GameObject temp = (GameObject)Instantiate(m_WeaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(temp, 2.0f);
    }

    [Client]
    void Shoot() {
        if (!isLocalPlayer)
            return;

        // We are shooting call shoot method on Server
        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, m_CurrentWeapon.range, mask)) {
            // We hit Something
            if (hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(hit.collider.name, m_CurrentWeapon.damage);
            }

            // Play Hit effect on the sever
            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    void CmdPlayerShot(string hit_id, int damage) {
        // Do the damage stuff
        Debug.Log(hit_id + " has been shot");

        // In the future shoud pass the source to grant assists
        Player p = GameManager.GetPlayer(hit_id);
        p.RpcTakeDamage(damage);

        Debug.Log(hit_id + " has " + p.getHealth());
    }
}
