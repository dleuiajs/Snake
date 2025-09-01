using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    private int score = 0;

    [Header("GameObjects")]
    [SerializeField] Text ScoreText;

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
        ScoreText.text = $"Score: {score}";
    }
}
