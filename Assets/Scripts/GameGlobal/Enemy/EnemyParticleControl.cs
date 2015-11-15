using UnityEngine;
using System.Collections;

public class EnemyParticleControl : MonoBehaviour {

	public ParticleSystem elecParticles1;
	public ParticleSystem elecParticles2;
	public ParticleSystem elecParticles3;
	public ParticleSystem elecParticles4;
	private float timeInterval = 0.2f;
	private float repeatRate = 0.6f;

	// Use this for initialization
	void Start () 
	{
		elecParticles1.enableEmission = false;
		elecParticles2.enableEmission = false;
		elecParticles3.enableEmission = false;
		elecParticles4.enableEmission = false;
		InvokeRepeating ("callParticleRepeat", 0, repeatRate);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void callParticleRepeat()
	{
		if(gameObject.activeInHierarchy)
		{
			StartCoroutine ("particleRegulator", timeInterval);
		}
	}

	private IEnumerator particleRegulator ()
	{
		/*elecParticles1.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles1.enableEmission = false;

		elecParticles2.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles2.enableEmission = false;

		elecParticles3.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles3.enableEmission = false;*/

		elecParticles4.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles4.enableEmission = false;

		elecParticles3.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles3.enableEmission = false;

		elecParticles2.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles2.enableEmission = false;

		elecParticles1.enableEmission = true;
		yield return new WaitForSeconds (timeInterval);
		elecParticles1.enableEmission = false;

		yield return new WaitForSeconds (timeInterval);
	}
}
