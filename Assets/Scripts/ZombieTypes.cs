using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ZombieType", menuName = "Zombie")]
public class ZombieTypes : ScriptableObject
{
    public int health;
    public float speed;
    public int damage;
    public float range = .5f;
    public float eatCooldown = 1;
    public Sprite sprite;
    public RuntimeAnimatorController eatAnimation;
    public RuntimeAnimatorController deathAnimation;
    public RuntimeAnimatorController animator;
    public AudioClip hitClip;

    //Boss zombie
    public GameObject chat;
    public GameObject avatar;
    public GameObject portrait;
    public GameObject ultimateEffect;
    public Sprite newAvatar;
    public AudioSource guardAudioSource;
    public AudioSource teleAudioSource;
    public AudioSource ultimateAudioSource;
    public AudioSource attackAudioSource;

}
