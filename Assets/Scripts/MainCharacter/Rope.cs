using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject ropeMax;    // Độ dài của dây thừng

    private LineRenderer ropeLineRenderer;          // Dây thừng khi chưa căng
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.25f;
    [SerializeField] private int segmentLength;
    private float lineWidth = 0.1f;

    private LineRenderer maxRopeLineRenderer;       // Dây thừng khi đã căng ra (khoảng cách 2 player bằng độ dài của dây thừng)

    private SpringJoint2D joint;

    private PlayerController playerController1;
    private PlayerController playerController2;

    void Start()
    {
        ropeLineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = new Vector3(this.player1.transform.position.x, this.player1.transform.position.y, 0.1f);

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        joint = player1.GetComponent<SpringJoint2D>();
        joint.connectedBody = player2.GetComponent<Rigidbody2D>();

        maxRopeLineRenderer = ropeMax.GetComponent<LineRenderer>();
        maxRopeLineRenderer.enabled = false;

        playerController1 = player1.GetComponent<PlayerController>();
        playerController2 = player2.GetComponent<PlayerController>();
    }

    void Update()
    {
        this.DrawRope();

        float distance = Vector2.Distance(player1.transform.position, player2.transform.position);

        // Khi 1 trong 2 đi về hướng của nhau, tắt SpringJoint2D và vẽ dây thừng bình thường
        if ((playerController1.isRunToPlayer(playerController2) || playerController2.isRunToPlayer(playerController1))
           && joint.enabled
           && playerController1.isGrounded() 
           && playerController2.isGrounded())
        {
            joint.enabled = false;
            maxRopeLineRenderer.enabled = false;
            ropeLineRenderer.enabled = true;
        }

        // Khi khoảng cách đạt tối đa, bật SpringJoint2D và vẽ dây thừng căng
        if (distance >= 5 && !joint.enabled)
        {
            joint.enabled = true;
            maxRopeLineRenderer.enabled = true;
            ropeLineRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        Vector3 forceGravity = new Vector3(0f, -1.5f, 0);

        // Tính vị trí mới cho từng segment của dây thừng
        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment ropeSegment = this.ropeSegments[i];
            Vector3 velocity = ropeSegment.posNow - ropeSegment.posOld;
            ropeSegment.posOld = ropeSegment.posNow;
            ropeSegment.posNow += velocity;
            ropeSegment.posNow += forceGravity * Time.deltaTime;
            this.ropeSegments[i] = ropeSegment;
        }

        this.ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        // Constrant to First Point
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = new Vector3(this.player1.transform.position.x, this.player1.transform.position.y, 0.1f);
        this.ropeSegments[0] = firstSegment;

        // Constrant to Final Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = new Vector3(this.player2.transform.position.x, this.player2.transform.position.y, 0.1f);
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        // Constrant between 2 Point
        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector3 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector3 changeAmount = changeDir * error;
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

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        ropeLineRenderer.startWidth = lineWidth;
        ropeLineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];

        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        ropeLineRenderer.positionCount = ropePositions.Length;
        ropeLineRenderer.SetPositions(ropePositions);

        // Max rope length
        Vector3[] ropeMaxPositions = new Vector3[2];
        ropeMaxPositions[0] = new Vector3(player1.transform.position.x, player1.transform.position.y, 0.1f);
        ropeMaxPositions[1] = new Vector3(player2.transform.position.x, player2.transform.position.y, 0.1f);
        maxRopeLineRenderer.startWidth = lineWidth;
        maxRopeLineRenderer.endWidth = lineWidth;
        maxRopeLineRenderer.positionCount = 2;
        maxRopeLineRenderer.SetPositions(ropeMaxPositions);
    }

    // Từng đoạn dây thừng
    public struct RopeSegment
    {
        public Vector3 posNow;
        public Vector3 posOld;

        public RopeSegment(Vector3 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
