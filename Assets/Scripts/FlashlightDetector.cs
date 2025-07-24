using UnityEngine;
using TMPro;

public class FlashlightDetector : MonoBehaviour
{
    [Header("Light Angles")]
    [Range(0f, 180f)]
    public float innerAngle = 30f; // Strong light cone (center area)
    [Range(0f, 180f)]
    public float outerAngle = 60f; // Full light cone (including weak light)

    [Header("Detection Distances")]
    public float strongLightDistance = 5f; // Max distance for strong light (triggers alarm)
    public float weakLightDistance = 8f;   // Max distance for weak light (just visible)

    [Header("Ray Settings")]
    public int rayCount = 30; // Number of rays we cast
    public LayerMask detectionLayer; // Things that trigger alarm
    public LayerMask visibleLayer; // Things visible in weak light

    [Header("Debug")]
    public bool drawGizmos = true; // Show gizmos in Scene view

    [Header("UI Reference")]
    public TextMeshProUGUI alarmLogText; // Drag your AlarmLog TextMeshProUGUI here

    // Private states
    private bool isAlarmOn = false; // Whether alarm is currently active

    void Start()
    {
        // make sure the UI is hidden at start
        if (alarmLogText != null)
        {
            alarmLogText.enabled = false;
        }
    }

    void Update()
    {
        DetectLight();
    }

    private void DetectLight()
    {
        Vector2 origin = transform.position;
        Vector2 forward = transform.up; // change to transform.right if needed

        float halfOuter = outerAngle / 2f;
        float step = outerAngle / (rayCount - 1);

        bool alarmTriggeredThisFrame = false;

        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = -halfOuter + (step * i);
            Vector2 dir = Quaternion.Euler(0, 0, angleOffset) * forward;
            float absAngle = Mathf.Abs(angleOffset);

            // --- Strong light area ---
            if (absAngle <= innerAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, dir, strongLightDistance, detectionLayer);
                if (hit.collider != null)
                {
                    alarmTriggeredThisFrame = true;

                    if (!isAlarmOn) // only trigger once when entering alarm state
                    {
                        isAlarmOn = true;
                        TriggerAlarm(hit.collider.gameObject);
                    }
                }
            }
            // Weak light area
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, dir, weakLightDistance, visibleLayer);
                if (hit.collider != null)
                {
                    OnWeakLightVisible(hit.collider.gameObject);
                }
            }
        }

        // If no alarm triggered this frame but alarm was on before, turn it off 
        if (!alarmTriggeredThisFrame && isAlarmOn)
        {
            isAlarmOn = false;
            StopAlarm();
        }
    }

    private void TriggerAlarm(GameObject detectedObject)
    {
        Debug.Log("Alarm ON: " + detectedObject.name);

        if (alarmLogText != null)
        {
            alarmLogText.text = " Flashlight Detected!";
            alarmLogText.enabled = true;
        }
    }

    private void StopAlarm()
    {
        Debug.Log("Alarm OFF");

        if (alarmLogText != null)
        {
            alarmLogText.enabled = false;
        }
    }

    private void OnWeakLightVisible(GameObject visibleObject)
    {
        // Optional logic
        // Debug.Log("Weak light sees: " + visibleObject.name);
    }

    // Debug only
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Vector2 origin = transform.position;
        Vector2 forward = transform.up;

        // Strong light cone
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        DrawSector(origin, forward, innerAngle, strongLightDistance);

        // Weak light cone
        Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
        DrawSector(origin, forward, outerAngle, weakLightDistance);
    }

    private void DrawSector(Vector2 origin, Vector2 forward, float angle, float distance)
    {
        int segments = 30;
        float halfAngle = angle / 2f;
        Vector3 lastPoint = origin;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float currentAngle = -halfAngle + t * angle;
            Vector3 dir = Quaternion.Euler(0, 0, currentAngle) * forward;
            Vector3 point = origin + (Vector2)(dir.normalized * distance);

            if (i > 0)
            {
                Gizmos.DrawLine(lastPoint, point);
            }
            else
            {
                Gizmos.DrawLine(origin, point);
            }

            lastPoint = point;
        }
    }
}
