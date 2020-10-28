using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
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
}
