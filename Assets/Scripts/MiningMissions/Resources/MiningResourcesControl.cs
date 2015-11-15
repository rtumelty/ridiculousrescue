using UnityEngine;
using System.Collections;

public class MiningResourcesControl : MonoBehaviour 
{
	//*************************************************************//
	public const int MINIMUM_METAL_RESOURCES = 1;
	public const int MAXIMUM_METAL_RESOURCES = 1;
	
	public const int MINIMUM_PLASTIC_RESOURCES = 1;
	public const int MAXIMUM_PLASTIC_RESOURCES = 1;
	
	public const int MINIMUM_VINES_RESOURCES = 1;
	public const int MAXIMUM_VINES_RESOURCES = 3;

	public const int MINIMUM_POPUP_RESOURCES = 1;
	public const int MAXIMUM_POPUP_RESOURCES = 3;
	//*************************************************************//
	public Transform iconMetalTransform;
	public Transform iconPlasticTransform;
	public Transform iconVinesTransform;
	//*************************************************************//
	private TextMesh _metalAmountText;
	private TextMesh _plasticAmountText;
	private TextMesh _vinesAmountText;
	
	private GameObject _tileInteractivePrefab;
	private float countdown = 0;
	//*************************************************************//
	private static MiningResourcesControl _meInstance;
	public static MiningResourcesControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		_meInstance = this;

		_metalAmountText = transform.Find ( "number" ).GetComponent < TextMesh > ();
		//_plasticAmountText = transform.Find ( "textAmountPlastic" ).GetComponent < TextMesh > ();
		//_vinesAmountText = transform.Find ( "textAmountVines" ).GetComponent < TextMesh > ();
		
		//iconMetalTransform = transform.Find ( "iconMetal" );
		//iconPlasticTransform = transform.Find ( "iconPlastic" );
		//iconVinesTransform = transform.Find ( "iconVines" );
		
		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractive" );
	}
	
	void Update () 
	{
		_metalAmountText.text = GameGlobalVariables.Stats.METAL_IN_CONTAINERS.ToString();
		//_plasticAmountText.text = GameGlobalVariables.Stats.NewResources.PLASTIC.ToString ();
		//_vinesAmountText.text = GameGlobalVariables.Stats.NewResources.VINES.ToString ();
	}
	
	public void createResourcesOnLevelAroundPosition ( int resourceElementID, int number, int[] position )
	{
		for ( int i = 0; i < number; i++ )
		{
			GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LevelControl.LEVEL_HEIGHT - position[1] ) + 3f, ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
			GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
			interactiveObjectMesh.transform.localScale = Vector3.one;
			interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x + Random.Range ( -0.2f, 0.2f ), interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f + Random.Range ( -0.2f, 0.2f ));
			interactiveObjectMesh.collider.enabled = false;
			interactiveObjectMesh.tag = GlobalVariables.Tags.RESOURCES;
			interactiveObjectMesh.renderer.material.mainTexture = LevelControl.getInstance ().gameElements[resourceElementID];
			IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
			currentIComponent.myID = resourceElementID;
			
			MNTapResourcesObjectControl currentMNTapResourcesObjectControl = interactiveObjectMesh.AddComponent < MNTapResourcesObjectControl > ();
			currentMNTapResourcesObjectControl.autoCollect = true;
		}
	}
	
	public void createRandomResourcesFromCollectedAroundPosition ( int[] position )
	{
		int number = UnityEngine.Random.Range ( MINIMUM_POPUP_RESOURCES, MAXIMUM_POPUP_RESOURCES + 1 );
		for ( int i = 0; i < number; i++ )
		{
			bool countMetal = false;
			bool countPlstic = false;
			bool countVines = false;
			
			if ( GameGlobalVariables.Stats.NewResources.METAL > 0 ) countMetal = true;
			if ( GameGlobalVariables.Stats.NewResources.PLASTIC > 0 ) countPlstic = true;
			if ( GameGlobalVariables.Stats.NewResources.VINES > 0 ) countVines = true;
			
			if ( ! countMetal && ! countPlstic && ! countVines ) return;
			
			bool randomResourceChossen = false;
			int resourceElementID = 0;
			
			while ( ! randomResourceChossen )
			{
				resourceElementID = UnityEngine.Random.Range ( 1 + 60, 4 + 60 );
				
				switch ( resourceElementID )
				{
					case GameElements.ICON_METAL:
						if ( countMetal )
						{
							randomResourceChossen = true;
							GameGlobalVariables.Stats.NewResources.METAL--;
						}
						break;
					case GameElements.ICON_PLASTIC:
						if ( countPlstic )
						{
							randomResourceChossen = true;
							GameGlobalVariables.Stats.NewResources.PLASTIC--;
						}
						break;
					case GameElements.ICON_VINES:
						if ( countVines )
						{
							randomResourceChossen = true;
							GameGlobalVariables.Stats.NewResources.VINES--;
						}
						break;
				}
			}
			
			GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LevelControl.LEVEL_HEIGHT - position[1] ) + 3f, ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
			GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
			interactiveObjectMesh.transform.localScale = Vector3.one;
			interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x + Random.Range ( -2f, 2f ), interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f + Random.Range ( -2f, 2f ));
			interactiveObjectMesh.collider.enabled = false;
			interactiveObjectMesh.tag = GlobalVariables.Tags.RESOURCES;
			interactiveObjectMesh.renderer.material.mainTexture = LevelControl.getInstance ().gameElements[resourceElementID];
			IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
			currentIComponent.myID = resourceElementID;
			
			MNTapResourcesObjectControl currentMNTapResourcesObjectControl = interactiveObjectMesh.AddComponent < MNTapResourcesObjectControl > ();
			currentMNTapResourcesObjectControl.autoCollect = false;
		}
	}
}
