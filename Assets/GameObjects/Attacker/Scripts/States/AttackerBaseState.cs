using UnityEngine;

public abstract class AttackerBaseState
{
    public abstract void EnterState(AttackerStateManager attacker);

    public abstract void UpdateState(AttackerStateManager attacker);

    public abstract void ExitState(AttackerStateManager attacker);

    public abstract void OnCollisionEnter(AttackerStateManager attacker, Collider collider);
}
