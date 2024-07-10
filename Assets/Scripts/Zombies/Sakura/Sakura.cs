using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Sakura : MonoBehaviour
{
    // Movement properties
    public float speed = 0.4f; // Movement speed
    public float range;
    private bool canTeleport = true;
    private bool canDash = true;
    public GameObject currentTile;
    public GameObject targetTile;

    // Health properties
    public float health = 10;
    private int damage;

    // Attacking properties
    private bool canEat = true;
    private bool canFire = true;
    private float eatCooldown;
    public Plant targetPlant;

    // Audio properties
    public AudioSource ultimateAudioSource;
    public AudioSource normalAttackAudioSource;
    public AudioSource teleAudioSource;
    public AudioSource guardAudioSource;
    public AudioClip[] groans;
    public AudioClip fireClip;

    // Trap properties
    public Transform trapPosition;
    public GameObject frozenTrap;
    private GameObject activeFrozenTrap;

    // Animation properties
    public Animator animator;
    public Sprite newAvatar;
    public RuntimeAnimatorController boomAnimation;
    public GameObject explosionEffect;
    private Vector3 ultimatePosition;
    private string curAnimation = "walking";
    public GameObject portrait;
    public GameObject avatar;
    public GameObject chat;

    // Layer masks
    public LayerMask plantMask;
    public LayerMask targetMask;
    public LayerMask tileMask;

    // Grid properties
    public GameObject gridObject;
    public int numColumns = 9;
    GameObject[] tiles;



    // Start is called before the first frame update
    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        // // Invoke("Groan", Random.Range(1f, 14f));
        tiles = new GameObject[gridObject.transform.childCount];
        for (int i = 0; i < gridObject.transform.childCount; i++)
        {
            tiles[i] = gridObject.transform.GetChild(i).gameObject;
        }
    }

    void Groan()
    {
        // audioSource.PlayOneShot(groans[Random.Range(0, groans.Length)]);
    }

    private void Update()
    {
        if(curAnimation != "walking") {
            return;
        }
        RaycastHit2D tileHit = Physics2D.Raycast(transform.position, Vector2.left, range, tileMask);
        if (tileHit.collider)
        {
            bool isRowHasPlant = CheckRowHasPlant(tileHit.collider.gameObject);
            currentTile = tileHit.collider.gameObject;
            if(isRowHasPlant) {
                if(canFire) {
                    curAnimation = "fire";
                    return;
                }
            }
            else {
                if(canTeleport) {
                    GameObject teleportTile = GetTeleportTile();
                    if(teleportTile != null) {
                        curAnimation  = "teleport";
                        return;
                    }
                }
            }
        }

        RaycastHit2D plantHit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);
        if(plantHit.collider)
        {
            targetPlant = plantHit.collider.GetComponent<Plant>();
            if (curAnimation != "eating" && targetPlant)
            {
                curAnimation = "eating";
            }
        }

        if(canDash) {
            curAnimation = "dash";
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        switch(curAnimation) {
            case "walking": 
                transform.position -= new Vector3(speed, 0, 0);
                break;
            case "fire":
                curAnimation = ""; 
                Fire();
                canFire = false;
                Invoke("ResetFireCoolDown", 15);
                break;
            case "teleport":
                curAnimation = "";
                canTeleport = false;
                Invoke("ResetTeleportCoolDown",10);
                TeleportToAnotherRow();
                break;
            case "guard":
                curAnimation = "";
                Guard();
                break;
            case "eating":
                curAnimation = "";
                Eat();
                break;
            case "dash":
                curAnimation = "";
                Invoke("ResetDashCoolDown",5);
                canDash = false;
                Dash();
                break;
            case "":
                break;
        }   
    }

    bool CheckRowHasPlant(GameObject target) {
        
        int targetIndex = System.Array.IndexOf(tiles, target);

        int rowIndex = targetIndex / numColumns;

        // Calculate the start and end indices for the row
        int rowStartIndex = rowIndex * numColumns;
        int rowEndIndex = rowStartIndex + numColumns;

        // Iterate through the row and check for the hasPlant property
        for (int i = rowStartIndex; i < rowEndIndex; i++)
        {
            if (tiles[i].GetComponent<Tile>().hasPlant)
            {
                return true;
            }
        }
        return false;
    }

    GameObject GetTeleportTile() {
        int numColumns = 9;
        int targetIndex = System.Array.IndexOf(tiles, currentTile);
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<Tile>().hasPlant)
            {
                int plantRowIndex = i/numColumns;
                targetTile = tiles[plantRowIndex*numColumns + targetIndex%numColumns];
                return targetTile;
            }
        }
        return null;
    }

    void ResetFireCoolDown() {
        canFire = true;
    }

    void ResetTeleportCoolDown() {
        canTeleport = true;
    }

    void ResetDashCoolDown() {
        canDash = true;
    }

    void Dash() {
        int tileIndex = Array.IndexOf(tiles, currentTile);
        if(tileIndex % numColumns ==0) {
            ResetState();
            return;
        }
        targetTile = tiles[tileIndex-1];
        animator.Play("SakuraTele");
        Instantiate(teleAudioSource);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("Teleport", stateInfo.length - 0.5f);
    }

    void TeleportToAnotherRow() {
        animator.Play("SakuraTele");
        Instantiate(teleAudioSource);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("Teleport", stateInfo.length - 0.5f);
    }

    void Teleport() {
        transform.position = targetTile.transform.position;     
        ResetState();
    }

    void GetUltimatePosition() {
        int targetIndex = System.Array.IndexOf(tiles, currentTile);
        int rowIndex = targetIndex / numColumns;
        int startIndex = rowIndex*numColumns;
        for(int i = startIndex; i < targetIndex; i++) {
            Tile tile = tiles[i].GetComponent<Tile>();
            if(tile.hasPlant) {
                ultimatePosition = tile.transform.position;
                return;
            }
        }
    }

 
    void  Fire() {
        Instantiate(portrait);
        Instantiate(avatar);
        Instantiate(chat);
        Instantiate(ultimateAudioSource);
        animator.Play("SakuraUltimate");
        GetUltimatePosition();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("WaitForAnimationAndExplode", stateInfo.length);
    }

    void Eat()
    {
        if (!targetPlant)
            return;
        GameObject ava = Instantiate(avatar);
        ava.GetComponent<SpriteRenderer>().sprite = newAvatar;
        Instantiate(normalAttackAudioSource);
        animator.Play("SakuraAttack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("EatPlant", stateInfo.length);
    }

    void EatPlant() {
        targetPlant.Destroy();
        ResetState();
    }

    void ResetEatCooldown()
    {
        canEat = true;
    }
    

    public void Hit(float damage, bool freeze)
    {
        if(curAnimation == "walking")
           curAnimation = "guard";
    }

    public void Guard() {
        animator.Play("SakuraGuard");
        Instantiate(guardAudioSource);
        speed = 0.02f;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("ResetState", stateInfo.length);
    }


    private void ResetState() {
        speed = 0.01f;
        curAnimation = "walking";
        animator.Play("SakuraRun");
    }



private void WaitForAnimationAndExplode()
    {
        // Instantiate the explosion effect
        GameObject ultimate = Instantiate(explosionEffect, ultimatePosition, Quaternion.identity);
        RaycastHit2D[] Plants= Physics2D.RaycastAll(ultimatePosition, new Vector2(4.05f, 1.66f), 0, targetMask);
        // RaycastHit2D[] Plants= Physics2D.RaycastAll(ultimate);
        foreach (RaycastHit2D plant in Plants)
        {
            if(plant.collider != null) {
                plant.collider.GetComponent<Plant>().Destroy();
            }
        }

        // Destroy the explosion effect after 1 second
        Destroy(ultimate, 1.5f);
        ResetState();
    }

    public void Boom()
    {
        if (curAnimation != "death")
            Die(boomAnimation);
    }

    public void Die(RuntimeAnimatorController dieAnimation)
    {
        curAnimation = "death";
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().runtimeAnimatorController = dieAnimation;
        Destroy(gameObject, 1.4f);
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
        // if (speed > 0) speed = type.speed / 2;
        Invoke("UnFreeze", 5);
    }
    void UnFreeze()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        // speed = type.speed;
    }
    public void Frozen(int time)
    {
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
    void UnFrozen()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        // speed = type.speed; 
        Destroy(activeFrozenTrap);
        GetComponent<Animator>().enabled = true;
        activeFrozenTrap = null;
    }

    public float GetHealth() {
        return health;
    }
}
