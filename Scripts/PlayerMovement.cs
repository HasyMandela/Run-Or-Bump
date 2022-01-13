using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
public class PlayerMovement : NetworkBehaviour
{
    [Header("Speed")]
    [SerializeField] float normalSpeed, turnSpeed;
    float horizontalInput, verticalInput, speed, fastSpeed;
    [SerializeField] float maxAddedSpeed;      //The maximum speed added to the player (The speed is determined by the height and so this is the maxiumum height the player can get max speed on)
    [SerializeField] float maxSpeed;
    [SerializeField] float cutSpeed;  //This speed will cut between the walk speed and run speed
    [Range(1,10)]
    [SerializeField]float momentumBuild; //Use To See How Fast The Player Build Momentum (1 for fastest & 10 for slowest)


    [Header("Jump")]
    [SerializeField] float jumpAmount, timeBtwJumps;
    [SerializeField] float jumpForce;
    float timeShowBtwJumps;


    [Header("Stomp")]
    private float stompTimer, stompForce;
    private bool readyToStomp, groundCollision = true;      //bool to check if the player has pressed the stomp key
    private Vector3 stomp;


    [Header("Components")]
    Rigidbody rb;
    Animator anim;
    [SerializeField] CinemachineFreeLook cinemachine;

    //Singleton instantation
    private static PlayerMovement instance;
    public static PlayerMovement Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<PlayerMovement>();
            return instance;
        }
    }
    
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        timeShowBtwJumps = timeBtwJumps;
        speed = normalSpeed;
        cinemachine.Follow = transform;
        cinemachine.LookAt = transform;
    }
     public override void OnStartLocalPlayer()
    {
         rb = GetComponent<Rigidbody>();
         anim = GetComponent<Animator>();
         cinemachine.m_Priority = 100;
         cinemachine.transform.parent = null;
    }
    void FixedUpdate(){
        //New code starts here
        //Setting stompForce
        if (transform.position.y/4 > maxAddedSpeed && groundCollision == false){
            stompForce = maxAddedSpeed;
        } else if (transform.position.y/4 < maxAddedSpeed && groundCollision == false){
            stompForce = transform.position.y/2;
        } else{
            stompForce = 0;
        }


        //Triggers when the stomp Key is pressed
        if(readyToStomp){
            stompTimer = 1;    
            readyToStomp = false;
        }


        //Add Stomp Force To The Player
        if(stompTimer > 0 && groundCollision == false){
            stompTimer -= Time.fixedDeltaTime;
            stomp = Vector3.Normalize(new Vector3(rb.velocity.x, 0, rb.velocity.z)) * stompForce;
            //The line above adds horizontal force to the player's direction which reduces over time
            rb.AddForce(new Vector3(0, (-(stompForce)), 0));
            //The line above adds a downward force which reduces until -10. You can modify the "-10" as you wish
        } else stomp = Vector3.zero;


        // Initial Player Movement Code
        Vector3 move = transform.forward * verticalInput * speed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z) + stomp;
        transform.Rotate(0, horizontalInput * turnSpeed * verticalInput, 0, Space.World);

        
        //Jumping Code
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
            speed -= momentumBuild * Time.deltaTime;
        }


        //Check for animation 
        if (verticalInput == 0){
            anim.SetBool("Is Running", false);
            anim.SetBool("Is Walking", false);

        }else if (speed > cutSpeed){
            anim.SetBool("Is Running", true);
            anim.SetBool("Is Walking", false);
        }else if (speed < cutSpeed){
            anim.SetBool("Is Running", false);
            anim.SetBool("Is Walking", true);
        }

    }
    void Update(){
        //Network For Local Players (This will make so that nobody can move the other players player).
         if (!isLocalPlayer)
            return;

        //Getting The Input From The Player
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");


        //Check if the stomp Key is Pressed
        if(Input.GetMouseButtonDown(1)){
            fastSpeed = stompForce;
            if (speed + fastSpeed < maxSpeed)
            {
                speed += fastSpeed;
            } else{
                speed = maxSpeed;
            }
            readyToStomp = true;
        }

    }
    void OnCollisionEnter(Collision collider){
        if (collider.collider.CompareTag("Ground")){
            groundCollision = true;
        }
    }
}