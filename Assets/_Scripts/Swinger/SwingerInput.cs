﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwingerController))]
public class SwingerInput : MonoBehaviour
{   
    [SerializeField]
    private float m_maxReach = 100.0f;

    private SwingerController m_controller;

    private RaycastHit m_clickAt;

    private Vector2 m_move, m_look;

	// Use this for initialization
	void Start ()
    {
        m_controller = GetComponent<SwingerController>();
        if (m_controller == null)
        {
            Debug.Log("[SwingerInput] m_controller not found!");
        }
    }
	
	// Update is called once per frame
	void Update () {
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
    }
}