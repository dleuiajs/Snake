using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] Color foodColor;

    [Header("Food")]
    FieldManager.Cell FoodCell;

    [Header("AudioClips")]
    [SerializeField] AudioClip FoodEatClip;

    [Header("Scripts")]
    [SerializeField] GameManager gameManager;
    FieldManager fieldManager;
    SnakeManager snakeManager;
    [SerializeField] AudioManager audioManager;

    void Awake()
    {
        fieldManager = GetComponent<FieldManager>();
        snakeManager = GetComponent<SnakeManager>();
    }

    void Update()
    {
        if (snakeManager.snakeCells.Contains(FoodCell))
        {
            EatFood();
        }
    }

    public void GenerateFood()
    {
        if (snakeManager.snakeCells.Count < fieldManager.cells.Length)
        {
            do
            {
                FoodCell = fieldManager.cells[Random.Range(0, fieldManager.cellsPerColumn), Random.Range(0, fieldManager.cellsPerRow)];
            }
            while (snakeManager.snakeCells.Contains(FoodCell));
            FoodCell.FillCustomColor(foodColor);
            Debug.Log($"Food: {FoodCell.x}, {FoodCell.y}");
        }
    }

    void EatFood()
    {
        if (FoodCell != null)
        {
            FoodCell = null;
            gameManager.ScoreIncrease();
            snakeManager.Grow();
            GenerateFood();
            audioManager.PlaySound(FoodEatClip);
        }
    }
}
