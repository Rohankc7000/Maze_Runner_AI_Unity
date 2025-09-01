using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;

    [SerializeField] private GameObject _rightWall;

    [SerializeField] private GameObject _frontWall;

    [SerializeField] private GameObject _backWall;

    [SerializeField] private GameObject _unvisitedBlock;

    public bool CanGoRight, CanGoLeft, CanGoForward, CanGoBackward;

    public bool HasVisited { get; private set; }

    public int GCost;
    public int HCost;
    public int FCost;

    public MazeCell CameFromMazeCell;

    private void Awake()
    {
        GCost = int.MaxValue;
        FCost = int.MaxValue;
        HCost = 0;
        CameFromMazeCell = null;
    }

    public void CalculateFCost()
    {
        FCost = GCost+ HCost;
    }

    public void DoVisit()
    {
        HasVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        CanGoLeft = true;
        _leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        CanGoRight = true;
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        CanGoForward = true;
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        CanGoBackward = true;
        _backWall.SetActive(false);
    }
}