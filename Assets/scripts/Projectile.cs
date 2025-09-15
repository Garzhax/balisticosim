using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 10f; // destruimos después de cierto tiempo

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si choca con target, notificar y destruir
        if (collision.collider.CompareTag("Target"))
        {
            // intentar buscar script TargetBlock para efectos
            TargetBlock tb = collision.collider.GetComponent<TargetBlock>();
            if (tb != null) tb.OnHit(collision.contacts[0].point);

            Destroy(gameObject);
        }
        else
        {
            // Si choca con otro objeto, destruimos la bala (o hacemos rebote si quieres)
            Destroy(gameObject);
        }
    }
}
