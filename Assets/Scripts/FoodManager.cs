using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] Color foodColor;
    [Header("Food")]
    FieldManager.Cell FoodCell;

    [Header("Scripts")]
    FieldManager fieldManager;
    SnakeManager snakeManager;
    GameManager gameManager;

    void Awake()
    {
        fieldManager = GetComponent<FieldManager>();
        snakeManager = GetComponent<SnakeManager>();
        gameManager = GetComponent<GameManager>();
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
        do
        {
            FoodCell = fieldManager.cells[Random.Range(0, fieldManager.cellsPerColumn), Random.Range(0, fieldManager.cellsPerRow)];
        }
        while (snakeManager.snakeCells.Contains(FoodCell));
        FoodCell.FillCustomColor(foodColor);
        Debug.Log($"Food: {FoodCell.x}, {FoodCell.y}");
    }

    void EatFood()
    {
        if (FoodCell != null)
        {
            FieldManager.Cell lastSnakeCell = snakeManager.snakeCells[snakeManager.snakeCells.Count - 1];
            FoodCell = null;
            gameManager.ScoreIncrease();
            snakeManager.Grow();
            GenerateFood();
        }
    }
}
