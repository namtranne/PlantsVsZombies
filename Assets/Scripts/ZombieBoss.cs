using System;
using UnityEditor.Animations;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZombieBoss : Zombie 
{
    // Movement properties
    private bool canTeleport = true;
    private bool canDash = true;
    public GameObject currentTile;
    public GameObject targetTile;

    // Attacking properties
    private bool canUseUltimate = true;

    // Audio properties
    public AudioSource ultimateAudioSource;
    public AudioSource teleAudioSource;
    public AudioSource guardAudioSource;

    // Animation properties
    public Animator animator;
    public AnimatorController animationController;
    public Sprite newAvatar;
    public GameObject ultimateEffect;
    public GameObject portrait;
    public GameObject avatar;
    public GameObject chat;

    // Layer masks
    public LayerMask tileMask;

    // Grid properties
    public GameObject gridObject;
    private int numColumns = 9;
    private GameObject[] tiles;



    // Start is called before the first frame update
    private void Start()
    {
        health = type.health;
        damage = type.damage;
        range = type.range;
        speed = type.speed;
        eatCooldown = type.eatCooldown;
        ultimateAudioSource = type.ultimateAudioSource;
        teleAudioSource = type.teleAudioSource;
        guardAudioSource = type.guardAudioSource;
        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = type.animator;
        newAvatar = type.newAvatar;
        ultimateEffect = type.ultimateEffect;
        portrait = type.portrait;
        avatar = type.avatar;
        chat = type.chat;
        eatSource = type.attackAudioSource;

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        tiles = new GameObject[gridObject.transform.childCount];
        for (int i = 0; i < gridObject.transform.childCount; i++)
        {
            tiles[i] = gridObject.transform.GetChild(i).gameObject;
        }
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
                if(canUseUltimate) {
                    curAnimation = "ultimate";
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
            if (curAnimation != "eating" && targetPlant && canEat)
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
            case "ultimate":
                curAnimation = ""; 
                Ultimate();
                canUseUltimate = false;
                Invoke("ResetUltimateCoolDown", 15);
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

    void ResetUltimateCoolDown() {
        canUseUltimate = true;
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
        animator.Play("Tele");
        Instantiate(teleAudioSource);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("Teleport", stateInfo.length - 0.5f);
    }

    void TeleportToAnotherRow() {
        animator.Play("Tele");
        Instantiate(teleAudioSource);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("Teleport", stateInfo.length - 0.5f);
    }

    void Teleport() {
        transform.position = targetTile.transform.position;     
        ResetState();
    }
 
    void  Ultimate() {
        Instantiate(portrait);
        Instantiate(avatar);
        Instantiate(chat);
        Instantiate(ultimateAudioSource);
        animator.Play("Ultimate");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("PlayUltimateAnimation", stateInfo.length);
    }

    void Eat()
    {
        if (!targetPlant)
            return;
        canEat = false;
        GameObject ava = Instantiate(avatar);
        ava.GetComponent<SpriteRenderer>().sprite = newAvatar;
        Instantiate(eatSource);
        animator.Play("Attack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("ResetEatCooldown", eatCooldown);
        Invoke("EatPlant", stateInfo.length);
    }

    void EatPlant() {
        targetPlant.Destroy();
        ResetState();
    }
    

    public override void Hit(float damage, bool freeze)
    {
        if(freeze) Freeze();
        if(curAnimation == "walking")
           curAnimation = "guard";
    }

    private void Guard() {
        animator.Play("Guard");
        Instantiate(guardAudioSource);
        speed = 0.02f;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Invoke("ResetState", stateInfo.length);
    }


    private void ResetState() {
        speed = 0.01f;
        curAnimation = "walking";
        animator.Play("Run");
    }

    private void PlayUltimateAnimation()
    {
        GameObject ultimate = Instantiate(ultimateEffect, transform.position, Quaternion.identity);
        ResetState();
    }
}
