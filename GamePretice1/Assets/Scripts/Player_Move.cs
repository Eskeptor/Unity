using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {
    public float Speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRig;
    int floorMask;
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRig = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }
	
    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * Speed * Time.deltaTime;
        playerRig.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRot = Quaternion.LookRotation(playerToMouse);
            playerRig.MoveRotation(newRot);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalk", walking);
    }
}
