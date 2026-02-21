using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public GameObject bullet;

    public ScriptingBinding PlayerInput;

    public InputAction fireAction;
    public InputAction moveAction;
    private Vector2 moveInput;
    private Vector2 facingDirection = Vector2.right; // Default facing direction


    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerInput = new ScriptingBinding();  
    }

    private void OnEnable()
    {
        fireAction = PlayerInput.Player.Fire;
        fireAction.Enable();


        moveAction = PlayerInput.Player.Move;
        moveAction.Enable();
        fireAction.performed += Fire;
    }

    void OnDisable()
    {

        fireAction.performed -= Fire;
        moveAction.Disable();
        fireAction.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
         
       // Flip sprite based on direction
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
            facingDirection = new Vector2(moveInput.x, 0).normalized; // Update facing direction based on horizontal input
        }
           

        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject bulletInstance = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);    
            if(bulletInstance.TryGetComponent(out Bullet bulletComponent))
            {
                bulletComponent.Initialize(facingDirection); // Pass the current movement direction to the bullet
            }
        }
    }
}