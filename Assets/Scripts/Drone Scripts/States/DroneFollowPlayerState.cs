// FollowPlayerState.cs
public class FollowPlayerState : DroneState
{
    private IDroneMovement followPlayerMovement;

    public FollowPlayerState(DroneController droneController) : base(droneController)
    {
        followPlayerMovement = new FollowPlayerMovement(droneController);
    }

    public override void EnterState()
    {
        droneController.SetMovement(followPlayerMovement);
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