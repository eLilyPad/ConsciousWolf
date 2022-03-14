using System.Collections;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    #region CollisionChecks
        [SerializeField] string collisionType;

    #endregion
    [SerializeField] private float timeBetweenDestroy = 0.5f;

	void OnCollisionEnter(Collision collide)
	{
		Debug.Log("DestroyCollider");
		StartCoroutine(DestroyCollider(collide.gameObject));
	}
	void OnCollisionExit(Collision collide)
	{

	}

	IEnumerator DestroyCollider(GameObject collider)
	{
		if (collider.gameObject.tag == collisionType)
		{
			Destroy(collider);
			yield return new WaitForSeconds(timeBetweenDestroy);
		}
	}
}
