using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {
	Animation animation;
	[SerializeField] TextMesh textMesh;
	[SerializeField] TextMesh dropShadow;

	void Awake() {
		animation = GetComponent<Animation> () as Animation ; 
	}

	// Use this for initialization
	void OnEnable () {
		animation.Play ("Points Animation");
		
		Color color = textMesh.color;
		color.a = 1;
		textMesh.color = color;
		
		color = dropShadow.color;
		color.a = 1;
		dropShadow.color = color;
	}

	public void OnFinish () {
		gameObject.SetActive (false);
	}

	public void Init(string text, Color colour ) {
		textMesh.text = text;
		dropShadow.text = text;
		if (colour != null)
	 		textMesh.color = colour;
	}
	
	public void FadeText() {
		StartCoroutine (_FadeText());
	}

	IEnumerator _FadeText() {
		float fadeTime = animation["Points Animation"].length - animation["Points Animation"].time;
		float currentTime = 0;

		while (currentTime < fadeTime) {
			float alpha = Mathf.Lerp(1, 0, currentTime / fadeTime);


			Color color = textMesh.color;
			color.a = alpha;
			textMesh.color = color;

			color = dropShadow.color;
			color.a = alpha;
			dropShadow.color = color;

			yield return new WaitForSeconds(Time.deltaTime);
			currentTime += Time.deltaTime;
		}
	}
}