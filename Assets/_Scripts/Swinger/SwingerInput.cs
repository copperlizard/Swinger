using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwingerController))]
public class SwingerInput : MonoBehaviour
{   
    [SerializeField]
    private float m_maxReach = 50.0f, m_clickRadius = 0.3f;

    private SwingerController m_swinger;
    //private SwingerCamera m_cam;

    private SwingerRopeController m_ropes;

    private RaycastHit m_clickAt;

    private Vector2 m_move, m_look;

    private bool m_shotLeft = false, m_shotRight = false; 

	// Use this for initialization
	void Start ()
    {
        m_swinger = GetComponent<SwingerController>();
        if (m_swinger == null)
        {
            Debug.Log("[SwingerInput] m_controller not found!");
        }

        m_ropes = GameObject.FindGameObjectWithTag("RopeController").GetComponent<SwingerRopeController>();
        if (m_ropes == null)
        {
            Debug.Log("[SwingerInput] m_ropes not found!");
        }

        /*m_cam = GetComponent<SwingerCamera>();
        if (m_cam == null)
        {
            Debug.Log("[SwingerInput] m_cam not found!");
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Jump"))
        {
            m_swinger.Jump();
        }

        if (Input.GetButton("Fire1") && !m_shotLeft)
        {
            m_shotLeft = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out m_clickAt, m_maxReach))
            if (Physics.SphereCast(ray, m_clickRadius, out m_clickAt, m_maxReach))
            {
                // Left rope
                m_ropes.ShootRope(SwingerRopeController.Rope.Left, m_clickAt);
            }
        }

        if (Input.GetButton("Fire2") && !m_shotRight)
        {
            m_shotRight = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out m_clickAt, m_maxReach))
            if(Physics.SphereCast(ray, m_clickRadius, out m_clickAt, m_maxReach))
            {
                // Right Rope
                m_ropes.ShootRope(SwingerRopeController.Rope.Right, m_clickAt);
            }
        }

        if (!Input.GetButton("Fire1") && m_shotLeft)
        {
            //Debug.Log("Not holding Left!");
            // Release left rope
            m_ropes.ReleaseRope(SwingerRopeController.Rope.Left);
            m_shotLeft = false;
        }

        if (!Input.GetButton("Fire2") && m_shotRight)
        {
            //Debug.Log("Not holding Right!");
            // Release right rope
            m_ropes.ReleaseRope(SwingerRopeController.Rope.Right);
            m_shotRight = false;
        }

        // Get WASD input
        m_move.x = Input.GetAxis("Horizontal");
        m_move.y = Input.GetAxis("Vertical");

        // Prep mouse look input
        m_look = Input.mousePosition;
        m_look.x /= Screen.width;
        m_look.y /= Screen.height;
        m_look *= 2.0f;
        m_look -= Vector2.one;        

        m_swinger.Move(m_move, m_look);
    }

    /*private void FixedUpdate()
    {
        // Get Mouse1 Input
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out m_clickAt, m_maxReach))
            {
                // Left Rope
            }
        }

        // Get Mouse2 Input
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out m_clickAt, m_maxReach))
            {
                // Right Rope
            }
        }

        // Get WASD input
        m_move.x = Input.GetAxis("Horizontal");
        m_move.y = Input.GetAxis("Vertical");

        m_look.x = Input.GetAxis("Mouse X");
        m_look.y = Input.GetAxis("Mouse Y");

        m_controller.Move(m_move, m_look);
    }*/
}
