using UnityEngine;

public class Exploder : MonoBehaviour
{
    public void Explode(Rigidbody cube, Vector3 explodePosition, float explosionForce, float explosionRadius)
    {
        Debug.Log("Explode");
        cube.AddExplosionForce(explosionForce, explodePosition, explosionRadius);
    }
}
