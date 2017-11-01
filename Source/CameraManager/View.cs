using UnityEngine;
using UnityEngine.VR;
using System;
using System.Collections.Generic;

namespace PeenScreen
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles camera switching and adjustment (VR, split-screen, etc.)</summary>
	[AddComponentMenu( "PeenScreen/View" )]
	public class View : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Array of desired slot indices exposed in inspector</summary>
		[SerializeField]
		protected uint[] _slotIndicesArray;
		/// <summary>Desired indices for rendering this instance, passed to the <see cref="ScreenManager"/></summary>
		protected HashSet<uint> _slotIndices;
		/// <summary>Array of <see cref="Rig"/> prefabs exposed in the inspector</summary>
		[SerializeField]
		protected Rig[] _baseRigsArray;
		/// <summary>Prefab dictionary of <see cref="Rig"/>s with the key as their type name in VRSettings.supportedDevices (e.g., "OpenVR")</summary>
		protected Dictionary<string,Rig> _baseRigs;
		/// <summary>Dictionary of rendered <see cref="Rig"/>s with the key as the renderable index assigned from the <see cref="ScreenManager"/></summary>
		protected Dictionary<uint,Rig> _renderedRigs;
		
		//=======================
		// Initialization
		//=======================
		/// <summary>Converts exposed <see cref="_slotIndicesArray"/> and <see cref="_baseRigsArray"/> into actual containers</summary>
		protected virtual void Awake()
		{
			// Convert raw rigs into dictionary
			if ( _baseRigsArray != null )
			{
				Rig tempRig;
				for ( int i = ( _baseRigsArray.Length - 1 ); i >= 0; --i )
				{
					tempRig = _baseRigsArray[i];
					if ( tempRig != null )
					{
						if ( _baseRigs == null )
						{
							_baseRigs = new Dictionary<string,Rig>();
						}
						_baseRigs[ tempRig.type ] = tempRig;
						
						// Disable the base rigs by default
						if ( tempRig.gameObject.activeSelf )
						{
							tempRig.gameObject.SetActive( false );
						}
					}
				}
				
				Array.Clear( _baseRigsArray, 0, _baseRigsArray.Length );
				_baseRigsArray = null;
			}
			
			// Convert raw indices into hashset
			if ( _slotIndicesArray != null )
			{
				for ( int i = ( _slotIndicesArray.Length - 1 ); i >= 0; --i )
				{
					if ( _slotIndices == null )
					{
						_slotIndices = new HashSet<uint>();
					}
					_slotIndices.Add( _slotIndicesArray[i] );
				}
				
				Array.Clear( _slotIndicesArray, 0, _slotIndicesArray.Length );
				_slotIndicesArray = null;
			}
		}
		
		/// <summary>Initializes assigned slot indices</summary>
		protected virtual void Start()
		{
			// Effects
			if ( _slotIndices != null )
			{
				List<uint> tempSlotIndices = new List<uint>( _slotIndices );
				for ( int i = ( tempSlotIndices.Count - 1 ); i >= 0; --i )
				{
					effectsAddSlotIndex( tempSlotIndices[i] );
				}
			}
		}
		
		//=======================
		// Destruction
		//=======================
		/// <summary>Clears slot indices and <see cref="Rig"/>s</summary>
		protected virtual void OnDestroy()
		{			
			// Clear slots
			if ( _slotIndices != null )
			{
				List<uint> tempSlotIndices = new List<uint>( _slotIndices );
				for ( int i = ( tempSlotIndices.Count - 1 ); i >= 0; --i )
				{
					removeSlotIndex( tempSlotIndices[i] );
				}
				
				if ( _slotIndices != null )
				{
					_slotIndices.Clear();
					_slotIndices = null;
				}
			}
			
			// Clear Rigs
			if ( _baseRigs != null )
			{
				_baseRigs.Clear();
				_baseRigs = null;
			}
			
			if ( _renderedRigs != null )
			{
				List<KeyValuePair<uint,Rig>> tempRigs = new List<KeyValuePair<uint,Rig>>();
				foreach ( KeyValuePair<uint,Rig> tempPair in _renderedRigs )
				{
					tempRigs.Add( tempPair );
				}
				
				for ( int i = ( tempRigs.Count - 1 ); i >= 0; --i )
				{
					disableRig( tempRigs[i].Key );
				}
				
				if ( _renderedRigs != null )
				{
					_renderedRigs.Clear();
					_renderedRigs = null;
				}
			}
		}

		//=======================
		// Slot Indices
		//=======================
		/// <summary>Returns copied list of slot indices</summary>
		public virtual List<uint> slotIndices
		{
			get
			{
				return _slotIndices == null ? null : new List<uint>( _slotIndices );
			}
		}
		
		/// <summary>Checks if this contains <paramref name="tSlotIndex"/></summary>
		/// <param name="tSlotIndex">Slot index to check</param>
		/// <returns>True if contains <paramref name="tSlotIndex"/></returns>
		public virtual bool hasSlotIndex( uint tSlotIndex )
		{
			return _slotIndices != null && _slotIndices.Contains( tSlotIndex );
		}
		
		/// <summary>Attempts to add <paramref name="tSlotIndex"/></summary>
		/// <param name="tSlotIndex">Slot index to add</param>
		/// <returns>True if succcessful and doesn't already contain the index</returns>
		public virtual bool addSlotIndex( uint tSlotIndex )
		{
			if ( _slotIndices == null )
			{
				_slotIndices = new HashSet<uint>();
			}
			
			if ( _slotIndices.Add( tSlotIndex ) )
			{
				effectsAddSlotIndex( tSlotIndex );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Registers to the <see cref="ScreenManager"/></summary>
		/// <param name="tSlotIndex">Slot index that was added</param>
		protected virtual void effectsAddSlotIndex( uint tSlotIndex )
		{
			ScreenManager tempManager = ScreenManager.GetInstance<ScreenManager>(); // will try to lazy instantiate if added
			if ( tempManager != null )
			{
				tempManager.addView( this, tSlotIndex );
			}
		}
		
		/// <summary>Attempts to remove <paramref name="tSlotIndex"/></summary>
		/// <param name="tSlotIndex">Slot index to remove</param>
		/// <returns>True if found and succcessfully removed</returns>
		public virtual bool removeSlotIndex( uint tSlotIndex )
		{
			if ( _slotIndices != null && _slotIndices.Remove( tSlotIndex ) )
			{			
				if ( _slotIndices.Count == 0 )
				{
					_slotIndices = null;
				}
				
				effectsRemoveSlotIndex( tSlotIndex );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Unregisters to the <see cref="ScreenManager"/></summary>
		/// <param name="tSlotIndex">Slot index that was removed</param>
		protected virtual void effectsRemoveSlotIndex( uint tSlotIndex )
		{
			ScreenManager tempManager = ScreenManager.instance;
			if ( tempManager != null )
			{
				tempManager.removeView( this, tSlotIndex );
			}
		}
		
		//=======================
		// Rigs
		//=======================
		/// <summary>Returns copied list of <see cref="Rig"/> prefabs in <see cref="KeyValuePair{TKey,TValue}"/> format with the key representing the type</summary>
		public virtual List<KeyValuePair<string,Rig>> baseRigs
		{
			get
			{
				if ( _baseRigs != null )
				{
					List<KeyValuePair<string,Rig>> tempRigs = new List<KeyValuePair<string,Rig>>();
					foreach ( KeyValuePair<string,Rig> tempPair in _baseRigs )
					{
						tempRigs.Add( tempPair );
					}
					
					return tempRigs;
				}
				
				return null;
			}
		}
		
		/// <summary>Tries to get a base <see cref="Rig"/> from the <paramref name="tType"/></summary>
		/// <param name="tType">Type of <see cref="Rig"/></param>
		/// <param name="tRig">Output <see cref="Rig"/></param>
		/// <returns>True if successful</returns>
		public virtual bool getBaseRig( string tType, out Rig tRig )
		{
			if ( _baseRigs != null && _baseRigs.TryGetValue( tType, out tRig ) )
			{
				return true;
			}
			
			tRig = null;
			return false;
		}
		
		/// <summary>Returns copied list of rendered <see cref="Rig"/>s in <see cref="KeyValuePair{K,V}"/> format with the key representing the rendered index</summary>
		public virtual List<KeyValuePair<uint,Rig>> renderedRigs
		{
			get
			{
				if ( _renderedRigs != null )
				{
					List<KeyValuePair<uint,Rig>> tempRigs = new List<KeyValuePair<uint,Rig>>();
					foreach ( KeyValuePair<uint,Rig> tempPair in _renderedRigs )
					{
						tempRigs.Add( tempPair );
					}
					
					return tempRigs;
				}
				
				return null;
			}
		}
		
		/// <summary>Tries to get a <see cref="Rig"/> from the <paramref name="tType"/></summary>
		/// <param name="tRenderedIndex">Rendered index of the <see cref="Rig"/></param>
		/// <param name="tRig">Output <see cref="Rig"/></param>
		/// <returns>True if successful</returns>
		public virtual bool getRenderedRig( uint tRenderedIndex, out Rig tRig )
		{
			if ( _renderedRigs != null && _renderedRigs.TryGetValue( tRenderedIndex, out tRig ) )
			{
				return true;
			}
			
			tRig = null;
			return false;
		}
		
		/// <summary>Tries to disable the <see cref="Rig"/> associated with this instance's <paramref name="tRenderedIndex"/></summary>
		/// <param name="tRenderedIndex">Rendered index to be disabled</param>
		/// <returns><see cref="Rig"/> instance that was disabled, null if not found</returns>
		public virtual Rig disableRig( uint tRenderedIndex )
		{
			if ( _renderedRigs != null )
			{
				Rig tempRig;
				if ( _renderedRigs.TryGetValue( tRenderedIndex, out tempRig ) )
				{
					// Destroy Rig if cloned, otherwise just disable
					if ( tempRig != null )
					{
						if ( tempRig.isCloned )
						{
							Destroy( tempRig.gameObject );
						}
						else
						{
							tempRig.gameObject.SetActive( false );
						}
					}
					
					// Remove
					_renderedRigs.Remove( tRenderedIndex );
					if ( _renderedRigs.Count == 0 )
					{
						_renderedRigs = null;
					}
					
					return tempRig;
				}
			}
			
			return null;
		}
		
		/// <summary>Tries to create a <see cref="Rig"/> instance bound with the <paramref name="tRenderedIndex"/></summary>
		/// <param name="tRenderedIndex">Renderable index to create</param>
		/// <returns><see cref="Rig"/> instance created, null if already exists or unsuccessful</returns>
		public virtual Rig createRig( uint tRenderedIndex )
		{
			if ( _baseRigs != null && ( _renderedRigs == null || !_renderedRigs.ContainsKey( tRenderedIndex ) ) )
			{
				Rig tempBase;
				if ( _baseRigs.TryGetValue( ( VRSettings.enabled ? VRSettings.loadedDeviceName : "" ), out tempBase ) && tempBase != null )
				{
					// Use and toggle base Rig on if at 0 rendered index, otherwise try to clone a new one
					Rig tempRig = null;
					if ( tRenderedIndex == 0 )
					{
						tempRig = tempBase;
					}
					else if ( !VRSettings.enabled )
					{
						tempRig = UnityEngine.Object.Instantiate<GameObject>( tempBase.gameObject ).GetComponent<Rig>();
						tempRig.name = tempBase.name + " (" + tRenderedIndex + ")";
						tempRig.transform.SetParent( tempBase.transform.parent );
						tempRig.transform.localPosition = Vector3.zero;
						tempRig.isCloned = true;
					}
					
					// Add
					if ( tempRig != null )
					{						
						if ( _renderedRigs == null )
						{
							_renderedRigs = new Dictionary<uint,Rig>();
						}
						_renderedRigs[ tRenderedIndex ] = tempRig;
						return tempRig;
					}
				}
			}
			
			return null;
		}
		
		/// <summary>Tries to enable and adjust a <see cref="Rig"/> instance associated with the <paramref name="tRenderedIndex"/></summary>
		/// <param name="tRenderedIndex">Renderable index to adjust</param>
		/// <param name="tScreenRect">The allowed screen area for the <see cref="Rig"/></param>
		/// <returns><see cref="Rig"/> instance that was adjusted, null if not found</returns>
		public virtual Rig adjustRig( uint tRenderableIndex, Rect tScreenRect )
		{
			Rig tempRig;
			if ( _renderedRigs != null && _renderedRigs.TryGetValue( tRenderableIndex, out tempRig ) && tempRig != null )
			{
				// Toggle the Rig's audio listener only if the first renderable index
				if ( tempRig.listener != null )
				{
					tempRig.listener.enabled = tRenderableIndex == 0;
				}
				
				// Enable and set screen rect
				tempRig.gameObject.SetActive( true );
				tempRig.setRect( tScreenRect );

				return tempRig;
			}
			
			return null;
		}
	}
}