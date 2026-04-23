using UnityEngine;
using UnityEngine.InputSystem;

public class MovementTemp : MonoBehaviour
{
    private Rigidbody rb;
    private byte LookSpeed = 10;
    private byte MoveSpeed = 25;
    private Transform CamTransform;
    private Vector3 Movement;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputNormalized = new Vector3(Movement.x, 0, Movement.z);
        Debug.Log("Input Normalized: " + inputNormalized);
        Vector3 moveInput = Vector3.zero;

        // If there is movement, rotate to face the movement direction.
        if (inputNormalized.sqrMagnitude > 0.0001f)
        {
            // rotate the movement direction with the direction that the camera is facing
            moveInput = Quaternion.Euler(0, CamTransform.eulerAngles.y, 0) * inputNormalized;
            // rotate towards movement
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, LookSpeed * Time.deltaTime));
        }
        // Move in direction of the camera
            rb.AddForce(moveInput * Time.deltaTime * MoveSpeed, ForceMode.Impulse);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Movement.x = context.ReadValue<Vector2>().x;
        Movement.z = context.ReadValue<Vector2>().y;

    }
}

