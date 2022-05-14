using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public GameManager gameManagerScript;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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

        SetBestScoreText();
    }

    private void Update()
    {
        bool alreadyHaveUser = false;

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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("menu");
            }
        }

        if (gameManagerScript.bestScore.Count != 0)
        {
            foreach (string item in gameManagerScript.userName)
            {
                if (item == gameManagerScript.tempUserName)
                {
                    if (m_Points > gameManagerScript.bestScore[gameManagerScript.userName.IndexOf(item)])
                    {
                        gameManagerScript.bestScore[gameManagerScript.userName.IndexOf(item)] = m_Points;
                        gameManagerScript.SaveDataToJSON();
                    }
                    alreadyHaveUser = true;
                    break;
                }
            }
        }

        if (!alreadyHaveUser && m_Points >= 1)
        {
            gameManagerScript.userName.Add(gameManagerScript.tempUserName);
            gameManagerScript.bestScore.Add(m_Points);
            gameManagerScript.SaveDataToJSON();
        }


        SetBestScoreText();
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

    private void SetBestScoreText()
    {
        if (gameManagerScript.bestScore.Count == 0)
        {
            bestScoreText.text = "Best Score: " + gameManagerScript.tempUserName + ": 0";
        }
        else
        {
            bestScoreText.text = "Best Score: " + gameManagerScript.tempUserName + ": " + gameManagerScript.bestScore[gameManagerScript.userName.IndexOf(gameManagerScript.tempUserName)];
        }
    }
}
