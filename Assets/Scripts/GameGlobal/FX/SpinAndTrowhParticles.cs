using UnityEngine;
using System.Collections;

public class SpinAndTrowhParticles : MonoBehaviour 
{
	public int divideByScale = 1;	
	void Start () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.3f, 1f ), "easetype", iTween.EaseType.easeInOutQuad, "position", new Vector3 ( transform.position.x + Random.Range ( -3f, 3f )/divideByScale, transform.position.y + 2, transform.position.z + Random.Range ( -3f, 3f )/divideByScale)));
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.2f, 0.5f ), "easetype", iTween.EaseType.easeOutCubic, "scale", transform.localScale * Random.Range ( 1.5f, 3.5f )/divideByScale, "oncomplete", "onComplete"));
	}
	
	private void onComplete () 
	{
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.2f, 0.5f ), "easetype", iTween.EaseType.easeInCubic, "scale", transform.localScale * 0.3f/divideByScale, "oncomplete", "onCompleteAll"));
	}
	
	private void onCompleteAll ()
	{
		Destroy ( this.gameObject );
	}
	
	void Update ()
	{
		transform.Rotate ( 0f, Random.Range ( 18f, 720f ) * Time.deltaTime, 0f );
	}
}
