using UnityEngine;
using Unity.Netcode;
public class PlayerMovement : NetworkBehaviour
{
    public Rigidbody rb;
    public float Speed = 50;
    private Vector3 dir;

    void Update()
    {
        // IsOwner will also work in a distributed-authoritative scenario as the owner 
        // has the Authority to update the object.
        if (!IsOwner || !IsSpawned) return;



        var hor = Input.GetAxisRaw("Horizontal");
        var ver = Input.GetAxisRaw("Vertical");

        dir = new Vector3(hor, 0, ver);
        dir.Normalize();

        //if(dir.magnitude < 0)
        //{
        //    transform
        //}
        //else
        //{
        //}
        Jump();


        //TODO: Make it slow in the beginning and then faster
        Ray ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, 5) is false)
        {
            var downForce = 5f;
            rb.AddForce(Vector3.down * downForce, ForceMode.Impulse);
        }
       // transform.position += dir * Speed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!IsOwner || !IsSpawned) return;

        rb.AddForce(dir * Speed);


        

    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var jumpForce = 15f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
