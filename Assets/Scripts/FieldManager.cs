using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    [Header("Vars")]
    [HideInInspector] public Cell[,] cells;
    [HideInInspector] public int cellsPerRow;
    [HideInInspector] public int cellsPerColumn;
    int cellsTotal;

    [Header("Colors")]
    [SerializeField] Color backgroundColor;
    [SerializeField] Color notFilledCellColor;
    [SerializeField] Color filledCellColor;

    [Header("GameObjects")]
    [SerializeField] Transform GameField;
    RectTransform GameFieldRT;
    Image BackgroundImage;

    [Header("Prefabs")]
    [SerializeField] GameObject CellPrefab;

    [Header("Other")]
    GridLayoutGroup FieldGrid;

    [Header("Scripts")]
    SnakeManager snakeManager;
    FoodManager foodManager;

    public class Cell
    {
        GameObject obj;

        // Position
        public int x { get; set; }
        public int y { get; set; }

        // Direction
        public Vector2Int direction;

        // GameObjects
        private Image OutlineImg;
        private Image InsideImg;
        private Outline Outline;

        // Scripts
        FieldManager manager;

        public Cell(FieldManager m, int x, int y)
        {
            // задаем переменные
            manager = m;
            this.x = x;
            this.y = y;

            // создаем нашу клетку
            obj = Instantiate(manager.CellPrefab, manager.GameField);

            // задаем InsideImg (наш пиксель)
            InsideImg = obj.transform.Find("InsideImg").GetComponent<Image>();
            OutlineImg = obj.GetComponent<Image>();
            Outline = InsideImg.GetComponent<Outline>();
            Outline.effectColor = manager.backgroundColor;
            Clear();
        }

        public void FillCustomColor(Color color)
        {
            InsideImg.color = color;
            OutlineImg.color = color;
        }

        public void Fill()
        {
            FillCustomColor(manager.filledCellColor);
        }

        public void Clear()
        {
            InsideImg.color = manager.notFilledCellColor;
            OutlineImg.color = manager.notFilledCellColor;
        }
    }

    void Start()
    {
        // загрузка
        GameFieldRT = GameField.GetComponent<RectTransform>();
        FieldGrid = GameField.GetComponent<GridLayoutGroup>();
        BackgroundImage = GameField.GetComponent<Image>();
        snakeManager = GetComponent<SnakeManager>();
        foodManager = GetComponent<FoodManager>();

        // задаем значения
        BackgroundImage.color = backgroundColor;
        cellsPerRow = Mathf.FloorToInt((GameFieldRT.sizeDelta.x - FieldGrid.padding.horizontal) / (FieldGrid.cellSize.x + FieldGrid.spacing.x));
        cellsPerColumn = Mathf.FloorToInt((GameFieldRT.sizeDelta.y - FieldGrid.padding.vertical) / (FieldGrid.cellSize.y + FieldGrid.spacing.y));
        cellsTotal = cellsPerRow * cellsPerColumn;
        Debug.Log($"Cells per row: {cellsPerRow}, Cells per column: {cellsPerColumn}, Total cells: {cellsTotal}");
        cells = new Cell[cellsPerRow, cellsPerColumn];
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                cells[y, x] = new Cell(this, x, y);
                Debug.Log($"Created new cell at x: {x}, y: {y}");
            }
        }

        // создаем червя
        snakeManager.CreateSnake();

        // создаем еду
        foodManager.GenerateFood();
    }
}
