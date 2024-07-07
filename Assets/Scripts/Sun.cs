using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float dropToYPos;
    private float speed = 0.1f;
    public int value;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 12);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.isPaused) return;

        if (transform.position.y > dropToYPos)
        {
            transform.position -= new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }
    }
}
