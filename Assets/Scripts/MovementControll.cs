using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]  ////padaro kad eitu keist reiksmes inspektoriui
public class Movement : System.Object
{
    public WheelCollider leftWheel;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheel;
    public GameObject rightWheelMesh;
    public bool motor;
    public bool steering;
    public bool reverseTurn;
}

public class MovementControll : NetworkBehaviour
{
    public int playerNum = 1;
    public float motorPower;
    public float steeringAngle;
    ///     forward or backward :D 
    public string inputV = "Vertical2";
    ///     steering
    public string inputH = "Horizontal2";
    ///     brakes
    public string brake = "Submit";

    public List<Movement> carData;

    public void MoveWheel(Movement wheelPair)
    {
        Quaternion rot;
        Vector3 pos;
        wheelPair.leftWheel.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;
        wheelPair.rightWheel.GetWorldPose(out pos, out rot);
        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = rot;
    }

	void Update () {

        float motor = motorPower * Input.GetAxis(inputV);
        float steering = steeringAngle * Input.GetAxis(inputH);
        float brakePower = Mathf.Abs(Input.GetAxis(brake));
        if (brakePower > 0.001)
        {
            brakePower = motorPower;
            motor = 0;
        }
        else brakePower = 0;

        foreach (Movement car in carData)
        {
            if (car.steering == true)
            {
                car.leftWheel.steerAngle = car.rightWheel.steerAngle = ((car.reverseTurn) ? -1 : 1) * steering;
            }

            if (car.motor == true)
            {
                car.leftWheel.motorTorque = motor;
                car.rightWheel.motorTorque = motor;
            }
            car.leftWheel.brakeTorque = brakePower;
            car.rightWheel.brakeTorque = brakePower;

            MoveWheel(car);
        }
	}
}