using System;
using System.Collections;
using UnityEngine;

public class DefenderStateManager : MonoBehaviour
{
    public Material inactiveMaterial;
    public GameObject defenderBody;

    private Animator m_animator;
    [NonSerialized] public Vector3 defenderOriginalPosition; //this is the defender spawn position
    [NonSerialized] public float currentTimer = 0.0f;
    [NonSerialized] public GameObject attackerToChase;

    private DefenderBaseState m_currentState;

    public DefenderChaseState m_defenderChaseState = new DefenderChaseState();
    public DefenderInactiveState m_defenderInactiveState = new DefenderInactiveState();
    public DefenderStandbyState m_defenderStandbyState = new DefenderStandbyState();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (defenderBody == null)
        {
            Debug.Log(string.Format("assign defender body to {}", gameObject.ToString()));
        }
        m_animator = defenderBody.GetComponent<Animator>();
        m_animator.SetBool("onSpawn", true);
        StartCoroutine(ChangeAnimState("onSpawn", false, 1.1f));

        defenderOriginalPosition = transform.position;
        currentTimer = DefenderVariables.SpawnTime;
        m_currentState = m_defenderInactiveState;
        m_currentState.EnterState(this);
    }

    IEnumerator ChangeAnimState(string varName, bool value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        m_animator.SetBool(varName, value);
    }

    // Update is called once per frame
    void Update()
    {
        m_currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attacker"))
        {
            m_animator.SetBool("onCollide", true);
            StartCoroutine(ChangeAnimState("onCollide", false, 0.5f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        m_currentState.OnCollisionEnter(this, other);
    }

    public void SwitchState(DefenderBaseState otherState)
    {
        m_currentState = otherState;
        m_currentState.EnterState(this);
    }
}
