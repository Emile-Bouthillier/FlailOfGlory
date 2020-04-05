using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTracking : MonoBehaviour
{
    public Text Position;
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
        Position.text = "Player Position : " + playerPosition.position.x.ToString("G4") + "x" + ", " + playerPosition.position.y.ToString("G4") + "y";
    }
}
