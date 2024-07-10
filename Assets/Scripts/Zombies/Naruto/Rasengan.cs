using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rasengan : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1;
    public int range = 1;
    public LayerMask plantMask ;
    public LayerMask lawnMowerMask ;
    private bool isMoving = true;
    public Animator animator;
    void Start()
    {
        transform.position += new Vector3(-0.6f,0.5f,0);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D plant = Physics2D.Raycast(transform.position, Vector3.left, range, plantMask);
        if(plant.collider) {
            plant.collider.GetComponent<Plant>().Destroy();
        }

        RaycastHit2D lawnMower = Physics2D.Raycast(transform.position, Vector3.left, 1.5f, lawnMowerMask);
        if(lawnMower.collider) {
            isMoving=false;
            animator.Play("End");
            Invoke("Destroy", 0.9f);
        }

        
        if(isMoving) {
            transform.position-=new Vector3(speed *Time.deltaTime, 0,0);
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
