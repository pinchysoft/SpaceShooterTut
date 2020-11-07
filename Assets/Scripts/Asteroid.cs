using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private GameObject _explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source (Asteroid) Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _speed = 0;
            Destroy(other.gameObject);
            Vector3 explode = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject boom = Instantiate(_explodePrefab, explode, Quaternion.identity);
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.25f);
            Destroy(boom, 2.35f);
            _spawnManager.startSpawning();
        }
    }
}
