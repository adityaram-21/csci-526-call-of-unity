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
        if (Input.GetMouseButtonDown(1)) // Right Click
        {
            flashlightOn = !flashlightOn;
            if (flashlight != null)
                flashlight.SetActive(flashlightOn);
        }

        // interaction click... 
        if (Input.GetMouseButtonDown(0))
        {
            // logic
            Debug.Log("Left click: interact (function under dev)");
        }
    }
    
    void FixedUpdate()
    {
        // Movement
        rb.linearVelocity = moveInput * moveSpeed;
    }
    
}
