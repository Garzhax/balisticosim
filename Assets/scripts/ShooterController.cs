using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShooterController : MonoBehaviour
{
    [Header("Prefabs & Transforms")]
    public GameObject projectilePrefab; // prefab de la bala
    public Transform muzzle;            // punto desde donde sale la bala

    [Header("UI Controls")]
    public Slider angleSlider;          // 0..90 (grados)
    public Slider yawSlider;            // -180..180 (grados yaw/direccion horizontal)
    public Slider forceSlider;          // fuerza inicial  
    public TMP_Dropdown massDropdown;   // seleccionar masa
    public Button shootButton;
    public Toggle showTrajectoryToggle;

    [Header("Trajectory")]
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 50;
    public float timeStep = 0.1f;

    void Start()
    {
        if (shootButton != null)
        {
            shootButton.onClick.AddListener(Shoot);
            Debug.Log("✅ Botón de disparo conectado.");
        }
        else
        {
            Debug.LogWarning("⚠️ No hay botón de disparo asignado en el inspector!");
        }
    }

    void Update()
    {
        // 🔫 Detectar disparo (clic izquierdo o espacio)
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // 🎯 Dibujar trayectoria
        if (showTrajectoryToggle != null && showTrajectoryToggle.isOn)
            DrawTrajectoryPreview();
        else if (trajectoryLine != null)
            trajectoryLine.enabled = false;
    }

    public void Shoot()
    {
        Debug.Log("🔫 Intentando disparar...");

        if (projectilePrefab == null || muzzle == null)
        {
            Debug.LogError("❌ Falta asignar el prefab o el muzzle en el inspector!");
            return;
        }

        // Obtener valores desde UI
        float angleDeg = angleSlider != null ? angleSlider.value : 45f;
        float yawDeg = yawSlider != null ? yawSlider.value : 0f;
        float force = forceSlider != null ? forceSlider.value : 10f;
        float mass = 1f;

        if (massDropdown != null)
        {
            switch (massDropdown.value)
            {
                case 0: mass = 0.5f; break;
                case 1: mass = 1f; break;
                case 2: mass = 2f; break;
                default: mass = 1f; break;
            }
        }

        Debug.Log($"🎛 Ángulo: {angleDeg}°, Yaw: {yawDeg}°, Fuerza: {force}, Masa: {mass}");

        // Calcular dirección
        float angleRad = angleDeg * Mathf.Deg2Rad;
        float yawRad = yawDeg * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(
            Mathf.Cos(angleRad) * Mathf.Sin(yawRad), // x
            Mathf.Sin(angleRad),                      // y
            Mathf.Cos(angleRad) * Mathf.Cos(yawRad)  // z
        );

        Debug.Log("📐 Dirección normalizada: " + dir.normalized);

        // Instanciar proyectil
        GameObject proj = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        Debug.Log("✅ Proyectil instanciado en: " + muzzle.position);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = mass;
            rb.linearVelocity = dir.normalized * (force / rb.mass); // ✅ CORREGIDO
            Debug.Log("🚀 Velocidad aplicada: " + rb.linearVelocity);
        }
        else
        {
            Debug.LogError("❌ El proyectil no tiene Rigidbody!");
        }
    }

    void DrawTrajectoryPreview()
    {
        if (trajectoryLine == null || muzzle == null || projectilePrefab == null) return;

        trajectoryLine.enabled = true;
        List<Vector3> points = new List<Vector3>();

        float angleDeg = angleSlider != null ? angleSlider.value : 45f;
        float yawDeg = yawSlider != null ? yawSlider.value : 0f;
        float force = forceSlider != null ? forceSlider.value : 10f;
        float mass = 1f;

        if (massDropdown != null)
        {
            switch (massDropdown.value) { case 0: mass = 0.5f; break; case 1: mass = 1f; break; case 2: mass = 2f; break; default: mass = 1f; break; }
        }

        float angleRad = angleDeg * Mathf.Deg2Rad;
        float yawRad = yawDeg * Mathf.Deg2Rad;

        Vector3 v0 = new Vector3(Mathf.Cos(angleRad) * Mathf.Sin(yawRad),
                                 Mathf.Sin(angleRad),
                                 Mathf.Cos(angleRad) * Mathf.Cos(yawRad)).normalized * (force / mass);

        Vector3 pos = muzzle.position;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;
            Vector3 g = Physics.gravity;
            Vector3 p = pos + v0 * t + 0.5f * g * t * t;
            points.Add(p);

            if (i > 0)
            {
                RaycastHit hit;
                if (Physics.Linecast(points[i - 1], points[i], out hit))
                {
                    points[i] = hit.point;
                    break;
                }
            }
        }

        trajectoryLine.positionCount = points.Count;
        trajectoryLine.SetPositions(points.ToArray());
    }
}
