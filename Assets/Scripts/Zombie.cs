using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private float speed = 0.4f; // Movement speed
    private int health = 10;
    private int damage;
    private float range;
    public LayerMask plantMask;
    public ZombieTypes type;

    private bool canEat = true;
    private string curAnimation = "walking";
    private float eatCooldown;
    public Plant targetPlant;

    private AudioSource audioSource;
    public AudioClip[] groans;

    public AudioSource eatSource;
    public AudioClip eatClip;

    // Start is called before the first frame update
    private void Start()
    {
        health = type.health;
        damage = type.damage;
        range = type.range;
        speed = type.speed;
        eatCooldown = type.eatCooldown;

        GetComponent<SpriteRenderer>().sprite = type.sprite;
        GetComponent<Animator>().runtimeAnimatorController = type.animator;
        audioSource = GetComponent<AudioSource>();

        Invoke("Groan", Random.Range(1f, 14f));
    }

    void Groan()
    {
        audioSource.PlayOneShot(groans[Random.Range(0, groans.Length)]);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);
        if(hit.collider)
        {
            targetPlant = hit.collider.GetComponent<Plant>();
            if (curAnimation != "eating" && targetPlant)
            {
                GetComponent<Animator>().runtimeAnimatorController = type.eatAnimation;
                curAnimation = "eating";
                eatSource.Play();
            }
            Eat();
        } else if(curAnimation == "eating" && !targetPlant)
        {
            GetComponent<Animator>().runtimeAnimatorController = type.animator;
            curAnimation = "walking";
            eatSource.Stop();
        }
    }

    void Eat()
    {
        if (!canEat || !targetPlant)
            return;
        canEat = false;
        Invoke("ResetEatCooldown", eatCooldown);

        targetPlant.Hit(damage);
    }

    void ResetEatCooldown()
    {
        canEat = true;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // Move the zombie to the left
        if (!targetPlant)
            transform.position -= new Vector3(speed, 0, 0);
    }

    public void Hit(int damage, bool freeze)
    {
        audioSource.PlayOneShot(type.hitClips[Random.Range(0, type.hitClips.Length)]);
        health -= damage;
        if(freeze) Freeze();
        if (health <= 0 && curAnimation != "death")
        {
            Debug.Log("before " + curAnimation);
            curAnimation = "death";
            Debug.Log("after" + curAnimation);
            GetComponent<Animator>().runtimeAnimatorController = type.deathAnimation;
            Destroy(gameObject, 1f);
            ZombieSpawner zombieSpawner = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
            zombieSpawner.zombiesKilled++;
            if (zombieSpawner.zombiesKilled >= zombieSpawner.zombieMax)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().Win();
            }
        }
    }

    public void BeBited(float delay = 0)
    {
        Destroy(gameObject, delay);
        ZombieSpawner zombieSpawner = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
        zombieSpawner.zombiesKilled++;
        if (zombieSpawner.zombiesKilled >= zombieSpawner.zombieMax)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Win();
        }
    }

    public void BeSmashed(float delay = 0)
    {
        GetComponent<Animator>().runtimeAnimatorController = type.deathAnimation;
        Destroy(gameObject, delay);
        ZombieSpawner zombieSpawner = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
        zombieSpawner.zombiesKilled++;
        if (zombieSpawner.zombiesKilled >= zombieSpawner.zombieMax)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Win();
        }
    }

    void Freeze()
    {
        CancelInvoke("UnFreeze");
        GetComponent<SpriteRenderer>().color = Color.blue;
        speed = type.speed / 2;
        Invoke("UnFreeze", 5);
    }
    void UnFreeze()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed;
    }
}
