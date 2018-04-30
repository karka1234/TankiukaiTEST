using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerA : NetworkManager {
	////nauja scene resetint regintus psawn pointus
	///////////////////////////////laikinai kai prisijungia atspawnin
	private NetworkStartPosition[] spawnPoints;
	private NetworkStartPosition[] tempSpawn;
	private Vector3 spawnPoint;
	private int random = 0;

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{//////spawnPrefabs kazkaip kazka idet kordinates ir kad random paimtu
		spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		//Debug.Log(spawnPoints.Length);
		random = Random.Range(3,spawnPoints.Length);

		if(spawnPoints[random].tag != "UsedSpawn")
		{
			spawnPoint = spawnPoints[random].transform.position;
			spawnPoints[random].tag = "UsedSpawn";
		}else spawnPoint = spawnPoints[0].transform.position;


		var player = (GameObject)GameObject.Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}


	void Awake()
	{
		//Network.sendRate = 25;
	}
/*
	public override void OnClientSceneChanged(NetworkConnection conn)
	{
//			base.OnClientSceneChanged(conn);

					spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		Debug.Log(spawnPoints.Length);
		spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position;;

		var player = (GameObject)GameObject.Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

		mapSeed += 123; 

		NetworkServer.Spawn(player);

		//	ClientScene.Ready(conn);
	}
*/
	/*
	int count = 1;
	
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
	
		GameObject player = (GameObject)Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
		//skripptas kuris tikrina kiek zaideju ir pagal tai uzzdeda taga

		if(count >= 1)
			player.name = "Pl"+count;
		count+=1;

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
						
	}
	
	public override void OnServerDisconnect(NetworkConnection conn)
	{
		count -= 1;
	}
*/
}
