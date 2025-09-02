using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    [HideInInspector] public int highscore = 0;
    private int score = 0;

    [Header("GameObjects")]
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text HighscoreText;
    [SerializeField] GameObject PausePanel;

    [Header("Bools")]
    private bool gamePaused;

    [Header("Scripts")]
    [SerializeField] SavesManager savesManager;

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        // FullScreen Toggle
        if (Input.GetButtonDown("fullscreen"))
        {
            FullScreenToggle();
        }

        // Pause Toggle
        if (Input.GetButtonDown("pause"))
        {
            PauseToggle();
        }
    }

    public void Lose()
    {
        Debug.Log("You lose!");
        if (score > highscore)
        {
            highscore = score;
        }
        savesManager.SaveGame();
    }

    public void ScoreIncrease()
    {
        score++;
        UpdateText();
    }

    void UpdateText()
    {
        ScoreText.text = $"Score:\n{score:000000}";
        HighscoreText.text = $"Highscore:\n{highscore:000000}";
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FullScreenToggle()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, false);
            Cursor.visible = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
            Cursor.visible = false;
        }
    }

    public void PauseToggle()
    {
        gamePaused = !gamePaused;
        PausePanel.SetActive(gamePaused);
        if (gamePaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void Exit()
    {
        Lose();
        Application.Quit();
    }
}
