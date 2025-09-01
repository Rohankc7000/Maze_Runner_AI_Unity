using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject GameWonMenu;
    public GameObject GameLoseMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.Locked;
        GameWonMenu.SetActive(false);
        GameLoseMenu.SetActive(false);
    }
}
