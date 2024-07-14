using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnMower : MonoBehaviour
{
    public bool isMoving = false;

    public float speed = 5;

    public AudioClip sound;

    private AudioSource mowerSource;

    private void Start()
    {
        mowerSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            if (!isMoving) {
                mowerSource.volume = GameManager.soundVolume;
                mowerSource.PlayOneShot(sound);
            }
            other.GetComponent<Zombie>().Hit(1000, false);
            isMoving = true;

            Destroy(gameObject, 8f);
        }
    }

    private void Update()
    {
        if (isMoving)
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
