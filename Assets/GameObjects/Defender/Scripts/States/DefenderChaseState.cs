using UnityEngine;

public class DefenderChaseState : DefenderBaseState
{
    public override void EnterState(DefenderStateManager defender)
    {

    }

    public override void UpdateState(DefenderStateManager defender)
    {
        float step = DefenderVariables.NormalSpeed *Time.deltaTime;
        defender.transform.position = Vector3.MoveTowards(
            defender.transform.position,
            defender.AttackerToChase.transform.position,
            step);

        if (Vector3.Distance(defender.transform.position, defender.AttackerToChase.transform.position) <= 0.5f)
        {
            defender.CurrentTimer = DefenderVariables.ReactivateTime;
            defender.SwitchState(defender.m_defenderInactiveState);
        }
    }

    public override void ExitState(DefenderStateManager defender)
    {

    }

    public override void OnCollisionEnter(DefenderStateManager defender, Collider collider)
    {
       
    }
}
