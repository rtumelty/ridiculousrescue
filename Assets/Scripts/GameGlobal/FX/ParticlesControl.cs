using UnityEngine;
using System.Collections;

public class ParticlesControl : MonoBehaviour 
{
	//*************************************************************//
	public Texture2D[] myTexturesForParticles;
	public bool fromVines = false;
	//*************************************************************//
	void Start () 
	{
		GameObject tileParticlePrefab = ( GameObject ) Resources.Load ( "Tile/tileParticle" );
		
		int numberOfParticles = UnityEngine.Random.Range ( 3, 10 );
		
		for ( int i = 0; i < numberOfParticles; i++ )
		{
			GameObject currentParticle = ( GameObject ) Instantiate ( tileParticlePrefab, transform.position, tileParticlePrefab.transform.rotation );
			currentParticle.renderer.material.mainTexture = myTexturesForParticles[ UnityEngine.Random.Range ( 0, myTexturesForParticles.Length )];
			currentParticle.AddComponent < SpinAndTrowhParticles > ();
			if(fromVines == true)
			{
				currentParticle.GetComponent<SpinAndTrowhParticles>().divideByScale = 3;
			}
		}
		
		StartCoroutine ( "destroyOnComplete" );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		yield return new WaitForSeconds ( 1.5f );
		Destroy ( gameObject );
	}
}
