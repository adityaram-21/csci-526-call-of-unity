using UnityEngine;

public class CommonInteractive : MonoBehaviour
{
    public string objectName = "Placeholder"; // for log only, we use Tag for real interact

    public void Interact()
    {
        Debug.Log($"Interact with {objectName}");

        // if (CompareTag("Door"))
        //{
        // Debug.Log("Interact with Door");
        // Door-specific logic
        //}
        // else if (CompareTag("Bookshelf"))
        //{
        //Debug.Log("Interact with Bookshelf");
        // Bookshelf-specific logic
        //}
        //else
        //{
        //Debug.Log("Interact with something else");
        //}

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger: {objectName}");
        }
    }
}
