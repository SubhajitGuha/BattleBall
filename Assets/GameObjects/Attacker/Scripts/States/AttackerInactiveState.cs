using UnityEngine;

public class AttackerInactiveState : AttackerBaseState
{
    private float m_timer;
    private Material m_oldMaterial;
    public override void EnterState(AttackerStateManager attacker)
    {
        m_timer = 0.0f;
        attacker.IsActive = false; //set the Attacker to not active

        Renderer renderer = attacker.gameObject.GetComponent<Renderer>();
        //save the old material so that it can be switched later
        m_oldMaterial = renderer.material;
        //on Inactive state switch to the inactive material
        if (attacker.InactiveMaterial != null)
            renderer.material = attacker.InactiveMaterial;
        else
        {
            Debug.Log("Inactive Material Not Assigned To Attacker");
        }
    }

    public override void UpdateState(AttackerStateManager attacker)
    {
        m_timer += Time.deltaTime;
        if (m_timer >= attacker.CurrentTimer)
        {
            //switch to old material and change the state to chase the ball
            attacker.gameObject.GetComponent<Renderer>().material = m_oldMaterial;
            attacker.SwitchState(attacker.m_attackerChaseBallState);
        }
    }

    public override void ExitState(AttackerStateManager attacker)
    {

    }

    public override void OnCollisionEnter(AttackerStateManager attacker, Collider collider)
    {

    }
}
