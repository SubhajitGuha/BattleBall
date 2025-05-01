using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackerBaseState
{
    public abstract void EnterState(AttackerStateManager attacker);

    public abstract void UpdateState(AttackerStateManager state);

    public abstract void ExitState(AttackerStateManager state);

    public abstract void OnCollisionEnter(AttackerStateManager state, Collider collider);
}
