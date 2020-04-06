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
    }

    // Draw the rope
    private void DrawRope()
    {
        // Define the width of the line
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth/2;

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

    // FixedUpdate is called once per fixed framerate frame
    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        // Set Gravity to control the rope. 
        //TODO: Change the Gravity logic in a Coil logic for the MornigStar Whip
        //TODO: Use the mouse to act as a gravity well when attacking ?
        Vector2 forceGravity = new Vector2(0f, 0f);

        // Update the position of each point (segment) in the list of segments
        for (int i = 0; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        // Apply constraints, more iterations will result in a stiffer rope, less iterations will give a springier rope
        for (int i = 0; i < 50; i++)
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
