using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float jumpForce, speed, turnSpeed;
    float horizontalInput, verticalInput, CheckDistance;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask GroundMask;
    bool canJump;
    Rigidbody rb;
    void Awake(){
        rb = GetComponent<Rigidbody>();
    }
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate(){
        Vector3 move = transform.forward * verticalInput * speed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        transform.Rotate(0, horizontalInput * turnSpeed * verticalInput, 0, Space.World);
        if (canJump && Input.GetMouseButton(1)){
            rb.velocity = Vector3.up * jumpForce * Time.deltaTime;
        }
    }
    void Update(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        canJump = Physics.CheckSphere(GroundCheck.position, CheckDistance, GroundMask);
    }
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.transform.position, CheckDistance);
    }
    void OnCollisionEnter(Collision collider){
        if (collider.collider.CompareTag("Player")){
            Explosion(rb.velocity.x, rb.velocity.y, rb.velocity.z, new Vector3(transform.position.x - 10, transform.position.y, transform.position.z));
        }
    }
    void Explosion(float a, float b, float c, Vector3 explosionPos){ 
        rb.AddExplosionForce(Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2) + Mathf.Pow(c, 2)), explosionPos, 500, 1f, ForceMode.Force);
    }
}
