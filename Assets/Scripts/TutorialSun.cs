using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSun : MonoBehaviour
{
    public float dropToYPos;
    private float speed = 0.1f;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y > dropToYPos)
        {
            transform.position -= new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }
    }
}
