using UnityEngine;
//sing UnityEngine.InputSystem;


public class RainWizzardScript : MonoBehaviour
{

    [SerializeField] private float speed = 5f;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundedStick = -2f;
    
    private GameObject telephonePole;
    private int state = 0;



    private CharacterController characterController;
    private float yVelocity;

    public bool canMove;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()    
    {

        if (!canMove) return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, 0f, z).normalized * speed;
        

        if (characterController.isGrounded && yVelocity < 0f)
            yVelocity = groundedStick;

        yVelocity += gravity * Time.deltaTime;

        // Combine XZ + Y
        Vector3 velocity = new Vector3(move.x, yVelocity, move.z);
        characterController.Move(velocity * Time.deltaTime);
    }

    public void ResetVerticalVelocity()
    {
        yVelocity = 0f;
    }




}
