using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public  Player          m_owner;
    public  Rigidbody       m_body;
    [SerializeField]
    private ParticleSystem  m_explosion;
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
    public  int             m_maxHits           = 25;
    private Collider[]      m_hits;
    
    // Start is called before the first frame update
    void Start()
    {
        m_hits                  = new Collider[m_maxHits];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_direction * m_velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_targetMask == (m_targetMask | (1 << collision.gameObject.layer)))
        {
            Player  player  = collision.gameObject.GetComponent<Player>();

            player.RpcTakeDamage(m_contactDamage, m_owner.name);
        }

        Debug.Log($"Explode: {collision.gameObject.name}");
        Destroy(Instantiate(m_explosion, transform.position, transform.rotation).gameObject, 1.0f);

        int hits    = Physics.OverlapSphereNonAlloc(transform.position, m_MaxRadius, m_hits, m_targetMask);

        for (int i = 0; i < hits; i++)
        {
            if (m_hits[i].TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                float distance = Vector3.Distance(transform.position, m_hits[i].transform.position);
                if (!Physics.Raycast(transform.position, (m_hits[i].transform.position - transform.position).normalized, distance,  m_blockExplosion))
                {
                    rigidbody.AddExplosionForce(m_explosionForce, transform.position, m_MaxRadius, m_explosionLift, ForceMode.Impulse);
                    int     damage  = Mathf.RoundToInt(Mathf.Lerp(m_splashDamage, m_minSplashDamage, distance / m_MaxRadius));
                    Debug.Log($"Would hit {rigidbody.name} for {damage}");
                    rigidbody.GetComponent<Player>().RpcTakeDamage(damage, m_owner.name);
                }
                //RaycastHit hitInfo;
                //bool hit = Physics.Raycast(transform.position, (m_hits[i].transform.position - transform.position).normalized, out hitInfo, distance);
                //if (hit && hitInfo.collider.gameObject.CompareTag("Player"))
            }
        }

        Destroy(this.gameObject);
    }
}
