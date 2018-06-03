using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwingerInput))]
[RequireComponent(typeof(Rigidbody))]
public class SwingerController : MonoBehaviour
{    
    [Header("Camera Settings")]
    [SerializeField]
    private float m_camHeight = 1.0f;

    [Header("Movement Settings")]
    [SerializeField]
    private float m_moveSpeed = 10.0f;
    [SerializeField]
    private float m_turnSpeed = 5.0f;

    [Header("Misc Settings")]
    [SerializeField]
    private Vector3 m_leftHandPos = new Vector3(-0.25f, 0.25f, 0.0f); //in local space...

    public enum Rope { Left, Right };

    private static bool created = false;

    private SwingerInput m_input;

    private Rigidbody m_rigidbody;

    private RaycastHit m_leftTar, m_rightTar, m_groundAt;

    private Vector3 m_move;

    private bool m_grounded = false, m_shootLeft = false, m_shootRight = false;

	// Use this for initialization
	void Start ()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake(don't destroy on load): " + this.gameObject);
        }
        else
        {
            Debug.Log("Destroying Duplicate: " + this.gameObject);
            Destroy(this.gameObject);
        }

        m_input = GetComponent<SwingerInput>();
        if(m_input == null)
        {
            Debug.Log("[SwingerController] m_input not found!");
        }

        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            Debug.Log("[SwingerController] m_rigidbody not found!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        // Need to handle "Ground"
        //   - Detect on ground
        //   - Stay upright on the ground
        CheckGround();

        // Ropes (ropes have both physics and visuals...)
        //   - Need to "shoot" shot ropes (ropes should have travel speed???)...
        //   - Need to apply forces for held ropes...

        // Appropriate character/camera rotations/orientation...
        //   - Character Orientation should lerp towards character velocity (hopefully convey sense of "swinging")
        //   - Character Orientation should rotate based on mouse when at extents of screen (...allow clicking on things near middle of screen, turn camera at edges)
        //   - Character should self right when falling (not grounded or swinging)
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        m_grounded = Physics.Raycast(ray, out m_groundAt, m_camHeight, ~LayerMask.GetMask("Player"));
        if(m_grounded)
        {
            //m_rigidbody.useGravity = false;
            float sink = transform.position.y - m_groundAt.point.y;
            if (sink < m_camHeight)
            {
                transform.position += Vector3.up * (m_camHeight - sink);
            }

            m_rigidbody.AddForce(Vector3.up * m_rigidbody.mass * 9.8f);

            // Apply stopping force inversely-proportional to m_move
        }
        /*else
        {
            m_rigidbody.useGravity = true;
        }*/
    }

    public void Move (Vector2 move, Vector2 look)
    {
        m_move = transform.TransformDirection(new Vector3(move.normalized.x, 0.0f, move.normalized.y));        
        m_rigidbody.AddForce(m_move * m_moveSpeed);
        transform.Rotate(0.0f, m_turnSpeed * look.x, 0.0f);
    }

    public void ShootRope(Rope rope, RaycastHit tar)
    {
        if (rope == Rope.Left)
        {
            m_shootLeft = true;
            m_leftTar = tar;

        }
        else
        {
            m_shootRight = true;
            m_rightTar = tar;
        }
    }

    public void ReleaseRope(Rope rope)
    {
        if(rope == Rope.Left)
        {
            m_shootLeft = false;
        }
        else
        {
            m_shootRight = false;
        }
    }
}
