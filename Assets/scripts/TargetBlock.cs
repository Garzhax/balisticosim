using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    public int hitPoints = 1;
    public GameObject breakEffect; // particulas opcional

    public void OnHit(Vector3 hitPoint)
    {
        hitPoints--;
        if (breakEffect != null) Instantiate(breakEffect, hitPoint, Quaternion.identity);
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
