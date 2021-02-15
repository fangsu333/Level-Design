using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleController : MonoBehaviour
{
    private Vector3 move;
    // the world-relative desired move direction, calculated from the camForward and user input.

    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera (In Unity this is the Blue arrow)    

    [SerializeField] public float m_MovePower; // The force added to the player to move it.            

    private const float k_GroundRayLength = 1f; // The length of the ray to check if the player is grounded. We use 

    private Rigidbody m_Rigidbody; //this will hold the player's rigidbody

    public bool invertMovement;


    public bool isVerticalRunner;

    public float playerVSpeed;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        invertMovement = false;
    }


    private void Awake()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        //bdp: We calculate the direction of the movement of player's input at Update.        

        //bdp: I'm sure Abel covered Inputs in Masterclasses. This just "reads" the player input using the default controls set in Unity
        //bdp: by default it reads "left" and "right", or "A" and "D". You can change these at Edit > Project Settings > Input Manager
        float h = Input.GetAxis("Horizontal");

        //bdp: similar to above, just that this is "up", "down" (or "W" and "S")
        float v = Input.GetAxis("Vertical");        
                
        if (invertMovement)
        {
            h = -h;
            if (!isVerticalRunner)
            {
                v = -v;
            }
            
        }

        //bdp: make sure that we have a camera as the direction of the movement is calculated in relation to the camera (not the player)
        if (cam != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;            
            
            move = (v * camForward + h * cam.right).normalized;
            
            if (isVerticalRunner)
            {

                //if this is a vertical runner we just normalise the horizontal movement
                //and the forward movement is controlled by the variable playerSpeed
                move = (h * cam.right).normalized + (playerVSpeed * camForward);
            }
            
        }
    }

    private void FixedUpdate()
    {
        //bdp: We make the player move at FixedUpdate only

        /*BDP: IMPORTANT QUESTION!
        
        Why do we calculate input on Update, but just move the player on FixedUpdate?

        Update is called "asap" - any major oscillation in framerate or processing will create an irregular calling pattern. However,
        we want our inputs to be read asap, so we use update to calculate the movement direction

        FixedUpdate is called at regular intervals. This means the player will never "Jump" from one position to another if there is a major
        framerate or processing oscillation. Although since we're moving it through the Rigidbody, this "jump" would never occur (as we always depart 
        from the same position...) anyway, it is good practice to just force the movement at FixedUpdate.
    */
        Move(move);
        
    }



   
   
    public void Move(Vector3 moveDirection)
    {
        //bdp: We move by adding the force of the movement (calculated earlier in Update and expressed through the Vector3 "move" - think about all the 
        //bdp: Physics exercises you have done in school - that's what we're doing) to the player's rigidbody.

        //bdp: This force is multiplied by an arbitrary factor named m_MovePower
        //bdp: m_MovePower can be used to "mess up" movement, create variations in movement speed, for example        

        m_Rigidbody.AddForce(moveDirection * m_MovePower - m_Rigidbody.velocity);
    }


}
