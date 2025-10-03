using System;
[Serializable]
public class ShotData
{
    public float angle;
    public float force;
    public float projectileMass;
    public string impactResult;

    public ShotData()
    {
        angle = BallisticData.angle;
        force = BallisticData.force;
        projectileMass = BallisticData.projectileMass;
        impactResult = BallisticData.impactResult;
    }
}
