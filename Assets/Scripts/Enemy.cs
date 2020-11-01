using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private float _speed = 4.0f;

    void Update()
    {
        // caches Player
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        { 
            // "respawn" (reuse) at top if was not destroyed
            // bonus: at a new random x position
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log ("Hit: " + other.transform.name);

        if (other.CompareTag("Player"))
        //if (other.tag == "Player") resource hog
        {
            // order matters!
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser"))
        //if (other.tag == "Laser") resource hog
        {
            // order matters!
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(this.gameObject);
        }
    }
}
