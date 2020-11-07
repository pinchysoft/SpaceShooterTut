using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _fireRate = 3.5f;
    private float _canFire = -1f;
    private GameObject _playerObject;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;

    void Start()
    {
        _playerObject = GameObject.FindWithTag("Player");            
        if (_playerObject != null)
        {
            _player = _playerObject.GetComponent<Player>();
        } 
        else
        {
            Debug.LogError("Player Not Found");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source (Enemy) Not Found");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator Not Found");
        }
    }

    void Update()
    {
        Movement();
        ShootLaser();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    void ShootLaser()
    {
        if (Time.time > _canFire)
        {
            _audioSource.clip = _laserSound;
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].SetEnemyFired();
            }
            _audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage();
            _speed = 0;
            _animator.SetTrigger("onEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
        if (other.CompareTag("Laser"))
        {
            _player.SetScore(10);
            Destroy(other.gameObject);
            _speed = 0;
            _animator.SetTrigger("onEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
        if (other.CompareTag("SuperWeapon"))
        {
            _player.SetScore(10);
            _speed = 0;
            _animator.SetTrigger("onEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
