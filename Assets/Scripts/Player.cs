using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Player Movement
/// This is a reference script, therefore notes have not been refactored out
/// 
/// Spawning Laser Objects
/// - With cool-down system to allow time between spawning (instantiating)
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _laserPrefab;   // from Prefabs folder (not heirarchy!)

    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _nextFire = -1.0f;

    [SerializeField]
    private float _waitTime;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    void Start()
    {
        // resets the transform's position for the object 
        // this script is attached to in the Inspector
        transform.position = new Vector3(0f, 0f, 0f);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is full");
        }
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
        // if space key is pressed
        //    spawn gameObject limited by cool-down timer
        Debug.Log("Instantiate: Space key was pressed.");
        Vector3 offset = new Vector3(0, 0.8f, 0);   // between player and laser launch point
        _nextFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
    }

    void CalculateMovement()
    {
        // Movement Basics:
        // 1 unit (Unity) = 1 meter (in the real world)

        // "speed of sound" movement!  1 meter/frame
        //transform.Translate(Vector3.right);         // pre-defined, so 'new' is not required
        //transform.Translate(new Vector3(1, 0, 0));  // same as above, change x=-1 to go left

        // normal speed movement default is 1 meter/second
        // deltaTim is the time from the last frame to the current frame
        //transform.Translate(Vector3.right * Time.deltaTime);

        // convert to speed to 5 meters/second, multiply by _speed = 5.0f
        // by the distributive property:
        //     new Vector3(1, 0, 0) * 5.0 * real time
        //  => new Vector3(5, 0, 0) * real time
        // Try adjusting (sliding over the Speed variable in the Inspector)
        //transform.Translate(Vector3.right * _speed * Time.deltaTime);   // _speed = 5.0f

        // Controlling Movement with the keyboard:
        // using UnityEngine.Input
        // see Edit - Project Settings - Input Manager
        // Note: Unity new Input System package (Window - Package Manager) is now recommended 

        // need to get the current postion of the object this script is attached to
        // Horizontal Movement: use the left and right arrow keys or A and D keys
        // Vertical Movement: use the up and down arrow keys or W and S keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // one-liner
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // Player Vertical (y) Bounds:
        // if y > 0, then set y = 0
        // if y <= - 3.8, set y = -3.8f

        //if (transform.position.y > 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}
         
        // one-liner uses clamping (specify min and max values)
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        // Player Horizontal (x) bounds (wrap around):
        // if x > 11.4f, then x = -11.4f
        // if x < -11.4f, then x = 11.4f
        if (transform.position.x > 11.4f)
        {
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.4f)
        {
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        }

        // note: no clamping for x position due to wrap around
    }

    public void Damage()
    {
        _lives--;

        if(_lives <= 0)
        {
             _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
