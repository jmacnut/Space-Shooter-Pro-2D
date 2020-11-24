using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private int collectableID; // 0 = Ammo, 1 = Health, 2 = TBD

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
                AudioSource.PlayClipAtPoint(_clip, transform.position, 2000f);

                switch (this.collectableID)
                {
                    case 0:
                        Debug.Log("Collected Ammo Refill");
                        player.RestoreAmmoCount();
                        break;
                    case 1:
                        Debug.Log("Collected Health");
                        player.RestoreHealth();
                        break;
                    //case 2:
                    //    Debug.Log("Collected TBD");
                    //    //player.ActivateTBDCollectable();
                    //    break;
                    default:
                        Debug.Log("Invalid collectableID");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
