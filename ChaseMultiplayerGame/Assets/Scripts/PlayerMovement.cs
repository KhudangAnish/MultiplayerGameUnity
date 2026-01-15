using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using Unity.Mathematics;
public class PlayerMovement : NetworkBehaviour
{
    public Rigidbody rb;
    private Vector3 dir;

    float horizontal;
    float vertical;
    Vector3 lastDirection;

    [SerializeField] GameObject footPoint;
    bool isGrounded = false;
    bool isJump = false;

    void Update()
    {
        // IsOwner will also work in a distributed-authoritative scenario as the owner 
        // has the Authority to update the object.
        if (!IsOwner || !IsSpawned) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        dir = new Vector3(horizontal, 0, vertical);
        dir.Normalize();

        Jump();

        if (dir.magnitude > 0.5f)
        {
           lastDirection = dir;
        }
        transform.forward = Vector3.Lerp(transform.forward, lastDirection,0.2f);
        // transform.position += dir * Speed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!IsOwner || !IsSpawned) return;

        //Checks the collision groinded or not
        if (Physics.OverlapBox(footPoint.transform.position, new Vector3(0.6f, 0.6f, 0.6f),quaternion.identity,LayerMask.GetMask("Ground")).Length > 0) isGrounded = true;
        else isGrounded = false;

        //Acceleration
        float speed = Mathf.Clamp(rb.linearVelocity.magnitude, 30, 60);
        //Deacceleration
        if (dir.magnitude == 0 && isGrounded) rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero,0.1f);

        //Movement
        //Applying Acceleration less while in Air
        dir = isGrounded ? dir : Vector3.ClampMagnitude(dir, 0.25f);
        rb.AddForce(dir * speed, ForceMode.Acceleration);

        //Gravity pushes downards by more force when in a air
        rb.AddForce(Physics.gravity * Mathf.Lerp(0,30,0.05f), ForceMode.Acceleration);
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            var jumpForce = 15f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(footPoint.transform.position, new Vector3(0.6f, 0.6f, 0.6f));
    }
}
