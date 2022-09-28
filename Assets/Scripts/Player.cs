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
        yield return new WaitForSeconds(GameManager.instance.MATCH_SETTINGS.RESPAWN_TIME);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount) {

        if (isDead())
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
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

    public int getHealth() {
        return currentHealth;
    }

    public bool isDead() {
        return dead;
    }

    protected void isDead(bool flag) {
        dead = flag;
    }

    public string ToString() {
        return this.transform.name;
    }
}
