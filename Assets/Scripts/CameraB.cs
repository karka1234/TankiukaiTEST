using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraB : MonoBehaviour
{
	public Transform LightLookAt=null;

    public Camera camera = null;
    public Transform test1=null;
    public Transform test2=null;
    private GameObject p1;
    private GameObject p2;
    Vector3 center;
    // Vector3 cameraPos = Camera.current.transform.position;

        /// <summary>
        /// /rast mapo centras ir nuo ten immmmt   cameros vieta
        /// </summary>
    Vector3 cameraPos = new Vector3(4.0f,60.0f,18.0f);

    float distance = 0.0f;
    float viewSize = 0.0f;

	float distanceToCamera = 0.0f;

    void Start()
    {
       

            p1 = GameObject.Find("Player1(Clone)");
            p2 = GameObject.Find("Player2(Clone)");
        test1 = p1.transform;
        test2 = p2.transform;

    }

    void Update()
    {
		///ifas jeigu tik vienas zaidejas  
		/// ////jei butu dar daughaiu zaideju
		/// 
        /*
        center = ((test2.position - test1.position) / 2.0f) + test1.position;
        transform.LookAt(center);
        */
        center = test1.position + 0.5f * (test2.position - test1.position);

		if(LightLookAt != null)
			LightLookAt.LookAt (center);


        //transform.LookAt(center);
        cameraPos.x = center.x;
        cameraPos.z = center.z;
        cameraPos.y = 60.0f;
        camera.transform.position = cameraPos;

        distance = Vector3.Distance(test2.position, test1.position);
        distanceToCamera = (camera.transform.position - center).magnitude;

        viewSize = distance + 10.0f;
        if (viewSize < 90)
            camera.fieldOfView = viewSize;
        else
            camera.fieldOfView = 90;




       // Debug.Log(distance);
        
    }


}