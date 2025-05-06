using Unity.XR.CoreUtils;
using UnityEngine;

public class DefenderStandbyState : DefenderBaseState
{
    private GameObject m_arena;
    private GameObject m_scanCircle;
    private float m_senseRadius;
    private float m_oldFieldScale;
    public override void EnterState(DefenderStateManager defender)
    {
        defender.isActive = true;
        m_oldFieldScale = MyUtils.FieldScale;
        //get the arena
        m_arena = GameObject.FindGameObjectWithTag("Arena");
        //calculate the chase radius (by default some % of the arena width)
        m_senseRadius = DefenderVariables.DetectionRange * m_arena.GetComponent<Renderer>().bounds.extents.x;

        //get the scan circle game object
        m_scanCircle = defender.gameObject.GetNamedChild("ScanCircle");
        m_scanCircle.GetComponent<MeshRenderer>().enabled = true;

        //scale the unit quad by 2 * m_senseRadius (then we get a circle with radius m_senseRadius)
        m_scanCircle.gameObject.transform.localScale = new Vector3(
            m_senseRadius * 2.0f,
            m_senseRadius * 2.0f,
            m_senseRadius * 2.0f)/ MyUtils.FieldScale;
    }

    private AttackerStateManager getAttackerInRadius(in DefenderStateManager fromDefender)
    {
        var ballGameObject = GameObject.FindGameObjectWithTag("Ball"); //only one ball is there

        if(ballGameObject.transform.parent == null)
        {
            return null;
        }

        AttackerStateManager attackerHoldingBall = ballGameObject.transform.parent.GetComponent<AttackerStateManager>();
        if(attackerHoldingBall == null)
        {
            return null;
        }
        if(Vector3.Distance(attackerHoldingBall.transform.position,ballGameObject.transform.position) > 1.5 * MyUtils.FieldScale)
        {
            //attacker might be still receiving the ball so check for that and dont chase the attacker
            return null;
        }
        if (Vector3.Distance(attackerHoldingBall.transform.position, fromDefender.transform.position) <= m_senseRadius)
        {
            //check if attacker holding the ball is inside the radius or not
            return attackerHoldingBall;
        }
        return null;
    }

    public override void UpdateState(DefenderStateManager defender)
    {
        if(MyUtils.FieldScale != m_oldFieldScale) //scale has changed
        {
            m_senseRadius = DefenderVariables.DetectionRange * m_arena.GetComponent<Renderer>().bounds.extents.x;
            m_scanCircle.gameObject.transform.localScale = new Vector3(
            m_senseRadius * 2.0f,
            m_senseRadius * 2.0f,
            m_senseRadius * 2.0f) / MyUtils.FieldScale;
        }
        AttackerStateManager attackerHoldingBall = getAttackerInRadius(defender);
        if (attackerHoldingBall != null)
        {
            m_scanCircle.GetComponent<MeshRenderer>().enabled = false;
            defender.attackerToChase = attackerHoldingBall.gameObject;
            defender.SwitchState(defender.m_defenderChaseState);
        }
    }

    public override void ExitState(DefenderStateManager defender)
    {

    }

    public override void OnCollisionEnter(DefenderStateManager defender, Collider collider)
    {

    }
}
