// MoveToPointState.cs
public class MoveToPointState : DroneState
{
    private IDroneMovement moveToPointMovement;

    public MoveToPointState(DroneController droneController) : base(droneController)
    {
        moveToPointMovement = new MoveToPointMovement();
    }

    public override void EnterState()
    {
        droneController.SetMovement(moveToPointMovement);
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