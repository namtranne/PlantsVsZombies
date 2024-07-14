using System;
using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 0.4f; // Movement speed
    public float health = 10;
    public float damage;
    public float range;
    public LayerMask plantMask;
    public ZombieTypes type;

    protected bool canEat = true;
    protected string curAnimation = "walking";
    protected float eatCooldown;
    protected Plant targetPlant;

    private AudioSource audioSource;
    public AudioClip hitClip;

    public AudioSource eatSource;

    public Transform trapPosition;
    public GameObject frozenTrap;
    public GameObject activeFrozenTrap;

    public RuntimeAnimatorController boomAnimation;
    public float xBoost = 1;
    private readonly object dieLock = new object();

    // Start is called before the first frame update
    private void Start()
    {
        health = type.health * xBoost;
        damage = type.damage;
        range = type.range;
        speed = type.speed * (1+xBoost/10);
        eatCooldown = type.eatCooldown;

        GetComponent<SpriteRenderer>().sprite = type.sprite;
        GetComponent<Animator>().runtimeAnimatorController = type.animator;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.isPaused) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);
        if(hit.collider)
        {
            targetPlant = hit.collider.GetComponent<Plant>();
            if (curAnimation != "eating" && curAnimation!="death" && targetPlant)
            {
                GetComponent<Animator>().runtimeAnimatorController = type.eatAnimation;
                curAnimation = "eating";
                Instantiate(eatSource);
            }
            Eat();
        } else if(curAnimation == "eating" && !targetPlant)
        {
            GetComponent<Animator>().runtimeAnimatorController = type.animator;
            curAnimation = "walking";
        }
    }

    void Eat()
    {
        if (!canEat || !targetPlant)
            return;
        canEat = false;
        Invoke("ResetEatCooldown", eatCooldown);

        targetPlant.Hit((int)damage);
    }

    public void ResetEatCooldown()
    {
        canEat = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameManager.isPaused) return;

        // Move the zombie to the left
        if (curAnimation == "walking")
            transform.position -= new Vector3(speed, 0, 0);
    }

    public virtual void Hit(float damage, bool freeze)
    {
        if (!gameObject) return;
        audioSource.PlayOneShot(hitClip);
        health -= damage;
        if(freeze) Freeze();
        if (health <= 0 && curAnimation != "death")
        {
            curAnimation = "death";
            Die(type.deathAnimation);
        }
    }

    public void BowlingHit() { 
        health = 0;
        curAnimation = "death";
        Die(type.deathAnimation);
    }

    public void Boom()
    {
        if (curAnimation != "death")
            Die(boomAnimation);
    }

    public void Die(RuntimeAnimatorController dieAnimation)
    {
        GetComponent<Animator>().enabled = true;
        if (dieAnimation)
        {
            GetComponent<Animator>().runtimeAnimatorController = dieAnimation;
            Destroy(gameObject, 1.4f);
        }
        else
        {
            Destroy(gameObject, 0f);
        }
        GameObject spawner = GameObject.Find("ZombieSpawner");

        ZombieSpawner zombieSpawner = spawner.GetComponent<ZombieSpawner>();
        zombieSpawner.zombiesKilled++;
        if (zombieSpawner.zombiesKilled >= zombieSpawner.zombieMax)
        {
          
            GameObject.Find("GameManager").GetComponent<GameManager>().Win();
            
        }
    }

    public void Freeze()
    {
        if (!gameObject) return;
        CancelInvoke("UnFreeze");
        GetComponent<SpriteRenderer>().color = Color.blue;
        if (speed > 0) speed = type.speed / 2;
        Invoke("UnFreeze", 5);
    }
    public void UnFreeze()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed;
    }
    public void Frozen(int time)
    {
        if (!gameObject) return;
        CancelInvoke("UnFrozen");
        GetComponent<SpriteRenderer>().color = Color.blue;
        speed = 0;
        if (activeFrozenTrap)
            Destroy(activeFrozenTrap);
        activeFrozenTrap = Instantiate(frozenTrap, trapPosition.position, Quaternion.identity, transform);
        activeFrozenTrap.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        GetComponent<Animator>().enabled = false;
        Invoke("UnFrozen", time);
    }

    public void UnFrozen()
    {
        if (!gameObject) return;
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed; 
        Destroy(activeFrozenTrap);
        GetComponent<Animator>().enabled = true;
        activeFrozenTrap = null;
    }

    public float GetHealth() {
        return health;
    }

    internal bool IsDead()
    {
        return curAnimation == "death";
    }

    public void BeBited(float delay = 0)
    {
        if (!gameObject) return;
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
        try
        {
            if (curAnimation != "death")
                Die(null);
        }
        catch
        {
            Debug.Log("Zombie is Die");
        }

    }

}
