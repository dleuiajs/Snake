using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [Header("Vars")]
    [SerializeField] int startingLength = 3;
    [SerializeField] float speed = 1f;

    [Header("Scripts")]
    FieldManager fieldManager;
    GameManager gameManager;

    [Header("Game values")]
    List<FieldManager.Cell> snakeCells = new List<FieldManager.Cell>();
    Vector2Int directionNow;

    void Awake()
    {
        fieldManager = GetComponent<FieldManager>();
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        int xInput = 0;
        int yInput = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            yInput = -1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            yInput = 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            xInput = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            xInput = 1;

        if ((xInput != 0 || yInput != 0) && xInput * yInput == 0)
        {
            directionNow = new Vector2Int(xInput, yInput);
            Debug.Log($"{xInput}, {yInput}");
        }
        Debug.Log($"{Input.GetAxis("Horizontal")}, {Input.GetAxis("Vertical")}");

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
        directionNow = Vector2Int.left;
        Debug.Log(directionNow);
        StartCoroutine(MovingSnake());
    }

    void AddTailSnakeCell(int x, int y)
    {
        snakeCells.Add(fieldManager.cells[y, x]);
        fieldManager.cells[y, x].Fill();
    }

    void AddHeadSnakeCell(int x, int y)
    {
        snakeCells.Insert(0, fieldManager.cells[y, x]);
        fieldManager.cells[y, x].Fill();
    }

    void RemoveSnakeCell(int x, int y)
    {
        snakeCells.Remove(fieldManager.cells[y, x]);
        fieldManager.cells[y, x].Clear();
    }

    void MoveSnake(Vector2Int direction)
    {
        int newPosX = snakeCells[0].x + direction.x;
        int newPosY = snakeCells[0].y + direction.y;
        if (newPosX >= 0 && newPosX < fieldManager.cellsPerRow && newPosY >= 0 && newPosY < fieldManager.cellsPerColumn)
        {
            AddHeadSnakeCell(newPosX, newPosY);
            RemoveSnakeCell(snakeCells[snakeCells.Count - 1].x, snakeCells[snakeCells.Count - 1].y);
            Debug.Log($"New Head pos: {snakeCells[0].x}, {snakeCells[0].y}. Tail pos: {snakeCells[snakeCells.Count - 1].x}, {snakeCells[snakeCells.Count - 1].y}. Direction: {direction.x}, {direction.y}");
        }
        else
        {
            StopAllCoroutines();
            gameManager.Lose();
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
}
