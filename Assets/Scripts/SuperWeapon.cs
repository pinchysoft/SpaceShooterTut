using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperWeapon : MonoBehaviour
{

    private float _scaleX = 0f, _scaleY = 0f;
    private SpriteRenderer _sprite;
    private AudioSource _audioSource;

    private void Start()
    {
        transform.localScale = new Vector3(_scaleX, _scaleY, 0);
        _sprite = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float step = 5f * Time.deltaTime;
        transform.localScale += new Vector3(0.01f, 0.01f, 0);
        if (transform.localScale.x > 2f && transform.localScale.x < 2.5f)
        {
            _audioSource.Play();
        }
        if (transform.localScale.x > 8f)
        {
            Destroy(GetComponent<Collider2D>());
            _sprite.color = new Color(1f, 1f, 1f, Mathf.Lerp(_sprite.color.a, 0f, step));
            Destroy(this.gameObject, 3f);
        }
    }

}
