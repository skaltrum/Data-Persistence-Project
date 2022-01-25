using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string m_UserName;

    
    // Start is called before the first frame update
    void Start()
    {
        m_UserName = UserManager.Instance ? UserManager.Instance.PlayerName : "Unknown";

        AddPoint(0);

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

        SetHighScoreText();
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
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_UserName} | {m_Points}";

        SetHighScoreText();
    }

    public void GameOver()
    {
        if (m_Points > UserManager.Instance.HighScoreData.HighScore)
        {
            Debug.Log("New highscore detected");
            UserManager.Instance.SaveHighScore(m_UserName, m_Points);
            SetHighScoreText();
        }

        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    void SetHighScoreText()
    {
        string hs_Name = "";
        int hs_Score = 0;

        if(UserManager.Instance != null)
        {
            hs_Name = UserManager.Instance.HighScoreData.Name;
            hs_Score = UserManager.Instance.HighScoreData.HighScore;
        }

        if(m_Points > hs_Score)
        {
            hs_Score = m_Points;
            hs_Name = m_UserName;
        }

        HighScoreText.text = $"High Score: {hs_Name} | {hs_Score}";
    }
}
