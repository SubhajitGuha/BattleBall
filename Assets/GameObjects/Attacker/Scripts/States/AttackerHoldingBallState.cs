using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackerHoldingBallState : AttackerBaseState
{
    private GameObject m_ball = null;
    private GameObject m_defenderGate;
    private bool m_isBallClose; //variable that determines whether the ball is close to the attacker or not
    public override void EnterState(AttackerStateManager attacker)
    {
        attacker.IsActive = true;
        m_isBallClose = false;
        m_ball = GameObject.FindGameObjectWithTag("Ball");
        m_defenderGate = GameObject.FindGameObjectWithTag("DefenderGate");
        if (m_ball == null)
        {
            Debug.Log("palce the ball object on the level");
        }
        if (m_defenderGate == null)
        {
            Debug.Log("palce the gate object on the level");
        }
    }

    public override void UpdateState(AttackerStateManager attacker)
    {
        float ballToAttackerDist = Vector3.Distance(m_ball.transform.position, attacker.transform.position);
        
        if (!m_isBallClose)
        {
            //this attacker is waiting to receive the ball
            Vector3 ballMoveLocation = attacker.transform.position + attacker.transform.forward * 1.0f;
            Vector3 dir = (attacker.transform.position - m_ball.transform.position).normalized;
            Vector3 changeAxis = new Vector3(1.0f, 0.0f, 1.0f); //donot change the y-axis
            m_ball.transform.position += dir.Multiply(changeAxis) * AttackerVariables.BallSpeed * Time.deltaTime;
            m_ball.transform.forward = dir.Multiply(changeAxis);
            if(ballToAttackerDist <= 1.0f)
            {
                m_ball.transform.position = ballMoveLocation;
                m_isBallClose = true;
            }

        }
        else
        {
            //once the ball is received move towards the enemy gate
            Vector3 currentPos = attacker.transform.position;
            Vector3 direction = (m_defenderGate.transform.position - currentPos).normalized;
            Vector3 changeAxis = new Vector3(1.0f, 0.0f, 1.0f); //donot change the y-axis
            attacker.transform.position += direction.Multiply(changeAxis) * AttackerVariables.CarryingSpeed * Time.deltaTime;
            attacker.transform.forward = direction.Multiply(changeAxis);
        }
    }

    public override void ExitState(AttackerStateManager state)
    {

    }

    private AttackerStateManager getNearestAttacker(in AttackerStateManager fromAttacker)
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Attacker");

        float minDist = float.MaxValue;
        AttackerStateManager nearestAttacker = null;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            //get the script attached to this gameobject
            AttackerStateManager otherAttacker = gameObjects[i].GetComponent<AttackerStateManager>();
            //if not active move to next attacker
           if (fromAttacker == otherAttacker || !otherAttacker.IsActive)
               continue;

           float dist = Vector3.Distance(gameObjects[i].transform.position, fromAttacker.transform.position);
            //get the minimum distance
           if (dist < minDist)
           {
               minDist = dist;
               nearestAttacker = otherAttacker;
           }
        }

        return nearestAttacker;
    }

    public override void OnCollisionEnter(AttackerStateManager attacker, Collider collider)
    {

        if (!m_isBallClose)
            return;

        if (collider.gameObject == m_defenderGate)
        {
            //game over Attacker wins
            Debug.Log("Attacker wins");
            return;
        }
        //if attacker collides with the defender then pass the ball to the nearest attacker
        if(collider.gameObject.GetComponent<DefenderStateManager>() != null)
        {
            Debug.Log("Defender caught");
            //get nearest active attaker
            AttackerStateManager nearestAttacker = getNearestAttacker(attacker);
            //if no attacker is found nearby game over defender won
            if (nearestAttacker == null)
            {
                //Game over Defender wins
                Debug.Log("game over, defender wins");
                return;
            }
            else
            {
                //pass the ball to the nearest attacker
                m_ball.transform.SetParent(nearestAttacker.transform, true);
                nearestAttacker.SwitchState(nearestAttacker.m_attackerHoldingBallState);
            }
            attacker.CurrentTimer = AttackerVariables.ReactivateTime;
            attacker.SwitchState(attacker.m_attackerInactiveState);
        }

    }
}
