using UnityEngine;

public abstract class DefenderBaseState
{
    public abstract void EnterState(DefenderStateManager defender);

    public abstract void UpdateState(DefenderStateManager defender);

    public abstract void ExitState(DefenderStateManager defender);

    public abstract void OnCollisionEnter(DefenderStateManager defender, Collider collider);
}
