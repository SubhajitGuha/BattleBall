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
    private Material m_oldMaterial;
    public override void EnterState(AttackerStateManager attacker)
    {
        attacker.isActive = true;
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

        if(attacker.attackerHoldingBallMaterial == null)
        {
            Debug.Log(string.Format("assign holding ball material to {}", attacker.ToString()));
        }
        //change the material when holding the ball
        var renderer = attacker.attackerBody.GetComponent<Renderer>();
        m_oldMaterial = renderer.material;
        renderer.material = attacker.attackerHoldingBallMaterial;
    }

    public override void UpdateState(AttackerStateManager attacker)
    {
        float ballToAttackerDist = Vector3.Distance(m_ball.transform.position, attacker.transform.position);
        
        if (!m_isBallClose)
        {
            //this attacker is waiting to receive the ball
            Vector3 ballMoveLocation = attacker.transform.position + attacker.transform.forward * 1.0f * MyUtils.FieldScale;
            float step = AttackerVariables.BallSpeed * Time.deltaTime;
            m_ball.transform.position = MyUtils.TranslateOnXZPlane(
                m_ball.transform.position,
                attacker.transform.position,
                step);

            m_ball.transform.forward = MyUtils.GetDirectionInXZPlane(
                m_ball.transform.position,
                attacker.transform.position);
            if (ballToAttackerDist <= 1.0f * MyUtils.FieldScale)
            {
                m_ball.transform.position = MyUtils.CreateVecOnXZPlane(ballMoveLocation.x, ballMoveLocation.z);
                m_isBallClose = true;
            }

        }
        else
        {
            //once the ball is received move towards the enemy gate
            Vector3 currentPos = attacker.transform.position;
            float step = AttackerVariables.CarryingSpeed * Time.deltaTime;
            attacker.transform.position = MyUtils.TranslateOnXZPlane(
                currentPos,
                m_defenderGate.transform.position,
                step);

            attacker.transform.forward = MyUtils.GetDirectionInXZPlane(
                attacker.transform.position,
                m_defenderGate.transform.position);
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
           if (fromAttacker == otherAttacker || !otherAttacker.isActive)
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
            GameManager.instance.AttackerWins();
            Debug.Log("Attacker wins");
            return;
        }
        //if attacker collides with the defender and the defender is active then pass the ball to the nearest attacker
        DefenderStateManager defender = collider.gameObject.GetComponent<DefenderStateManager>();
        if (defender != null && defender.isActive)
        {
            Debug.Log("Defender caught the attacker");
            //get nearest active attaker
            AttackerStateManager nearestAttacker = getNearestAttacker(attacker);
            //if no attacker is found nearby game over, defender won
            if (nearestAttacker == null)
            {
                //Game over Defender wins
                GameManager.instance.DefenderWins();
                Debug.Log("game over, defender wins");
                return;
            }
            else
            {
                //pass the ball to the nearest attacker
                m_ball.transform.SetParent(nearestAttacker.transform, true);
                nearestAttacker.SwitchState(nearestAttacker.m_attackerHoldingBallState);
            }

            //reassign the old material
            var renderer = attacker.attackerBody.GetComponent<Renderer>();
            renderer.material = m_oldMaterial;
            attacker.currentTimer = AttackerVariables.ReactivateTime;
            attacker.SwitchState(attacker.m_attackerInactiveState);
        }

    }
}
