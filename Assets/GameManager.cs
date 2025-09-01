using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject PausedMenu;
    public bool IsPaused;

    private void Awake()
    {
        Instance = this;
        IsPaused = false;
    }

    private void Update()
    {
        if (IsPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
        else if (!IsPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            PausedGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausedMenu.SetActive(false);
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void PausedGame()
    {
        Time.timeScale = 0;
        PausedMenu.SetActive(true);
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
