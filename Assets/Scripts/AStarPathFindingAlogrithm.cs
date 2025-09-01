using System.Collections.Generic;
using UnityEngine;

public class AStarPathFindingAlogrithm : MonoBehaviour
{
    //[SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private LevelDetails _levelDetails;
    private const int MOVE_STRAIGHT_COST = 10;
    private List<MazeCell> _openList;
    private List<MazeCell> _closeList;

    public List<MazeCell> FindPath(int startX, int startZ, int endX, int endZ)
    {
        //_levelDetails.CostRest();
        //MazeCell startCell = mazeGenerator.MazeGrid[startX, startZ];
        //MazeCell endCell = mazeGenerator.MazeGrid[endX, endZ];
        MazeCell startCell = _levelDetails.MazeGrid[startX, startZ];
        MazeCell endCell = _levelDetails.MazeGrid[endX, endZ];

        //Debug.Log("-------" + startCell + "  " + "-------" + endCell);

        _openList = new List<MazeCell> { startCell };
        _closeList = new List<MazeCell>();

        //Debug.Log("OpenList Count: " + openList.Count);
        //Debug.Log("CloseList Count: " + closeList.Count);


        for (int x = 0; x < _levelDetails.MazeWidth; x++)
        {
            for (int z = 0; z < _levelDetails.MazeDepth; z++)
            {
                MazeCell cell = _levelDetails.MazeGrid[x, z];
                cell.GCost = int.MaxValue;
                cell.CalculateFCost();
                cell.CameFromMazeCell = null;
                //Debug.Log(z);
            }
            //Debug.Log(x);
        }

        //Debug.Log(levelDetails.MazeWidth + "  " + levelDetails.MazeDepth);

        startCell.GCost = 0;
        startCell.HCost = CalculateDistanceCost(startCell, endCell);
        startCell.CalculateFCost();

        while (_openList.Count > 0)
        {
            //Debug.Log("While VItra");
            MazeCell currentCell = GetLowestFCostNode(_openList);
            //Debug.Log(currentCell.transform.name);
            if (currentCell == endCell)
            {
                //Debug.Log("While Vitra ko endcell == current cell");
                //has reached to the final node
                return CalculatePath(endCell);
            }

            _openList.Remove(currentCell);
            _closeList.Add(currentCell);
            //Debug.Log("While Vitra ko ");

            foreach (MazeCell neighbourCell in GetNeighbourList(currentCell))
            {
                //Debug.Log("For each vitra ");
                if (_closeList.Contains(neighbourCell)) continue;

                int tentativeGCost = currentCell.GCost + CalculateDistanceCost(currentCell, neighbourCell);
                if (tentativeGCost < neighbourCell.GCost)
                {
                    neighbourCell.CameFromMazeCell = currentCell;
                    neighbourCell.GCost = tentativeGCost;
                    neighbourCell.HCost = CalculateDistanceCost(neighbourCell, endCell);
                    neighbourCell.CalculateFCost();

                    if (!_openList.Contains(neighbourCell))
                    {
                        _openList.Add(neighbourCell);
                    }
                }
            }
        }

        //Out of the cell in the open list
        return null;
    }

    private List<MazeCell> CalculatePath(MazeCell endCell)
    {
        List<MazeCell> path = new List<MazeCell>();
        path.Add(endCell);
        MazeCell currentCell = endCell;
        //Debug.Log("-------");
        while (currentCell.CameFromMazeCell != null)
        {
            path.Add(currentCell.CameFromMazeCell);
            currentCell = currentCell.CameFromMazeCell;
        }
        //Debug.Log("++++++...." + path.Count);
        path.Reverse();
        return path;
    }

    private MazeCell GetLowestFCostNode(List<MazeCell> mazeCellList)
    {
        MazeCell lowestFCostCell = mazeCellList[0];
        for (int i = 0; i < mazeCellList.Count; i++)
        {
            if (mazeCellList[i].FCost < lowestFCostCell.FCost)
            {
                lowestFCostCell = mazeCellList[i];
            }
        }
        return lowestFCostCell;
    }

    private int CalculateDistanceCost(MazeCell a, MazeCell b)
    {
        int xDistance = Mathf.Abs((int)(a.transform.position.x) - (int)(b.transform.position.x));
        int zDistance = Mathf.Abs((int)(a.transform.position.z) - (int)(b.transform.position.z));
        int remaining = Mathf.Abs(xDistance - zDistance);
        //return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        return MOVE_STRAIGHT_COST * remaining;
    }

    private List<MazeCell> GetNeighbourList(MazeCell currentCell)
    {
        List<MazeCell> neighbours = new List<MazeCell>();
        //Debug.Log("Neighbour before: " + neighbours.Count);

        // Get references to neighboring cells
        MazeCell upNeighbor = GetForwardNeighbor(currentCell);
        MazeCell downNeighbor = GetBackwardNeighbor(currentCell);
        MazeCell leftNeighbor = GetLeftNeighbor(currentCell);
        MazeCell rightNeighbor = GetRightNeighbor(currentCell);
        //Debug.Log("Up Neighbour: " + upNeighbor.transform.gameObject);
        //Debug.Log("Down Neighbour: " + downNeighbor.transform.gameObject);
        //Debug.Log("Left Neighbour: " + leftNeighbor.transform.gameObject);
        //Debug.Log("Right Neighbour: " + rightNeighbor.transform.gameObject);

        // Add neighboring cells that are accessible
        if (upNeighbor != null)
            neighbours.Add(upNeighbor);

        if (downNeighbor != null)
            neighbours.Add(downNeighbor);

        if (leftNeighbor != null)
            neighbours.Add(leftNeighbor);

        if (rightNeighbor != null)
            neighbours.Add(rightNeighbor);

        //Debug.Log("Neighbour after: " + neighbours.Count);
        return neighbours;
    }

    private MazeCell GetForwardNeighbor(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x);
        int z = (int)(currentCell.transform.position.z);
        //Debug.Log("x = " + x + " " + "z = " + z);
        //Debug.Log("CanGoForward : " + currentCell.CanGoForward);

        if (currentCell.CanGoForward && z + 1 < _levelDetails.MazeDepth)
        {
            //Debug.Log("Forward found");
            return _levelDetails.MazeGrid[x, z + 1];
        }
        return null;
    }

    private MazeCell GetBackwardNeighbor(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x);
        int z = (int)(currentCell.transform.position.z);


        //Debug.Log("CanGoBack : " + currentCell.CanGoBackward);

        if (currentCell.CanGoBackward && z - 1 >= 0)
        {
            //Debug.Log("Backward found");
            return _levelDetails.MazeGrid[x, z - 1];
        }
        return null;
    }

    private MazeCell GetLeftNeighbor(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x);
        int z = (int)(currentCell.transform.position.z);


        //Debug.Log("CanGoLeft : " + currentCell.CanGoLeft);

        if (currentCell.CanGoRight && x + 1 < _levelDetails.MazeWidth)
        {
            //Debug.Log("Left found");
            return _levelDetails.MazeGrid[x + 1, z];
        }
        return null;
    }

    private MazeCell GetRightNeighbor(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x);
        int z = (int)(currentCell.transform.position.z);


        //Debug.Log("CanGoRight : " + currentCell.CanGoRight);

        if (currentCell.CanGoLeft && x - 1 >= 0)
        {
            //Debug.Log("Right found");
            return _levelDetails.MazeGrid[x - 1, z];
        }
        return null;
    }
}
