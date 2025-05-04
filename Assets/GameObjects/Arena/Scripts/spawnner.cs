using System;
using UnityEngine;

public class Spawnner : MonoBehaviour
{

    [SerializeField] private GameObject m_attacker; //spawnnable game object "attacker"
    [SerializeField] private GameObject m_defender; //spawnnable game object "defender"
    [SerializeField] private GameObject m_ball;

    private GameObject m_attackerSpawnRegion; //defines the bounds where attacker can spawn
    private GameObject m_defenderSpawnRegion; //defines the bounds where defender can spawn

    private Bounds m_attackerSpawnBounds;

    private Vector3 m_boundsOffset = new Vector3(0.5f,0,0.5f);
    private System.Random rand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_attackerSpawnRegion = GameObject.FindGameObjectWithTag("AttackerSpawn");
        m_defenderSpawnRegion = GameObject.FindGameObjectWithTag("DefenderSpawn");

        m_attackerSpawnBounds = m_attackerSpawnRegion.GetComponent<Collider>().bounds;
        m_attackerSpawnBounds.max -= m_boundsOffset;
        m_attackerSpawnBounds.min += m_boundsOffset;

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        spawnBall();
    }

    private void spawnBall()
    {
        float x = UnityEngine.Random.Range(m_attackerSpawnBounds.min.x, m_attackerSpawnBounds.max.x);
        float z = UnityEngine.Random.Range(m_attackerSpawnBounds.min.z, m_attackerSpawnBounds.max.z);

        Instantiate(m_ball, new Vector3(x, 0, z), Quaternion.identity);
    }
    //This function spawns either the attacker or the defender based on ray cast result
    void spawn()
    {
        Vector3 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(rayOrigin, new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //spawn the attacker
            Vector3 spawnPoint = new Vector3(hit.point.x, 0.0f, hit.point.z);
            if(hit.collider.gameObject == m_attackerSpawnRegion && GameManager.instance.CanAttackerSpawn())
            {
                Instantiate(m_attacker, spawnPoint, Quaternion.identity);
            }
            //spawn the defender
            if (hit.collider.gameObject == m_defenderSpawnRegion && GameManager.instance.CanDefenderSpawn())
            {
                Instantiate(m_defender, spawnPoint, Quaternion.identity);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.IsGamePaused())
        {
           spawn();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
    }
    
}
