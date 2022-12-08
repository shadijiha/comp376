using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool dead = false;
    [SyncVar]
    private int currentHealth;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnable;

    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Call this when the Player setup is ready
    /// </summary>
    public void Setup() {
        wasEnable = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnable.Length; i++) {
            wasEnable[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    private void Die() {
        audioSrc.Stop();
        audioSrc.PlayOneShot(deathSound);
        isDead(true);

        // Disable components
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Call respawn after A TIME
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.instance.MATCH_SETTINGS.PlayerRespawnTime);

        WeaponManager weaponManager = GetComponent<WeaponManager>();
        weaponManager.Respawn();

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount, string damageSrc) {

        if (IsDead())
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();

            currentHealth = 0;  // FOR GUI

            Player dmgSrcPlayerObject = GameManager.GetPlayer(damageSrc);
            if (!(dmgSrcPlayerObject is null))
            {
                GameManagerServer.RegisterStat(dmgSrcPlayerObject, this);
                GameManagerServer.LogStats();
            }
        }
    }


    public void SetDefaults() {
        isDead(false);
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnable[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }

    public int GetHealth() {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void HealBy(uint amount)
    {
        var vAmount = Math.Min(amount + currentHealth, maxHealth);
        vAmount = Math.Max(vAmount, 0);
        currentHealth = (int)vAmount;
    }

    // Allow green wall to heal hover the max player health.  And stop at twice the player maxHealth
    public void HoverHealBy(uint amount)
    {
        var vAmount = Math.Min(amount + currentHealth, maxHealth * 2);
        vAmount = Math.Max(vAmount, 0);
        currentHealth = (int)vAmount;
    }

    public bool IsDead() {
        return dead;
    }

    protected void isDead(bool flag) {
        dead = flag;
    }

    public string ToString() {
        return this.transform.name;
    }
}
