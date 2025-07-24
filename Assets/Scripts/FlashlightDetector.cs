using UnityEngine;
using TMPro; 


public class FlashlightDetector : MonoBehaviour
{
    [Header("Light Angles")]
    [Range(0f, 180f)]
    public float innerAngle = 30f; // inner cone for strong light
    [Range(0f, 180f)]
    public float outerAngle = 60f; // full cone including weak light

    [Header("Detection Distances")]
    public float strongLightDistance = 5f; // max distance for strong light
    public float weakLightDistance = 8f; // max distance for weak light

    [Header("Ray Settings")]
    public int rayCount = 30; // how many rays to cast
    public LayerMask detectionLayer; // layer to trigger alarm
    public LayerMask visibleLayer; // layer just visible in weak light

    [Header("Debug")]
    public bool drawGizmos = true; // show gizmos in Scene view

    [Header("References")]
    public GameObject flashlight; // assign Light2D here
    public TextMeshProUGUI alarmLogText; // assign your AlarmLog TextMeshProUGUI here

    // private state
    private bool isAlarmOn = false;

    void Start()
    {
        // hide alarm text at start
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
        // if flashlight is off, stop checking
        if (flashlight == null || !flashlight.activeInHierarchy)
        {
            if (isAlarmOn)
            {
                isAlarmOn = false;
                StopAlarm();
            }
            return;
        }

        Vector2 origin = transform.position;
        Vector2 forward = transform.up; // adjust if flashlight direction is different

        float halfOuter = outerAngle / 2f;
        float step = outerAngle / (rayCount - 1);

        bool alarmTriggeredThisFrame = false;

        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = -halfOuter + (step * i);
            Vector2 dir = Quaternion.Euler(0, 0, angleOffset) * forward;
            float absAngle = Mathf.Abs(angleOffset);

            // Strong light area
            if (absAngle <= innerAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, dir, strongLightDistance, detectionLayer);
                if (hit.collider != null)
                {
                    alarmTriggeredThisFrame = true;

                    if (!isAlarmOn)
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

        // no alarm this frame but was on last frame, turn off
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
            alarmLogText.text = "Flashlight Detected!";
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
        // optional
        // Debug.Log("Weak light sees: " + visibleObject.name);
    }

    // debug only
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Vector2 origin = transform.position;
        Vector2 forward = transform.up;

        // strong light cone
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        DrawSector(origin, forward, innerAngle, strongLightDistance);

        // weak light cone
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
