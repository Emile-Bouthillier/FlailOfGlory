using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D EnemyBody;
    public float MaxHealth = 50f;
    private float enemyHealth;
    private float enemySpeed;
    private Vector2 enemyDirection;
    public Animator animator;
    public Slider slider;
    public GameObject HealthBarUI;
    public BoxCollider2D EnemyCollider;

    private Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = MaxHealth;
        enemySpeed = 0.7f;
        playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        slider.value = calculateHealth();
        HealthBarUI.SetActive(false);
        EnemyCollider = GetComponent<BoxCollider2D>();
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

        // Manage Health Bar
        slider.value = calculateHealth();

        if (enemyHealth < MaxHealth)
        {
            HealthBarUI.SetActive(true);
        }

        if (enemyHealth > MaxHealth)
        {
            enemyHealth = MaxHealth;
        }

        if (enemyHealth <= 0)
        {
            animator.Play("Demon_Death");
            HealthBarUI.SetActive(false);
            enemySpeed = 0f;
            EnemyCollider.enabled = false;
        }
    }

    void FixedUpdate()
    {
        // Movement
        EnemyBody.MovePosition(EnemyBody.position + enemyDirection * enemySpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        SpriteRenderer followerSprite = EnemyBody.gameObject.GetComponent<SpriteRenderer>();
        enemyHealth -= damage;
        Debug.Log("Take Damage");
    }

    float calculateHealth()
    {
        float health = enemyHealth / MaxHealth;
        return health;
    }
}
