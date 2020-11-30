﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assign IDs for Powerups
/// Activate, move, audio
/// </summary>
public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupID; // 0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Octo Shot

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private AudioClip _clip;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_clip, transform.position, 5f);

                switch (this.powerupID)
                {
                    case 0:
                        Debug.Log("Collected Triple Shot Powerup");
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        Debug.Log("Collected Speed Boost Powerup");
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        Debug.Log("Collected Shield Powerup");
                        player.ActivateShields();
                        break;
                    case 3:   // Secondary Fire
                        Debug.Log("Collected Octo Shot Powerup");
                        player.ActivateOctoShot();
                        break;
                    default:
                        Debug.Log("Invalid powerupID");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
