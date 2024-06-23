using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletDirection
{
    Top,
    Bottom,
    Straight,
    Behind
}

public class Bullet : MonoBehaviour
{
    public float damage;

    public float speed = 2f;

    public bool freeze;

    public BulletDirection direction = BulletDirection.Straight;

    private float angle = Mathf.Deg2Rad * 75f;

    public float laneWidth;

    private Vector3 shootOrigin;

    private void Start()
    {
        shootOrigin = transform.position;
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (direction)
        {
            case BulletDirection.Straight:
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                break;
            case BulletDirection.Top:
                transform.position += new Vector3(speed * Time.deltaTime, Mathf.Sin(angle) * speed * Time.deltaTime, 0);
                if (transform.position.y - shootOrigin.y >= laneWidth)
                {
                    direction = BulletDirection.Straight;
                }
                break;
            case BulletDirection.Bottom:
                transform.position += new Vector3(speed * Time.deltaTime, Mathf.Sin(-angle) * speed * Time.deltaTime, 0);
                if (shootOrigin.y - transform.position.y >= laneWidth) direction = BulletDirection.Straight;
                break;
            case BulletDirection.Behind:
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            zombie.Hit(damage, freeze);
            Destroy(gameObject);
        }
    }
}
