using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")] 
    public float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    [Header("Camera")]
    public Camera mainCamera;

    [Header("Flashlight")]
    public GameObject flashlight;
    private bool flashlightOn = false;
    
    // distance check, use for interaction
    public float maxInteractDistance = 3f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // WASD input
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A D
        moveInput.y = Input.GetAxisRaw("Vertical");   // W S
        moveInput.Normalize();

        // Rotation with Mouse
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorld - transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // direction
        rb.rotation = angle;
        
        
        // Flashlight Switch
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) // Right click or Space
        {
            flashlightOn = !flashlightOn;
            if (flashlight != null)
            {
                flashlight.SetActive(flashlightOn);
            }    
        }

        // interaction click, left click
        if (Input.GetMouseButtonDown(0))
        { 
            // Convert the mouse position from screen coordinates to world coordinates
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Send out a ray, the direction is Vector2.zero, means only the position of this point is detected
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                
            if (hit.collider != null)
            {
                //check distance
                float distance = Vector2.Distance(transform.position, hit.point);
                
                if (distance <= maxInteractDistance)
                {
                    // try to get InteractiveObject
                    CommonInteractive interactive = hit.collider.GetComponent<CommonInteractive>();
                    if (interactive != null)
                    {
                        interactive.Interact();
                    }
                }
            }
        }
    }
    
    void FixedUpdate()
    {
        // Movement
        rb.linearVelocity = moveInput * moveSpeed;
    }
    
}
