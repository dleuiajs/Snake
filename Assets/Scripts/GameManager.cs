using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    private int score = 0;

    [Header("GameObjects")]
    [SerializeField] TMP_Text ScoreText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.F11))
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(1280, 720, false);
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
            }
        }
    }

    public void Lose()
    {
        Debug.Log("You lose!");
    }

    public void ScoreIncrease()
    {
        score++;
        UpdateText();
    }

    void UpdateText()
    {
        ScoreText.text = $"Score:\n{score:000000}";
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
