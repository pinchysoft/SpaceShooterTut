using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _playerScore;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _pauseText;
    [SerializeField]
    private Text _pauseTextInfo;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    private GameManager _gameManager;

    void Start()
    {
        _playerScore.text = "Score: 0";
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _pauseText.gameObject.SetActive(false);
        _pauseTextInfo.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Not Found");
        }
    }

    public void UpdateScore(int score)
    {
        _playerScore.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int lives)
    {
        if (lives >= 0)
        {
            _livesImage.sprite = _liveSprites[lives];
        }
        if (lives < 1)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverCoroutine());
    }
    public void GamePaused()
    {
        _pauseText.gameObject.SetActive(true);
        _pauseTextInfo.gameObject.SetActive(true);
    }

    public void GameNotPaused()
    {
        _pauseText.gameObject.SetActive(false);
        _pauseTextInfo.gameObject.SetActive(false);
    }

    private IEnumerator GameOverCoroutine()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
