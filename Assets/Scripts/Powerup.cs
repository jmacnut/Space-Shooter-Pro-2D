﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ID for Powerups

/// </summary>
public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupID; // 0 = Triple Shot, 1 = Speed, 2 = Shields

    [SerializeField]
    private float _speed = 3.0f;

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
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (this.powerupID)
                {
                    case 0:
                        Debug.Log("Collected Triple Shot");
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        Debug.Log("Collected Speed Boost");
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        Debug.Log("Collected Shield");
                        //player.ActivateShieldBoost();
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
