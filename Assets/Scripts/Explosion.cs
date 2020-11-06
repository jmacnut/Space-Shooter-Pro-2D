using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //private AudioClip _explosionSound;
    //private AudioSource _audioSource;

    void Start()
    {
        Destroy(this.gameObject, 3.0f);
    }

    void Update()
    {

    }
}
