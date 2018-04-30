using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
///
public class PlayerHealth1 : NetworkBehaviour
{
    public const int maxHealth = 100;//nesikeicianti per visa game const or static
    [SyncVar(hook = "onHealthChange")]   ////surisa kad visi matytu servas klientas ir kitas zaidejas
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
    private NetworkStartPosition[] spawnPoints;
    Vector3 spwanPoint = Vector3.zero;
    public GameManager gM = new GameManager();
    

    void Start()
    {
        if(isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();/////padaryt kad is maze ar i  maze kazkaip kazka//maze skripte kad surinktu keturis taskus starto
        }
    }

    void Update()
    {/*
      if(Input.GetKeyDown("escape"))///atgal is masina pisa, atitraukia atgal
        {
            //ClientScene.ClearSpawners();
            NetworkManagerA.singleton.ServerChangeScene("MazeMP");
            //SceneManager.LoadScene("MazeMP");
            
            
            Debug.Log("aasadasjnfgbjhsdgbjfhsdgsdfggsdhjfgsadhjhxczv");
        }*/
    }

    public void TakeDamage(int amount)
    {
        if(!isServer)
        {
            return;
        }
        currentHealth -= amount;
        //Debug.Log(gameObject.name + " - " + amount);  //////skirtingos spalvos kulkos ir tikrint, keist taip kaip ir masinos spalva keiciau        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Debug.Log("Numire  :::::::::::::   " + gameObject.name);
            // RpcRespawn();////nauja  scena dadeda per nauja nereik respawno           
            // ServerChangeScene("MazeMP");
            //NetworkServer.singleton.ServerChangeScene("MazeMP");
           /* if(gameObject.name == "Pl1")
                gM.addPl2();
            else
                gM.addPl1();*/

            NetworkManagerA.singleton.ServerChangeScene("MazeMP");

        }  
    }

    void onHealthChange(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        /*
        if(isLocalPlayer)
        {
            //transform.position = Vector3.zero;
            if(spawnPoints!=null&&spawnPoints.Length>0)
            {
                spwanPoint = spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position;
            }
            transform.position = spwanPoint;
        }
        */
    }
}