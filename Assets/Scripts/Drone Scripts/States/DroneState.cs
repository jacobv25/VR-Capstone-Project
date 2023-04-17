public abstract class DroneState
{
    protected DroneController droneController;

    public DroneState(DroneController droneController)
    {
        this.droneController = droneController;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}

