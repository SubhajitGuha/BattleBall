using System;
using UnityEngine;

public class AttackerStateManager : MonoBehaviour
{
    public Material InactiveMaterial;
    [NonSerialized] public float CurrentTimer = 0.0f;
    [NonSerialized] public bool IsActive;

    private AttackerBaseState m_currentState;
    //different states of attacker
    public AttackerInactiveState m_attackerInactiveState = new AttackerInactiveState();
    public AttackerChaseBallState m_attackerChaseBallState = new AttackerChaseBallState();
    public AttackerHoldingBallState m_attackerHoldingBallState = new AttackerHoldingBallState();
    public AttackerInvadeState m_attackerInvadeState = new AttackerInvadeState();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsActive = true;
        CurrentTimer = AttackerVariables.SpawnTime;
        m_currentState = m_attackerInactiveState;
        m_currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        m_currentState.UpdateState(this);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    m_currentState.OnCollisionEnter(this, other);

    //}

    private void OnTriggerStay(Collider other)
    {
        m_currentState.OnCollisionEnter(this, other);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
    public void SwitchState(AttackerBaseState state)
    {
        m_currentState=state;
        m_currentState.EnterState(this);
    }
}
