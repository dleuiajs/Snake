using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    private int score = 0;

    [Header("GameObjects")]
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] GameObject PausePanel;

    [Header("Bools")]
    private bool gamePaused;

    void Update()
    {
        // FullScreen Toggle
        if (Input.GetButtonDown("fullscreen"))
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

        // Pause Toggle
        if (Input.GetButtonDown("pause"))
        {
            gamePaused = !gamePaused;
            PausePanel.SetActive(gamePaused);
            if (gamePaused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
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
