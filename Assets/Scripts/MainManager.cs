using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreBest;
    public GameObject GameOverText;

    public int leaderScore;
    public string leaderName;
    private string pName;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static MainManager Instance;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        Load();

        pName = MenuHandler.playerName;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        ShowBestScore();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    void ShowBestScore()
    {
        ScoreBest.text = "BESTSCORE: " + leaderName + " " + leaderScore;
        if (m_GameOver)
        {
            if (leaderScore < m_Points)
            {
                leaderScore = m_Points;
                leaderName = pName;
                Save();
            }
        }

    }

    [System.Serializable]

    public class SaveData
    {
        public string pName;
        public int leaderScore;
    }

    public void Save()
    {
        SaveData data = new SaveData();

        data.leaderScore = leaderScore;
        data.pName = leaderName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            leaderScore = data.leaderScore;
            leaderName = data.pName;
        }
    }

}
