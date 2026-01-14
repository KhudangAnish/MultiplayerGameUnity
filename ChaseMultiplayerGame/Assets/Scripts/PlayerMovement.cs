using UnityEngine;
using Unity.Netcode;
public class PlayerMovement : NetworkBehaviour
{
    public float Speed = 50;

    void Update()
    {
        // IsOwner will also work in a distributed-authoritative scenario as the owner 
        // has the Authority to update the object.
        if (!IsOwner || !IsSpawned) return;



        var hor = Input.GetAxisRaw("Horizontal");
        var ver = Input.GetAxisRaw("Vertical");

        var dir = new Vector3(hor, 0, ver);
        dir.Normalize();

        //if(dir.magnitude < 0)
        //{
        //    transform
        //}
        //else
        //{
        //}
        transform.position += dir * Speed * Time.deltaTime;


      
    }
}
