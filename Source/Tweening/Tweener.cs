using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace PeenTween
{
	//##########################
	// Class Declaration
	//##########################
	public class Tweener : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		public static Type defaultType;
		protected static Tweener _instance;
		protected static bool isExiting; // used to prevent instances from being created in onDestroy
		[SerializeField]
		protected bool _isDefaultPlaying = true;
		protected bool _isDefaultTicking;
		protected List<ITween> _defaultTweens;
		[SerializeField]
		protected bool _isLatePlaying = true;
		protected bool _isLateTicking;
		protected List<ITween> _lateTweens;
		[SerializeField]
		protected bool _isFixedPlaying = true;
		protected bool _isFixedTicking;
		protected List<ITween> _fixedTweens;
		public float inactivityTime = 5; // will destroy this gameobject after specified seconds of being inactive... infinite if negative
		protected float _inactivityTimeCounter;
		
		//=======================
		// Initialization
		//=======================
		protected virtual void Awake()
		{
			// Set instance only if one doesn't already exist
			if ( ReferenceEquals( _instance, null ) )
			{
				_instance = this;
				DontDestroyOnLoad( gameObject );
			}
			else
			{
				Debug.Log( "CANNOT HAVE MORE THAN ONE INSTANCE OF TWEENER!" );
				Destroy( gameObject );
			}
		}
		
		protected virtual void Start()
		{
			// Effects
			effectsDefaultPlaying( _isDefaultPlaying );
			effectsLatePlaying( _isLatePlaying );
			effectsFixedPlaying( _isFixedPlaying );
		}
		
		//=======================
		// Deconstruction
		//=======================
		protected virtual void OnApplicationQuit()
		{
			isExiting = true;
		}
		
		protected virtual void OnDestroy()
		{				
			// Clear Tweens
			if ( _defaultTweens != null )
			{
				for ( int i = ( _defaultTweens.Count - 1 ); i >= 0; --i )
				{
					removeDefault( i );
				}
				if ( _defaultTweens != null )
				{
					_defaultTweens.Clear();
					_defaultTweens = null;
				}
			}
			if ( _lateTweens != null )
			{
				for ( int i = ( _lateTweens.Count - 1 ); i >= 0; --i )
				{
					removeLate( i );
				}
				if ( _lateTweens != null )
				{
					_lateTweens.Clear();
					_lateTweens = null;
				}
			}
			if ( _fixedTweens != null )
			{
				for ( int i = ( _fixedTweens.Count - 1 ); i >= 0; --i )
				{
					removeFixed( i );
				}
				if ( _fixedTweens != null )
				{
					_fixedTweens.Clear();
					_fixedTweens = null;
				}
			}
			
			// Clear instance
			if ( ReferenceEquals( _instance, this ) )
			{
				_instance = null;
			}
		}
		
		//=======================
		// Singleton
		//=======================
		public static T GetInstance<T>() where T : Tweener // lazy instantiation... use this to force your own subclass!
		{
			if ( isExiting )
			{
				return null;
			}
			else if ( ReferenceEquals( _instance, null ) )
			{
				GameObject tempObject = new GameObject();
				tempObject.name = "Tweener";
				if ( defaultType != null && defaultType == typeof(T) )
				{
					tempObject.AddComponent( defaultType );
				}
				else
				{
					tempObject.AddComponent<T>();
				}
			}
			
			return _instance as T;
		}
		
		public static Tweener instance
		{
			get
			{				
				return isExiting ? null : _instance;
			}
		}
		
		public static Tweener CreateInstance( Tweener tPrefab )
		{
			if ( !isExiting && ReferenceEquals( _instance, null ) && tPrefab != null )
			{
				Instantiate( tPrefab );
			}
			
			return _instance;
		}
		
		//=======================
		// Tick
		//=======================
		public bool isDefaultTicking
		{
			get
			{
				return _isDefaultTicking;
			}
		}
		
		public bool isLateTicking
		{
			get
			{
				return _isLateTicking;
			}
		}
		
		public bool isFixedTicking
		{
			get
			{
				return _isFixedTicking;
			}
		}
		
		public virtual bool enable()
		{
			// Cache bools to squeeze performance
			_isDefaultTicking = _isDefaultPlaying && _defaultTweens != null && _defaultTweens.Count > 0;
			_isLateTicking = _isLatePlaying && _lateTweens != null && _lateTweens.Count > 0;
			_isFixedTicking = _isFixedPlaying && _fixedTweens != null && _fixedTweens.Count > 0;
			
			// Toggle, check if inactivity time out
			enabled = _inactivityTimeCounter > 0 || _isDefaultTicking || _isLateTicking || _isFixedTicking;
			
			return enabled;
		}
		
		protected virtual void Update()
		{
			if ( _isDefaultTicking )
			{
				ITween tempTween;
				for ( int i = ( _defaultTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _defaultTweens[i].timeMode != TimeMode.Stopped )
					{
						tempTween = _defaultTweens[i];
						if ( tempTween.tick( tempTween.timeMode == TimeMode.Scaled ? Time.deltaTime : Time.unscaledDeltaTime ) == TweenTick.Completed )
						{
							if ( _defaultTweens != null && i < _defaultTweens.Count && tempTween == _defaultTweens[i] )
							{
								removeDefault( i );
							}
							
							if ( !_isDefaultTicking )
							{
								return;
							}
						}
					}
				}
			}
			// Assumed if ticking and there are no tweens, it is counting down to its own destruction
			else if ( !_isLateTicking && !_isFixedTicking )
			{
				float tempCounter = _inactivityTimeCounter - Time.unscaledDeltaTime;
				if ( tempCounter < 0 )
				{
					tempCounter = 0;
				}
				setInactivityTimeCounter( tempCounter );
			}
		}
		
		protected virtual void LateUpdate()
		{
			if ( _isLateTicking )
			{
				ITween tempTween;
				for ( int i = ( _lateTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _lateTweens[i].timeMode != TimeMode.Stopped )
					{
						tempTween = _lateTweens[i];
						if ( tempTween.tick( tempTween.timeMode == TimeMode.Scaled ? Time.deltaTime : Time.unscaledDeltaTime ) == TweenTick.Completed )
						{
							if ( _lateTweens != null && i < _lateTweens.Count && tempTween == _lateTweens[i] )
							{
								removeLate( i );
							}
							
							if ( !_isLateTicking )
							{
								return;
							}
						}
					}
				}
			}
		}
		
		protected virtual void FixedUpdate()
		{
			if ( _isFixedTicking )
			{
				ITween tempTween;
				for ( int i = ( _fixedTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _fixedTweens[i].timeMode != TimeMode.Stopped )
					{
						tempTween = _fixedTweens[i];
						if ( tempTween.tick( tempTween.timeMode == TimeMode.Scaled ? Time.deltaTime : Time.unscaledDeltaTime ) == TweenTick.Completed )
						{
							if ( _fixedTweens != null && i < _fixedTweens.Count && tempTween == _fixedTweens[i] )
							{
								removeFixed( i );
							}
							
							if ( !_isFixedTicking )
							{
								return;
							}
						}
					}
				}
			}
		}
		
		//=======================
		// Inactivity Time
		//=======================			
		public virtual float inactivityTimeCounter
		{
			get
			{
				return _inactivityTimeCounter;
			}
			set
			{
				setInactivityTimeCounter( value );
			}
		}
		
		public virtual bool setInactivityTimeCounter( float tTime )
		{
			float tempOld = _inactivityTimeCounter;
			_inactivityTimeCounter = tTime;
			effectsInactivityTimeCounter( tempOld );
			
			return true;
		}
		
		protected virtual void effectsInactivityTimeCounter( float tOld )
		{
			// Destroy if timeout reached
			if ( !enable() && _inactivityTimeCounter == 0 )
			{
				Destroy( gameObject );
			}
		}
		
		//=======================
		// All
		//=======================		
		public virtual bool add( ITween tTween, TickMode tMode = TickMode.Default )
		{
			switch ( tMode )
			{
				case TickMode.Late:
					return addLate( tTween );
				case TickMode.Fixed:
					return addFixed( tTween );
				default:
					return addDefault( tTween );
			}
		}
		
		public virtual bool remove( ITween tTween, TickMode tMode = TickMode.Default )
		{
			switch ( tMode )
			{
				case TickMode.Late:
					return removeLate( tTween );
				case TickMode.Fixed:
					return removeFixed( tTween );
				default:
					return removeDefault( tTween );
			}
		}
		
		public virtual void toggle( bool tIsPlaying )
		{
			setDefaultPlaying( tIsPlaying );
			setLatePlaying( tIsPlaying );
			setFixedPlaying( tIsPlaying );
		}
		
		public virtual void kill()
		{
			killDefault();
			killLate();
			killFixed();
		}
		
		public virtual void kill( Component tOwner )
		{
			killDefault( tOwner );
			killLate( tOwner );
			killFixed( tOwner );
		}
		
		//=======================
		// Default
		//=======================		
		public virtual bool isDefaultPlaying
		{
			get
			{
				return _isDefaultPlaying;
			}
			set
			{
				setDefaultPlaying( value );
			}
		}
		
		public virtual bool setDefaultPlaying( bool tIsPlaying )
		{
			if ( _isDefaultPlaying != tIsPlaying )
			{
				_isDefaultPlaying = tIsPlaying;
				effectsDefaultPlaying( !_isDefaultPlaying );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsDefaultPlaying( bool tOld )
		{
			if ( !setInactivityTimeCounter( _defaultTweens == null ? inactivityTime : -1 ) )
			{
				enable();
			}
		}
		
		public virtual List<ITween> defaultTweens
		{
			get
			{
				return _defaultTweens == null ? null : new List<ITween>( _defaultTweens );
			}
		}
		
		public virtual bool getDefault( int tIndex, out ITween tTween )
		{
			if ( tIndex >= 0 && _defaultTweens != null && tIndex < _defaultTweens.Count )
			{
				tTween = _defaultTweens[ tIndex ];
				return true;
			}
			
			tTween = null;
			return false;
		}
		
		public virtual bool getDefault( Component tOwner, out List<ITween> tTweens )
		{
			if ( tOwner != null && _defaultTweens != null )
			{
				tTweens = null;
				for ( int i = ( _defaultTweens.Count - 1 ); i >= 0; --i )
				{
					if ( tTweens == null )
					{
						tTweens = new List<ITween>();
					}
					tTweens.Add( _defaultTweens[i] );
				}
				
				return tTweens != null;
			}
			
			tTweens = null;
			return false;
		}
		
		public virtual bool addDefault( ITween tTween )
		{
			if ( tTween != null )
			{
				if ( _defaultTweens == null )
				{
					_defaultTweens = new List<ITween>();
					_defaultTweens.Add( tTween );
					effectsAddDefault( tTween, 0 );
					
					return true;
				}
				else if ( !_defaultTweens.Contains( tTween ) ) // add only if doesn't exist
				{
					_defaultTweens.Add( tTween );
					effectsAddDefault( tTween, ( _defaultTweens.Count - 1 ) );
				
					return true;
				}
			}
		
			return false;
		}
		
		protected virtual void effectsAddDefault( ITween tTween, int tIndex )
		{
			setInactivityTimeCounter( -1 );
		}
		
		public virtual bool removeDefault( ITween tTween )
		{
			if ( _defaultTweens != null )
			{
				for ( int i = ( _defaultTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _defaultTweens[i] == tTween )
					{
						_defaultTweens.RemoveAt( i );
						if ( _defaultTweens.Count == 0 )
						{
							_defaultTweens = null;
						}
						
						effectsRemoveDefault( tTween, i );
						
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual bool removeDefault( int tIndex )
		{
			if ( tIndex >= 0 && _defaultTweens != null && tIndex < _defaultTweens.Count )
			{
				ITween tempTween = _defaultTweens[ tIndex ];
				_defaultTweens.RemoveAt( tIndex );
				if ( _defaultTweens.Count == 0 )
				{
					_defaultTweens = null;
				}
				
				effectsRemoveDefault( tempTween, tIndex );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveDefault( ITween tTween, int tIndex )
		{
			if ( _defaultTweens == null )
			{
				setInactivityTimeCounter( inactivityTime );
			}
		}
		
		public virtual void killDefault()
		{
			if ( _defaultTweens != null )
			{
				_defaultTweens.Clear();
				_defaultTweens = null;
			}
		}
		
		public virtual void killDefault( Component tOwner )
		{
			if ( tOwner != null && _defaultTweens != null )
			{
				for ( int i = ( _defaultTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _defaultTweens[i].owner == tOwner )
					{
						removeDefault( i );
					}
				}
			}
		}
		
		//=======================
		// Late
		//=======================		
		public virtual bool isLatePlaying
		{
			get
			{
				return _isLatePlaying;
			}
			set
			{
				setLatePlaying( value );
			}
		}
		
		public virtual bool setLatePlaying( bool tIsPlaying )
		{
			if ( _isLatePlaying != tIsPlaying )
			{
				_isLatePlaying = tIsPlaying;
				effectsLatePlaying( !_isLatePlaying );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsLatePlaying( bool tOld )
		{
			if ( !setInactivityTimeCounter( _lateTweens == null ? inactivityTime : -1 ) )
			{
				enable();
			}
		}
		
		public virtual List<ITween> lateTweens
		{
			get
			{
				return _lateTweens == null ? null : new List<ITween>( _lateTweens );
			}
		}
		
		public virtual bool getLate( int tIndex, out ITween tTween )
		{
			if ( tIndex >= 0 && _lateTweens != null && tIndex < _lateTweens.Count )
			{
				tTween = _lateTweens[ tIndex ];
				return true;
			}
			
			tTween = null;
			return false;
		}
		
		public virtual bool getLate( Component tOwner, out List<ITween> tTweens )
		{
			if ( tOwner != null && _lateTweens != null )
			{
				tTweens = null;
				for ( int i = ( _lateTweens.Count - 1 ); i >= 0; --i )
				{
					if ( tTweens == null )
					{
						tTweens = new List<ITween>();
					}
					tTweens.Add( _lateTweens[i] );
				}
				
				return tTweens != null;
			}
			
			tTweens = null;
			return false;
		}
		
		public virtual bool addLate( ITween tTween )
		{
			if ( tTween != null )
			{
				if ( _lateTweens == null )
				{
					_lateTweens = new List<ITween>();
					_lateTweens.Add( tTween );
					effectsAddLate( tTween, 0 );
					
					return true;
				}
				else if ( !_lateTweens.Contains( tTween ) ) // add only if doesn't exist
				{
					_lateTweens.Add( tTween );
					effectsAddLate( tTween, ( _lateTweens.Count - 1 ) );
				
					return true;
				}
			}
		
			return false;
		}
		
		protected virtual void effectsAddLate( ITween tTween, int tIndex )
		{
			setInactivityTimeCounter( -1 );
		}
		
		public virtual bool removeLate( ITween tTween )
		{
			if ( _lateTweens != null )
			{
				for ( int i = ( _lateTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _lateTweens[i] == tTween )
					{
						_lateTweens.RemoveAt( i );
						if ( _lateTweens.Count == 0 )
						{
							_lateTweens = null;
						}
						
						effectsRemoveLate( tTween, i );
						
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual bool removeLate( int tIndex )
		{
			if ( tIndex >= 0 && _lateTweens != null && tIndex < _lateTweens.Count )
			{
				ITween tempTween = _lateTweens[ tIndex ];
				_lateTweens.RemoveAt( tIndex );
				if ( _lateTweens.Count == 0 )
				{
					_lateTweens = null;
				}
				
				effectsRemoveLate( tempTween, tIndex );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveLate( ITween tTween, int tIndex )
		{
			if ( _lateTweens == null )
			{
				setInactivityTimeCounter( inactivityTime );
			}
		}
		
		public virtual void killLate()
		{
			if ( _lateTweens != null )
			{
				_lateTweens.Clear();
				_lateTweens = null;
			}
		}
		
		public virtual void killLate( Component tOwner )
		{
			if ( tOwner != null && _lateTweens != null )
			{
				for ( int i = ( _lateTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _lateTweens[i].owner == tOwner )
					{
						removeLate( i );
					}
				}
			}
		}
		
		//=======================
		// Fixed
		//=======================		
		public virtual bool isFixedPlaying
		{
			get
			{
				return _isFixedPlaying;
			}
			set
			{
				setFixedPlaying( value );
			}
		}
		
		public virtual bool setFixedPlaying( bool tIsPlaying )
		{
			if ( _isFixedPlaying != tIsPlaying )
			{
				_isFixedPlaying = tIsPlaying;
				effectsFixedPlaying( !_isFixedPlaying );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsFixedPlaying( bool tOld )
		{
			if ( !setInactivityTimeCounter( _fixedTweens == null ? inactivityTime : -1 ) )
			{
				enable();
			}
		}
		
		public virtual List<ITween> fixedTweens
		{
			get
			{
				return _fixedTweens == null ? null : new List<ITween>( _fixedTweens );
			}
		}
		
		public virtual bool getFixed( int tIndex, out ITween tTween )
		{
			if ( tIndex >= 0 && _fixedTweens != null && tIndex < _fixedTweens.Count )
			{
				tTween = _fixedTweens[ tIndex ];
				return true;
			}
			
			tTween = null;
			return false;
		}
		
		public virtual bool getFixed( Component tOwner, out List<ITween> tTweens )
		{
			if ( tOwner != null && _fixedTweens != null )
			{
				tTweens = null;
				for ( int i = ( _fixedTweens.Count - 1 ); i >= 0; --i )
				{
					if ( tTweens == null )
					{
						tTweens = new List<ITween>();
					}
					tTweens.Add( _fixedTweens[i] );
				}
				
				return tTweens != null;
			}
			
			tTweens = null;
			return false;
		}
		
		public virtual bool addFixed( ITween tTween )
		{
			if ( tTween != null )
			{
				if ( _fixedTweens == null )
				{
					_fixedTweens = new List<ITween>();
					_fixedTweens.Add( tTween );
					effectsAddFixed( tTween, 0 );
					
					return true;
				}
				else if ( !_fixedTweens.Contains( tTween ) ) // add only if doesn't exist
				{
					_fixedTweens.Add( tTween );
					effectsAddFixed( tTween, ( _fixedTweens.Count - 1 ) );
				
					return true;
				}
			}
		
			return false;
		}
		
		protected virtual void effectsAddFixed( ITween tTween, int tIndex )
		{
			setInactivityTimeCounter( -1 );
		}
		
		public virtual bool removeFixed( ITween tTween )
		{
			if ( _fixedTweens != null )
			{
				for ( int i = ( _fixedTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _fixedTweens[i] == tTween )
					{
						_fixedTweens.RemoveAt( i );
						if ( _fixedTweens.Count == 0 )
						{
							_fixedTweens = null;
						}
						
						effectsRemoveFixed( tTween, i );
						
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual bool removeFixed( int tIndex )
		{
			if ( tIndex >= 0 && _fixedTweens != null && tIndex < _fixedTweens.Count )
			{
				ITween tempTween = _fixedTweens[ tIndex ];
				_fixedTweens.RemoveAt( tIndex );
				if ( _fixedTweens.Count == 0 )
				{
					_fixedTweens = null;
				}
				
				effectsRemoveFixed( tempTween, tIndex );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveFixed( ITween tTween, int tIndex )
		{
			if ( _fixedTweens == null )
			{
				setInactivityTimeCounter( inactivityTime );
			}
		}
		
		public virtual void killFixed()
		{
			if ( _fixedTweens != null )
			{
				_fixedTweens.Clear();
				_fixedTweens = null;
			}
		}
		
		public virtual void killFixed( Component tOwner )
		{
			if ( tOwner != null && _fixedTweens != null )
			{
				for ( int i = ( _fixedTweens.Count - 1 ); i >= 0; --i )
				{
					if ( _fixedTweens[i].owner == tOwner )
					{
						removeFixed( i );
					}
				}
			}
		}
		
		//=======================
		// Counter
		//=======================
		public virtual Tween counter( float tDuration, TickMode tMode = TickMode.Default )
		{
			Tween tempTween = new Tween( tDuration );
			add( tempTween, tMode );
			
			return tempTween;
		}
		
		//=======================
		// Camera
		//=======================
		public virtual TweenFloat fov( float tDuration, Camera tFrom, float tTo, EasingType tEasing = EasingType.Linear, TickMode tMode = TickMode.Default )
		{
			if ( tFrom != null )
			{
				TweenFloat tempTween = new TweenFloat( tDuration, tFrom.fieldOfView, tTo );
				tempTween.easing = tEasing;
				tempTween.onCompleted += ( Tween<float> tTween ) =>
				{
					if ( tFrom != null )
					{
						tFrom.fieldOfView = tTo;
					}
				};
				tempTween.onTicked += ( Tween<float> tTween ) =>
				{
					if ( tFrom != null )
					{
						tFrom.fieldOfView = tTween.result;
					}
				};
				add( tempTween, tMode );
				
				return tempTween;
			}
			
			return null;
		}
		
		//=======================
		// Material
		//=======================
		public virtual TweenFloat alpha( float tDuration, Material tFrom, float tTo, EasingType tEasing = EasingType.Linear, TickMode tMode = TickMode.Default )
		{
			if ( tFrom != null )
			{
				TweenFloat tempTween = new TweenFloat( tDuration, tFrom.color.a, tTo );
				tempTween.easing = tEasing;
				tempTween.onCompleted += ( Tween<float> tTween ) =>
				{
					if ( tFrom != null )
					{
						Color tempColor = tFrom.color;
						tempColor.a = tTo;
						tFrom.color = tempColor;
					}
				};
				tempTween.onTicked += ( Tween<float> tTween ) =>
				{
					if ( tFrom != null )
					{
						Color tempColor = tFrom.color;
						tempColor.a = tTween.result;			
						tFrom.color = tempColor;
					}
				};
				add( tempTween, tMode );
				
				return tempTween;
			}
			
			return null;
		}
		
		//=======================
		// Canvas
		//=======================
		public virtual TweenFloat alpha( float tDuration, CanvasGroup tFrom, float tTo, EasingType tEasing = EasingType.Linear, TickMode tMode = TickMode.Default )
		{
			if ( tFrom != null )
			{
				TweenFloat tempTween = new TweenFloat( tDuration, tFrom.alpha, tTo );
				tempTween.easing = tEasing;
				tempTween.onCompleted += ( Tween<float> tTween ) =>
				{
					if ( tFrom != null )
					{
						tFrom.alpha = tTo;
					}
				};
				tempTween.onTicked += ( Tween<float> tTween ) =>
				{
					if ( tFrom != null )
					{
						tFrom.alpha = tTween.result;
					}
				};
				add( tempTween, tMode );
				
				return tempTween;
			}
			
			return null;
		}
	
		//=======================
		// 3D Transforms
		//=======================
		public virtual TweenVector3Transform move( float tDuration, Transform tFrom, Vector3 tTo, EasingType tEasing = EasingType.Linear, TickMode tMode = TickMode.Default )
		{
			if ( tFrom != null )
			{
				TweenVector3Transform tempTween = new TweenVector3Transform( tFrom, tDuration, tFrom.position, tTo );
				tempTween.easing = tEasing;
				add( tempTween, tMode );
				
				return tempTween;
			}
			
			return null;
		}
	}
}