using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float LANE_DISTANCE = 3f;
    private const float TURN_SPEED = 0.05f;

    //Movement
    private CharacterController characterController;
    public float jumpForce = 4.0f;
    public float gravity = 12.0f;
    private float verticalVelocity;
    public float speed = 7f;
    private int desiredLane = 1; //0 - Left, 1-Middle, 2-Right;


    //Animation

    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gather input on which lane we should be
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Move Left
            MoveLane(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Move Right
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
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        playerAnimator.SetBool("Grounded", isGrounded);
        //calculate Y
        if (isGrounded)
        {
            verticalVelocity = -0.1f;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Jump
                playerAnimator.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
        }

        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            //fast falling mechanic
            if (Input.GetKeyDown(KeyCode.Space))
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
}
