using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMngr : MonoBehaviour {
    /// <summary>
    /// //jei daugiau saugot reiktu 
    /// https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/
    /// </summary>
    public static int score1=0;
    public static int score2=0;

    public Text scoreText;
    public void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        PlayerPrefs.GetInt("A", score1);
        PlayerPrefs.GetInt("B", score2);    
    }

    public void Update()
    {
        scoreText.text = score1.ToString() + " : " + score2.ToString();
        if (Input.GetKey("escape"))
        {
            PlayerPrefs.SetInt("A", 0);
            PlayerPrefs.SetInt("B", 0);
            PlayerPrefs.Save();
            score1 = score2 = 0;
            SceneManager.LoadScene("Menu");
        }
    }

    public void addPl1()
    {
        score1 += 1;
        PlayerPrefs.SetInt("A", score1);
        PlayerPrefs.Save();
    }

    public void addPl2()
    {
        score2 += 1;
        PlayerPrefs.SetInt("B", score2);
        PlayerPrefs.Save();
    }


    /*
public void SaveData()
{
    if (!Directory.Exists("Saves"))
        Directory.CreateDirectory("Saves");

    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile = File.Create("Saves/score.bin");

    formatter.Serialize(saveFile, score);

    saveFile.Close();
}

public void LoadData()
{
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile = File.Open("Saves/score.bin", FileMode.Open);
    score = formatter.Deserialize(saveFile);


}
*/

}
