using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using BNG;

public class MyCarController : MonoBehaviour
{
    public bool intheCar;

    public float steerCarFloat;
    public float accelerationFloat;
    public float handleBreakFloat;

    public SteeringWheel steeringWheel; // Add a reference to the SteeringWheel script

    private CarUserControl carController;

    private void Start()
    {
        carController = GetComponent<CarUserControl>();

        // Subscribe to the onValueChange event
        if (steeringWheel != null)
        {
            steeringWheel.onValueChange.AddListener(CarSteering);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the onValueChange event
        if (steeringWheel != null)
        {
            steeringWheel.onValueChange.RemoveListener(CarSteering);
        }
    }

    void FixedUpdate()
    {
        if (intheCar == true)
        {
            accelerationFloat = InputBridge.Instance.RightTrigger + -InputBridge.Instance.LeftTrigger;

            if (InputBridge.Instance.AButton)
            {
                handleBreakFloat = 1;
            }
            else
            {
                handleBreakFloat = 0;
            }

            // set variables in carusercontroller
            //carController.steerCar = steerCarFloat;
            carController.acceleration = accelerationFloat;
            carController.handBrakeFloat = handleBreakFloat;
        }

        else
        {
            // cuts car input values to zero if you are not in the car
            carController.steerCar = 0;
            carController.acceleration = 0;
            carController.handBrakeFloat = 0;
        }
    }


    public void CarSteering(float onValueChange)
    {
        Debug.Log("my car controller steering:" + onValueChange);
        carController.steerCar = -onValueChange; // negative because the car controller uses opposite values than the steering wheel
    }
}
