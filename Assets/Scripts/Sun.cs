using System.Collections;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float dropToYPos;
    public float initialSpeed = 1f;
    public float accelerationRate = 0.1f;
    private float currentSpeed;
    private int value;
    public float lifetimeDuration = 20f;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentSpeed = initialSpeed;
        StartCoroutine(SelfDestruct());
    }

    private void Update()
    {
        if (!GameManager.isPaused)
        {
            if (transform.position.y > dropToYPos)
            {
                currentSpeed += accelerationRate * Time.deltaTime;
                transform.position -= new Vector3(0, currentSpeed * Time.deltaTime, 0);
            }
        }
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    private void OnMouseDown()
    {
        if (!GameManager.isPaused && !gameManager.getIsDragging())
        {
            gameManager.AddSun(value);
            Destroy(gameObject);
        }
    }

    private IEnumerator SelfDestruct()
    {
        if(GameManager.isPaused)
            yield return null;
        
        yield return new WaitForSeconds(lifetimeDuration);
        Destroy(gameObject);
    }
}