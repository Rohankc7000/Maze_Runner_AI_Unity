using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _environment; // to instanstiate the maze cell in one game object

    [SerializeField] private MazeCell _mazeCellPrefab; // maze cell prefab

    public int MazeWidth;

    public int MazeDepth;

    public MazeCell[,] MazeGrid; // the storarge for the all the maze cell in rows and columns



    private void Awake()
    {
        MazeGrid = new MazeCell[MazeWidth, MazeDepth]; // instantiating the new grid

        // Making the cells in grid with specified rows and columns
        for (int i = 0; i < MazeWidth; i++)
        {
            for (int j = 0; j < MazeDepth; j++)
            {
                //instantiating the maze cell with the index to store in the grid
                MazeGrid[i, j] = Instantiate(_mazeCellPrefab, new Vector3(i, 0, j), Quaternion.identity);
                MazeGrid[i, j].transform.SetParent(_environment.transform); //Making each maze cell in one envrionment gameobject.
            }
        }

        //StartCoroutine(GenerateMaze(null, _mazeGrid[0, 0]));
        GenerateMaze(null, MazeGrid[0, 0]);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.DoVisit(); // Helps to visit the cells and mark them vistited.

        RemoveWalls(previousCell, currentCell); // Removing the respective walls according the the visited node

        //yield return new WaitForSeconds(0.05f);

        MazeCell nextCell; // Declaring the new MazeCell object for storing the maze cell which will be returned by the GetNextUnvisitedCell method

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                //yield return GenerateMaze(currentCell, nextCell); // making recursive untill all the maze is filled
                GenerateMaze(currentCell, nextCell); // making recursive untill all the maze is filled
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell); // Getting the list of the unvisited cells from the current cell

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault(); // Returning the one random cell among the unvisited cells.
    }

    /// <summary>
    /// This helps to fetch the lists of the unvistied nodes and returns the list. Here the IEnumberable roles is to make the method runs
    /// from the top to bottom and collects the which are provided by the yield return.
    /// </summary>
    /// <param name="currentCell"></param>
    /// <returns></returns>
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        //Getting the index of the current maze cell
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        //Checking if maze on the right side is visited or not and if yes returning that cell in the IEnumerable list of unvisited cell
        if (x + 1 < MazeWidth)
        {
            var rightCellFromCurrnet = MazeGrid[x + 1, z]; // Getting the right side cell from the current cell

            if (!rightCellFromCurrnet.HasVisited)
            {
                yield return rightCellFromCurrnet;
            }
        }

        //Checking if maze on the left side is visited or not and if yes returning that cell in the IEnumerable list of unvisited cell
        if (x - 1 >= 0)
        {
            var leftCellFromCurrnet = MazeGrid[x - 1, z];// Getting the left side cell from the current cell

            if (!leftCellFromCurrnet.HasVisited)
            {
                yield return leftCellFromCurrnet;
            }
        }

        //Checking if maze on the forward side is visited or not and if yes returning that cell in the IEnumerable list of unvisited cell
        if (z + 1 < MazeDepth)
        {
            var frontCellFromCurrnet = MazeGrid[x, z + 1];// Getting the forward side cell from the current cell

            if (!frontCellFromCurrnet.HasVisited)
            {
                yield return frontCellFromCurrnet;
            }
        }

        //Checking if maze on the backward side is visited or not and if yes returning that cell in the IEnumerable list of unvisited cell
        if (z - 1 >= 0)
        {
            var backCellFromCurrnet = MazeGrid[x, z - 1];// Getting the backward side cell from the current cell

            if (!backCellFromCurrnet.HasVisited)
            {
                yield return backCellFromCurrnet;
            }
        }
    }

    /// <summary>
    /// Removing the cells based on the index or transform position of the cell which can be easily understood while looking at 
    /// the code.
    /// </summary>
    /// <param name="previousCell"></param>
    /// <param name="currentCell"></param>
    private void RemoveWalls(MazeCell previousCell, MazeCell currentCell)
    {
        // for the first element of the cell as it doesnot have the previous cell
        if (previousCell == null)
        {
            return;
        }

        // This means alogrithm has moved to the right side from the previous cell so removing the walls accordingly.
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        // This means alogrithm has moved to the backward side from the previous cell so removing the walls accordingly.
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }

        // This means alogrithm has moved to the left side from the previous cell so removing the walls accordingly.
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // This means alogrithm has moved to the forward side from the previous cell so removing the walls accordingly.
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
    }

}
