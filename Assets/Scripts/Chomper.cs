using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : MonoBehaviour
{
    private AudioSource biteAudioSource;
    public AudioClip biteClip;

    public float biteDuration = .2f;
    public float digestDuration = 5f;

    private bool isDigesting = false;
    private float biteTimer = 0f;
    private float digestTimer = 0f;

    public RuntimeAnimatorController normalAnimation;
    public RuntimeAnimatorController biteAnimation;
    public RuntimeAnimatorController digestAnimation;

    // Start is called before the first frame update
    void Start()    
    {
        biteAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDigesting)
        {
            // Handle digest animation and timer
            if (digestTimer > 0)
            {
                digestTimer -= Time.deltaTime;
                GetComponent<Animator>().runtimeAnimatorController = digestAnimation;
            }
            else
            {
                isDigesting = false;
                GetComponent<Animator>().runtimeAnimatorController = normalAnimation;
            }
        }
        else if (biteTimer > 0)
        {
            // Handle bite animation and timer
            biteTimer -= Time.deltaTime;
            GetComponent<Animator>().runtimeAnimatorController = biteAnimation;
        }
        else
        {
            GetComponent<Animator>().runtimeAnimatorController = normalAnimation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie) && !isDigesting)
        {
            BiteZombie(zombie);
        }
    }

    public void BiteZombie(Zombie zombie)
    {
        if (!isDigesting)
        {
            // Play bite audio
            biteAudioSource.PlayOneShot(biteClip);

            // Start bite timer
            biteTimer = biteDuration;

            // Change animation to biting
            GetComponent<Animator>().runtimeAnimatorController = biteAnimation;

            // After biting, start digesting and killed zombie
            StartCoroutine(StartDigestingCoroutine(zombie));
        }
    }

    private IEnumerator StartDigestingCoroutine(Zombie zombie)
    {
        yield return new WaitForSeconds(biteDuration);
        StartDigesting(zombie);
    }

    private void StartDigesting(Zombie zombie)
    {
        zombie.BeBited();

        isDigesting = true;
        digestTimer = digestDuration;
        
        // Change animation to digesting
        GetComponent<Animator>().runtimeAnimatorController = digestAnimation;
    }
}
