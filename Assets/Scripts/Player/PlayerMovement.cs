using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -13.81f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _controller;
    private IPlayerInput _input;
    private Vector3 _velocity;

    [SerializeField] private bool isGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<IPlayerInput>();
    }

    void Update()
    {
        if (_input == null) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Movimiento relativo hacia donde mira el cuerpo
        Vector3 moveDirection = transform.right * _input.MoveInput.x + transform.forward * _input.MoveInput.y;
        _controller.Move(moveDirection * speed * Time.deltaTime);

        // Gravedad y salto (simplificado)
        if (isGrounded && _velocity.y < 0) _velocity.y = -2f;
        if (_input.isJumping && isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime); 
        

    }

}
