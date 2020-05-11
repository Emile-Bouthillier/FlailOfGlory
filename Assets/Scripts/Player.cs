using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1f;

    public Rigidbody2D rb;
    public Animator animator;

    public float MaxHealth = 50f;
    public float CurrentHealth;

    public Slider healthSlider;

    Vector2 movement;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        healthSlider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Health
        healthSlider.value = CalculateHealth();
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    float CalculateHealth()
    {
        float health = CurrentHealth / MaxHealth;
        return health;
    }

    private void OnTriggerEnter2D(Collider2D others)
    {
        CurrentHealth -= 10;
    }
}
