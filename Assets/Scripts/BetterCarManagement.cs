using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public enum DriveType
{
	RearWheelDrive,
	FrontWheelDrive,
	AllWheelDrive
}

public class BetterCarManagement : NetworkBehaviour
{
    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
	[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;
	private WheelCollider[] m_Wheels;
	////////////////////////////////////////////////////////////////////////////////////
	public string shoot = "Fire1";
    public GameObject bulletPref;
    public Transform bulletSpawn;
	public float delayTime = 0.0f;
    public float shootDelay = 0.5f;
    public float tempTime = 0.5f;
    public float destroyBulletAfter = 30.0f;
	[SyncVar] public string bulletName;    

    // Find all the WheelColliders down in the hierarchy.
	void Start()
	{
		m_Wheels = GetComponentsInChildren<WheelCollider>();
		for (int i = 0; i < m_Wheels.Length; ++i) 
		{
			var wheel = m_Wheels [i];
			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
	}

	// This is a really simple approach to updating wheels.
	// We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
	// This helps us to figure our which wheels are front ones and which are rear.
	void Update()
	{
		if (!isLocalPlayer)
        {
            return;
        }
		////////////////////////////////////////////////////////////////////////////////
		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
		float angle = maxAngle * Input.GetAxis("Horizontal1");
		float torque = maxTorque * Input.GetAxis("Vertical1");
		float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;
		foreach (WheelCollider wheel in m_Wheels)
		{
			// A simple car where front wheels steer while rear ones drive.
			if (-wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;
			if (wheel.transform.localPosition.z < 0)
			{
				wheel.brakeTorque = handBrake;
			}
			if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
			{
				wheel.motorTorque = torque;
			}
			if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
			{
				wheel.motorTorque = torque;
			}
			// Update visual wheels if any.
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);
				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}
		///////////////////////////////////////////////////////////////////////////////
		delayTime+=Time.deltaTime;
        if(Input.GetButton(shoot) && delayTime > shootDelay )///atgal is masina pisa, atitraukia atgal
        {
            shootDelay = delayTime + tempTime;
            CmdShooting();
            shootDelay-=delayTime;
            delayTime = 0.0f;                      
        }
	}
	///////////////////////////////////////////////////////////////////////////////////
    [Command]
    void CmdShooting()
    {
        var bullet = (GameObject) Instantiate(bulletPref, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;
        bulletName = gameObject.name + "B";
        bullet.name = bulletName;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, destroyBulletAfter);        
    }
	[Command]////kviecia komnda 
    void CmdSendPlNameToServer(string playerName)
    {
        RpcSetPlName(playerName);
    }
    [ClientRpc]///kliente esanti komanda gauna signala is servo
    void RpcSetPlName(string name)
    {
        gameObject.name = name;
    }
	//////////////////////////////////////////////////////////////////////////////////
	public override void OnStartLocalPlayer()
    {        ///////su cmd siusti kokia spalva uzdejo
        CmdSendPlNameToServer("Pl"+NetworkServer.connections.Count);
        Material[] aa= GameObject.Find("Car").GetComponent<MeshRenderer>().materials;///sinhronizuot spalva
        Material[] a1= GameObject.Find("DoorL").GetComponent<MeshRenderer>().materials;
        Material[] a2= GameObject.Find("DoorR").GetComponent<MeshRenderer>().materials;
        Material[] a3= GameObject.Find("Hood").GetComponent<MeshRenderer>().materials;
        Material[] a4= GameObject.Find("Trunk").GetComponent<MeshRenderer>().materials;
        aa[0].color = a1[0].color = a2[0].color =a3[0].color =a4[1].color = Color.red;		
    }
}
