using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.0f)
        { 
            // "respawn" (reuse) at top if was not destroyed
            // bonus: with a new random x position
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log ("Hit: " + other.transform.name);

        if (other.CompareTag("Player"))
        //if (other.tag == "Player")
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
        //if (other.tag == "Laser")
        {
            // order matters!
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
