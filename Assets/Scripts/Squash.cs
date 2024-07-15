using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : MonoBehaviour
{
    private AudioSource squashAudioSource;
    public AudioClip smashClip;
    public AudioClip aimClip;

    public float aimDuration = .2f;
    public float smashDuration = 1;

    private float aimTimer = 0f;
    private float smashTimer = 0f;
    private bool smashed = false;

    public RuntimeAnimatorController normalAnimation;
    public RuntimeAnimatorController aimAnimation;
    public RuntimeAnimatorController smashAnimation;

    // Start is called before the first frame update
    void Start()
    {
        squashAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(smashed)
        {
            try
            {
                Plant plant = gameObject.GetComponentInParent<Plant>();

                plant.Destroy();
                Destroy(gameObject, 0.2f);
            }
            catch
            {
                Debug.Log("Already destroy");
            }
            
        }
        else if (aimTimer > 0)
        {
            // Handle aim animation and timer
            aimTimer -= Time.deltaTime;
            GetComponent<Animator>().runtimeAnimatorController = aimAnimation;
        }
        else if (smashTimer > 0)
        {
            // Handle smash animation and timer
            smashTimer -= Time.deltaTime;
            GetComponent<Animator>().runtimeAnimatorController = smashAnimation;
        }
        else
        {
            GetComponent<Animator>().runtimeAnimatorController = normalAnimation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie) && !smashed)
        {
            squashAudioSource.volume = GameManager.soundVolume;
            squashAudioSource.PlayOneShot(aimClip);
            AimZombie(zombie);
        }
    }

    public void AimZombie(Zombie zombie)
    {
        aimTimer = aimDuration;
        GetComponent<Animator>().runtimeAnimatorController = aimAnimation;
        StartCoroutine(StartSmashingCoroutine(zombie));
    }

    private IEnumerator StartSmashingCoroutine(Zombie zombie)
    {
        yield return new WaitForSeconds(aimDuration);
        StartSmashing(zombie);
    }

    private void StartSmashing(Zombie zombie)
    {
        smashTimer = smashDuration;
        GetComponent<Animator>().runtimeAnimatorController = smashAnimation;
        StartCoroutine(SmashSuccess(zombie));
    }

    private IEnumerator SmashSuccess(Zombie zombie)
    {
        yield return new WaitForSeconds(smashDuration);
        squashAudioSource.volume = GameManager.soundVolume;
        squashAudioSource.PlayOneShot(smashClip);
        smashed = true;
        zombie.BeSmashed(.2f);
    }
}
