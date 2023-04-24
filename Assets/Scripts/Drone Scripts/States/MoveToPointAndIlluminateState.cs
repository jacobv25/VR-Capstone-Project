// MoveToPointAndIlluminateState.cs
public class MoveToPointAndIlluminateState : DroneState
{
    private IDroneMovement moveToTargetAndIlluminateMovement;

    public MoveToPointAndIlluminateState(DroneController droneController) : base(droneController)
    {
        moveToTargetAndIlluminateMovement = new MoveToPointAndIlluminateMovement(droneController);
    }

    public override void EnterState()
    {
        droneController.SetMovement(moveToTargetAndIlluminateMovement);
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
