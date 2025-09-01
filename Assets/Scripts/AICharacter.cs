using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    //[SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Camera _camera;
    [SerializeField] private AStarPathFindingAlogrithm _aStarPathFindingAlogrithm;
    [SerializeField] private List<Vector3> _pathPosition;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _startMoving;
    [SerializeField] private int _currentIndex;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Vector3 _worldPosition;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _counter;
    [SerializeField] private float _maxTime;
    [SerializeField] private GameObject GameLoseMenu;
    private Vector3 _lastPlayerPosition;

    private void Update()
    {
        _counter++;

        //Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _layerMask))
        //{
        //    _pointerTransform.position = raycastHit.point;
        //    _worldPosition = raycastHit.point;
        //    _worldPosition.y = 0;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Use the world position as needed
        //    //Debug.Log("Mouse click at world position: " + (int)_worldPosition.x + " " + (int)_worldPosition.y + " " + (int)_worldPosition.z);

        //    //List<MazeCell> path = _aStarPathFindingAlogrithm.FindPath((int)transform.position.x, (int)transform.position.z, (int)_worldPosition.x, (int)_worldPosition.z);
        //    List<MazeCell> path = _aStarPathFindingAlogrithm.FindPath((int)transform.position.x, (int)transform.position.z, (int)_playerTransform.position.x, (int)_playerTransform.position.z);
        //    if (path != null)
        //    {
        //        for (int i = 0; i < path.Count - 1; i++)
        //        {
        //            Debug.DrawLine(new Vector3(path[i].transform.position.x, 0, path[i].transform.position.z), new Vector3(path[i + 1].transform.position.x, 0, path[i + 1].transform.position.z), Color.green, 1000f);
        //        }
        //    }

        //    foreach (MazeCell cell in path)
        //    {
        //        _pathPosition.Add(cell.transform.position);
        //    }
        //}
        // Use the world position as needed
        //Debug.Log("Mouse click at world position: " + (int)_worldPosition.x + " " + (int)_worldPosition.y + " " + (int)_worldPosition.z);

        //List<MazeCell> path = _aStarPathFindingAlogrithm.FindPath((int)transform.position.x, (int)transform.position.z, (int)_worldPosition.x, (int)_worldPosition.z);

        if (_counter > _maxTime)
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, _lastPlayerPosition);
            int playerX = Mathf.RoundToInt(_playerTransform.position.x);
            int playerZ = Mathf.RoundToInt(_playerTransform.position.z);


            if (distanceToPlayer > 0.1f)
            {
                List<MazeCell> path = _aStarPathFindingAlogrithm.FindPath((int)transform.position.x, (int)transform.position.z, playerX, playerZ);

                if (path != null)
                {
                    _pathPosition.Clear();
                    _currentIndex = 0;

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].transform.position.x, 0, path[i].transform.position.z), new Vector3(path[i + 1].transform.position.x, 0, path[i + 1].transform.position.z), Color.yellow, 1000f);
                    }
                    //DrawPathWithLineRenderer();

                    foreach (MazeCell cell in path)
                    {
                        _pathPosition.Add(cell.transform.position);
                    }
                    _lastPlayerPosition = _playerTransform.position;  // Update last player position
                }
                _counter = 0;
            }
        }

        if (_currentIndex < _pathPosition.Count)
        {
            Vector3 targetPosition = _pathPosition[_currentIndex];
            targetPosition.y = .4f;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _movementSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.0001f)
            {
                _currentIndex++;
            }
        }
    }
    //private void DrawPathWithLineRenderer()
    //{
    //    int pathCount = _pathPosition.Count;
    //    lineRenderer.positionCount = pathCount;

    //    for (int i = 0; i < pathCount; i++)
    //    {
    //        Vector3 point = new Vector3(_pathPosition[i].x, 0, _pathPosition[i].z);
    //        lineRenderer.SetPosition(i, point);
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            GameLoseMenu.SetActive(true);
        }
    }
}
