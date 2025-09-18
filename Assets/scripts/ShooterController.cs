using UnityEngine;
using UnityEngine.UI;

public class ShooterController : MonoBehaviour
{
    [Header("Prefabs y Referencias")]
    public GameObject projectilePrefab;
    public Transform shootPoint;

    [Header("Parámetros de Disparo")]
    public float force = 20f;
    public float angle = 45f;
    public float yaw = 0f;

    [Header("UI")]
    public Slider forceSlider;
    public Slider angleSlider;
    public Slider yawSlider;
    public Toggle showTrajectoryToggle;

    [Header("Trajectory Preview")]
    public LineRenderer trajectoryLine;
    public int linePoints = 30;
    public float timeStep = 0.1f;

    void Start()
    {
        if (forceSlider != null) forceSlider.value = force;
        if (angleSlider != null) angleSlider.value = angle;
        if (yawSlider != null) yawSlider.value = yaw;
    }

    void Update()
    {
        if (forceSlider != null) force = forceSlider.value;
        if (angleSlider != null) angle = angleSlider.value;
        if (yawSlider != null) yaw = yawSlider.value;

        if (showTrajectoryToggle != null && showTrajectoryToggle.isOn)
            DrawTrajectoryPreview();
        else if (trajectoryLine != null)
            trajectoryLine.enabled = false;
    }

    // 🚀 Este lo llamás desde el botón de UI
    public void Shoot()
    {
        Quaternion rotation = Quaternion.Euler(angle, yaw, 0);
        Vector3 direction = rotation * Vector3.forward;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = direction.normalized * force;

        Debug.Log($"✅ Proyectil disparado con fuerza {force}");
    }

    void DrawTrajectoryPreview()
    {
        if (trajectoryLine == null || projectilePrefab == null || shootPoint == null) return;

        Rigidbody rb = projectilePrefab.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3[] points = new Vector3[linePoints];
        Vector3 startPos = shootPoint.position;
        Quaternion rotation = Quaternion.Euler(angle, yaw, 0);
        Vector3 startVelocity = rotation * Vector3.forward * force / rb.mass;

        for (int i = 0; i < linePoints; i++)
        {
            float t = i * timeStep;
            points[i] = startPos + startVelocity * t + 0.5f * Physics.gravity * t * t;
        }

        trajectoryLine.positionCount = linePoints;
        trajectoryLine.SetPositions(points);
        trajectoryLine.enabled = true;
    }
}
