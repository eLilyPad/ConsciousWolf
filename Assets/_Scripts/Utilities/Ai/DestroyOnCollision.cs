using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyOnCollision : MonoBehaviour
{
    #region CollisionChecks
        [SerializeField] string collisionType;

    #endregion
    [SerializeField] private float timeBetweenDestroy = 0.5f;
		[SerializeField] private VisualEffect deathEffect;

	void OnCollisionEnter(Collision collider)
	{
		DestroyCollided(collider.gameObject);
	}
	void OnCollisionExit(Collision collider)
	{

	}

	void DestroyCollided(GameObject collider)
	{
		if (collider.gameObject.tag == "Predator")
		{
			Debug.Log("ded");
			deathEffect.Play();
			Destroy(this.gameObject);
			
		}
	}
	void OnDestroy()
	{

	}
}
