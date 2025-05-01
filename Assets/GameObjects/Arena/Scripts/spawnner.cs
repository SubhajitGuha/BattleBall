using UnityEngine;

public class Spawnner : MonoBehaviour
{

    public GameObject Attacker; //spawnnable game object "attacker"
    public GameObject Defender; //spawnnable game object "defender"

    private GameObject m_attackerSpawnRegion; //defines the bounds where attacker can spawn
    private GameObject m_defenderSpawnRegion; //defines the bounds where defender can spawn
    private float spawnYOffset = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_attackerSpawnRegion = GameObject.FindGameObjectWithTag("AttackerSpawn");
        m_defenderSpawnRegion = GameObject.FindGameObjectWithTag("DefenderSpawn");
    }

    //This function spawns either the attacker or the defender based on ray cast result
    void Spawn()
    {
        Vector3 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(rayOrigin, new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //spawn the attacker
            Vector3 spawnPoint = new Vector3(hit.point.x, transform.position.y + spawnYOffset, hit.point.z);
            if(hit.collider.gameObject == m_attackerSpawnRegion)
            {
                Instantiate(Attacker, spawnPoint, Quaternion.identity);
            }
            //spawn the defender
            if (hit.collider.gameObject == m_defenderSpawnRegion)
            {
                Instantiate(Defender, spawnPoint, Quaternion.identity);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           Spawn();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
    }
    
}
