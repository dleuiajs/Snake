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
    [SerializeField] Color notFilledCellColor;
    [SerializeField] Color filledCellColor;

    [Header("GameObjects")]
    [SerializeField] Transform GameField;
    RectTransform GameFieldRT;

    [Header("Prefabs")]
    [SerializeField] GameObject CellPrefab;

    [Header("Other")]
    GridLayoutGroup FieldGrid;

    [Header("Scripts")]
    SnakeManager snakeManager;

    public class Cell
    {
        GameObject obj;

        // Position
        public int x { get; set; }
        public int y { get; set; }

        // GameObjects
        private Image InsideImg;

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
        }

        public void Fill()
        {
            InsideImg.color = manager.filledCellColor;
        }

        public void Clear()
        {
            InsideImg.color = manager.notFilledCellColor;
        }
    }

    void Start()
    {
        // загрузка
        GameFieldRT = GameField.GetComponent<RectTransform>();
        FieldGrid = GameField.GetComponent<GridLayoutGroup>();
        snakeManager = GetComponent<SnakeManager>();

        // задаем значения
        cellsPerRow = Mathf.FloorToInt(GameFieldRT.sizeDelta.x / FieldGrid.cellSize.x);
        cellsPerColumn = Mathf.FloorToInt(GameFieldRT.sizeDelta.y / FieldGrid.cellSize.y);
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
    }
}
