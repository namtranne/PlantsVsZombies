using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletDirection
{
    Left,
    Right,
    Straight
}

public class Bullet : MonoBehaviour
{
    public int damage;

    public float speed = 2f;

    public bool freeze;

    public BulletDirection direction = BulletDirection.Straight;

    public float angle = Mathf.Deg2Rad * 2f;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (direction)
        {
            case BulletDirection.Left:
                transform.position += new Vector3(speed * Time.deltaTime, Mathf.Sin(angle) * speed * Time.deltaTime, 0);
                break;
            case BulletDirection.Right:
                transform.position += new Vector3(speed * Time.deltaTime, Mathf.Sin(-angle) * speed * Time.deltaTime, 0);
                break;
            case BulletDirection.Straight:
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
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
