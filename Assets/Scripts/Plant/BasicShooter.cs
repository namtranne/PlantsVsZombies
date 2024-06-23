using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : MonoBehaviour
{
    public GameObject bullet;

    public Transform shootOrigin;

    public float cooldown;

    public float range;

    public bool isRepeater;

    public bool isThreepeater;

    public bool isSplit;

    public bool isGaling;

    public LayerMask shootMask;

    private bool canShoot;

    public AudioClip[] shootClips;

    private AudioSource shootSource;

    private float laneWidth = 1.66f;
    private void Start()
    {
        shootSource = gameObject.AddComponent<AudioSource>();
        Invoke("ResetCooldown", cooldown);
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
        if(hit.collider) Shoot();
        else if (isThreepeater)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0, laneWidth, 0), Vector2.right, range, shootMask);
            if (hit.collider)
            {
                Shoot();
                return;
            }
            hit = Physics2D.Raycast(transform.position - new Vector3(0, laneWidth, 0), Vector2.right, range, shootMask);
            if (hit.collider) Shoot();
        }
        else if (isSplit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
            if (hit.collider) Shoot();
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    void Shoot()
    {
        if(!canShoot) return;

        shootSource.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);

        canShoot = false;
        Invoke("ResetCooldown", cooldown);

        straightBullet();
        if (isRepeater)
            Invoke("straightBullet", .1f);
        if (isThreepeater)
        {
            diagonalBullet();
        }
        if (isSplit)
        {
            GameObject behindBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
            behindBullet.GetComponent<Bullet>().direction = BulletDirection.Behind;
        }
        if (isGaling)
        {
            Invoke("straightBullet", .1f);
            Invoke("straightBullet", .2f);
            Invoke("straightBullet", .3f);
        }
    }

    void straightBullet()
    {
        Instantiate(bullet, shootOrigin.position, Quaternion.identity);
    }

    void diagonalBullet()
    {
        GameObject leftBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
        leftBullet.GetComponent<Bullet>().direction = BulletDirection.Top;

        GameObject rightBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
        rightBullet.GetComponent<Bullet>().direction = BulletDirection.Bottom;
    }

}
