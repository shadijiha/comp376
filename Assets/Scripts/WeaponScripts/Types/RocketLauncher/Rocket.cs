using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private readonly float  m_COLLISION_DELAY   = 0.01f;
    private readonly float  m_LIFETIME          = 20f;
    public           float  m_flightTime        = 0f;

    public  bool            m_active;
    public  string          m_owner;
    public  ParticleSystem  m_explosion;
    public  Vector3         m_direction;
    public  float           m_velocity;
    public  float           m_explosionForce;
    public  float           m_explosionLift;
    public  float           m_MaxRadius;
    public  int             m_contactDamage;
    public  int             m_splashDamage;
    public  int             m_minSplashDamage;
    public  LayerMask       m_targetMask;
    public  LayerMask       m_blockExplosion;
    private int             m_maxHits           = 25;
    private Collider[]      m_hits;
    private GameObject[]    m_playersHit;
    
    // Start is called before the first frame update
    void Start()
    {
        m_hits                  = new Collider[m_maxHits];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active)
        {   
            m_flightTime += Time.deltaTime;

            if (m_flightTime > m_LIFETIME)
            {
                Destroy(this.gameObject);
            }

            transform.rotation = Quaternion.LookRotation(m_direction);

            transform.position += (m_direction * m_velocity * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (m_flightTime > m_COLLISION_DELAY)
        {
            if (m_targetMask == (m_targetMask | (1 << collision.gameObject.layer)))
            {
                Debug.Log($"Direct hit on {collision.name}");
                Player  player  = collision.GetComponent<Player>();

                player.RpcTakeDamage(m_contactDamage, m_owner);
            }

            Destroy(Instantiate(m_explosion, transform.position, transform.rotation).gameObject, 1.0f);
                
            int             hits            = Physics.OverlapSphereNonAlloc(transform.position, m_MaxRadius, m_hits, m_targetMask);
            List<Player>    playersToHit    = new List<Player>();
            List<int>       playerDmg       = new List<int>();

            for (int i = 0; i < hits; i++)
            {
                if (m_hits[i].TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    float distance = Vector3.Distance(transform.position, m_hits[i].transform.position);
                    if (!Physics.Raycast(transform.position, (m_hits[i].transform.position - transform.position).normalized, distance,  m_blockExplosion))
                    {
                        rigidbody.AddExplosionForce(m_explosionForce, transform.position, m_MaxRadius, m_explosionLift, ForceMode.Impulse);
                        int     damage  = Mathf.RoundToInt(Mathf.Lerp(m_splashDamage, m_minSplashDamage, distance / m_MaxRadius));
                        Player  plr     = rigidbody.GetComponent<Player>();

                        if (!playersToHit.Contains(plr))
                        {
                            Debug.Log($"Hitting {plr.name} for {damage}.");
                            playersToHit.Add(plr);
                            playerDmg.Add(damage);
                        }
                        else
                        {
                            Debug.Log($"Already hitting {plr.name} for {damage}, skipping.");
                        }
                    }
                }
            }

            for (int dmgIndex = 0; dmgIndex < playersToHit.Count; ++dmgIndex)
            {
                playersToHit[dmgIndex].RpcTakeDamage(playerDmg[dmgIndex], m_owner);
            }

            Destroy(this.gameObject);
        }
    }

    public void Launch(string source)
    {
        m_owner     = source;
        m_active    = true;

        transform.parent = null;
    }
}
