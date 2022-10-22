using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05f;


    private bool isRunning = false;


    //Movement
    private CharacterController characterController;
    public float jumpForce = 4.0f;
    public float gravity = 12.0f;
    private float verticalVelocity;
   
    private int desiredLane = 1; //0 - Left, 1-Middle, 2-Right;


    //Speed modifier
    public  float originalSpeed = 7f;
    public float horizontalSpeed = 7f;
    public float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIcreaseAmount = 0.1f;


    //Animation

    private Animator playerAnimator;



    // Start is called before the first frame update
    void Start()
    {
        speed = originalSpeed;
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        if(Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIcreaseAmount;

            //Change modifier text
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }
        //Gather input on which lane we should be
        if (MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }
        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        //Calculate move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (int)((targetPosition - transform.position).x * horizontalSpeed);

        bool isGrounded = IsGrounded();
        playerAnimator.SetBool("Grounded", isGrounded);


        //calculate Y
        if (isGrounded)
        {
            verticalVelocity = -0.1f;


            if (MobileInput.Instance.SwipeUp)
            {
                //Jump
                playerAnimator.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }

            else if (MobileInput.Instance.SwipeDown)
            {
                //Slide
                StartSliding();
               
                Invoke("StopSliding", 1.7f);
            }
        }

        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            //fast falling mechanic
            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //Move the player;
        characterController.Move(moveVector * Time.deltaTime);

         //Rotate the player to where he is going
         Vector3 dir = characterController.velocity;
         dir.y = 0;
         transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new
        Vector3(characterController.bounds.center.x,
                (characterController.bounds.center.y - characterController.bounds.extents.y) + 0.2f,
                characterController.bounds.center.z),
                Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return (Physics.Raycast(groundRay, 0.2f + 0.1f));

    }


    public void StartRunning()
    {
        isRunning = true;
        playerAnimator.SetTrigger("StartRunning");
    }

    void StartSliding()
    {

        playerAnimator.SetBool("Sliding", true);
        characterController.height /= 2;
        characterController.center = new Vector3(characterController.center.x, characterController.center.y / 2, characterController.center.z);
    }

    void StopSliding()
    {
        playerAnimator.SetBool("Sliding", false);

        characterController.height *=2;
        characterController.center = new Vector3(characterController.center.x, characterController.center.y *2, characterController.center.z);
    }

    void Crash()
    {
        playerAnimator.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }
}
