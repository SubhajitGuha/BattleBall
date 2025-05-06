using System;
using System.Collections;
using UnityEngine;

public class AttackerStateManager : MonoBehaviour
{
    public Material inactiveMaterial;
    public Material attackerHoldingBallMaterial;

    public GameObject attackerBody;
    [NonSerialized] private Animator animationController;
    [NonSerialized] public float currentTimer = 0.0f;
    [NonSerialized] public bool isActive;

    private AttackerBaseState m_currentState;
    //different states of attacker
    public AttackerInactiveState m_attackerInactiveState = new AttackerInactiveState();
    public AttackerChaseBallState m_attackerChaseBallState = new AttackerChaseBallState();
    public AttackerHoldingBallState m_attackerHoldingBallState = new AttackerHoldingBallState();
    public AttackerInvadeState m_attackerInvadeState = new AttackerInvadeState();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(attackerBody == null)
        {
            Debug.LogError(string.Format("attach attacker body in {}", this.ToString()));
        }
        animationController = attackerBody.GetComponent<Animator>();
        animationController.SetBool("onSpawn", true);

        StartCoroutine (ChangeAnimState("onSpawn", false, 1.1f));

        isActive = true;
        currentTimer = AttackerVariables.SpawnTime;
        m_currentState = m_attackerInactiveState;
        m_currentState.EnterState(this);
    }

    IEnumerator ChangeAnimState(string varName, bool value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animationController.SetBool(varName, value);
    }

    // Update is called once per frame
    void Update()
    {
        m_currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            animationController.SetBool("onCollide", true);
            StartCoroutine(ChangeAnimState("onCollide", false, 0.5f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //handle animation
        if (other.CompareTag("DefenderFence") && !isActive)
        {
            animationController.SetBool("onDestroy", true);
            StartCoroutine(ChangeAnimState("onDestroy", false, 1.1f));
        }
        m_currentState.OnCollisionEnter(this, other);
    }

    public void SwitchState(AttackerBaseState state)
    {
        m_currentState=state;
        m_currentState.EnterState(this);
    }
}
