using UnityEngine;
using System.Collections;

public class FontDropShadowControl : MonoBehaviour 
{
	//*************************************************************//
	public bool createCopy = true;
	public bool whiteShadow = false;
	public bool greyShadow = false;
	public float turnShadowsAfterSeconds = 0f;
	//*************************************************************//
	private GameObject _myCopy;
	private TextMesh _myCopyTextMesh;
	private TextMesh _myTextMesh;
	private Material _myTextMeshMaterial;
	private Material _myCopyTextMeshMaterial;
	private Transform _myCopyTransform;
	private Transform _myTransform;
	//*************************************************************//	
	
	IEnumerator Start () 
	{
		_myTransform = this.transform;


		if ( turnShadowsAfterSeconds != 0f )
		{
			yield return new WaitForSeconds ( turnShadowsAfterSeconds );
		}
		
		if ( createCopy )
		{
			_myTextMesh = GetComponent < TextMesh > ();
			_myTextMeshMaterial = _myTextMesh.renderer.material;

			_myCopy = ( GameObject ) Instantiate ( this.gameObject, this.transform.position + this.transform.up * -0.025f + this.transform.right * 0.025f + this.transform.forward * 0.1f, this.transform.rotation );
			_myCopy.GetComponent < FontDropShadowControl > ().createCopy = false;
			if ( _myCopy.GetComponent < iTween > ()) Destroy ( _myCopy.GetComponent < iTween > ());
			_myCopy.transform.parent = transform.parent;
			_myCopy.transform.localScale = transform.localScale;
			_myCopy.transform.parent = transform;

			if(greyShadow)
			{
				if ( _myTextMesh.font.name == "AdLibBT Regular_copy" )
				{
					_myCopy.renderer.material = GameGlobalVariables.FontMaterials.GREY_TEXT;
				}
			}
			else if ( ! whiteShadow && !greyShadow)
			{
				if ( _myTextMesh.font.name == "KOMIKAX_copy" ) _myCopy.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TITLE;
				else if ( _myTextMesh.font.name == "KOMIKAX_copy1" ) _myCopy.renderer.material = GameGlobalVariables.FontMaterials.BLACK_BIG_TITLE;
				else if ( _myTextMesh.font.name == "AdLibBT Regular_copy" )
				{
					_myCopy.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}
				else if ( _myTextMesh.font.name == "AdLibBT Regular_copy1" )
				{
					_myCopy.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
				}
				else 
				{
					_myCopy.renderer.material = GameGlobalVariables.FontMaterials.BLACK_BIG_TEXT;
				}
			}
			else
			{
				if ( _myTextMesh.font.name == "KOMIKAX_copy" ) _myCopy.renderer.material = GameGlobalVariables.FontMaterials.WHITE_TITLE;
				else if ( _myTextMesh.font.name == "KOMIKAX_copy1" ) _myCopy.renderer.material = GameGlobalVariables.FontMaterials.WHITE_BIG_TITLE;
				else if ( _myTextMesh.font.name == "AdLibBT Regular_copy" ) _myCopy.renderer.material = GameGlobalVariables.FontMaterials.WHITE_TEXT;
				else _myCopy.renderer.material = GameGlobalVariables.FontMaterials.WHITE_BIG_TEXT;
			}
			
			_myCopyTextMesh = _myCopy.GetComponent < TextMesh > ();
			_myCopyTextMeshMaterial = _myCopyTextMesh.renderer.material;
			_myCopyTransform = _myCopy.transform;

			turnShadowsAfterSeconds = 0f;
		}

	}
	
	void Update ()
	{
		if ( ! createCopy ) return;
		if ( turnShadowsAfterSeconds > 0f ) return;
		if ( _myCopyTextMesh != null )
		{
			_myCopyTextMesh.text = _myTextMesh.text;
			_myCopyTextMeshMaterial.color = new Color ( _myCopyTextMeshMaterial.color.r, _myCopyTextMeshMaterial.color.g, _myCopyTextMeshMaterial.color.b, _myTextMeshMaterial.color.a );
		}

		if ( _myCopy != null )
		{
			_myCopyTransform.position = _myTransform.position + _myTransform.up * -0.018f + _myTransform.right * 0.018f + _myTransform.forward * 0.1f;
		}
	}
}
