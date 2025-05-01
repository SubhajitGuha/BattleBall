using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackerChaseBallState : AttackerBaseState
{
    private GameObject m_ball;
    public override void EnterState(AttackerStateManager attacker)
    {
        attacker.IsActive = true;
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
        if ((m_ball.transform.parent != null && m_ball.transform.parent.gameObject != attacker) && AttackerVariables.isBallOccupied)
        {
            attacker.SwitchState(attacker.m_attackerInvadeState);
            return;
        }
        //else chase to catch the ball
        Vector3 currentPos = attacker.transform.position;
        Vector3 direction = (m_ball.transform.position - currentPos).normalized;
        Vector3 changeAxis = new Vector3(1.0f, 0.0f, 1.0f); //donot change the y-axis
        attacker.transform.position += direction.Multiply(changeAxis) * AttackerVariables.NormalSpeed * Time.deltaTime;
        attacker.transform.forward = direction.Multiply(changeAxis);
    }

    public override void ExitState(AttackerStateManager attacker)
    {

    }

    public override void OnCollisionEnter(AttackerStateManager attacker, Collider collider)
    {
        //If attacker hits the ball, acquire the ball and switch to holding ball state
        if (!AttackerVariables.isBallOccupied && collider.gameObject == m_ball)
        {
            AttackerVariables.isBallOccupied = true;
            Vector3 ballMoveLocation = attacker.transform.position + attacker.transform.forward * 1.0f;
            m_ball.transform.SetParent(attacker.transform, true);
            m_ball.transform.position = ballMoveLocation;
            attacker.SwitchState(attacker.m_attackerHoldingBallState);
        }
    }
}