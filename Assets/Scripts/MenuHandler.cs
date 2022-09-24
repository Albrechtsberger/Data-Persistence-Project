using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuHandler : MonoBehaviour
{

    public Text inputField;
    public Text bestScore;
    public static string playerName;
    public string leaderName;
    public int leaderScore;

    // Start is called before the first frame update
    void Start()
    {
        Load();
        bestScore.text = "BEST SCORE: " + leaderName + " " + leaderScore;

        InputName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);

            leaderScore = data.leaderScore;
            leaderName = data.pName;
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   public void QuitButton()
    {
        Application.Quit();
    }

    public void InputName()
    {
        playerName = inputField.text;
    }
}
