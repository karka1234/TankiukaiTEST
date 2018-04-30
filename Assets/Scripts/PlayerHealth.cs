using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : MonoBehaviour
{
	public int PlayerNum = 1;
	private string aa;

    private GameMngr gM = new GameMngr();

    private float startHealth = 100f;

    private float currentHealth;

	public Text showHealth = null;
    /// <summary>
    /// T//kad paimtu masinos varda ir atskirtu taip zaideja	/// </summary>

    private bool dead = false;

    private void OnEnable()
	{
		aa = "Pl"+PlayerNum;
		showHealth = GameObject.Find (aa).GetComponent<Text> ();
        currentHealth = startHealth;
        dead = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name+ " - " + amount);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
		//Debug.Log("Mire:"+gameObject.name);
        dead = true;

        if (gameObject.name.Equals("Player1(Clone)"))
            gM.addPl2();
        else if(gameObject.name.Equals("Player2(Clone)"))
            gM.addPl1();
          


        SceneManager.LoadScene("Main");////i nauja skripta 
    }

    void Update()
	{
		showHealth.text = PlayerNum + " : " + currentHealth.ToString();
	}
}