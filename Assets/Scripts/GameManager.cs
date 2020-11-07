using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    [SerializeField]
    private bool _isGamePaused = false;
    private UIManager _UIManager;

    private void Start()
    {
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("UI Manager Not Found");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame()
    {
        if (_isGamePaused)
        {
            _UIManager.GameNotPaused();
            _isGamePaused = false;
            Time.timeScale = 1;
        }
        else
        {
            _UIManager.GamePaused();
            _isGamePaused = true;
            Time.timeScale = 0;
        }
    }


}
