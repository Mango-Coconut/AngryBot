using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0;
        Vector3 moveDir = (cameraForward * v) + cameraRight * h;
        moveDir.Set(moveDir.x, 0, moveDir.z);
        controller.SimpleMove(moveDir * moveSpeed);
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }
    
    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0;
        plane.Raycast(ray, out enter);
        hitpoint = ray.GetPoint(enter);
        Vector3 lookDir = hitpoint - transform.position;
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }
}
