using UnityEngine;

public class DefenderInactiveState : DefenderBaseState
{
    const float minCompareDist = 0.01f;
    private float m_timer;
    private Material m_oldMaterial;
    public override void EnterState(DefenderStateManager defender)
    {
        defender.isActive = false;
        m_timer = 0.0f;
        Renderer renderer = defender.defenderBody.GetComponent<Renderer>();
        //save the old material so that it can be switched later
        m_oldMaterial = renderer.material;
        //on Inactive state switch to the inactive material
        if (defender.inactiveMaterial != null)
            renderer.material = defender.inactiveMaterial;
        else
        {
            Debug.Log("Inactive Material Not Assigned To Attacker");
        }
    }

    public override void UpdateState(DefenderStateManager defender)
    {
        m_timer += Time.deltaTime;
        //stay inactive until the timer runs out and until we reach the old position
        if (
            Vector3.Distance(defender.transform.position, defender.defenderOriginalPosition)
            <= minCompareDist * MyUtils.FieldScale && m_timer
            >= defender.currentTimer
            )
        {
            //switch to old material
            defender.defenderBody.GetComponent<Renderer>().material = m_oldMaterial;
            defender.SwitchState(defender.m_defenderStandbyState);
            return;
        }
        float step = DefenderVariables.ReturnSpeed * Time.deltaTime;
        defender.transform.position = MyUtils.TranslateOnXZPlane(defender.transform.position, defender.defenderOriginalPosition, step);
    }

    public override void ExitState(DefenderStateManager defender)
    {

    }

    public override void OnCollisionEnter(DefenderStateManager defender, Collider collider)
    {

    }
}
