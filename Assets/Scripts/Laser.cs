using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    [SerializeField]
    private AudioClip _explosionAudioClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null){
            Debug.LogError("AudioSource _audioSource is NULL");
        }
        else
        {
            _audioSource.clip = _explosionAudioClip;
        }
    }
    void Update()
    {
        if (_isEnemyLaser == true)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // if laser position y is off the screen, destroy the object
        if (transform.position.y > 8.0f)
        {
            // clean up triple shot objects
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            // clean up single shot laser
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser == true)
        {
            Debug.Log("Enemy Laser Hit Player");
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                _audioSource.Play();
            }
            else
            {
                Debug.LogError("The Player player is NULL");
            }

        }
    }
}
