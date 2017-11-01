using UnityEngine;
using UnityEngine.VR;
using System;
using System.Collections.Generic;

namespace PeenScreen
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Manages <see cref="View"/>s, VR settings, and split-screen functionality, should be put on its own GameObject</summary>
	[AddComponentMenu( "PeenScreen/Screen Manager" )]
	public class ScreenManager : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Default Type of singleton instance if lazily instantiated</summary>
		public static Type defaultType;
		/// <summary>Singleton instance</summary>
		protected static ScreenManager _instance;
		/// <summary>True if application if quitting, used to lock out instantiation</summary>
		protected static bool isExiting;
		/// <summary>Allowed inactivity time before singleton instance destroys itself (will leak if anything stores this as a reference!)</summary>
		public float inactivityTime = 5;
		/// <summary>Countdown before this singleton instance destroys itself</summary>
		protected float _inactivityTimeCounter = -1;
		/// <summary>Wrapper for toggling VR inside of VRSettings, alerts managed <see cref="View"/>s of change</summary>
		[SerializeField]
		protected bool _isVREnabled = true;
		/// <summary>True if split-screen should not attempt to remove empty slot spaces on the screen (i.e., if the screen is split in 4 with only 3 players, the 1st player will have 1/2 of the screen instead of just 1/4)</summary>
		[SerializeField]
		protected bool _isSplitScreenCompressed = true;
		/// <summary>Total split-screen slots allotted if <see cref="_isSplitScreenCompressed"/> is false</summary>
		[SerializeField]
		protected uint _uncompressedSlots = 1;
		/// <summary>Type of split-screen mode to use</summary>
		[HideInInspector]
		[SerializeField]
		protected uint _splitScreenMode = 1;
		/// <summary>Master list of all <see cref="View"/> queues, indexed by desired slot index with possible empty elements</summary>
		protected List<List<View>> _views;
		/// <summary>Compiled list of <see cref="Renderable"/>s, indexed by renderable index</summary>
		protected List<Renderable> _renderables;
		/// <summary>Dictionary that pairs desired slot index as key, and actual renderable index as value</summary>
		protected Dictionary<uint,uint> _renderedSlots;
		
		//=======================
		// Initialization
		//=======================
		/// <summary>Attempts to declare this instance as the singleton if there currently is none, otherwise forces this object to destroy itself</summary>
		protected virtual void Awake()
		{
			if ( ReferenceEquals( _instance, null ) )
			{
				_instance = this;
				DontDestroyOnLoad( gameObject );
			}
			else
			{
				Debug.Log( "CANNOT HAVE MORE THAN ONE INSTANCE OF SCREEN MANAGER!" );
				Destroy( gameObject );
			}
		}
		
		/// <summary>Initializes settings if this instance is the singleton</summary>
		protected virtual void Start()
		{
			if ( ReferenceEquals( instance, this ) )
			{
				if ( !setVREnabled( _isVREnabled ) )
				{
					effectsVREnabled( _isVREnabled );
				}
				
				if ( !setInactivityTimeCounter( inactivityTime ) )
				{
					effectsInactivityTimeCounter( _inactivityTimeCounter );
				}
			}
		}
		
		//=======================
		// Deconstruction
		//=======================
		/// <summary>Sets the <see cref="isExiting"/> variable to true if the application is closing to prevent future instantiation</summary>
		protected virtual void OnApplicationQuit()
		{
			isExiting = true;
		}
		
		/// <summary>Clears <see cref="View"/>s and the singleton from memory</summary>
		protected virtual void OnDestroy()
		{
			// Clear Views
			if ( _views != null )
			{
				for ( int i = ( _views.Count - 1 ); i >= 0; --i )
				{
					if ( _views[i] != null )
					{
						_views[i].Clear();
						_views[i] = null;
					}
					_views.RemoveAt( i );
				}
				_views = null;
			}
			
			if ( _renderables != null )
			{
				Renderable tempRenderable;
				for ( int i = ( _renderables.Count - 1 ); i >= 0; --i )
				{
					tempRenderable = _renderables[i];
					if ( tempRenderable.view != null )
					{
						tempRenderable.view.disableRig( (uint)i );
					}
					_renderables.RemoveAt( i );
				}
			
				_renderables = null;
			}
			
			if ( _renderedSlots != null )
			{
				_renderedSlots.Clear();
				_renderedSlots = null;
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
		/// <summary>Get the current instance of the singleton, lazy-instantiates one if none found and is not exiting</summary>
		/// <returns>Singleton instance</returns>
		public static T GetInstance<T>() where T : ScreenManager
		{
			if ( isExiting )
			{
				return null;
			}
			else if ( ReferenceEquals( _instance, null ) )
			{
				GameObject tempObject = new GameObject();
				tempObject.name = "ScreenManager";
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
		
		public static ScreenManager instance
		{
			get
			{
				return isExiting ? null : _instance;
			}
		}
		
		/// <summary>Create an instance from a prefab object</summary>
		/// <param name="tPrefab">Prefab of a ScreenManager</param>
		/// <returns>Singleton instance</returns>
		public static ScreenManager CreateInstance( ScreenManager tPrefab )
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
		/// <summary>Tries to enable only if <see cref="_views"/> is empty and <see cref="_inactivityTimeCounter"/> is greater than 0</summary>
		public virtual bool enable()
		{
			enabled = _views == null && _inactivityTimeCounter > 0;
			return enabled;
		}

		/// <summary>Counts down the inactivity timer</summary>
		protected virtual void Update()
		{
			float tempCounter = _inactivityTimeCounter - Time.unscaledDeltaTime;
			if ( tempCounter < 0 )
			{
				tempCounter = 0;
			}
			setInactivityTimeCounter( tempCounter );
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
		
		/// <summary>Sets the inactivity countdown timer</summary>
		/// <param name="tTime">Time to set the counter to</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setInactivityTimeCounter( float tTime )
		{
			if ( tTime != _inactivityTimeCounter )
			{
				float tempOld = _inactivityTimeCounter;
				_inactivityTimeCounter = tTime;
				effectsInactivityTimeCounter( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Checks if this instance is still enabled/ticking; if it no longer is and the counter reaches 0, destroy this game object</summary>
		/// <param name="tOld">Previous time</param>
		protected virtual void effectsInactivityTimeCounter( float tOld )
		{
			enable();
			if ( !enabled && _inactivityTimeCounter == 0 )
			{
				Debug.Log( _inactivityTimeCounter );
				Destroy( gameObject );
			}
		}
		
		//=======================
		// VR Enabled
		//=======================
		public virtual bool isVREnabled
		{
			get
			{
				return _isVREnabled;
			}
			set
			{
				setVREnabled( value );
			}
		}
		
		/// <summary>Toggles VR if there are valid VR devices</summary>
		/// <param name="tIsEnabled">True for enabled, false for disabled</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setVREnabled( bool tIsEnabled )
		{
			// VR cannot be enabled if no device loaded
			if ( VRSettings.supportedDevices == null || VRSettings.supportedDevices.Length == 0 )
			{
				tIsEnabled = false;
			}
		
			// Apply
			if ( tIsEnabled != _isVREnabled )
			{
				_isVREnabled = tIsEnabled;
				effectsVREnabled( !_isVREnabled );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Applies toggle to VRSettings and attempts to rebuild all <see cref="View"/>s</summary>
		/// <param name="tOld">Previous toggle state</param>
		protected virtual void effectsVREnabled( bool tOld )
		{
			VRSettings.enabled = _isVREnabled;
			rebuild();
		}
		
		//=======================
		// Split Mode
		//=======================
		public virtual uint splitScreenMode
		{
			get
			{
				return _splitScreenMode;
			}
			set
			{
				setSplitScreenMode( value );
			}
		}
		
		/// <summary>Sets the split-screen mode</summary>
		/// <param name="tMode">Enumerator of the mode</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setSplitScreenMode( SplitScreenMode tMode )
		{
			return setSplitScreenMode( (uint)tMode );
		}
		
		/// <summary>Sets the split-screen mode</summary>
		/// <param name="tMode">Integer representing the mode</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setSplitScreenMode( uint tMode )
		{
			if ( tMode != _splitScreenMode )
			{
				uint tempOld = _splitScreenMode;
				_splitScreenMode = tMode;
				effectsSplitScreenMode( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Applies the mode to VRSettings and attempts to rebuild and refresh all <see cref="View"/>s</summary>
		/// <param name="tOld">Previous mode</param>
		protected virtual void effectsSplitScreenMode( uint tOld )
		{
			if ( !_isVREnabled )
			{
				// Only need to do a refresh if changing between horizontal/vertical (assumed to have matching renderables), otherwise rebuild the whole screen
				if ( ( tOld == (uint)SplitScreenMode.Horizontal && _splitScreenMode == (uint)SplitScreenMode.Vertical ) || ( tOld == (uint)SplitScreenMode.Vertical && _splitScreenMode == (uint)SplitScreenMode.Horizontal ) )
				{
					refresh();
				}
				else
				{
					rebuild();
				}
			}
		}
		
		//=======================
		// Split Screen
		//=======================
		public virtual bool isSplitScreenCompressed
		{
			get
			{
				return _isSplitScreenCompressed;
			}
			set
			{
				setSplitScreenCompressed( value );
			}
		}
		
		/// <summary>Toggles split-screen to be compressed/allow empty slots or not</summary>
		/// <param name="tIsSplitScreenCompressed">True if compressed</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setSplitScreenCompressed( bool tIsSplitScreenCompressed )
		{
			if ( tIsSplitScreenCompressed != _isSplitScreenCompressed  )
			{
				_isSplitScreenCompressed = tIsSplitScreenCompressed;
				effectsSplitScreenCompressed( !_isSplitScreenCompressed );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Rebuilds the screen if split-screen is enabled and VR is disabled</summary>
		/// <param name="tOld">Previous compressed state</param>
		protected virtual void effectsSplitScreenCompressed( bool tOld )
		{
			if ( !_isVREnabled && _splitScreenMode > 0 )
			{
				rebuild();
			}
		}
		
		//=======================
		// Views
		//=======================
		public virtual List<List<View>> views
		{
			get
			{
				if ( _views != null )
				{
					List<List<View>> tempViews = new List<List<View>>();
					int tempListLength = _views.Count;
					for ( int i = 0; i < tempListLength; ++i )
					{
						if ( _views[i] == null )
						{
							tempViews.Add( null );
						}
						else
						{
							tempViews.Add( new List<View>( _views[i] ) );
						}
					}
					
					return tempViews;
				}
				
				return null;
			}
		}
		
		/// <summary>Returns a list of <see cref="View"/>s from a specific slot index</summary>
		/// <param name="tSlotIndex">Slot index</param>
		/// <param name="tList">Copied list that was found at the element of <paramref name="tSlotIndex"/>, may be null</param>
		/// <returns>True if successfully found</returns>
		public virtual bool getViews( uint tSlotIndex, out List<View> tList )
		{
			if ( _views != null && tSlotIndex < _views.Count )
			{
				tList = _views[ (int)tSlotIndex ] == null ? null : new List<View>( _views[ (int)tSlotIndex ] );
				return true;
			}
		
			tList = null;
			return false;
		}
		
		/// <summary>Attempts to add and render <paramref name="tView"/></summary>
		/// <param name="tView">View to add</param>
		/// <param name="tSlotIndex">Specific slot index associated with the <paramref name="tView"/></param>
		/// <returns>True if succcessful and doesn't already contain the View</returns>
		public virtual bool addView( View tView, uint tSlotIndex )
		{
			if ( tView != null )
			{
				if ( _views == null )
				{
					_views = new List<List<View>>();
				}
				
				if ( tSlotIndex >= _views.Count )
				{
					int i = (int)tSlotIndex - _views.Count;
					while ( i >= 0 )
					{
						_views.Add( null );
						--i;
					}
				}
				
				if ( _views[ (int)tSlotIndex ] == null )
				{
					_views[ (int)tSlotIndex ] = new List<View>();
				}
				
				if ( !_views[ (int)tSlotIndex ].Contains( tView ) )
				{
					_views[ (int)tSlotIndex ].Add( tView );
					if ( _views[ (int)tSlotIndex ].Count == 1 )
					{
						addRenderable( tView, tSlotIndex );
					}
					
					effectsAddView( tView, tSlotIndex );
					
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>Toggles the inactivity countdown off if added</summary>
		/// <param name="tView">View that was added</param>
		/// <param name="tSlotIndex">Specific slot index associated with the <paramref name="tView"/></param>
		protected virtual void effectsAddView( View tView, uint tSlotIndex )
		{
			setInactivityTimeCounter( -1 );
		}
		
		/// <summary>Attempts to remove and render <paramref name="tView"/></summary>
		/// <param name="tView">View to remove</param>
		/// <param name="tSlotIndex">Specific slot index associated with the <paramref name="tView"/></param>
		/// <returns>True if found and successfully removed</returns>
		public virtual bool removeView( View tView, uint tSlotIndex )
		{
			if ( _views != null && tSlotIndex < _views.Count && _views[ (int)tSlotIndex ] != null )
			{
				View tempFirst = _views[ (int)tSlotIndex ][0];
				if ( _views[ (int)tSlotIndex ].Remove( tView ) )
				{
					// Remove old and truncate
					if ( _views[ (int)tSlotIndex ].Count == 0 )
					{
						removeRenderable( tSlotIndex );
						_views[ (int)tSlotIndex ] = null;
						if ( tSlotIndex == ( _views.Count - 1 ) )
						{
							for ( int i = (int)tSlotIndex; i >= 0 && _views[i] == null; --i )
							{
								_views.RemoveAt( i );
							}
							
							if ( _views.Count == 0 )
							{
								_views = null;
							}
						}
					}
					// Replace
					else if ( tempFirst == tView )
					{
						removeRenderable( tSlotIndex, false );
						addRenderable( _views[ (int)tSlotIndex ][0], tSlotIndex );
					}
					
					effectsRemoveView( tView, tSlotIndex );
				
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>Toggles the inactivity countdown on if removed and there are no <see cref="View"/>s</summary>
		/// <param name="tView">View that was removed</param>
		/// <param name="tSlotIndex">Specific slot index associated with the <paramref name="tView"/></param>
		protected virtual void effectsRemoveView( View tView, uint tSlotIndex )
		{
			if ( _views == null )
			{
				setInactivityTimeCounter( inactivityTime );
			}
		}
		
		//=======================
		// Renderables
		//=======================
		public virtual List<Renderable> renderables
		{
			get
			{
				return _renderables == null ? null : new List<Renderable>( _renderables );
			}
		}
		
		public virtual List<KeyValuePair<uint,uint>> renderedSlots
		{
			get
			{
				if ( _renderedSlots != null )
				{
					List<KeyValuePair<uint,uint>> tempSlots = new List<KeyValuePair<uint,uint>>();
					foreach ( KeyValuePair<uint,uint> tempPair in _renderedSlots )
					{
						tempSlots.Add( tempPair );
					}
					
					return tempSlots;
				}
				
				return null;
			}
		}
		
		/// <summary>Returns a <see cref="Renderable"/> container</summary>
		/// <param name="tRenderableIndex">Renderable index</param>
		/// <param name="tRenderable">Renderable element found at <paramref name="tRenderableIndex"/></param>
		/// <returns>True if successfully found</returns>
		public virtual bool getRenderable( uint tRenderableIndex, out Renderable tRenderable )
		{
			if ( _renderables != null && tRenderableIndex < _renderables.Count )
			{
				tRenderable = _renderables[ (int)tRenderableIndex ];
				return true;
			}
			
			tRenderable = null;
			return false;
		}
		
		/// <summary>Returns a renderable index associated with a specific slot index</summary>
		/// <param name="tSlotIndex">Slot index</param>
		/// <param name="tRenderableIndex">Renderable index</param>
		/// <returns>True if successfully found</returns>
		public virtual bool getRenderedSlot( uint tSlotIndex, out uint tRenderableIndex )
		{
			if ( _renderedSlots != null && _renderedSlots != null && _renderedSlots.TryGetValue( tSlotIndex, out tRenderableIndex ) )
			{
				return true;
			}
			
			tRenderableIndex = 0;
			return false;
		}

		/// <summary>Tries to create and display a <see cref="Renderable"/> container for the <paramref name="tView"/> and its slot index</summary>
		/// <param name="tView">View to render</param>
		/// <param name="tSlotIndex">Specific slot index associated with the <paramref name="tView"/></param>
		/// <returns>True if succcessfully added</returns>
		protected virtual bool addRenderable( View tView, uint tSlotIndex )
		{
			if ( tView != null )
			{
				// New
				if ( _renderables == null )
				{
					_renderables = new List<Renderable>();
					_renderables.Add( new Renderable( tView, tSlotIndex ) );
					_renderedSlots = new Dictionary<uint,uint>();
					_renderedSlots[ tSlotIndex ] = 0;
					tView.createRig( 0 );
					refresh();
					
					return true;
				}
				else
				{
					// Replace single screen if slot index is lower than current
					if ( _isVREnabled || _splitScreenMode == (uint)SplitScreenMode.None )
					{
						Renderable tempRendered = _renderables[0];
						if ( tSlotIndex < tempRendered.slotIndex )
						{
							tempRendered.view.disableRig( 0 );
							_renderables[0] = new Renderable( tView, tSlotIndex );
							_renderedSlots.Clear();
							_renderedSlots[ tSlotIndex ] = 0;
							tView.createRig( 0 );
							refresh();
							
							return true;
						}
					}
					// Insert if the split-screen is compressed or the slot index is less than the limit
					else if ( _isSplitScreenCompressed || tSlotIndex < _uncompressedSlots )
					{
						// Determine earliest position for insert
						int tempListLength = _renderables.Count;
						uint tempRenderedIndex = (uint)tempListLength;
						for ( int i = ( tempListLength - 1 ); i >= 0; --i )
						{
							if ( tSlotIndex < _renderables[i].slotIndex )
							{
								tempRenderedIndex = (uint)i;
							}
							else
							{
								break;
							}
						}
						
						// Create and ensure slot indices are properly bumped up
						_renderables.Insert( (int)tempRenderedIndex, new Renderable( tView, tSlotIndex ) );
						_renderedSlots[ tSlotIndex ] = tempRenderedIndex;
						++tempListLength;
						
						for ( int i = ( (int)tempRenderedIndex + 1 ); i < tempListLength; ++i )
						{
							_renderedSlots[ _renderables[i].slotIndex ] += 1;
						}
						
						tView.createRig( tempRenderedIndex );
						refresh();
						
						return true;
					}
				}
			}
			
			return false;
		}
		
		/// <summary>Attempts to remove and disable a <see cref="Renderable"/> associated with the <paramref name="tSlotIndex"/></summary>
		/// <param name="tSlotIndex">Specific slot index of a Renderable to remove</param>
		/// <param name="tIsRefreshed">Toggles automatic refreshing</param>
		/// <returns>True if found and successfully removed</returns>
		protected virtual bool removeRenderable( uint tSlotIndex, bool tIsRefreshed = true )
		{
			uint tempRenderedIndex;
			if ( _renderedSlots != null && _renderedSlots.TryGetValue( tSlotIndex, out tempRenderedIndex ) && _renderedSlots.Remove( tSlotIndex ) )
			{
				if ( _renderedSlots.Count == 0 )
				{
					_renderedSlots = null;
				}
				
				// Disable
				Renderable tempRenderable = _renderables[ (int)tempRenderedIndex ];
				if ( tempRenderable.view != null )
				{
					tempRenderable.view.disableRig( tempRenderedIndex );
				}
				
				// Remove and truncate
				_renderables.RemoveAt( (int)tempRenderedIndex );
				int tempListLength = _renderables.Count;
				if ( tempListLength == 0 )
				{
					_renderables = null;
				}
				// Ensure slot indices are properly bumped down
				else
				{
					for ( int i = (int)tempRenderedIndex; i < tempListLength; ++i )
					{
						_renderedSlots[ _renderables[i].slotIndex ] -= 1;
					}
				}
				
				if ( tIsRefreshed )
				{
					refresh();
				}
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Calculates a camera-screen rect at the <paramref name="tRenderableIndex"/> using the split-screen settings</summary>
		/// <param name="tRenderableIndex"><see cref="Renderable"/> index to calculate</param>
		/// <param name="tTotalSlots">Total number of slots on a screen</param>
		/// <returns>Calculated camera-screen rect</returns>
		public virtual Rect calculateRect( uint tRenderableIndex, uint tTotalSlots )
		{
			if ( tTotalSlots > 1 && ( _splitScreenMode == (uint)SplitScreenMode.Horizontal || _splitScreenMode == (uint)SplitScreenMode.Vertical ) )
			{
				// Ideal
				uint tempRows = (uint)Mathf.Floor( Mathf.Sqrt( tTotalSlots ) );
				uint tempColumns = tempRows;
				uint tempRestingRow = 0;
				uint tempRestingColumn = 0;
				
				// Horizontal
				if ( _splitScreenMode == (uint)SplitScreenMode.Horizontal )
				{
					if ( tempRows == 1 )
					{
						tempRows = 2;
					}
					tempColumns = (uint)Mathf.Floor( (float)tTotalSlots / tempRows ); // min columns
					
					// First row
					if ( tRenderableIndex < tempColumns )
					{
						tempRestingRow = 0;
						tempRestingColumn = tRenderableIndex;
					}
					else
					{
						// Squared
						uint tempRemainder = tTotalSlots % ( tempColumns * tempRows );
						if ( tempRemainder == 0 )
						{
							tempRestingRow = (uint)Mathf.Floor( (float)tRenderableIndex / tempColumns );
							tempRestingColumn = tRenderableIndex - ( tempRestingRow * tempColumns );
						}
						// Bottom rows
						else
						{
							tempRows -= tempRemainder; // # of rows that have fewest columns
							tTotalSlots = tempRows * tempColumns; // table of rows with fewest columns
							
							// Fewest column count
							if ( tRenderableIndex < tTotalSlots )
							{
								tempRestingRow = (uint)Mathf.Floor( (float)tRenderableIndex / tempColumns );
								tempRestingColumn = tRenderableIndex - ( tempRestingRow * tempColumns );
							}
							// Largest column count
							else
							{
								tRenderableIndex -= tTotalSlots;
								++tempColumns;
								tempRestingRow = (uint)Mathf.Floor( (float)tRenderableIndex / tempColumns );
								tempRestingColumn = tRenderableIndex - ( tempRestingRow * tempColumns );
								tRenderableIndex += tTotalSlots;
								tempRestingRow += tempRows;
							}
							tempRows += tempRemainder;
						}
					}
				}
				// Vertical
				else
				{
					if ( tempColumns == 1 )
					{
						tempColumns = 2;
					}
					tempRows = (uint)Mathf.Floor( (float)tTotalSlots / tempColumns ); // min rows
					
					// First column
					if ( tRenderableIndex < tempRows )
					{
						tempRestingColumn = 0;
						tempRestingRow = tRenderableIndex;
					}
					else
					{
						// Squared
						uint tempRemainder = tTotalSlots % ( tempColumns * tempRows );
						if ( tempRemainder == 0 )
						{
							tempRestingColumn = (uint)Mathf.Floor( (float)tRenderableIndex / tempRows );
							tempRestingRow = tRenderableIndex - ( tempRestingColumn * tempRows );
						}
						// Bottom columns
						else
						{
							tempColumns -= tempRemainder; // # of columns that have fewest columns
							tTotalSlots = tempColumns * tempRows; // table of columns with fewest rows
							
							// Fewest column count
							if ( tRenderableIndex < tTotalSlots )
							{
								tempRestingColumn = (uint)Mathf.Floor( (float)tRenderableIndex / tempRows );
								tempRestingRow = tRenderableIndex - ( tempRestingColumn * tempRows );
							}
							// Largest column count
							else
							{
								tRenderableIndex -= tTotalSlots;
								++tempRows;
								tempRestingColumn = (uint)Mathf.Floor( (float)tRenderableIndex / tempRows );
								tempRestingRow = tRenderableIndex - ( tempRestingColumn * tempRows );
								tRenderableIndex += tTotalSlots;
								tempRestingColumn += tempColumns;
							}
							tempColumns += tempRemainder;
						}
					}
				}
				
				return new Rect( ( (float)tempRestingColumn / tempColumns ), ( (float)( tempRows - 1 - tempRestingRow ) / tempRows ), ( 1 / (float)tempColumns ), ( 1 / (float)tempRows ) );
			}
			
			return new Rect( 0, 0, 1, 1 );
		}
		
		/// <summary>Clears and rebuilds all <see cref="Renderable"/>s</summary>
		protected virtual void rebuild()
		{
			// Clear old
			Renderable tempRenderable;
			if ( _renderables != null )
			{
				for ( int i = ( _renderables.Count - 1 ); i >= 0; --i )
				{
					tempRenderable = _renderables[i];
					if ( tempRenderable.view != null )
					{
						tempRenderable.view.disableRig( (uint)i );
					}
					_renderables.RemoveAt( i );
				}
				
				_renderables = null;
				if ( _renderedSlots != null )
				{
					_renderedSlots.Clear();
					_renderedSlots = null;
				}
			}
			
			// Add new
			if ( _views != null )
			{
				uint tempRenderableIndex = 0;
				int tempListLength = _views.Count;
				if ( !_isSplitScreenCompressed && tempListLength > _uncompressedSlots ) // if compressed, limit to uncompressed allotment
				{
					tempListLength = (int)_uncompressedSlots;
				}
				
				for ( int i = 0; i < tempListLength; ++i )
				{
					if ( _views[i] != null && _views[i].Count > 0 )
					{
						if ( _renderables == null )
						{
							_renderables = new List<Renderable>();
							_renderedSlots = new Dictionary<uint,uint>();
						}
						
						tempRenderable = new Renderable( _views[i][0], (uint)i );
						_renderables.Add( tempRenderable );
						_renderedSlots[ (uint)i ] = tempRenderableIndex;
						tempRenderable.view.createRig( tempRenderableIndex );
						++tempRenderableIndex;
						
						if ( _isVREnabled || _splitScreenMode == (uint)SplitScreenMode.None ) // single screen
						{
							break;
						}
					}
				}
				
				refresh();
			}
		}
		
		/// <summary>Refreshes adjustments for all <see cref="Renderable"/>s</summary>
		public virtual void refresh()
		{
			if ( _renderables != null )
			{
				Renderable tempRenderable;
				uint tempListLength = (uint)_renderables.Count;
				for ( int i = ( (int)tempListLength - 1 ); i >= 0; --i )
				{
					tempRenderable = _renderables[i];
					if ( tempRenderable.view != null )
					{
						if ( _isSplitScreenCompressed )
						{
							tempRenderable.view.adjustRig( (uint)i, calculateRect( (uint)i, tempListLength ) );
						}
						else
						{
							tempRenderable.view.adjustRig( (uint)i, calculateRect( tempRenderable.slotIndex, _uncompressedSlots ) );
						}
					}
				}
			}
		}
	}
}