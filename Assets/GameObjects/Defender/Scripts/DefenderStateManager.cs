using System;
using UnityEngine;

public class DefenderStateManager : MonoBehaviour
{
    public Material inactiveMaterial;

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
        defenderOriginalPosition = transform.position;
        currentTimer = DefenderVariables.SpawnTime;
        m_currentState = m_defenderInactiveState;
        m_currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        m_currentState.UpdateState(this);
    }

    public void SwitchState(DefenderBaseState otherState)
    {
        m_currentState = otherState;
        m_currentState.EnterState(this);
    }
}
