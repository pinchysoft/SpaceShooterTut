using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;
    private int _shieldLives = 0;
    [SerializeField]
    private bool _isImmortal; //for testing purposes ;)
    [SerializeField]
    private int _score = 0;
    private float _canFire = -1f;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _thrustMultiplier = 6f;
    [SerializeField]
    private int _speedMultiplier = 3;
    private AudioSource _audioSource;
    private Animator _animator;
    private CameraShake _cameraShake;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private GameObject _shieldObject;
    [SerializeField]
    private AudioClip _powerupSound;
    [SerializeField]
    private AudioClip _explodeSound;
    private float _laserOffset = 1.1f;
    private float _fireRate = 0.15f;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private GameObject _superWeaponPrefab;
    private bool _canTripleShot = false;
    private bool _canSuperWeapon = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject[] _playerHurt;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager Not Found");
        }
        if (_animator == null)
        {
            Debug.LogError("Animator Not Found");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source (Player) Not Found");
        }
        if (_cameraShake == null)
        {
            Debug.LogError("Main Camera Not Found");
        }

    }

    void Update()
    {
        Movement();
        ShootLaser();
    }

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_uiManager.UpdateAmmo())
            {
                _canFire = Time.time + _fireRate;
                _audioSource.clip = _laserSound;
                if (_canTripleShot)
                {
                    Vector3 laser = new Vector3(transform.position.x + 1.3f, transform.position.y + _laserOffset, 0);
                    Instantiate(_tripleLaserPrefab, laser, Quaternion.identity);
                    _audioSource.Play();
                } 
                else if (_canSuperWeapon)
                {
                    Vector3 super = new Vector3(transform.position.x, transform.position.y, 0);
                    Instantiate(_superWeaponPrefab, super, Quaternion.identity);
                    _canSuperWeapon = false;
                }
                else
                {
                    Vector3 laser = new Vector3(transform.position.x, transform.position.y + _laserOffset, 0);
                    Instantiate(_laserPrefab, laser, Quaternion.identity);
                    _audioSource.Play();
                }
            }
        }
    }

    public void SetScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void TripleShotActive()
    {
        _audioSource.clip = _powerupSound;
        _audioSource.Play();
        _canTripleShot = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public void SuperActive()
    {
        _audioSource.clip = _powerupSound;
        _audioSource.Play();
        _canSuperWeapon = true;
        StartCoroutine(SuperWeaponPowerDown());
    }

    public void ShieldActive()
    {
        _shieldLives = 3;
        _uiManager.UpdateShields(_shieldLives);
        _audioSource.clip = _powerupSound;
        _audioSource.Play();
        _isShieldActive = true;
        _shieldObject.SetActive(true);
    }

    public void SpeedActive()
    {
        _audioSource.clip = _powerupSound;
        _audioSource.Play();
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDown());
    }
    private IEnumerator SuperWeaponPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _canSuperWeapon = false;
    }

    private IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _canTripleShot = false;
    }
    private IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
    }

    public void AddHealth()
    {
        _lives++;
        if (_lives > 3)
        {
            _lives = 3;
        }
        switch (_lives)
        {
            case 1:
                _playerHurt[0].SetActive(false);
                break;
            case 2:
                _playerHurt[1].SetActive(false);
                break;
            default:
                _playerHurt[0].SetActive(false);
                _playerHurt[1].SetActive(false);
                break;
        }
        _uiManager.UpdateLives(_lives);
    }

    public void AddAmmo()
    {
        _uiManager.RestoreAmmo();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldLives--;
            _cameraShake.Shake();
            switch (_shieldLives)
            {
                case 0:
                    _uiManager.UpdateShields(_shieldLives);
                    _isShieldActive = false;
                    _shieldObject.SetActive(false);
                    break;
                default:
                    _uiManager.UpdateShields(_shieldLives);
                    break;
            }
            return;
        }
        if (!_isImmortal)
        {
            _cameraShake.Shake();
            _lives--;
            _uiManager.UpdateLives(_lives);
            _audioSource.clip = _explodeSound;
        }
        switch (_lives)
        {
            case 0:
                _audioSource.Play();
                _playerHurt[2].SetActive(true);
                Destroy(this.gameObject, 2f);
                _spawnManager.OnPlayerDeath();
                break;
            case 1:
                _audioSource.Play();
                _playerHurt[0].SetActive(true);
                break;
            case 2:
                _audioSource.Play();
                _playerHurt[1].SetActive(true);
                break;
            default:
                break;
        }
    }

    void Movement()
    {
        float horizontalAxes = Input.GetAxis("Horizontal");
        float verticalAxes = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalAxes, verticalAxes, 0);

        if (Input.GetKey(KeyCode.LeftShift) && _uiManager.ThrustersEnabled())
        {
            transform.Translate(direction * _speed * _thrustMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _animator.SetBool("movingRight", true);
            _animator.SetBool("movingLeft", false);
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _animator.SetBool("movingLeft", true);
            _animator.SetBool("movingRight", false);
        }
        else
        {
            _animator.SetBool("movingLeft", false);
            _animator.SetBool("movingRight", false);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        } 
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

}
