using UnityEngine;

public class FightState : DroneState
{
    private IDroneMovement attackEnemyMovement;

    public FightState(DroneController droneController, GameObject enemy, Transform weaponTransform) : base(droneController)
    {
        attackEnemyMovement = new AttackEnemyMovement(droneController, enemy, weaponTransform);
    }

    public override void EnterState()
    {
        droneController.SetMovement(attackEnemyMovement);
    }

    public override void UpdateState()
    {
        // Handle any state-specific updates, if necessary.
    }

    public override void ExitState()
    {
        // Handle any state-specific cleanup, if necessary.
    }
}
