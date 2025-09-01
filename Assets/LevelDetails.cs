using UnityEngine;

public class LevelDetails : MonoBehaviour
{
    [SerializeField] private GameObject _valueFetcher;
    public MazeCell[] Cells;
    public MazeCell[] CellsWithValue;
    public MazeCell[,] MazeGrid;
    public int MazeWidth;
    public int MazeDepth;
    [SerializeField] private int _index;


    private void Start()
    {
        Cells = transform.GetComponentsInChildren<MazeCell>();
        CellsWithValue = _valueFetcher.GetComponentsInChildren<MazeCell>();

        MazeGrid = new MazeCell[MazeWidth, MazeDepth];
        _index = 0;

        for (int i = 0; i < MazeWidth; i++)
        {
            for (int j = 0; j < MazeDepth; j++)
            {
                MazeGrid[i, j] = Cells[_index];
                MazeGrid[i, j].gameObject.name = MazeGrid[i, j].name + _index;
                MazeGrid[i, j].gameObject.GetComponent<MazeCell>().CanGoBackward = CellsWithValue[_index].CanGoBackward;
                MazeGrid[i, j].gameObject.GetComponent<MazeCell>().CanGoForward = CellsWithValue[_index].CanGoForward;
                MazeGrid[i, j].gameObject.GetComponent<MazeCell>().CanGoLeft = CellsWithValue[_index].CanGoLeft;
                MazeGrid[i, j].gameObject.GetComponent<MazeCell>().CanGoRight = CellsWithValue[_index].CanGoRight;
                _index++;
            }
        }

        //CostRest();
    }

    public void CostRest()
    {
        MazeCell[] cellList = transform.GetComponentsInChildren<MazeCell>();
        foreach (MazeCell cell in cellList)
        {
            cell.GCost = int.MaxValue;
            cell.FCost = int.MaxValue;
            cell.HCost = 0;
            cell.CameFromMazeCell = null;
        }
    }

    //private void Start()
    //{
    //    List<MazeCell> cells = a.FindPath(0, 0, 5, 5);
    //    Debug.Log(cells.Count());


    //    if (cells != null)
    //    {
    //        for (int i = 0; i < cells.Count - 1; i++)
    //        {
    //            Debug.DrawLine(new Vector3(cells[i].transform.position.x, 0, cells[i].transform.position.z), new Vector3(cells[i + 1].transform.position.x, 0, cells[i + 1].transform.position.z), Color.green, 1000f);
    //        }
    //    }
    //}
}
