using UnityEngine;
using System.Collections;

public class PooledPrefab : MonoBehaviour {
	
	[SerializeField] bool recycleOnCollision;
	[SerializeField] bool recycleOnCollision2D;
	[SerializeField] bool recycleAfterTime;
	[SerializeField] float recycleDelay = .8f;

	void OnEnable() {
		StartCoroutine(Recycle(recycleDelay));
	}

	void OnCollisionEnter(Collision c) {
		if (recycleOnCollision) Recycle(0);
	}
	
	void OnCollisionEnter2D(Collision2D c) {
		if (recycleOnCollision) Recycle(0);
	}

	IEnumerator Recycle (float delay) {
		yield return new WaitForSeconds(delay);
		gameObject.SetActive(false);
	}
}
