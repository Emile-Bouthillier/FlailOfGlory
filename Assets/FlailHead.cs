using UnityEngine;

public class FlailHead : MonoBehaviour
{
    public Rigidbody2D rbmouse;
    private Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Player Object
        GameObject PlayerObject = GameObject.Find("Player");

        // Get the Transform of the Player Object
        playerPosition = PlayerObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the vector between the Mouse and the Player
        Vector3 playerToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerPosition.position;

        Vector2 playerToMouse2D = new Vector2(playerToMouse.x, playerToMouse.y);
        Vector2 playerPosition2D = new Vector2(playerPosition.position.x, playerPosition.position.y);

        // Update the Coin position
        const float mousefollowerDist = 4;
        if (playerToMouse2D.sqrMagnitude <= mousefollowerDist)
        {
            rbmouse.MovePosition(playerPosition2D + playerToMouse2D);
        }
        else { rbmouse.MovePosition(playerPosition2D + (playerToMouse2D.normalized * 2)); }

        // Get the Player Object
        SpriteRenderer followerSprite = rbmouse.gameObject.GetComponent<SpriteRenderer>();
        if (playerToMouse2D.sqrMagnitude > mousefollowerDist)
        {
            followerSprite.color = Color.red;
        }
        else { followerSprite.color = Color.white; }
        
    }
}
