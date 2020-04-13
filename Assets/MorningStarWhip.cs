using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningStarWhip : MonoBehaviour
{
    // Declare a variable of type LineRenderer
    private LineRenderer lineRenderer;
    // Declare list of RopeSegment elements
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    // Define the length of every rope segments
    private float ropeSegLen = 0.25f;
    // Define the total amount of segment in the rope
    private int segmentLength = 15;
    // Define the line width
    private float lineWidth = 0.1f;
    // Declare a variable of type Transform
    private Transform playerPosition;
    // Declare a variable for the whip direction
    private Vector2 directionWhip = new Vector2(0f, 0f);
    // Declare a flag to detect when a whip acction has happened
    private bool whipped = false;
    // Sprite Rendering
    public float spriteScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player object and link its Transform information to a private variable
        GameObject PlayerObject = GameObject.FindGameObjectsWithTag("Player")[0];
        playerPosition = PlayerObject.GetComponent<Transform>();

        // Link the LineRenderer information of the current object to a private variable
        this.lineRenderer = this.GetComponent<LineRenderer>();

        // Link the start of the rope to the Player
        Vector3 ropeStartPoint = playerPosition.position;

        // Add the desired amount of rope segments to the list of rope segments
        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawRope();
        //this.DrawWhip();
    }

    // Draw the rope
    private void DrawRope()
    {
        // Define the width of the line
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Instantiate and assign an arry of vectors to a variable
        Vector3[] ropePositions = new Vector3[this.segmentLength];

        // For each segment, we add the coordinates of the point to the array
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        // Set the number of vertices in the line
        lineRenderer.positionCount = ropePositions.Length;
        // Set the position of each vertice in the line
        lineRenderer.SetPositions(ropePositions);
    }

    // TODO: Function to add a chain link shader on the whip. It doesnèt work right now.
    public void DrawWhip()
    {
        // Instantiate and assign an arry of vectors to a variable
        Vector3[] ropePositions = new Vector3[this.segmentLength];

        // For each segment, we add the coordinates of the point to the array
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        int spriteCount = Mathf.FloorToInt(Vector3.Distance(ropePositions[this.segmentLength-1], ropePositions[0]) / spriteScale);

        Vector3[] positions = new Vector3[] {
                     transform.position,
                     (ropePositions[this.segmentLength-1] - ropePositions[0]).normalized * spriteScale * spriteCount
                 };

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);

        if (lineRenderer.material != null)
            lineRenderer.material.mainTextureScale = new Vector2(spriteScale * spriteCount, 1);
        else
            Debug.LogError(name + "'s Line Renderer has no material!");
    }

    // FixedUpdate is called once per fixed framerate frame
    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        // Get the whip direction
        directionWhip.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - playerPosition.position.x;
        directionWhip.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - playerPosition.position.y;

        // Whip only if certain conditions are filled
        if (directionWhip.sqrMagnitude > 9 && !whipped)
        {
            Whip();
        }
        else // Retract the whip towards the player
        {
            if (directionWhip.sqrMagnitude < 9)
            {
                whipped = false;
            }

            Retract();
        }
    }

    private void Whip()
    {
        // Use the mouse to act as a pulling force on the whip when attacking
        Vector2 forceWhip;

        // Set a force to control the whip
        forceWhip = directionWhip.normalized * 30;

        // Update the position of each point (segment) in the list of segments
        for (int i = 0; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceWhip * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        // Apply constraints, more iterations will result in a stiffer rope, less iterations will give a springier rope
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraints();
        }

        if ((this.ropeSegments[0].posNow - this.ropeSegments[segmentLength-1].posNow).sqrMagnitude >= Mathf.Pow(segmentLength * ropeSegLen,2))
        {
            whipped = true;
        }
    }

    private void Retract()
    {
        // Retract the whip towards the player when not attacking
        Vector2 towardsPlayer = new Vector2(playerPosition.position.x, playerPosition.position.y);

        // Update the position of each point (segment) in the list of segments
        for (int i = 0; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += (towardsPlayer - firstSegment.posNow).normalized * 10 * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        // Apply constraints, more iterations will result in a stiffer rope, less iterations will give a springier rope
        for (int i = 0; i < 5; i++)
        {
            this.ApplyConstraints();
        }

    }

    private void ApplyConstraints()
    {
        // Attach the first segment to the Player
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = playerPosition.position;
        this.ropeSegments[0] = firstSegment;

        // 
        for (int i = 0; i < this.segmentLength -1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            } else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    // Method used to construct objects of type RopeSegment ?
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
