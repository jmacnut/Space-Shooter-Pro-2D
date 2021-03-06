﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Movement
///
/// Spawning Laser Objects
/// - With cool-down system to allow time between spawning (instantiating)
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedMultiplier = 2.0f;

    [SerializeField]
    private GameObject _laserPrefab;   // from Prefabs folder (not heirarchy!)
    [SerializeField]
    private GameObject _tripleShotLaserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _nextFire = -1.0f;

    [SerializeField]
    private float _waitTime = 5.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldsVisualizer;

    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0f, 0f, 0f);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource _audioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager _spawnManater is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }

        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        Vector3 offset = new Vector3(0, 1.05f, 0);   // between player and laser launch point

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotLaserPrefab, transform.position + offset, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }

        _audioSource.Play();

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        
        // vertical bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        // horizontal bounds
        if (transform.position.x > 10.28f)
        {
            transform.position = new Vector3(-10.28f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.28f)
        {
            transform.position = new Vector3(10.28f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;   // good for a single hit
            _shieldsVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if(_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_waitTime);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_waitTime);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ActivateShields()
    {
        _isShieldsActive = true;
        _shieldsVisualizer.SetActive(true);
        StartCoroutine(ShieldsPowerDownRoutine());
    }

    IEnumerator ShieldsPowerDownRoutine()
    {
        yield return new WaitForSeconds(_waitTime);
        _isShieldsActive = false;
        _shieldsVisualizer.SetActive(false);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
