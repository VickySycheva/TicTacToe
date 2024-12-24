using UnityEngine;

public class PlayingField : MonoBehaviour
{
    [SerializeField] Cell[] cells;

    public void Init(out Cell[,] cellsOnField)
    {
        cellsOnField = new Cell [3,3];
        foreach (var cell in cells)
        {
            cellsOnField[cell.Index.x, cell.Index.y] = cell;
            cellsOnField[cell.Index.x, cell.Index.y].Init();
        }
    }


}
