using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using BNG;

public class MyCarController : MonoBehaviour
{
    public bool intheCar;

    public float steerCarFloat;
    public float accelerationFloat;
    public float handleBreakFloat;

    private CarUserControl carController;
    


    // Update is called once per frame

    private void Start()
    {
        carController = GetComponent<CarUserControl>();
    }


    void Update()
    {


        if (intheCar == true)
        {
            accelerationFloat = InputBridge.Instance.RightTrigger + -InputBridge.Instance.LeftTrigger;

            if (InputBridge.Instance.AButton) // turn true false to a float value
            {
                handleBreakFloat = 1;
            }
            else
            {
                handleBreakFloat = 0;
            }




            // set variables in carusercontroller
            carController.steerCar = steerCarFloat;
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
        steerCarFloat = -onValueChange; // negative because the car controller uses opposite values than the steering wheel
    }
}
