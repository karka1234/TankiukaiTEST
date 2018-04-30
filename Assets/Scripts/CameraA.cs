using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraA : NetworkBehaviour {
    public Camera camera;
    GameObject[] players = null;
    [SyncVar] Vector3 center;
    [SyncVar] Vector3 cameraPos = new Vector3(0.0f,50.0f,0.0f);
    [SyncVar]  float distance = 0.0f;
    [SyncVar]  float viewSize = 0.0f;
	[SyncVar]  float distanceToCamera = 0.0f;
    Transform a = null;
    Transform b = null;

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players.Length);
        
        if(players.Length == 1)
        {
            a = players[0].transform;

            Transform lightLookAt = GameObject.FindGameObjectWithTag("Light").transform;
            lightLookAt.LookAt(a.position);

            cameraPos.x = a.position.x;
            cameraPos.z = a.position.z;
            cameraPos.y = 50.0f;
            camera.transform.position = cameraPos;
            camera.fieldOfView = 90;
        }else if(players.Length == 2)
        {
            a = players[0].transform;
            b = players[1].transform;

            center = a.position + 0.5f * (b.position - a.position);

            Transform lightLookAt = GameObject.FindGameObjectWithTag("Light").transform;
            lightLookAt.LookAt(center);

                    cameraPos.x = center.x;
                 cameraPos.z = center.z;
            cameraPos.y = 60.0f;
            camera.transform.position = cameraPos;

            distance = Vector3.Distance(b.position, a.position);
            distanceToCamera = (camera.transform.position - center).magnitude;

            viewSize = distance + 10.0f;
            if (viewSize < 90)
             camera.fieldOfView = viewSize;
            else
                camera.fieldOfView = 90;
        }////kad ziuretu i kazak
    }
}

