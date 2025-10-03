using UnityEngine;
using Proyecto26;
using TMPro;

public class BallisticData : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_InputField nameText;

    public static float angle;
    public static float force;
    public static float projectileMass;
    public static string impactResult;

    private ShotData shotData = new ShotData();

    void Start()
    {
        // No pongas valores fijos acá ❌
        resultText.text = "Esperando disparo...";
    }

    private void UpdateResult()
    {
        resultText.text =
            $"Ángulo: {shotData.angle}° | Fuerza: {shotData.force} | Masa: {shotData.projectileMass} | Resultado: {shotData.impactResult}";
    }

    // 📤 Guardar datos en Firebase
    private void PostToDataBase()
    {
        // ✅ Antes de guardar, pasamos los valores reales a shotData
        shotData.angle = ShooterController.lastAngle;
        shotData.force = ShooterController.lastForce;
        shotData.projectileMass = ShooterController.lastProjectileMass;
        shotData.impactResult = ShooterController.lastImpactResult;

        RestClient.Put("https://balisticosim-default-rtdb.firebaseio.com/" + nameText.text + ".json", shotData);
        Debug.Log("✅ Datos guardados en Firebase");
    }

    public void OnSubmit()
    {
        PostToDataBase();
    }

    // 📥 Recuperar datos desde Firebase
    private void RetrieveFromDataBase()
    {
        RestClient.Get<ShotData>("https://balisticosim-default-rtdb.firebaseio.com/" + nameText.text + ".json").Then(response =>
        {
            shotData = response;
            UpdateResult();
        });
    }

    public void OnGetData()
    {
        RetrieveFromDataBase();
    }
}
