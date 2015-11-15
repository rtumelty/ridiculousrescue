using UnityEngine;
using System.Collections;

public class RedirectorComponent : CoresComponent 
{
	//*************************************************************//	
	public const int DIRECTION_UP = 0;
	public const int DIRECTION_DOWN = 1;
	public const int DIRECTION_RIGHT = 2;
	public const int DIRECTION_LEFT = 3;
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleChoosenCharacter;
	private int _redirectorID = 0;
	private bool _build = false;
	private bool _placedOnLevel = false;
	private int[] _rememberedPositionOnMouseDown;
	
	private GameObject _lightinigEmmiterPrefab;
	private GameObject _lightinigEmmiterInstant01;
	private GameObject _lightinigEmmiterInstant02;
	private GameObject _lightinigEmmiterInstant03;
	private GameObject _lightinigEmmiterInstant04;
	private LightningBolt _lightinigEmmiterBolt01;
	private LightningBolt _lightinigEmmiterBolt02;
	private LightningBolt _lightinigEmmiterBolt03;
	private LightningBolt _lightinigEmmiterBolt04;
	private GameObject _lightinigTargetInstant03;
	private GameObject _lightinigTargetInstant04;
	private Material _normalLightingMat;
	private Material _previewLightingMat;
	
	private IComponent _myIComponent;
	private Material _myMaterial;
	private bool _plugIsReallyConected = false;
	private bool _connectedForTheFirstTime = false;
	
	private int[][] _myBeamTiles1;
	private int[][] _myBeamTiles2;
	
	private bool _beam01Connected = false;
	private bool _beam02Connected = false;
	
	private bool _previewConnected = false;
	//*************************************************************//	
	void Start () 
	{
		_normalLightingMat = ( Material ) Resources.Load ( "LightningMat" );
		_previewLightingMat = ( Material ) Resources.Load ( "LightningPreviewMat" );
		
		_myIComponent = gameObject.GetComponent < IComponent > ();
		
		_myIComponent.position[0] = Mathf.RoundToInt ( transform.root.position.x );
		_myIComponent.position[1] = Mathf.RoundToInt ( transform.root.position.z + 0.5f );	
		
		_lightinigEmmiterPrefab = ( GameObject ) Resources.Load ( "LightningEmitter" );
		_lightinigEmmiterInstant01 = ( GameObject ) Instantiate ( _lightinigEmmiterPrefab, transform.root.position - Vector3.up * 0.5f, transform.rotation );
		_lightinigEmmiterInstant02 = ( GameObject ) Instantiate ( _lightinigEmmiterPrefab, transform.root.position - Vector3.up * 0.5f, transform.rotation );
		_lightinigEmmiterInstant03 = ( GameObject ) Instantiate ( _lightinigEmmiterPrefab, transform.root.position - Vector3.up * 0.5f, transform.rotation );
		_lightinigEmmiterInstant04 = ( GameObject ) Instantiate ( _lightinigEmmiterPrefab, transform.root.position - Vector3.up * 0.5f, transform.rotation );
		_lightinigEmmiterInstant01.name = "lightinigEmmiterInstant01";
		_lightinigEmmiterInstant02.name = "lightinigEmmiterInstant02";
		_lightinigEmmiterInstant03.name = "lightinigEmmiterInstant03";
		_lightinigEmmiterInstant04.name = "lightinigEmmiterInstant04";
		_lightinigEmmiterInstant01.transform.parent = transform.root;
		_lightinigEmmiterInstant02.transform.parent = transform.root;
		_lightinigEmmiterInstant03.transform.parent = transform.root;
		_lightinigEmmiterInstant04.transform.parent = transform.root;
		_lightinigEmmiterBolt01 = _lightinigEmmiterInstant01.GetComponent < LightningBolt > ();
		_lightinigEmmiterBolt02 = _lightinigEmmiterInstant02.GetComponent < LightningBolt > ();
		_lightinigEmmiterBolt03 = _lightinigEmmiterInstant03.GetComponent < LightningBolt > ();
		_lightinigEmmiterBolt04 = _lightinigEmmiterInstant04.GetComponent < LightningBolt > ();
		_lightinigTargetInstant03 = new GameObject ( "target03" );
		_lightinigTargetInstant04 = new GameObject ( "target04" );
		_myHandleChoosenCharacter = handleBuild;
		_myMaterial = gameObject.renderer.material;
		
		_rememberedPositionOnMouseDown = new int[2];
	}
	
	void Update () 
	{
		if ( _build )
		{
			testForConnection ();
		}
		else
		{
			testForConnection ( true );
		}
		
		transform.localScale = Vector3.one;
	}
	
	public void forceBeingBuild ()
	{
		_build = true;
		_previewConnected = true;
	}
	
	public void handleNewRedirectorBuilt ()
	{
		connected = false;
		//_previewConnected = false;
		
		/*
		for ( int i = 0; i < LevelControl.LEVEL_WIDTH; i++ )
		{
			for ( int j = 0; j < LevelControl.LEVEL_HEIGHT; j++ )
			{
				if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_BEAM][i][j] == transform.parent.gameObject )
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_BEAM][i][j] = null;
					LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][i][j] = GameElements.EMPTY;
				}
			}
		}
		*/
		
		if ( _myBeamTiles1 != null )
		{
			foreach ( int[] tile in _myBeamTiles1 )
			{
				if ( GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, tile[0], tile[1], transform.parent.gameObject, _myIComponent.myID, false, LevelControl.GRID_LAYER_BEAM ))
				{
					//print ( "EMPTY: " + LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][tile[0]][tile[1]] + " | " + tile[0] + " | " + tile[1] );
				}
			}
		}
		
		if ( _myBeamTiles2 != null )
		{
			foreach ( int[] tile in _myBeamTiles2 )
			{
				if ( GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, tile[0], tile[1], transform.parent.gameObject, _myIComponent.myID, false, LevelControl.GRID_LAYER_BEAM ))
				{
					//print ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][tile[0]][tile[1]] + " | " + tile[0] + " | " + tile[1] );
				}
			}
		}
	}
	
	private void fillBeamTilesFormPointToPoint ( Vector3 position1, Vector3 position2, int beamNumber )
	{
		if ( ! _build ) return;
		int[][] tilesOccupiedByBeam = ToolsJerry.getAllTilesFromPointToPoint ( position1, position2 );
		switch ( beamNumber )
		{
			case 0:
				if ( ! ToolsJerry.comparePathes ( _myBeamTiles1, tilesOccupiedByBeam ))
				{
					if ( _myBeamTiles1 != null )
					{
						foreach ( int[] tile in _myBeamTiles1 )
						{
							if ( GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, tile[0], tile[1], transform.parent.gameObject, _myIComponent.myID, false, LevelControl.GRID_LAYER_BEAM ))
							{
								//print ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][tile[0]][tile[1]] + " | " + tile[0] + " | " + tile[1] );
							}
						}
					}
				}
				_myBeamTiles1 = tilesOccupiedByBeam;
				break;
			case 1:
				if ( ! ToolsJerry.comparePathes ( _myBeamTiles2, tilesOccupiedByBeam ))
				{
					if ( _myBeamTiles2 != null )
					{
						foreach ( int[] tile in _myBeamTiles2 )
						{
							if ( GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, tile[0], tile[1], transform.parent.gameObject, _myIComponent.myID, false, LevelControl.GRID_LAYER_BEAM ))
							{
								//print ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][tile[0]][tile[1]] + " | " + tile[0] + " | " + tile[1] );
							}
						}
					}
				}
				_myBeamTiles2 = tilesOccupiedByBeam;
				break;
		}
		
		if ( tilesOccupiedByBeam != null )
		{
			foreach ( int[] tile in tilesOccupiedByBeam )
			{
				if ( GridReservationManager.getInstance ().fillTileWithMe ( GameElements.BEAM, tile[0], tile[1], transform.parent.gameObject, _myIComponent.myID, false, LevelControl.GRID_LAYER_BEAM ))
				{
					//if ( beamNumber == 0 ) print ( "FILL: " + LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][tile[0]][tile[1]] + " | " + tile[0] + " | " + tile[1] );
				}
			}
		}
	}
	
	public void testForConnection ( bool onlyPreview = false )
	{
		Transform objectOnPath = null;
		Transform anyObjectOnPath = null;
		bool showBeam01Connected = false;
		bool showBeam02Connected = false;
		
		bool beam01Connected = false;
		bool beam02Connected = false;
		
		if ( onlyPreview )
		{
			_lightinigEmmiterInstant01.renderer.material = _previewLightingMat;
			_lightinigEmmiterInstant02.renderer.material = _previewLightingMat;
			_lightinigEmmiterInstant03.renderer.material = _previewLightingMat;
			_lightinigEmmiterInstant04.renderer.material = _previewLightingMat;
		}
		else
		{
			_lightinigEmmiterInstant01.renderer.material = _normalLightingMat;
			_lightinigEmmiterInstant02.renderer.material = _normalLightingMat;
			_lightinigEmmiterInstant03.renderer.material = _normalLightingMat;
			_lightinigEmmiterInstant04.renderer.material = _normalLightingMat;
		
			_lightinigEmmiterBolt03.tunrOnOff ( false );
			_lightinigEmmiterBolt04.tunrOnOff ( false );
		}
		
		_myIComponent.position[0] = Mathf.RoundToInt ( transform.root.position.x );
		_myIComponent.position[1] = Mathf.RoundToInt ( transform.root.position.z + 0.5f );	
		
		switch ( _myIComponent.myID )
		{
			case GameElements.ENVI_REDIRECTOR_01:
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_DOWN );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_DOWN );
				_lightinigEmmiterInstant01.transform.localPosition = new Vector3 ( 0f, 1f, 0.48f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam02Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant01.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt01.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant01" ).transform;
						else _lightinigEmmiterBolt01.target = objectOnPath;						
				
						_lightinigEmmiterBolt01.tunrOnOff ( true );
						
						showBeam01Connected = true;
					}
					
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 0 );
					
					_lightinigEmmiterBolt03.tunrOnOff ( false );
				
					beam01Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( anyObjectOnPath.position.x, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.4f );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, 0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
						else
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( transform.root.position.x, LevelControl.LEVEL_HEIGHT, -1 );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, 0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
					}
				}
			
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_LEFT );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_LEFT );
				_lightinigEmmiterInstant02.transform.localPosition = new Vector3 ( 0.48f, 1f, 0f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam01Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant02.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt02.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant02" ).transform;
						else _lightinigEmmiterBolt02.target = objectOnPath;
				
						_lightinigEmmiterBolt02.tunrOnOff ( true );
						showBeam02Connected = true;
					}
					
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt02.target.position + Vector3.forward * 0.5f, 1 );
					
					_lightinigEmmiterBolt04.tunrOnOff ( false );
				
					beam02Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( anyObjectOnPath.position.x + 0.4f, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( 0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position, 1 );
						}
						else
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( -1, LevelControl.LEVEL_HEIGHT - transform.root.position.z, transform.root.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( 0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position, 1 );
						}
					}
				}
			
				break;
			case GameElements.ENVI_REDIRECTOR_02:
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_DOWN );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_DOWN );
				_lightinigEmmiterInstant01.transform.localPosition = new Vector3 ( 0f, 1f, 0.48f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam02Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant01.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt01.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant01" ).transform;
						else _lightinigEmmiterBolt01.target = objectOnPath;
				
						_lightinigEmmiterBolt01.tunrOnOff ( true );
						
						showBeam01Connected = true;
					}
					
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 0 );
					
					_lightinigEmmiterBolt03.tunrOnOff ( false );
				
					beam01Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( anyObjectOnPath.position.x, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.4f );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, 0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
						else
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( transform.root.position.x, LevelControl.LEVEL_HEIGHT, -1 );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, 0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
					}
				}
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_RIGHT );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_RIGHT );
				_lightinigEmmiterInstant02.transform.localPosition = new Vector3 ( -0.48f, 1f, 0f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam01Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant02.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt02.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant02" ).transform;
						else _lightinigEmmiterBolt02.target = objectOnPath;
					
						_lightinigEmmiterBolt02.tunrOnOff ( true );
						
						showBeam02Connected = true;
					}
					
				
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 1 );
					
					_lightinigEmmiterBolt04.tunrOnOff ( false );
					
					beam02Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( anyObjectOnPath.position.x - 0.4f, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( -0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position, 1 );
						}
						else
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( LevelControl.LEVEL_WIDTH, transform.position.y, transform.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( -0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position + Vector3.back * 0.5f, 1 );
						}
					}
				}
				break;
			case GameElements.ENVI_REDIRECTOR_03:
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_UP );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_UP );
				_lightinigEmmiterInstant01.transform.localPosition = new Vector3 ( 0f, 1f, -0.48f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam02Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant01.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt01.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant01" ).transform;
						else _lightinigEmmiterBolt01.target = objectOnPath;	
				
						_lightinigEmmiterBolt01.tunrOnOff ( true );
						
						showBeam01Connected = true;
					}
					
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 0 );
					
					_lightinigEmmiterBolt03.tunrOnOff ( false );
				
					beam01Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( anyObjectOnPath.position.x, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.1f );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, -0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
						else
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( transform.position.x, 0.48f, LevelControl.LEVEL_HEIGHT );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, -0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
					}
				}
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_RIGHT );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_RIGHT );
				_lightinigEmmiterInstant02.transform.localPosition = new Vector3 ( -0.48f, 1f, 0f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam01Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant02.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt02.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant02" ).transform;
						else _lightinigEmmiterBolt02.target = objectOnPath;
				
						_lightinigEmmiterBolt02.tunrOnOff ( true );
						
						showBeam02Connected = true;
					}
				
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 1 );
					
					_lightinigEmmiterBolt04.tunrOnOff ( false );
				
					beam02Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( anyObjectOnPath.position.x - 0.4f, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( -0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position, 1 );
						}
						else
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( LevelControl.LEVEL_WIDTH, transform.position.y, transform.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( -0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position + Vector3.back * 0.5f, 1 );
						}
					}
				}
				break;
			case GameElements.ENVI_REDIRECTOR_04:
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_UP );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_UP );
				_lightinigEmmiterInstant01.transform.localPosition = new Vector3 ( 0f, 1f, -0.48f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath )) || _beam02Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant01.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt01.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant01" ).transform;
						else _lightinigEmmiterBolt01.target = objectOnPath;
					
						_lightinigEmmiterBolt01.tunrOnOff ( true );
						
						showBeam01Connected = true;
					}
				
					
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 0 );
					
					_lightinigEmmiterBolt03.tunrOnOff ( false );
				
					beam01Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( anyObjectOnPath.position.x, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.1f );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, -0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
						else
						{
							_lightinigTargetInstant03.transform.position = new Vector3 ( transform.position.x, 0.48f, LevelControl.LEVEL_HEIGHT );
							_lightinigEmmiterInstant03.transform.localPosition = new Vector3 ( 0f, 1f, -0.48f - 0.5f );
							_lightinigEmmiterBolt03.target = _lightinigTargetInstant03.transform;
							_lightinigEmmiterBolt03.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt03.target.position, 0 );
						}
					}
				}
				
				objectOnPath = LevelControl.getInstance ().findEnergyObjectOnPath ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_LEFT );
				anyObjectOnPath = LevelControl.getInstance ().findAnyObjectOnPathForRedirectorAndElectrify ( _myIComponent.position[0], _myIComponent.position[1], DIRECTION_LEFT );
				_lightinigEmmiterInstant02.transform.localPosition = new Vector3 ( 0.48f, 1f, 0f - 0.5f );
				if (( objectOnPath != null ) && (( checkIfMayConnect ( objectOnPath ))  || _beam01Connected ))
				{
					bool showConnection = false;
					if ( ! onlyPreview ) connected = true;
					else
					{
						showConnection = true;
					}
					bool redirector = false;
					if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ())
					{
						redirector = true;
						if ( objectOnPath.transform.Find ( "tile" ).GetComponent < RedirectorComponent > ().checkIfIAmTargetInThisRedirector ( _lightinigEmmiterInstant02.transform ))
						{
							showConnection = false;
						}
						else
						{
							showConnection = true;
						}
					}
					else
					{
						showConnection = true;
					}
				
					if ( showConnection )
					{
						if ( redirector ) _lightinigEmmiterBolt02.target = objectOnPath.transform.Find ( "lightinigEmmiterInstant02" ).transform;
						else _lightinigEmmiterBolt02.target = objectOnPath;
					
						_lightinigEmmiterBolt02.tunrOnOff ( true );
					
						showBeam02Connected = true;
					}
				
					fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, objectOnPath.transform.position + Vector3.forward * 0.5f, 1 );
					
					_lightinigEmmiterBolt04.tunrOnOff ( false );
				
					beam02Connected = true;
				}
				else if (( onlyPreview ) || _connectedForTheFirstTime )
				{
					if ( _previewConnected )
					{
						if ( anyObjectOnPath != null )
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( anyObjectOnPath.position.x + 0.4f, LevelControl.LEVEL_HEIGHT - anyObjectOnPath.transform.position.z - 0.48f, anyObjectOnPath.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( 0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position, 1 );
						}
						else
						{
							_lightinigTargetInstant04.transform.position = new Vector3 ( -1, LevelControl.LEVEL_HEIGHT - transform.root.position.z, transform.root.position.z + 0.5f );
							_lightinigEmmiterInstant04.transform.localPosition = new Vector3 ( 0.48f, 1f, 0f - 0.5f );
							_lightinigEmmiterBolt04.target = _lightinigTargetInstant04.transform;
							_lightinigEmmiterBolt04.tunrOnOff ( true );
							fillBeamTilesFormPointToPoint ( transform.parent.position + Vector3.forward * 0.5f, _lightinigEmmiterBolt04.target.position, 1 );
						}
					}
				}
				break;
		}
		
		if ( ! showBeam01Connected ) _lightinigEmmiterBolt01.tunrOnOff ( false );
		if ( ! showBeam02Connected ) _lightinigEmmiterBolt02.tunrOnOff ( false );
		
		if ( ! onlyPreview && ( beam01Connected || beam02Connected ) && ! _connectedForTheFirstTime )
		{
			_connectedForTheFirstTime = true;
			SoundManager.getInstance ().playSound ( SoundManager.REDIRECTOR_POWER_UP, _myIComponent.myID );
		}
		
		if ( ! beam01Connected && ! beam02Connected )
		{
			_connectedForTheFirstTime = false;
			connected = false;
			handleNewRedirectorBuilt ();
		}
		
		if ( onlyPreview && ( beam01Connected || beam02Connected ))
		{
			_previewConnected = true;
		}
		else if ( onlyPreview )
		{
			_lightinigEmmiterBolt01.tunrOnOff ( false );
			_lightinigEmmiterBolt02.tunrOnOff ( false );
			_lightinigEmmiterBolt03.tunrOnOff ( false );
			_lightinigEmmiterBolt04.tunrOnOff ( false );
			_previewConnected = false;
		}
		
		if ( connected ) _previewConnected = true;
		
		_beam01Connected = beam01Connected;
		_beam02Connected = beam02Connected;
	}
	
	private bool checkIfMayConnect ( Transform objectOnPathTransform )
	{
		CoresComponent currentCoresComponent = objectOnPathTransform.Find ( "tile" ).GetComponent < CoresComponent > ();
		IComponent objectsOnPathIComponent = objectOnPathTransform.Find ( "tile" ).GetComponent < IComponent > ();		
		if (( currentCoresComponent != null ) && (( currentCoresComponent.connected ) || (( objectsOnPathIComponent.myID == GameElements.ENVI_POWERPLUG_01 ) && connected )))
		{
			if ( objectsOnPathIComponent.myID == GameElements.ENVI_POWERPLUG_01 ) _plugIsReallyConected = true;
			currentCoresComponent.connected = true;
			return true;
		}
		
		return false;
	}
	
	public bool checkIfIAmTargetInThisRedirector ( Transform transformToCheck )
	{
		if (( _lightinigEmmiterBolt01.target == transformToCheck ) || ( _lightinigEmmiterBolt02.target == transformToCheck )) return true;
		
		return false;
	}
	
	public void handlePlacedOnLevel ()
	{
		_placedOnLevel = true;
		
		foreach ( Transform child in transform.parent )
		{
			if ( child.gameObject.name == "visibleCheck" )
			{
				Destroy ( child.gameObject );
			}
		}
		
		_myIComponent.position[0] = Mathf.RoundToInt ( transform.root.position.x );
		_myIComponent.position[1] = Mathf.RoundToInt ( transform.root.position.z + 0.5f );		
		
		transform.root.position = new Vector3 ( _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ) - 0.48f, _myIComponent.position[1] - 0.5f );
		
		gameObject.GetComponent < DragComponent > ().destoryMarkers ();
		gameObject.GetComponent < DragComponent > ().blockMove ();
		
		Main.getInstance ().handleChooseCharacter ( _myHandleChoosenCharacter, CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE, _myIComponent );
		CharacterData characterSelected = LevelControl.getInstance ().getSelectedCharacter ();
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterSelected.position[0]][characterSelected.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterSelected.position[0]][characterSelected.position[1]] );
	}
	
	public void handleBuild ( CharacterData characterToBuild, bool onlyUnblocking = false )
	{
		if ( onlyUnblocking )
		{
			return;
		}
		
		LevelControl.getInstance ().redirectorsOnLevel.Add ( this );
		
		foreach ( RedirectorComponent redirector in LevelControl.getInstance ().redirectorsOnLevel )
		{
			redirector.handleNewRedirectorBuilt ();
		}
		
		SoundManager.getInstance ().playSound ( SoundManager.REDIRECTOR_BUILD, _myIComponent.myID );
		StartCoroutine ( "startBuildProcedure", characterToBuild );
	}
	
	private IEnumerator startBuildProcedure ( CharacterData characterToBuild )
	{
		int differenceX = _myIComponent.position[0] - characterToBuild.position[0];
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_01_ANIMATION, 1.3f, differenceX ); 
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).SendMessage ( "handleTouched" );
		
		yield return new WaitForSeconds ( 2f );
		characterToBuild.interactAction = false;
		Destroy ( GetComponent < DragComponent > ());
		_build = true;
		testForConnection ();
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( false );
		
		if ( characterToBuild.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] <= 0 )
		{
			//LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.POWERLESS );
			StartCoroutine ( "checkForLooseCondition", characterToBuild );
		}
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.DOWN );
		RedirectorButton.getInstance ().redirectorIsOnlevel = false;
		/*
		yield return new WaitForSeconds ( 2f );
		if ( GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT <= 0 && characterToBuild.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
		{
			if ( ! GlobalVariables.PLUG_CONNECTED )
			{
				ResoultScreen.SPECIFIC_FAILED_INFO_KEY = GameTextManager.KEY_SCREEN_RESCUEMISSION_FAILED_REDIRECTORS;
				ResoultScreen.CHARACTER_NAME = characterToBuild.name;
				ResoultScreen.WIN_RESOULT = false;
				ResoultScreen.getInstance ().character = LevelControl.getInstance ().gameElements[GameElements.CHAR_FARADAYDO_1_DRAINED];
				Main.getInstance ().showFaliedScreen ();
			}
		}
		*/
	}
	
	private IEnumerator checkForLooseCondition ( CharacterData characterToBuild )
	{
		yield return new WaitForSeconds ( 0.5f );
		if ( ! GlobalVariables.PLUG_CONNECTED )
		{
			Main.getInstance ().checkForOtherCharacterWithSkill ( characterToBuild, CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE, true );
		}
	}
	
	void OnMouseDown ()
	{
		_rememberedPositionOnMouseDown[0] = Mathf.RoundToInt ( transform.root.position.x );
		_rememberedPositionOnMouseDown[1] = Mathf.RoundToInt ( transform.root.position.z + 0.5f );
	}
	
	void OnMouseUp ()
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		if ( _build ) return;
		if ( _placedOnLevel ) return;
		
		if (( _rememberedPositionOnMouseDown[0] != Mathf.RoundToInt ( transform.root.position.x )) || ( _rememberedPositionOnMouseDown[1] != Mathf.RoundToInt ( transform.root.position.z + 0.5f )))
		{
			return;
		}
		
		handleTouchedRotate ();
	}
	
	private void handleTouchedRotate ()
	{
		_redirectorID++;
		if ( _redirectorID == 4 )
		{
			_redirectorID = 0;
		}
		
		SoundManager.getInstance ().playSound ( SoundManager.REDIRECTOR_ROTATE, _myIComponent.myID );
		
		switch ( _redirectorID )
		{
			case 0:
				_myMaterial.mainTexture = LevelControl.getInstance ().gameElements[GameElements.ENVI_REDIRECTOR_01];
				_myIComponent.myID = GameElements.ENVI_REDIRECTOR_01;
				break;
			case 1:
				_myMaterial.mainTexture = LevelControl.getInstance ().gameElements[GameElements.ENVI_REDIRECTOR_02];
				_myIComponent.myID = GameElements.ENVI_REDIRECTOR_02;
				break;
			case 2:
				_myMaterial.mainTexture = LevelControl.getInstance ().gameElements[GameElements.ENVI_REDIRECTOR_03];
				_myIComponent.myID = GameElements.ENVI_REDIRECTOR_03;
				break;
			case 3:
				_myMaterial.mainTexture = LevelControl.getInstance ().gameElements[GameElements.ENVI_REDIRECTOR_04];
				_myIComponent.myID = GameElements.ENVI_REDIRECTOR_04;
				break;
		}
		
		int x = Mathf.RoundToInt ( transform.root.position.x );
		int z = Mathf.RoundToInt ( transform.root.position.z + 0.5f );
		
		GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, x, z, transform.root.gameObject, _myIComponent.myID, false );
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true );
	}
	
	public bool checkIfBuilded ()
	{
		return _build;
	}
}
