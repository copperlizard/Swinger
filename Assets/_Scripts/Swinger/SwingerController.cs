﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwingerInput))]
[RequireComponent(typeof(Rigidbody))]
public class SwingerController : MonoBehaviour
{    
    [Header("Camera Settings")]
    [SerializeField]
    private float m_camHeight = 1.0f;
    [SerializeField]
    private float m_camLookEdge0 = 0.3f, m_camLookEdge1 = 1.0f;

    [Header("Movement Settings")]
    [SerializeField]
    private float m_moveSpeed = 10.0f;
    [SerializeField]
    private float m_airMoveSpeed, m_turnSpeed = 5.0f, m_jumpForce = 10.0f;

    /*[Header("Misc Settings")]
    //[SerializeField]
    public Vector3 m_leftHandPos = new Vector3(-0.25f, 0.25f, 0.0f); //in local space...

    [HideInInspector]
    public Vector3 m_rightHandPos; //Ymirror of left hand pos*/

    private static bool created = false;

    private SwingerInput m_input;

    private Rigidbody m_rigidbody;
    
    private RaycastHit m_groundAt;

    private Vector3 m_move;

    private float m_xAng = 0.0f, m_yAng = 0.0f;

    private bool m_grounded = false, m_jumping = false;

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

        //m_rightHandPos = new Vector3(-m_leftHandPos.x, m_leftHandPos.y, m_leftHandPos.z);

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
                

        //Starting orientation (z always 0)
        m_xAng = transform.rotation.eulerAngles.x;
        m_yAng = transform.rotation.eulerAngles.y;
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
        //m_grounded = Physics.SphereCast(ray, 0.5f, out m_groundAt, m_camHeight - 0.5f, ~LayerMask.GetMask("Player"));
        if(m_grounded)
        {
            m_rigidbody.useGravity = false;
            float sink = transform.position.y - m_groundAt.point.y;
            if (sink < m_camHeight)
            {
                transform.position += Vector3.up * (m_camHeight - sink - 0.01f);
            }

            m_rigidbody.AddForce(Vector3.up * m_rigidbody.mass * 9.8f);
        }
        else
        {
            //Debug.Log("airborne!");
            m_jumping = false;
            m_rigidbody.useGravity = true;
        }
    }

    public void Move (Vector2 move, Vector2 look)
    {
        m_move = transform.TransformDirection(new Vector3(move.normalized.x, 0.0f, move.normalized.y));
        m_move = Vector3.ProjectOnPlane(m_move, Vector3.up).normalized;
        //m_move = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * new Vector3(move.normalized.x, 0.0f, move.normalized.y);
        
        if(m_grounded)
        {
            //m_rigidbody.AddForce(m_move * m_moveSpeed, ForceMode.Impulse);
            m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, m_move * m_moveSpeed, 0.8f);
            
        }
        else
        {
            m_rigidbody.AddForce(m_move * m_airMoveSpeed, ForceMode.Impulse);
        }
                
        look.x = FSmoothstep(m_camLookEdge0, m_camLookEdge1, Mathf.Abs(look.x)) * ((look.x > 0.0f)? 1.0f : -1.0f);
        look.y = FSmoothstep(m_camLookEdge0, m_camLookEdge1, Mathf.Abs(look.y)) * ((look.y > 0.0f) ? 1.0f : -1.0f); 

        m_xAng += m_turnSpeed * -look.y;
        m_yAng += m_turnSpeed * look.x * ((Vector3.Dot(transform.up, Vector3.up) > 0.0f) ? 1.0f : -1.0f);
        transform.rotation = Quaternion.Euler(m_xAng, m_yAng, 0.0f);
    }

    public void Jump ()
    {
        if(m_grounded && !m_jumping)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            m_grounded = false;
            m_jumping = true;
        }
    }

    private float FSmoothstep(float edge0, float edge1, float x)
    {
        float t; 
        t = Mathf.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return t * t * (3.0f - 2.0f * t);
    }

    public Vector3 Momentum()
    {
        return m_rigidbody.mass * m_rigidbody.velocity;
    }
}
