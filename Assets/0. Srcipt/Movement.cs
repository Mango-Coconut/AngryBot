using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    new Transform transform;
    Animator animator;
    new Camera camera;
    Plane plane;
    Ray ray;
    Vector3 hitpoint;
    public float moveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
        plane = new Plane(transform.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
    }

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");
    void Move()
    {
        
    }
    
    void Turn()
    {
        
    }
}
