using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D EnemyBody;
    private float enemyHealth;
    private float enemySpeed;
    private Vector2 enemyDirection;
    public Animator animator;

    private Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 50f;
        enemySpeed = 0.7f;
        playerPosition = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // EnemyBody.position
        enemyDirection = new Vector2(playerPosition.position.x - EnemyBody.position.x, playerPosition.position.y - EnemyBody.position.y).normalized;

        // Animation
        animator.SetFloat("Horizontal", enemyDirection.x);
        animator.SetFloat("Vertical", enemyDirection.y);
        animator.SetFloat("Speed", enemySpeed);
    }

    void FixedUpdate()
    {
        // Movement
        EnemyBody.MovePosition(EnemyBody.position + enemyDirection * enemySpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        SpriteRenderer followerSprite = EnemyBody.gameObject.GetComponent<SpriteRenderer>();
        followerSprite.color = Color.red;
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            followerSprite.color = Color.black;
        }
        Debug.Log("Take Damage");
    }
}
