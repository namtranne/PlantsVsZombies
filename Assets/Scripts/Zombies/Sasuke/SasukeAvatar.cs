using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SasukeAvatar : MonoBehaviour
{

    public Vector3 target;  // Target position to move towards
    public float speed = 10f;  // Duration of the movement

    // Start is called before the first frame update
    // public Canvas objectCanvas;
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1000;
        target = transform.position + new Vector3(0,0,0);
        transform.position-=new Vector3(6, 0,0);
    }

    // Call this method to start the movement
    void Update() {
        if(transform.position.x < target.x) {
            transform.position+= new Vector3(1*speed *Time.deltaTime, 0, 0);
        }
        else {
            Invoke("Destroy", 2.3f);
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
