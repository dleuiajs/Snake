using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [Header("Vars")]
    [SerializeField] int startingLength = 3;
    [SerializeField] float speed = 1f;
    public Vector2Int defaultDir = Vector2Int.left;

    [Header("Scripts")]
    [SerializeField] GameManager gameManager;
    FieldManager fieldManager;

    [Header("Game values")]
    [HideInInspector] public List<FieldManager.Cell> snakeCells = new List<FieldManager.Cell>();
    Vector2Int directionNow;

    void Awake()
    {
        fieldManager = GetComponent<FieldManager>();
    }

    void Update()
    {
        int xInput = 0;
        int yInput = 0;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && snakeCells[0].direction.y != 1)
            yInput = -1;
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && snakeCells[0].direction.y != -1)
            yInput = 1;
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && snakeCells[0].direction.x != 1)
            xInput = -1;
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && snakeCells[0].direction.x != -1)
            xInput = 1;

        if ((xInput != 0 || yInput != 0) && xInput * yInput == 0)
        {
            directionNow = new Vector2Int(xInput, yInput);
        }
    }

    public void CreateSnake()
    {
        // задаем стартовые координаты
        int startPosX = fieldManager.cellsPerRow / 2 - 1;
        int startPosY = fieldManager.cellsPerColumn / 2 - 1;
        for (int i = 0; i < startingLength; i++)
        {
            if (startPosX != fieldManager.cellsPerRow - 1)
                startPosX++;
            else if (startPosY != fieldManager.cellsPerColumn - 1)
                startPosY++;
            else
            {
                Debug.LogWarning("Стартовый змей слишком большой!");
                return;
            }
            AddTailSnakeCell(startPosX, startPosY);
        }

        // задаем начальное направление червя
        directionNow = defaultDir;
        Debug.Log(directionNow);
        StartCoroutine(MovingSnake());
    }

    bool growSnake = false;

    public void Grow()
    {
        growSnake = true;
    }


    void AddTailSnakeCell(int x, int y)
    {
        snakeCells.Add(fieldManager.cells[y, x]);
        fieldManager.cells[y, x].Fill();
        if (snakeCells.Count >= 2)
            fieldManager.cells[y, x].direction = snakeCells[snakeCells.Count - 2].direction;
        else
            fieldManager.cells[y, x].direction = directionNow;
    }

    void AddHeadSnakeCell(int x, int y)
    {
        snakeCells.Insert(0, fieldManager.cells[y, x]);
        fieldManager.cells[y, x].Fill();
        fieldManager.cells[y, x].direction = directionNow;
    }

    void RemoveSnakeCell(int x, int y)
    {
        snakeCells.Remove(fieldManager.cells[y, x]);
        fieldManager.cells[y, x].Clear();
    }

    bool CheckCollision(int x, int y)
    {
        foreach (var obj in snakeCells)
        {
            if (obj.x == x && obj.y == y)
                return true;
        }
        return false;
    }

    void MoveSnake(Vector2Int direction)
    {
        int newPosX = snakeCells[0].x + direction.x;
        int newPosY = snakeCells[0].y + direction.y;
        if (newPosX >= 0 && newPosX < fieldManager.cellsPerRow && newPosY >= 0 && newPosY < fieldManager.cellsPerColumn && !CheckCollision(newPosX, newPosY))
        {
            AddHeadSnakeCell(newPosX, newPosY);
            if (!growSnake)
                RemoveSnakeCell(snakeCells[snakeCells.Count - 1].x, snakeCells[snakeCells.Count - 1].y);
            else
                growSnake = false;
            Debug.Log($"New Head pos: {snakeCells[0].x}, {snakeCells[0].y}. Tail pos: {snakeCells[snakeCells.Count - 1].x}, {snakeCells[snakeCells.Count - 1].y}. Direction: {direction.x}, {direction.y}");
        }
        else
        {
            StopAllCoroutines();
            gameManager.Lose();
            StartCoroutine(SnakeFlashing());
        }
    }

    IEnumerator MovingSnake()
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
            MoveSnake(directionNow);
        }
    }

    IEnumerator SnakeFlashing()
    {
        bool filled = true;
        for (int i = 0; i < 3 / speed; i++)
        {
            foreach (var snakeCell in snakeCells)
            {
                if (filled)
                    snakeCell.Clear();
                else
                    snakeCell.Fill();
            }
            filled = !filled;
            yield return new WaitForSeconds(speed);
        }
        gameManager.Reset();
    }
}
