using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using PeenScreen;

namespace WaltsGame
{
	//##########################
	// Class Declaration
	//##########################
	public class Game : MonoBehaviour
	{	
		//=======================
		// Variables
		//=======================
		protected static Game _instance;
		public static Type defaultType;
		protected static bool isExiting;
		[SerializeField]
		protected List<Controller> _localControllers;
		[SerializeField]
		protected SceneSettings[] _sceneSettingsBinds;
		protected Dictionary<string,SceneSettings> _sceneSettings; // key is Scene name, value is settings
		protected HashSet<string> _scenesLoading; // treated as queue that is wiped upon load
		protected Dictionary<string,HashSet<Level>> _requiredSceneRequests; // key is Scene name, value is requesting Levels
		protected Dictionary<string,Level> _levels; // key is Scene name, value is Level
		
		//=======================
		// Initialization
		//=======================
		protected virtual void Awake()
		{
			if ( ReferenceEquals( _instance, null ) )
			{
				_instance = this;
				DontDestroyOnLoad( gameObject );
				preAwake();
				postAwake();
			}
			else
			{
				Debug.Log( "CANNOT HAVE MORE THAN ONE INSTANCE OF GAME!" );
				Destroy( gameObject );
			}
		}
		
		protected virtual void preAwake()
		{
			// Dump SceneSettings binds to dictionary
			if ( _sceneSettingsBinds != null )
			{
				int tempListLength = _sceneSettingsBinds.Length;
				if ( tempListLength > 0 )
				{
					if ( _sceneSettings == null )
					{
						_sceneSettings = new Dictionary<string,SceneSettings>();
					}
					
					for ( int i = ( tempListLength - 1 ); i >= 0; --i )
					{
						if ( _sceneSettingsBinds[i] != null )
						{
							_sceneSettings[ _sceneSettingsBinds[i].name ] = _sceneSettingsBinds[i];
						}
					}
				}
				
				_sceneSettingsBinds = null;
			}
		}
		
		protected virtual void postAwake()
		{
		}
		
		protected virtual void Start()
		{
			// Set only if one doesn't already exist
			if ( ReferenceEquals( _instance, this ) )
			{
				preStart();
				postStart();
			}
		}
		
		protected virtual void preStart()
		{
			// Run effects on Controllers and force refresh of ScreenManager
			if ( _localControllers != null )
			{
				for ( int i = ( _localControllers.Count - 1 ); i >= 0; --i )
				{
					effectsAddLocalController( _localControllers[i], i );
				}
			}
		}
		
		protected virtual void postStart()
		{
		}
		
		//=======================
		// Deconstruction
		//=======================
		protected virtual void OnDestroy()
		{			
			// Clear and destroy Scene info
			if ( _sceneSettingsBinds != null )
			{
				Array.Clear( _sceneSettingsBinds, 0, _sceneSettingsBinds.Length );
				_sceneSettingsBinds = null;
			}
			
			if ( _sceneSettings != null )
			{
				_sceneSettings.Clear();
				_sceneSettings = null;
			}
			
			if ( _scenesLoading != null )
			{
				_scenesLoading.Clear();
				_scenesLoading = null;
			}
			
			if ( _requiredSceneRequests != null )
			{
				foreach ( KeyValuePair<string,HashSet<Level>> tempPair in _requiredSceneRequests )
				{
					tempPair.Value.Clear();
				}
				_requiredSceneRequests.Clear();
				_requiredSceneRequests = null;
			}
			
			if ( _levels != null )
			{
				foreach ( KeyValuePair<string,Level> tempPair in _levels )
				{
					Destroy( tempPair.Value );
				}
				_levels.Clear();
				_levels = null;
			}
			
			// Clear Local Controllers
			if ( _localControllers != null )
			{
				for ( int i = ( _localControllers.Count - 1 ); i >= 0; --i )
				{
					removeLocalController( i );
				}
				
				if ( _localControllers != null )
				{
					_localControllers.Clear();
					_localControllers = null;
				}
			}
			
			// Clear instance
			if ( ReferenceEquals( _instance, this ) )
			{
				_instance = null;
			}
		}
		
		public virtual void exit()
		{
			# if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
		
		public virtual void OnApplicationQuit()
		{
			isExiting = true;
			StopAllCoroutines();
		}
		
		//=======================
		// Singleton
		//=======================
		public static T GetInstance<T>() where T : Game
		{
			if ( isExiting )
			{
				return null;
			}
			else if ( ReferenceEquals( _instance, null ) )
			{
				Instantiate<GameObject>( Resources.Load<GameObject>( "Game" ) ); // this only works because engine is single-threaded
				if ( _instance == null )
				{
					Debug.Log( "NO \"Game\" PREFAB FOUND IN A \"Resources\" FOLDER! FALLING BACK TO GENERIC." );
					
					GameObject tempObject = new GameObject();
					tempObject.name = "Game";
					if ( defaultType != null && defaultType == typeof(T) )
					{
						tempObject.AddComponent( defaultType );
					}
					else
					{
						tempObject.AddComponent<T>();
					}
				}
			}
			
			return _instance as T;
		}
		
		public static Game instance
		{
			get
			{
				return isExiting ? null : _instance;
			}
		}
		
		//=======================
		// Local Controllers
		//=======================
		public virtual List<Controller> localControllers
		{
			get
			{
				return _localControllers == null ? null : new List<Controller>( _localControllers );
			}
		}
		
		public virtual bool getLocalController( int tIndex, out Controller tController )
		{
			if ( tIndex >= 0 && _localControllers != null && tIndex < _localControllers.Count )
			{
				tController = _localControllers[ tIndex ];
				return true;
			}
			
			tController = null;
			return false;
		}
		
		public virtual bool addLocalController( Controller tController )
		{
			if ( tController != null )
			{
				if ( _localControllers == null )
				{
					_localControllers = new List<Controller>();
					_localControllers.Add( tController );
					effectsAddLocalController( tController, 0 );
					
					return true;
				}
				else if ( !_localControllers.Contains( tController ) ) // add only if doesn't exist
				{
					_localControllers.Add( tController );
					effectsAddLocalController( tController, ( _localControllers.Count - 1 ) );
				
					return true;
				}
			}
		
			return false;
		}
		
		protected virtual void effectsAddLocalController( Controller tController, int tIndex )
		{
			// Notify Controller
			tController.setLocalIndex( this, tIndex );
		}
		
		public virtual bool removeLocalController( Controller tController )
		{
			if ( _localControllers != null )
			{
				for ( int i = ( _localControllers.Count - 1 ); i >= 0; --i )
				{
					if ( _localControllers[i] == tController )
					{
						_localControllers.RemoveAt( i );
						if ( _localControllers.Count == 0 )
						{
							_localControllers = null;
						}
						
						effectsRemoveLocalController( tController, i );
						
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual bool removeLocalController( int tIndex )
		{
			if ( tIndex >= 0 && _localControllers != null && tIndex < _localControllers.Count )
			{
				Controller tempController = _localControllers[ tIndex ];
				_localControllers.RemoveAt( tIndex );
				if ( _localControllers.Count == 0 )
				{
					_localControllers = null;
				}
				
				effectsRemoveLocalController( tempController, tIndex );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveLocalController( Controller tController, int tIndex )
		{
			// Notify Controllers of shift
			if ( _localControllers != null )
			{
				int tempListLength = _localControllers.Count;
				for ( int i = tIndex; i < tempListLength; ++i )
				{
					_localControllers[i].setLocalIndex( this, i );
				}
				
				if ( tController != null )
				{
					tController.setLocalIndex( this, -1 );
				}
			}
		}
		
		//=======================
		// Scene Settings
		//=======================
		public virtual List<KeyValuePair<string,SceneSettings>> sceneSettings
		{
			get
			{
				if ( _sceneSettings != null )
				{
					List<KeyValuePair<string,SceneSettings>> tempSettings = new List<KeyValuePair<string,SceneSettings>>();
					foreach ( KeyValuePair<string,SceneSettings> tempPair in _sceneSettings )
					{
						tempSettings.Add( tempPair );
					}
					
					return tempSettings;
				}
				
				return null;
			}
		}
		
		public virtual bool getSceneSettings( string tScene, out SceneSettings tSettings )
		{
			if ( _sceneSettings != null && _sceneSettings.TryGetValue( tScene, out tSettings ) )
			{
				return true;
			}
			
			tSettings = null;
			return false;
		}
		
		public virtual List<KeyValuePair<string,HashSet<Level>>> requiredSceneRequests
		{
			get
			{
				if ( _requiredSceneRequests != null )
				{
					List<KeyValuePair<string,HashSet<Level>>> tempRequests = new List<KeyValuePair<string,HashSet<Level>>>();
					foreach ( KeyValuePair<string,HashSet<Level>> tempPair in _requiredSceneRequests )
					{
						tempRequests.Add( tempPair );
					}
					
					return tempRequests;
				}
				
				return null;
			}
		}
		
		public virtual bool getRequiredSceneRequests( string tScene, out HashSet<Level> tRequests )
		{
			if ( _requiredSceneRequests != null && _requiredSceneRequests.TryGetValue( tScene, out tRequests ) )
			{
				return true;
			}
			
			tRequests = null;
			return false;
		}
		
		public virtual bool checkIfSceneRequired( string tScene )
		{
			// Determines if a Scene should be destroyed if it isn't being used by any that are currently loading
			if ( _sceneSettings != null && !String.IsNullOrEmpty( tScene ) )
			{
				SceneSettings tempSettings;
				if ( _sceneSettings.TryGetValue( tScene, out tempSettings ) && ( !tempSettings.isDestroyedWhenNotRequired || ( _requiredSceneRequests != null && _requiredSceneRequests.ContainsKey( tScene ) ) ) )
				{
					return true;
				}
			}
			
			return false;
		}
		
		//=======================
		// Scene Loading
		//=======================
		public virtual List<string> scenesLoading
		{
			get
			{
				return _scenesLoading == null ? null : new List<string>( _scenesLoading );
			}
		}
		
		public virtual bool hasSceneLoading( string tScene )
		{
			return _scenesLoading != null && _scenesLoading.Contains( tScene );
		}
		
		public virtual void loadScene( string tScene )
		{
			// Load if not already loading or loaded
			if ( !isExiting && !String.IsNullOrEmpty( tScene ) && ( _scenesLoading == null || !_scenesLoading.Contains( tScene ) ) && !SceneManager.GetSceneByName( tScene ).isLoaded )
			{
				StartCoroutine( loadSceneAsync( tScene ) );
			}
		}
		
		protected virtual IEnumerator loadSceneAsync( string tScene )
		{
			// Load Scene
			if ( _scenesLoading == null )
			{
				_scenesLoading = new HashSet<string>();
			}
			_scenesLoading.Add( tScene );
			
			AsyncOperation tempAsync = SceneManager.LoadSceneAsync( tScene, LoadSceneMode.Additive );
			while ( tempAsync != null && !tempAsync.isDone )
			{
				yield return null;
			}
			
			// Remove Scene from loading queue
			if ( _scenesLoading != null && _scenesLoading.Remove( tScene ) && _scenesLoading.Count == 0 )
			{
				_scenesLoading = null;
			}
			
			// Remove any outstanding required requests
			if ( _requiredSceneRequests != null && _requiredSceneRequests.Remove( tScene ) && _requiredSceneRequests.Count == 0 )
			{
				_requiredSceneRequests = null;
			}
		}
		
		public virtual void unloadScene( string tScene )
		{
			// Only unload if exists and not only scene
			if ( !isExiting && !String.IsNullOrEmpty( tScene ) && SceneManager.sceneCount > 1 && SceneManager.GetSceneByName( tScene ).isLoaded )
			{
				StartCoroutine( unloadSceneAsync( tScene ) );
			}
		}
		
		protected virtual IEnumerator unloadSceneAsync( string tScene )
		{
			AsyncOperation tempAsync = SceneManager.UnloadSceneAsync( tScene );
			while ( tempAsync != null && !tempAsync.isDone )
			{
				yield return null;
			}
		}
		
		//=======================
		// Level
		//=======================
		public virtual List<KeyValuePair<string,Level>> levels
		{
			get
			{
				if ( _levels != null )
				{
					List<KeyValuePair<string,Level>> tempLevels = new List<KeyValuePair<string,Level>>();
					foreach ( KeyValuePair<string,Level> tempPair in _levels )
					{
						tempLevels.Add( tempPair );
					}
					
					return tempLevels;
				}
				
				return null;
			}
		}
		
		public virtual bool getLevel( string tName, out Level tLevel )
		{
			if ( _levels != null && _levels.TryGetValue( tName, out tLevel ) )
			{
				return true;
			}
		
			tLevel = null;
			return false;
		}
		
		public virtual bool addLevel( Level tLevel )
		{
			if ( tLevel != null )
			{
				// Add
				string tempScene = tLevel.gameObject.scene.name;
				if ( _levels == null  )
				{
					_levels = new Dictionary<string,Level>();
				}
				_levels[ tempScene ] = tLevel;
				
				// Bind existing required Levels or load required Scenes
				SceneSettings tempSettings;
				if ( _sceneSettings != null && _sceneSettings.TryGetValue( tempScene, out tempSettings ) && tempSettings != null && tempSettings.requiredScenes != null && tempSettings.requiredScenes.Length > 0 )
				{
					List<string> tempScenesToLoad = null;
					if ( _levels != null )
					{
						Level tempLevel;
						for ( int i = ( tempSettings.requiredScenes.Length - 1 ); i >= 0; --i )
						{
							if ( !_levels.TryGetValue( tempSettings.requiredScenes[i], out tempLevel ) || !tLevel.addRequired( tempLevel ) )
							{
								if ( tempScenesToLoad == null )
								{
									tempScenesToLoad = new List<string>();
								}
								tempScenesToLoad.Add( tempSettings.requiredScenes[i] );
							}
						}
					}
					
					if ( tempScenesToLoad != null )
					{
						HashSet<Level> tempLevels;
						int tempListLength = tempScenesToLoad.Count  - 1;
						for ( int i = tempListLength; i >= 0; --i )
						{
							if ( _requiredSceneRequests == null )
							{
								_requiredSceneRequests = new Dictionary<string,HashSet<Level>>();
								tempLevels = new HashSet<Level>();
								tempLevels.Add( tLevel );
								_requiredSceneRequests[ tempScenesToLoad[i] ] = tempLevels;
							}
							else if ( _requiredSceneRequests.TryGetValue( tempScenesToLoad[i], out tempLevels ) && tempLevels != null )
							{
								tempLevels.Add( tLevel );
							}
							else
							{
								tempLevels = new HashSet<Level>();
								tempLevels.Add( tLevel );
								_requiredSceneRequests[ tempScenesToLoad[i] ] = tempLevels;
							}
						}
						
						for ( int i = tempListLength; i >= 0; --i )
						{
							loadScene( tempScenesToLoad[i] );
						}
					}
				}
				
				// Bind if shared by another Level
				if ( _requiredSceneRequests != null )
				{
					HashSet<Level> tempRequests;
					if ( _requiredSceneRequests.TryGetValue( tempScene, out tempRequests ) )
					{
						if ( tempRequests != null )
						{
							foreach ( Level tempLevel in tempRequests )
							{
								tLevel.addRequiredBy( tempLevel );
							}
							
							tempRequests.Clear();
							tempRequests = null;
						}
						
						if ( _requiredSceneRequests.Remove( tempScene ) && _requiredSceneRequests.Count == 0 )
						{
							_requiredSceneRequests = null;
						}
					}
				}
				
				// Effects
				effectsAddLevel( tLevel );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsAddLevel( Level tLevel )
		{
		}
		
		public virtual bool removeLevel( Level tLevel )
		{
			if ( tLevel != null && _levels != null && _levels.Remove( tLevel.gameObject.scene.name ) )
			{
				if ( _levels.Count == 0 )
				{
					_levels = null;
				}
				
				effectsRemoveLevel( tLevel );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveLevel( Level tLevel )
		{			
			// Remove Level Scene
			unloadScene( tLevel.gameObject.scene.name );
		}
	}
}