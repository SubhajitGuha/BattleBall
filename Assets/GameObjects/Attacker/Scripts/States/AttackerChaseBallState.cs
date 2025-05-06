using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackerChaseBallState : AttackerBaseState
{
    private GameObject m_ball;
    public override void EnterState(AttackerStateManager attacker)
    {
        attacker.isActive = true;
        //get the ball from the scene
        m_ball = GameObject.FindGameObjectWithTag("Ball");
        if(m_ball == null )
        {
            Debug.Log("palce the ball object on the level");
        }

    }

    //move towards the ball at normal speed 
    public override void UpdateState(AttackerStateManager attacker)
    {
        //if not holding the ball and if not chaising the ball switch to invade state
        if ((m_ball.transform.parent != null && m_ball.transform.parent.gameObject != attacker)
            && GameManager.instance.isBallOccupied)
        {
            attacker.SwitchState(attacker.m_attackerInvadeState);
            return;
        }
        //else chase to catch the ball
        Vector3 currentPos = attacker.transform.position;
        float step = AttackerVariables.NormalSpeed * Time.deltaTime;
        attacker.transform.position = MyUtils.TranslateOnXZPlane(currentPos, m_ball.transform.position, step);
        attacker.transform.forward = MyUtils.GetDirectionInXZPlane(attacker.transform.position, m_ball.transform.position);
    }

    public override void ExitState(AttackerStateManager attacker)
    {

    }

    public override void OnCollisionEnter(AttackerStateManager attacker, Collider collider)
    {
        //If attacker hits the ball, acquire the ball and switch to holding ball state
        if (!GameManager.instance.isBallOccupied && collider.gameObject == m_ball)
        {
            GameManager.instance.isBallOccupied = true;
            Vector3 ballMoveLocation = attacker.transform.position + attacker.transform.forward * 1.0f * MyUtils.FieldScale;
            m_ball.transform.SetParent(attacker.transform, true);
            m_ball.transform.position = MyUtils.CreateVecOnXZPlane(ballMoveLocation.x, ballMoveLocation.z);
            attacker.SwitchState(attacker.m_attackerHoldingBallState);
        }
    }
}