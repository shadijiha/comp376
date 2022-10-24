using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[RequireComponent(typeof(HealthFunctions))]
public class LandMine : NetworkBehaviour
{
    [SerializeField] private int damage = 20;

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.MATCH_SETTINGS.LandMineRespawnTime);

        Enabled();
    }

    /// <summary>
    /// Hide from match until respawn
    /// </summary>
    private void Disable()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    private void Enabled()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains(PlayerShoot.PLAYER_TAG))
        {
            var playerScript = collision.gameObject.GetComponent<Player>();
            playerScript.RpcTakeDamage(damage, "LAND_MINE");

            Disable();
            StartCoroutine(Respawn());
        }

    }
}