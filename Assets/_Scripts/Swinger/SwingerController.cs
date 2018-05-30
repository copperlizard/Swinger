using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwingerInput))]
[RequireComponent(typeof(Rigidbody))]
public class SwingerController : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 10.0f, m_turnSpeed = 20.0f;

    private SwingerInput m_input;

    private Rigidbody m_rigidbody;

    private Vector3 m_move = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
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
        m_rigidbody.velocity = m_moveSpeed * m_move;
    }

    public void Move (Vector2 move, Vector2 look)
    {
        m_move = transform.TransformDirection(new Vector3(move.normalized.x, 0.0f, move.normalized.y));
        transform.Rotate(0.0f, m_turnSpeed * look.x, 0.0f);
    }
}
