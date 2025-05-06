using UnityEngine;

public class DefenderChaseState : DefenderBaseState
{
    const float minCollisionDistance = 0.8f;
    public override void EnterState(DefenderStateManager defender)
    {
        defender.isActive = true;

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
        defender.transform.forward = MyUtils.GetDirectionInXZPlane(
            defender.transform.position,
            defender.attackerToChase.transform.position);

        if (Vector3.Distance(defender.transform.position, defender.attackerToChase.transform.position) <= minCollisionDistance * MyUtils.FieldScale)
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
        //var ballObj = GameObject.FindGameObjectWithTag("Ball");
        //var parent = ballObj.transform.parent;
        //AttackerStateManager attacker = parent.GetComponent<AttackerStateManager>(); //attacker holding the ball
        //if (collider.gameObject == attacker.gameObject && attacker.isActive)
        //{
        //    defender.currentTimer = DefenderVariables.ReactivateTime;
        //    defender.SwitchState(defender.m_defenderInactiveState);
        //}
    }
}
