using UnityEngine;

public class FlailHead : MonoBehaviour
{
    public Rigidbody2D flailHead;
    private Transform playerPosition;
    private Vector2 flailPosition;
    private float flailRange;
    private float flailRangeSquared;

    public float flailSize;
    public float flailDamage;
    public LayerMask whatIsEnemies;


    // Start is called before the first frame update
    void Start()
    {
        // Get the Player Object
        GameObject PlayerObject = GameObject.Find("Player");

        // Get the Transform of the Player Object
        playerPosition = PlayerObject.GetComponent<Transform>();

        // Set the flail range
        flailRange = 2f;
        flailRangeSquared = flailRange * flailRange;
    }

    // Update is called once per frame
    void Update()
    {
        // // // // // // // // //
        // Flail head position  //
        // // // // // // // // //

        // Get the vector between the Mouse and the Player
        Vector3 playerToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerPosition.position;

        Vector2 playerToMouse2D = new Vector2(playerToMouse.x, playerToMouse.y);
        Vector2 playerPosition2D = new Vector2(playerPosition.position.x, playerPosition.position.y);

        SpriteRenderer followerSprite = flailHead.gameObject.GetComponent<SpriteRenderer>();

        // Cap the Flail position and set the color
        if (playerToMouse2D.sqrMagnitude <= flailRangeSquared)
        {
            flailPosition = playerPosition2D + playerToMouse2D;
            followerSprite.color = Color.white;
        }
        else 
        {
            flailPosition = playerPosition2D + (playerToMouse2D.normalized * flailRange);
            followerSprite.color = Color.red;
        }

        // Set the flail position
        flailHead.MovePosition(flailPosition);


        // // // // // // // //
        // Flail head damage //
        // // // // // // // //

        Collider2D[] enemieToDamage = Physics2D.OverlapCircleAll(flailHead.position, flailSize, whatIsEnemies);
        for (int i = 0; i < enemieToDamage.Length; i++)
        {
            enemieToDamage[i].GetComponent<Enemy>().TakeDamage(flailDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(flailHead.position, flailSize);
    }
}
