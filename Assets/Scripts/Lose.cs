using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public Animator animator;

    private bool hasLost;
    public AudioClip loseMusic;
    public AudioClip scream;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            if (hasLost || other.GetComponent<Zombie>().IsDead()) return;
            hasLost = true;
            source.PlayOneShot(loseMusic);
            source.PlayOneShot(scream);
            animator.Play("death");
            Invoke("RestartScene", 6);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
