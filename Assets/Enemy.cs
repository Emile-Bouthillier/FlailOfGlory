using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D EnemyBody;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        SpriteRenderer followerSprite = EnemyBody.gameObject.GetComponent<SpriteRenderer>();
        followerSprite.color = Color.red;
        health -= damage;
        if (health <= 0)
        {
            followerSprite.color = Color.black;
        }
        Debug.Log("Take Damage");
    }
}
