using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/**
 * NOTE: Ya de l'asti de sauce qui marche pas avec les layers!!!!
 */
[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    public const string PLAYER_TAG              = "Player";
    public const string PLAYER_HEAD_TAG         = "PlayerHead";

                        private PlayerWeapon    m_CurrentWeapon;
                        private PlayerControler m_controler;
                        private PlayerMotor     m_motor;
                        private AudioSource     m_audioSource;
                        public  WeaponManager   m_WeaponManager;
    [SerializeField]    public  RectTransform   crosshair;
    [SerializeField]    public  Camera          cam;
    [SerializeField]    public  Camera          weaponCam;
    [SerializeField]    public  LayerMask       mask;
    [SerializeField]    public  CameraRecoil    cameraRecoil;
    [SerializeField]    public  ModelRecoil     modelRecoil;
    [SerializeField]    public  AudioSource     weaponSound;
    [SerializeField]    public  HitCrosshair    m_hitCrosshair;
                        public  float           m_fMovement;
                        private GameObject      playerUIInstance;

    public GameObject laserShot;
    //public GameObject laserShotTarget;
    public GameObject weaponRotation;
    Ray ray;

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
        m_controler     = GetComponent<PlayerControler>();
        m_motor         = GetComponent<PlayerMotor>();
        m_audioSource   = GameObject.FindGameObjectWithTag("SoundPos").GetComponent<AudioSource>();

        m_CurrentWeapon = m_WeaponManager.GetCurrentWeapon();

        // Load the weapon so the player doesn't need to reload before first using it.
        m_CurrentWeapon.currentLoadedAmmo   = m_CurrentWeapon.magazineSize;
        m_CurrentWeapon.currentSpareAmmo    = m_CurrentWeapon.maxAmmo;
        m_CurrentWeapon.readyToShoot        = true;

        //mask &= LayerMask.NameToLayer("RemotePlayerLayer");
        playerUIInstance = GetComponent<PlayerSetup>().playerUIInstance;
    }

    // Update is called once per frame
    void Update() 
    {
        if (playerUIInstance == null)
            return;

        if (crosshair == null)
        {
            crosshair               = playerUIInstance.GetComponentInChildren<DynamicCrosshair>().GetComponent<RectTransform>();
        }

        if (m_hitCrosshair == null)
        {
            m_hitCrosshair          = playerUIInstance.GetComponentInChildren<HitCrosshair>();
        }

        if (m_CurrentWeapon.scope == null)
        {
            m_CurrentWeapon.scope   = playerUIInstance.GetComponentInChildren<ScopeOverlay>().GetComponent<Image>();
        }

        m_CurrentWeapon = m_WeaponManager.GetCurrentWeapon();

        // Unify both methods of shooting (semi/full-auto) under one variable
        if (m_CurrentWeapon.allowContinuousFire)
        {
            m_CurrentWeapon.shooting = Input.GetButton("Fire1") || Input.GetButtonDown("Fire1");
        }
        else
        {
            m_CurrentWeapon.shooting = Input.GetButtonDown("Fire1");
        }

        //Alt Fire
        if (Input.GetButton("Fire2"))
        {
            m_CurrentWeapon.AltFireActivate(this);
        }
        else
        {
            m_CurrentWeapon.AltFireDeactivate(this);
        }

        // Update player movement speed.
        m_controler.SetSpeedMultiplier(m_CurrentWeapon.speedMultiplier);

        // Reload
        if (Input.GetButtonDown("Reload") && 
            m_CurrentWeapon.currentLoadedAmmo < m_CurrentWeapon.magazineSize && 
            !(m_CurrentWeapon.currentSpareAmmo == 0)&&
            m_CurrentWeapon.reloading == false)
        {
            Reload();
        }

        // Shoot
        if (m_CurrentWeapon.readyToShoot            && 
            m_CurrentWeapon.shooting                && 
            !m_CurrentWeapon.reloading              && 
            m_CurrentWeapon.currentLoadedAmmo > 0)
        {
            Shoot();
        }

        // Update current movement speed
        m_fMovement = Mathf.Lerp(m_fMovement, Mathf.Clamp(m_motor.currentVelocity * 10, 0f, 2f), 5f * Time.deltaTime);

        // If not shooting, recover from spread
        if (m_CurrentWeapon.readyToShoot)
        { 
            m_CurrentWeapon.currentSpread -= m_CurrentWeapon.spreadRecovery * Time.deltaTime * 60;
            if (m_CurrentWeapon.currentSpread < m_CurrentWeapon.minSpread)
            {
                m_CurrentWeapon.currentSpread = m_CurrentWeapon.minSpread;
            }
        }

        
        UpdateCrosshair();
    }

    /// <summary>
    /// Called on the server when the player shoots
    /// </summary>
    [Command]
    public void CmdOnShoot() {
        RpcDoShootEffect();
    }

    /// <summary>
    /// Called on the server when we hit something
    /// </summary>
    /// <param name="pos">The hit point</param>
    /// <param name="normal">The hit normal</param>
    [Command]
    public void CmdOnHit(Vector3 pos, Vector3 normal) {
        RpcDoHitEffect(pos, normal);
    }

    
    //[Command]
    public void CmdOnHitLaser(Vector3 rayOrigine, Vector3 endPoint, float laserShotSpeed)
    {
        ShootLaser(rayOrigine, endPoint, laserShotSpeed);
    }
    

    /// <summary>
    /// Called on all clients when we need to do a
    /// shoot effect
    /// </summary>
    [ClientRpc]
    public void RpcDoShootEffect() {
        ParticleSystem particle = Instantiate(
                                        m_WeaponManager.GetCurrentWeaponGraphics().muzzelFlash, 
                                        GetComponentInChildren<ParticleOrigin>().gameObject.transform);

        Destroy(particle.gameObject, 0.5f);
    }

    /// <summary>
    /// Shows the hit effect on all Clients
    /// </summary>
    /// <param name="pos">The hit point</param>
    /// <param name="normal">The surface normal</param>
    [ClientRpc]
    public void RpcDoHitEffect(Vector3 pos, Vector3 normal) {
        GameObject temp = (GameObject)Instantiate(m_WeaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(temp, 2.0f);
    }

    /// <summary>
    /// Sets the weapon to reloading and invokes the ReloadFinished function after the reload time is completed.
    /// </summary>
    [Client]
    public void Reload()
    {
        if (m_CurrentWeapon.currentSpareAmmo == 0)
        {
            return;
        }
        else
        {
            m_CurrentWeapon.reloading = true;

            Vector3 originalPosition = m_CurrentWeapon.model.transform.localPosition;
            StartCoroutine(ReloadAnim());
            //To prevent the weapon getting permanently displaced in case of timing/calculation error
            //m_CurrentWeapon.model.transform.localPosition = originalPosition;

            //Invoke("ReloadFinished", 0f);
            Invoke("ReloadFinished", (m_CurrentWeapon.hasted ? m_CurrentWeapon.reloadTime * 0.5f : m_CurrentWeapon.reloadTime));

        }
    }

    /// <summary>
    /// Animates the reload of the weapon.
    /// </summary>
    [Client]
    public IEnumerator ReloadAnim()
    {
        //translation
        Vector3 originalPosition = m_CurrentWeapon.model.transform.localPosition;
        Vector3 holdPosition = m_CurrentWeapon.model.GetComponentInChildren<HoldPt>().gameObject.transform.localPosition;
        Vector3 moveDirection = m_CurrentWeapon.model.transform.localPosition - holdPosition;
        moveDirection.Normalize();
        float moveBy = 0.02f;

        //rotation
        Quaternion originalOrientation = m_CurrentWeapon.model.transform.localRotation;
        Vector3 currentOrientation = m_CurrentWeapon.model.transform.localRotation.eulerAngles;
        Vector3 rotationBy = new Vector3(0.5f, 0, 0);

        float firstStageTimer = 0;
        float secondStageTimer = 0;
        while (firstStageTimer <= m_CurrentWeapon.reloadTime)
        {
            if (firstStageTimer <= m_CurrentWeapon.reloadTime / 4f)
            {

                currentOrientation = currentOrientation + rotationBy * (firstStageTimer + 1f);

                m_CurrentWeapon.model.transform.localPosition = m_CurrentWeapon.model.transform.localPosition - moveDirection * moveBy * firstStageTimer;
                m_CurrentWeapon.model.transform.localRotation = Quaternion.Euler(currentOrientation);


            }
            else if(firstStageTimer < 3* m_CurrentWeapon.reloadTime / 4f)
            {

            }
            else
            {
                currentOrientation = currentOrientation - rotationBy * (secondStageTimer + 1f);
                m_CurrentWeapon.model.transform.localPosition = m_CurrentWeapon.model.transform.localPosition + moveDirection * moveBy * secondStageTimer;
                m_CurrentWeapon.model.transform.localRotation = Quaternion.Euler(currentOrientation);
                secondStageTimer += Time.deltaTime;
            }
            firstStageTimer += Time.deltaTime;
            yield return null;
        }
        //To prevent the weapon getting permanently displaced in case of timing/calculation error
        m_CurrentWeapon.model.transform.localPosition = originalPosition;
        m_CurrentWeapon.model.transform.localRotation = originalOrientation;
    }


    /// <summary>
    /// Reloads the weapon, subtracting from the spareAmmo and refilling the magazine.
    /// </summary>
    [Client]
    public void ReloadFinished()
    {
        if (!m_CurrentWeapon.reloading)
        {
            Debug.Log("Reload Aborted.");
        }
        else
        {
            if (m_CurrentWeapon.currentSpareAmmo        >=  m_CurrentWeapon.magazineSize - m_CurrentWeapon.currentLoadedAmmo)
            {
                m_CurrentWeapon.currentSpareAmmo        -=  m_CurrentWeapon.magazineSize - m_CurrentWeapon.currentLoadedAmmo;
                m_CurrentWeapon.currentLoadedAmmo       =   m_CurrentWeapon.magazineSize;
            }
            else if (m_CurrentWeapon.currentSpareAmmo   >   0)
            {
                // If there is not enough spare ammo to fully refill the magazine, partially reload it instead.
                m_CurrentWeapon.currentLoadedAmmo       +=  m_CurrentWeapon.currentSpareAmmo;
                m_CurrentWeapon.currentSpareAmmo        =   0;
            }
            else
            {
                // Infinite Ammo Weapon
                m_CurrentWeapon.currentLoadedAmmo       =   m_CurrentWeapon.magazineSize;
            }

            m_CurrentWeapon.reloading = false;
        }
    }
    
    
    public void ShootLaser(Vector3 shotOriginPosition, Vector3 endPoint,  float laserShotSpeed)
    {
        GameObject shot = Instantiate(laserShot, shotOriginPosition, Quaternion.LookRotation(endPoint - shotOriginPosition));
        shot.GetComponent<Rigidbody>().velocity = (endPoint- shotOriginPosition) * laserShotSpeed;

        GameObject.Destroy(shot, 2f);
    }


  

    [Client]
    public void Shoot()
    {
        m_CurrentWeapon.readyToShoot    = false;

        (string, float) invokeDetails   = m_CurrentWeapon.Shoot(this);
        AudioClip shootSound = m_CurrentWeapon.model.GetComponent<AudioSource>().clip;
        m_audioSource.PlayOneShot(shootSound);

        Invoke(invokeDetails.Item1, invokeDetails.Item2);
    }

    public void UpdateCrosshair()
    {
        crosshair.sizeDelta     = new Vector2(  500 * (m_CurrentWeapon.currentSpread + m_CurrentWeapon.currentSpread * m_fMovement * m_CurrentWeapon.movementSpread),
                                                500 * (m_CurrentWeapon.currentSpread + m_CurrentWeapon.currentSpread * m_fMovement * m_CurrentWeapon.movementSpread));
    }

    public void ReadyToShoot()
    {
        m_CurrentWeapon.readyToShoot = true;
    }

    [Command]
    public void CmdPlayerShot(string hit_id, string src, int damage) {
        // Do the damage stuff
        Debug.Log(hit_id + " has been shot");

        // In the future shoud pass the source to grant assists
        Player p = GameManager.GetPlayer(hit_id);
        p.RpcTakeDamage(damage, src);

        Debug.Log(hit_id + " has " + p.GetHealth());
    }
}
