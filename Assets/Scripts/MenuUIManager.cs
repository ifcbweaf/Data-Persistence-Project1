using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIManager : MonoBehaviour
{
    public GameManager gameManagerScript;

    public InputField nameInputField;
    public Text bestScoreText;

    private void Awake()
    {
        gameManagerScript = GameManager.Instance;
        nameInputField.text = gameManagerScript.tempUserName;
        SetBestScoreText();
        gameManagerScript.LoadFileFromJSON();
    }

    //switch to main scene
    public void SwitchToMainScene()
    {
        if (nameInputField.text != "")
        {
            SceneManager.LoadScene("main");
        }
    }

    //quits game or exit playmode if unity editor
    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void SetTempUserName()
    {
        gameManagerScript.tempUserName = nameInputField.text;
        SetBestScoreText();
    }

    private void SetBestScoreText()
    {
        if (gameManagerScript.userName.Count != 0 && gameManagerScript.userName.Contains(nameInputField.text))
        {
            bestScoreText.text = "Best Score: " + gameManagerScript.bestScore[gameManagerScript.userName.IndexOf(gameManagerScript.tempUserName)];
        }
        else
        {
            bestScoreText.text = "Best Score: ";
        }
    }
}
