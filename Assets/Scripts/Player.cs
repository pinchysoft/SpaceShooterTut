using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    private float _canFire = -1f;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private int _speedMultiplier = 2;
    private AudioSource _audioSource;
    private Animator _animator;
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
    private bool _canTripleShot = false;
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
            _canFire = Time.time + _fireRate;
            _audioSource.clip = _laserSound;
            if (_canTripleShot)
            {
                Vector3 laser = new Vector3(transform.position.x + 1.3f, transform.position.y + _laserOffset, 0);
                Instantiate(_tripleLaserPrefab, laser, Quaternion.identity);
                _audioSource.Play();
            } 
            else
            {
                Vector3 laser = new Vector3(transform.position.x, transform.position.y + _laserOffset, 0);
                Instantiate(_laserPrefab, laser, Quaternion.identity);
                _audioSource.Play();
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

    public void ShieldActive()
    {
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

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldObject.SetActive(false);
            return;
        }
        _lives--;
        _uiManager.UpdateLives(_lives);
        _audioSource.clip = _explodeSound;
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
        transform.Translate(direction * _speed * Time.deltaTime);

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
