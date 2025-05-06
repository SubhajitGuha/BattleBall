using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class AttackerInvadeState : AttackerBaseState
{
    private GameObject m_defenderFence;
    private Vector3 moveDirection;
    private bool hasCollidedWithFence = false;
    public override void EnterState(AttackerStateManager attacker)
    {
        attacker.isActive = true;
        hasCollidedWithFence = false;
        m_defenderFence = GameObject.FindGameObjectWithTag("DefenderFence");
        moveDirection = (m_defenderFence.transform.position - attacker.transform.position).normalized;
    }

    public override void UpdateState(AttackerStateManager attacker)
    {
        if(hasCollidedWithFence)
            return;

        //move the attacker only in z-axis (in order to move straight)
        Vector3 moveDirZ = new Vector3(0.0f, 0.0f, 1.0f * Mathf.Sign(moveDirection.z) * MyUtils.FieldScale); //do not change the xy-axis

        attacker.transform.position += (moveDirZ) * AttackerVariables.NormalSpeed * Time.deltaTime;
        attacker.transform.forward = (moveDirZ);
    }

    public override void ExitState(AttackerStateManager state)
    {

    }

    public override void OnCollisionEnter(AttackerStateManager attacker, Collider collider)
    {
        if(collider.CompareTag("DefenderFence"))
        {
            hasCollidedWithFence = true;
            attacker.isActive = false; //make it inactive
            Object.Destroy(attacker.gameObject, 1.0f);
        }
    }
}
