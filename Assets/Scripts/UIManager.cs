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
    private Slider _ammoSlider;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _ammoTextShadow;
    [SerializeField]
    private GameObject[] _shieldObjects;
    [SerializeField]
    private Sprite[] _liveSprites;
    private GameManager _gameManager;

    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Text _thrusterText;
    [SerializeField]
    private Text _thrusterTextShadow;
    [SerializeField]
    private Image _thrusterBar;

    private float _thrustersCost = 0.2f;
    private float _thrustersRegen = 0.1f;
    private int _thrustersMax = 100;

    void Start()
    {
        _playerScore.text = "Score: 0";
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _ammoSlider.GetComponent<Slider>();
        if (_ammoSlider == null)
        {
            Debug.LogError("Ammo Slider Not Found");
        }
        _ammoSlider.value = 15;
        _ammoText.text = "Ammo: " + _ammoSlider.value.ToString() + "/15";
        _ammoTextShadow.text = "Ammo: " + _ammoSlider.value.ToString() + "/15";

        _thrusterSlider.GetComponent<Slider>();
        if (_thrusterSlider == null)
        {
            Debug.LogError("Thruster Slider Not Found");
        }
        _thrusterBar.GetComponent<Image>();
        if (_thrusterBar == null)
        {
            Debug.LogError("Thruster Bar Not Found");
        }
        _thrusterSlider.value = _thrustersMax;
        _thrusterText.text = "Thrusters: " + _thrusterSlider.value.ToString() + "%";
        _thrusterTextShadow.text = "Thrusters: " + _thrusterSlider.value.ToString() + "%";
        _thrusterBar.color = new Color32(26, 236, 19, 225);

        _pauseText.gameObject.SetActive(false);
        _pauseTextInfo.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Not Found");
        }
    }

    public void RestoreAmmo()
    {
        _ammoSlider.value = 15;
        _ammoText.text = "Ammo: " + _ammoSlider.value.ToString() + "/15";
        _ammoTextShadow.text = "Ammo: " + _ammoSlider.value.ToString() + "/15";
    }

    public bool UpdateAmmo()
    {
        bool canFire = true;
        _ammoSlider.value -= 1;
        if (_ammoSlider.value > 0)
        {
            canFire = true;
        }
        else
        {
            _ammoSlider.value = 0;
            canFire = false;
        }
        _ammoText.text = "Ammo: " + _ammoSlider.value.ToString() + "/15";
        _ammoTextShadow.text = "Ammo: " + _ammoSlider.value.ToString() + "/15";
        return canFire;
    }

    private void Update()
    {
        if (_thrusterSlider.value > 60)
        {
            _thrusterBar.color = new Color32(26, 236, 19, 225);
        } 
        else if (_thrusterSlider.value > 30 && _thrusterSlider.value <= 60)
        {
            _thrusterBar.color = new Color32(236, 121, 19, 225);
        }
        else
        {
            _thrusterBar.color = new Color32(236, 50, 19, 225);
        }
    }

    public bool ThrustersEnabled()
    {
        StartCoroutine(ThrustersCoroutine());
        if (_thrusterSlider.value >= _thrustersCost)
        {
            _thrusterSlider.value -= _thrustersCost;
            _thrusterText.text = "Thrusters: " + ((int)_thrusterSlider.value).ToString() + "%";
            _thrusterTextShadow.text = "Thrusters: " + ((int)_thrusterSlider.value).ToString() + "%";
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateScore(int score)
    {
        _playerScore.text = "Score: " + score.ToString();
    }

    public void UpdateShields(int lives)
    {
        switch (lives)
        {
            case 1:
                _shieldObjects[0].GetComponent<ShieldUI>().TurnOnShield();
                _shieldObjects[1].GetComponent<ShieldUI>().TurnOffShield();
                _shieldObjects[2].GetComponent<ShieldUI>().TurnOffShield();
                break;
            case 2:
                _shieldObjects[0].GetComponent<ShieldUI>().TurnOnShield();
                _shieldObjects[1].GetComponent<ShieldUI>().TurnOnShield();
                _shieldObjects[2].GetComponent<ShieldUI>().TurnOffShield();
                break;
            case 3:
                _shieldObjects[0].GetComponent<ShieldUI>().TurnOnShield();
                _shieldObjects[1].GetComponent<ShieldUI>().TurnOnShield();
                _shieldObjects[2].GetComponent<ShieldUI>().TurnOnShield();
                break;
            default:
                _shieldObjects[0].GetComponent<ShieldUI>().TurnOffShield();
                _shieldObjects[1].GetComponent<ShieldUI>().TurnOffShield();
                _shieldObjects[2].GetComponent<ShieldUI>().TurnOffShield();
                break;
        }
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
    private IEnumerator ThrustersCoroutine()
    {
        while (_thrusterSlider.value < _thrustersMax)
        {
            yield return new WaitForSeconds(5f);
            _thrusterSlider.value += _thrustersRegen;
            _thrusterText.text = "Thrusters: " + ((int)_thrusterSlider.value).ToString() + "%";
            _thrusterTextShadow.text = "Thrusters: " + ((int)_thrusterSlider.value).ToString() + "%";
            yield return new WaitForSeconds(5f);
        }
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
