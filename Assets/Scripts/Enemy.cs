using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private float _fireRate = 5.0f;
    private float _canFire = -1f;

    private Animator _anim;
    //private AudioClip _explosionSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player _player is NULL");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator _anim is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource _audioSource is NULL");
        }
    }
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            FireEnemyLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        { 
            // "respawn" at random position at top not destroyed
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }

    void FireEnemyLaser()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire += Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        foreach (var laser in lasers)
        {
            laser.AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log ("Hit: " + other.transform.name);

        if (other.CompareTag("Player"))
        {
            // order matters!
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            { 
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.volume = 1f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("Laser"))
        {
            // order matters!
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.volume = 10f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
