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

    public LayerMask shootMask;

    private bool canShoot;

    private GameObject target;

    public AudioClip[] shootClips;

    private AudioSource shootSource;
    public float angleOffset = 30f;
    private void Start()
    {
        shootSource = gameObject.AddComponent<AudioSource>();
        Invoke("ResetCooldown", cooldown);
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
        if(hit.collider)
        {
            target = hit.collider.gameObject;
            Shoot();
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
            Invoke("straightBullet", .12f);
        if (isThreepeater)
        {
            diagonalBullet();
        }
    }

    void straightBullet()
    {
        GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
    }

    void diagonalBullet()
    {
        GameObject leftBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
        leftBullet.GetComponent<Bullet>().direction = BulletDirection.Left;

        GameObject rightBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
        rightBullet.GetComponent<Bullet>().direction = BulletDirection.Right;
    }

}
