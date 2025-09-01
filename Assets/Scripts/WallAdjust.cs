using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class WallAdjust : MonoBehaviour
{
    [SerializeField] private GameObject _valueFetcher;
    [SerializeField] private GameObject _levels;
    private MazeCell[] _valueFetcherList;
    private MazeCell[] _levelList;
    [SerializeField] private GameObject _wallLeft;
    [SerializeField] private GameObject _wallRight;
    [SerializeField] private GameObject _wallForward;
    [SerializeField] private GameObject _wallBackward;
    [SerializeField] private GameObject _unVisitedBlock;

    private List<GameObject> _walls;

    private void Update()
    {
        _valueFetcherList = _valueFetcher.gameObject.GetComponentsInChildren<MazeCell>();
        _levelList = _levels.gameObject.GetComponentsInChildren<MazeCell>();

        for (int i = 0; i < _levelList.Length; i++)
        {
            _valueFetcherList[i].gameObject.GetComponent<MazeCell>().CanGoBackward = _levelList[i].CanGoBackward;
            _valueFetcherList[i].gameObject.GetComponent<MazeCell>().CanGoForward = _levelList[i].CanGoForward;
            _valueFetcherList[i].gameObject.GetComponent<MazeCell>().CanGoLeft = _levelList[i].CanGoLeft;
            _valueFetcherList[i].gameObject.GetComponent<MazeCell>().CanGoRight = _levelList[i].CanGoRight;
        }


        foreach (Transform child in transform)
        {
            _walls.Add(child.gameObject);
        }
        _wallLeft = _walls[0];
        _wallRight = _walls[1];
        _wallForward = _walls[2];
        _wallBackward = _walls[3];
        _unVisitedBlock = _walls[4];

        CheckAndClearRightWall();
        CheckAndClearLeftWall();
        CheckAndClearForwardWall();
        CheckAndClearBackwardWall();
    }


    private void CheckAndClearRightWall()
    {
        if (!_wallRight.activeInHierarchy)
        {
            GetComponent<MazeCell>().CanGoRight = true;
        }
        else
        {
            GetComponent<MazeCell>().CanGoRight = false;
        }
    }


    private void CheckAndClearLeftWall()
    {
        if (!_wallLeft.activeInHierarchy)
        {
            GetComponent<MazeCell>().CanGoLeft = true;
        }
        else
        {
            GetComponent<MazeCell>().CanGoLeft = false;
        }
    }


    private void CheckAndClearForwardWall()
    {
        if (!_wallForward.activeInHierarchy)
        {
            GetComponent<MazeCell>().CanGoForward = true;
        }
        else
        {
            GetComponent<MazeCell>().CanGoForward = false;
        }
    }


    private void CheckAndClearBackwardWall()
    {
        if (!_wallBackward.activeInHierarchy)
        {
            GetComponent<MazeCell>().CanGoBackward = true;
        }
        else
        {
            GetComponent<MazeCell>().CanGoBackward = false;
        }
    }
}
