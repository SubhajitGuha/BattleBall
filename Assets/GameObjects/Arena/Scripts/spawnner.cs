using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawnner : MonoBehaviour
{

    [SerializeField] private GameObject m_attacker; //spawnnable game object "attacker"
    [SerializeField] private GameObject m_defender; //spawnnable game object "defender"
    [SerializeField] private GameObject m_ball;

    private GameObject m_attackerSpawnRegion; //defines the bounds where attacker can spawn
    private GameObject m_defenderSpawnRegion; //defines the bounds where defender can spawn

    private Bounds m_ballSpawnBounds;

    private Vector3 m_boundsOffset = new Vector3(0.5f,0,0.5f);
    bool m_isAttacking;

    private void Awake()
    {
        m_isAttacking = false;
        MyUtils.FieldScale = 1.0f;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rotate the board so that the attacker and defender position changes
        int matchCount = PlayerPrefs.GetInt(MyUtils.MATCH_COUNT, 0);
        m_isAttacking = matchCount % 2 == 0; //attacking side in even-match count
        gameObject.transform.Rotate(new Vector3(0.0f, m_isAttacking ? 0.0f : 180.0f, 0.0f));

        MyUtils.GameYValue = gameObject.transform.position.y; //the game Y value is equals the arena y coordinate value
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        m_attackerSpawnRegion = GameObject.FindGameObjectWithTag("AttackerSpawn");
        m_defenderSpawnRegion = GameObject.FindGameObjectWithTag("DefenderSpawn");

        //spawn the ball
        spawnBall();
    }

    private void spawnBall()
    {
        if (m_isAttacking)
        {
            m_ballSpawnBounds = m_attackerSpawnRegion.GetComponent<Collider>().bounds;
        }
        else 
        {
            m_ballSpawnBounds = m_defenderSpawnRegion.GetComponent<Collider>().bounds;
        }
        m_ballSpawnBounds.max -= m_boundsOffset;
        m_ballSpawnBounds.min += m_boundsOffset;

        float x = UnityEngine.Random.Range(m_ballSpawnBounds.min.x, m_ballSpawnBounds.max.x);
        float z = UnityEngine.Random.Range(m_ballSpawnBounds.min.z, m_ballSpawnBounds.max.z);

        Instantiate(m_ball, MyUtils.CreateVecOnXZPlane(x,z), Quaternion.identity, gameObject.transform.parent);
    }
    //This function spawns either the attacker or the defender based on ray cast result
    void spawn(in Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            //spawn the attacker
            Vector3 spawnPoint = MyUtils.CreateVecOnXZPlane(hit.point.x,hit.point.z);//new Vector3(hit.point.x, 0.0f, hit.point.z);
            if (hit.collider.gameObject == m_attackerSpawnRegion && GameManager.instance.CanAttackerSpawn())
            {
                Instantiate(m_attacker, spawnPoint, Quaternion.identity, gameObject.transform.parent);
            }
            //spawn the defender
            if (hit.collider.gameObject == m_defenderSpawnRegion && GameManager.instance.CanDefenderSpawn())
            {
                Instantiate(m_defender, spawnPoint, Quaternion.identity, gameObject.transform.parent);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.parent == null)
        {
            Debug.LogError("attach a parent to the attacker with uniform scale");
        }

        MyUtils.FieldScale = transform.parent.lossyScale.x; //assuming there is uniform scale

        if (Input.GetMouseButtonDown(0) && !GameManager.instance.IsGamePaused())
        {
           spawn(Input.mousePosition);
        }
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.touchCount < 2 &&
                Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                // We hit a UI element
                Debug.Log("We hit an UI Element");
                return;
            }

            spawn(touch.position);
        }
#endif
    }
    private void OnCollisionEnter(Collision collision)
    {
    }
}
