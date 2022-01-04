using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float jumpForce, normalSpeed, turnSpeed;
    float horizontalInput, verticalInput, timeShowBtwJumps, speed, fastSpeed;
    Rigidbody rb;
    [SerializeField] float jumpAmount, timeBtwJumps;

#region Stomp
    [SerializeField] float stompDecceleration, MaxHeightBoost;      //Downward and forward force the player applies, stomp decceleration/duration
    private float stompTimer, stompForce;
    private bool readyToStomp, groundCollision = true;      //bool to check if the player has pressed the stomp key
    private Vector3 stomp;
    [Range(1,10)]             //Range is used to set values using a slider in the inspector on the unity window
    public float speedMultiplier;           //The speed multiplier for the player based on height
    public float speedMultiplierHeight;     //The maximum height for effective speed multiplication
    public float speedDecceleration;
    public float maxSpeed;
#endregion

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        timeShowBtwJumps = timeBtwJumps;
        speed = normalSpeed;
    }
    void FixedUpdate(){
        //New code starts here
        if (transform.position.y/4 > MaxHeightBoost && groundCollision == false){
            stompForce = MaxHeightBoost;
        } else if (transform.position.y/4 < MaxHeightBoost && groundCollision == false){
            stompForce = transform.position.y/4;
        } else{
            stompForce = 0;
        }
        if(readyToStomp){                       //Takes input from Update method
            stompTimer = stompDecceleration;    
            readyToStomp = false;
        }
        if(stompTimer > 0 && groundCollision == false){
            stompTimer -= Time.fixedDeltaTime;
            stomp = Vector3.Normalize(new Vector3(rb.velocity.x, 0, rb.velocity.z)) * stompForce;
            //The line above adds horizontal force to the player's direction which reduces over time
            rb.AddForce(new Vector3(0, (-(stompForce)), 0));
            //The line above adds a downward force which reduces until -10. You can modify the "-10" as you wish
        } else stomp = Vector3.zero;

        float multiplier = ((speedMultiplier-1) * (transform.position.y/speedMultiplierHeight)) + 1;    //Calculates multiplier based on height above y at 0
        multiplier = Mathf.Clamp(multiplier, 1f, speedMultiplier);      //Clamps the multiplier so it is not less than 1
        //New code ends here
        
        Vector3 move = transform.forward * verticalInput * speed * multiplier;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z) + stomp;       //Line modification. Added stomp Vector to the initial movement code

        transform.Rotate(0, horizontalInput * turnSpeed * verticalInput, 0, Space.World);
        if (timeShowBtwJumps <= 0){
            if (jumpAmount != 0 && Input.GetKey(KeyCode.Space)){
                groundCollision = false;
                jumpAmount -= 1;
                rb.AddForce(Vector3.up * jumpForce);
                timeShowBtwJumps = timeBtwJumps;
            }
        }
        else
        {
            timeShowBtwJumps -= Time.deltaTime;
        }
        if (speed > normalSpeed && groundCollision){
            speed -= speedDecceleration * Time.deltaTime;
        }
    }
    void Update(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.C)){
            fastSpeed = stompForce;
            if (speed + fastSpeed < maxSpeed)
            {
                speed += fastSpeed;
            } else{
                speed = maxSpeed;
            }
            readyToStomp = true;            //Uses a bool to register input
        }
    }
    void OnCollisionEnter(Collision collider){
        if (collider.collider.CompareTag("Ground")){
            groundCollision = true;
        }
    }
}