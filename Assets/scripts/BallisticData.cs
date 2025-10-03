using UnityEngine;
using Proyecto26;
using TMPro; // 👈 importante para TMP

public class BallisticData : MonoBehaviour
{
    public TMP_Text resultText;           // 👈 TMP_Text en lugar de Text
    public TMP_InputField nameText;       // 👈 TMP_InputField en lugar de InputField

    public static float angle;
    public static float force;
    public static float projectileMass;
    public static string impactResult;

    private ShotData shotData = new ShotData();

    void Start()
    {
        // Valores de ejemplo - estos deberían venir desde tu simulador
        angle = 45f;
        force = 500f;
        projectileMass = 1f;
        impactResult = "Acierto";
        resultText.text = "Resultado: " + impactResult;
    }

    private void UpdateResult()
    {
        resultText.text =
            $"Ángulo: {shotData.angle}° | Fuerza: {shotData.force} | Masa: {shotData.projectileMass} | Resultado: {shotData.impactResult}";
    }

    // 📤 Guardar datos en Firebase
    private void PostToDataBase()
    {
        ShotData shotData = new ShotData();
        RestClient.Put("https://balisticosim-default-rtdb.firebaseio.com/" + nameText.text + ".json", shotData);
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
