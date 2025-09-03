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
    [SerializeField] TMP_Text PauseText;
    [SerializeField] GameObject ContinueButton;
    [SerializeField] GameObject ExitButton;

    [Header("Bools")]
    private bool gamePaused;
    private bool onlyResetMenuOpened;

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

        // Restart Game
        if (Input.GetButtonDown("restart"))
        {
            Restart();
        }
    }

    void SetHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
        }
        savesManager.SaveGame();
    }

    public void Lose()
    {
        OnlyResetMenuEnable("You lost :(");
    }

    public void Win()
    {
        OnlyResetMenuEnable("You won!");
    }

    void OnlyResetMenuEnable(string text)
    {
        PauseToggle();
        PauseText.text = text;
        ContinueButton.SetActive(false);
        ExitButton.SetActive(false);
        onlyResetMenuOpened = true;
        SetHighscore();
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

    public void Restart()
    {
        if (gamePaused)
        {
            onlyResetMenuOpened = false;
            PauseToggle();
        }
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
            if (!gamePaused)
                Cursor.visible = false;
        }
    }

    public void PauseToggle()
    {
        if (!onlyResetMenuOpened)
        {
            gamePaused = !gamePaused;
            PausePanel.SetActive(gamePaused);
            if (gamePaused)
            {
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            else
            {
                if (Screen.fullScreen)
                    Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void Exit()
    {
        Lose();
        Application.Quit();
    }
}
