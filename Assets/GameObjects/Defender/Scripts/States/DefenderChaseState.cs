using UnityEngine;

public class DefenderChaseState : DefenderBaseState
{
    public override void EnterState(DefenderStateManager defender)
    {

    }

    public override void UpdateState(DefenderStateManager defender)
    {
        float step = DefenderVariables.NormalSpeed * Time.deltaTime;
        if(defender.attackerToChase == null)
            return;
        defender.transform.position = MyUtils.TranslateOnXZPlane(
            defender.transform.position,
            defender.attackerToChase.transform.position,
            step);

        if (Vector3.Distance(defender.transform.position, defender.attackerToChase.transform.position) <= 0.5f)
        {
            defender.currentTimer = DefenderVariables.ReactivateTime;
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
