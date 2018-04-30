using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]  //////i lista sudeda inspektoriui kaip sarasas masyve
public class MovementN : System.Object
{
    public WheelCollider leftWheel;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheel;
    public GameObject rightWheelMesh;
    public bool motor;
    public bool steering;
    public bool reverseTurn;
}

public class MovementNetwork : NetworkBehaviour
{
   // [SyncVar]
    public string playerName;
    public float motorPower;
    public float steeringAngle;
    ///     forward or backward :D 
    public string inputV = "Vertical1";
    ///     steering
    public string inputH = "Horizontal1";
    ///     brakes
    public string brake = "Submit1";

    public string shoot = "Fire1";

    public GameObject bulletPref;

    public Transform bulletSpawn;

    public List<MovementN> carData;

    public float delayTime = 0.0f;
    public float shootDelay = 0.5f;
    public float tempTime = 0.5f;
    public float destroyBulletAfter = 30.0f;

    [SyncVar] public string bulletName;
    [SyncVar] public Color carColor;





    void MoveWheel(MovementN wheelPair)
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
        if (!isLocalPlayer)
        {
            return;
        }

        float motor = motorPower * Input.GetAxis(inputV);
        float steering = steeringAngle * Input.GetAxis(inputH);
        float brakePower = Mathf.Abs(Input.GetAxis(brake));

        if (brakePower > 0.001)
        {
            brakePower = motorPower;
            motor = 0;
        }
        else brakePower = 0;

        foreach (MovementN car in carData)
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
        ///FFFFIIIIIIRRRRRREEEE
        delayTime+=Time.deltaTime;
        if(Input.GetButton(shoot) && delayTime > shootDelay )///atgal is masina pisa, atitraukia atgal
        {
            shootDelay = delayTime + tempTime;

            CmdShooting();  

            shootDelay-=delayTime;
            delayTime = 0.0f;                      
        }
	}


    [Command]
    void CmdShooting()
    {
        var bullet = (GameObject) Instantiate(bulletPref, bulletSpawn.position, bulletSpawn.rotation);           

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;
        bulletName = gameObject.name + "B";
        bullet.name = bulletName;
        NetworkServer.Spawn(bullet);

        Debug.Log(gameObject.name);//jis ima ir kito zaidejo skripta
        //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GameObject.Find("Colliders").GetComponent<MeshCollider>());
        Destroy(bullet, destroyBulletAfter);        
    }

    [Command]
    void CmdSendPlNameToServer(string playerName)
    {
        RpcSetPlName(playerName);
    }
    [ClientRpc]
    void RpcSetPlName(string name)
    {
        gameObject.name = name;
    }

    ///kelt i networkManageri
      public override void OnStartLocalPlayer()
    {        ///////su cmd siusti kokia spalva uzdejo
        CmdSendPlNameToServer("Pl"+NetworkServer.connections.Count);
        Material[] aa= GameObject.Find("Car").GetComponent<MeshRenderer>().materials;///sinhronizuot spalva
        Material[] a1= GameObject.Find("DoorL").GetComponent<MeshRenderer>().materials;
        Material[] a2= GameObject.Find("DoorR").GetComponent<MeshRenderer>().materials;
        Material[] a3= GameObject.Find("Hood").GetComponent<MeshRenderer>().materials;
        Material[] a4= GameObject.Find("Trunk").GetComponent<MeshRenderer>().materials;
        carColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        aa[0].color = a1[0].color = a2[0].color =a3[0].color =a4[1].color =  carColor;		
    }
    

    
}