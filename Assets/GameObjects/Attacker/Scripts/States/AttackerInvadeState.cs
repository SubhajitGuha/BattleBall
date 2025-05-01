using Unity.XR.CoreUtils;
using UnityEngine;

public class AttackerInvadeState : AttackerBaseState
{
    private GameObject m_defenderFence;
    private Vector3 moveDirection;
    public override void EnterState(AttackerStateManager attacker)
    {
        attacker.IsActive = true;
        m_defenderFence = GameObject.FindGameObjectWithTag("DefenderFence");
        moveDirection = (m_defenderFence.transform.position - attacker.transform.position).normalized;
    }

    public override void UpdateState(AttackerStateManager attacker)
    {
        //Vector3 currentPos = attacker.transform.position;
        Vector3 changeAxis = new Vector3(0.0f, 0.0f, 1.0f * Mathf.Sign(moveDirection.z)); //do not change the xy-axis
        attacker.transform.position += (changeAxis) * AttackerVariables.NormalSpeed * Time.deltaTime;
        attacker.transform.forward = (changeAxis);
    }

    public override void ExitState(AttackerStateManager state)
    {

    }

    public override void OnCollisionEnter(AttackerStateManager attacker, Collider collider)
    {
        if(collider.CompareTag("DefenderFence"))
        {
            Object.Destroy(attacker.gameObject);
        }
    }
}
