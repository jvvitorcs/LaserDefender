using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    [Header("Enemy Status")]
	[SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;


    [Header("Shooting")]
    float shotCounter;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[SerializeField] GameObject projectile;
	[SerializeField] float projectileSpeed = 10f;

    [Header("ShoundEffects")]
    [SerializeField] GameObject prefabStars;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    void Start () {
		shotCounter = Random.Range (minTimeBetweenShots, maxTimeBetweenShots);

	}
	
	void Update () {
		CountDownAndShoot();
	}
	private void CountDownAndShoot(){
		shotCounter -= Time.deltaTime;
		if (shotCounter <= 0f) {
			Fire ();
			shotCounter = Random.Range (minTimeBetweenShots, maxTimeBetweenShots);
		}
	}

	private void Fire(){
		GameObject laser = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

	private void OnTriggerEnter2D(Collider2D other){
		DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
		ProcessHit (damageDealer);
        if(other.gameObject.tag == "Shot")
        {
            Destroy(other.gameObject);
        }
	}
	private void ProcessHit(DamageDealer damageDealer){
		health -= damageDealer.GetDamage();
		if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(prefabStars, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}