using UnityEngine;
using System.Collections;

public class TransferButtonControl : MonoBehaviour 
{
	//*************************************************************//
	public Texture2D[] progressBar;
	public Renderer transferRenderer;
	public Texture2D transfer01Texture;
	public Texture2D transfer02Texture;
	public Renderer bar01Renderer;
	public Renderer bar02Renderer;
	//*************************************************************//
	private bool _goRight = false;
	private bool _doingTransferInProgress = false;
	//*************************************************************//
	void Awake ()
	{
		bar01Renderer.material.mainTexture = progressBar[0];
		bar01Renderer.enabled = false;
		bar02Renderer.material.mainTexture = progressBar[4];
		transferRenderer.material.mainTexture = transfer01Texture;
	}

	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		if ( _doingTransferInProgress ) return;

		handleTouched ();
	}
	
	private void handleTouched ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );

		if ( _goRight ) StartCoroutine ( "goTransferRight" );
		else StartCoroutine ( "goTransferLeft" );
	}

	private IEnumerator goTransferRight ()
	{
		_doingTransferInProgress = true;

		bar01Renderer.material.mainTexture = progressBar[3];
		bar02Renderer.material.mainTexture = progressBar[0];
		bar02Renderer.enabled = true;
		yield return new WaitForSeconds ( 0.2f );
		bar01Renderer.material.mainTexture = progressBar[2];
		bar02Renderer.material.mainTexture = progressBar[1];
		yield return new WaitForSeconds ( 0.2f );
		bar01Renderer.material.mainTexture = progressBar[1];
		bar02Renderer.material.mainTexture = progressBar[2];
		yield return new WaitForSeconds ( 0.2f );
		bar01Renderer.material.mainTexture = progressBar[0];
		bar02Renderer.material.mainTexture = progressBar[3];
		yield return new WaitForSeconds ( 0.2f );
		bar01Renderer.enabled = false;
		bar02Renderer.material.mainTexture = progressBar[4];
		yield return new WaitForSeconds ( 0.1f );
		transferRenderer.material.mainTexture = transfer01Texture;

		_goRight = ! _goRight;
		_doingTransferInProgress = false;
	}

	private IEnumerator goTransferLeft ()
	{
		_doingTransferInProgress = true;
		
		bar02Renderer.material.mainTexture = progressBar[3];
		bar01Renderer.material.mainTexture = progressBar[0];
		bar01Renderer.enabled = true;
		yield return new WaitForSeconds ( 0.2f );
		bar02Renderer.material.mainTexture = progressBar[2];
		bar01Renderer.material.mainTexture = progressBar[1];
		yield return new WaitForSeconds ( 0.2f );
		bar02Renderer.material.mainTexture = progressBar[1];
		bar01Renderer.material.mainTexture = progressBar[2];
		yield return new WaitForSeconds ( 0.2f );
		bar02Renderer.material.mainTexture = progressBar[0];
		bar01Renderer.material.mainTexture = progressBar[3];
		yield return new WaitForSeconds ( 0.2f );
		bar02Renderer.enabled = false;
		bar01Renderer.material.mainTexture = progressBar[4];
		yield return new WaitForSeconds ( 0.1f );
		transferRenderer.material.mainTexture = transfer02Texture;
		
		_goRight = ! _goRight;
		_doingTransferInProgress = false;
	}
}
