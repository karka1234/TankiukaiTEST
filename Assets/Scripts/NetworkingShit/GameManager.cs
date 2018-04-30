using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {

	[SyncVar] double score = 0.00;/////arba taskus arba pati canvasa
	
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(GetComponent<GameManager>());
	}
	
	// Update is called once per frame
	void Update () {
		///parodyt ant canvaso kameros
		//GameObject.Find("Score").GetComponent<Text>().text = score.ToString();
		
	} 

	public void addPl1()
	{
		score+=1.00;
	}
	public void addPl2()
	{
		score+=0.01;
	}
	
}
