using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTracking : MonoBehaviour
{
    public Text MousePosition;
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
        // Get the mouse coordinates with respect to the player position
        Vector3 mouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerPosition.position;

        // Update the Text
        MousePosition.text = "Mouse Position : " + mouseCoord.x.ToString("G4") + "x" + ", " + mouseCoord.y.ToString("G4") + "y" + ", " + mouseCoord.z.ToString("G4") + "z";
    }
}
