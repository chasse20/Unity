using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeenIn
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles a button input</summary>
	[Serializable]
	public class InputButton : Input<KeyState>
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Simulated key strokes</summary>
		public IKey[] keys;
		/// <summary>If true, requires ALL <see cref="keys"/> to have the same state value in order for the <see cref="state"/> to change</summary>
		public bool isMatchingRequired;
		/// <summary>Event that fires during state changes or if <see cref="isContinuous">is true</summary>
		public event Action<InputButton> onStateChanged;
		
		//=======================
		// Constructor
		//=======================
		public InputButton( IKey[] tKeys, bool tIsMatchingRequired = false, KeyState tDefaultState = KeyState.Up, bool tIsContinuous = false ) : base( tIsContinuous )
		{
			keys = tKeys;
			isMatchingRequired = tIsMatchingRequired;
			defaultState = tDefaultState;
		}
		
		//=======================
		// Destructor
		//=======================
		~InputButton()
		{
			// Clear memory
			onStateChanged = null;
			if ( keys != null )
			{
				Array.Clear( keys, 0, keys.Length );
				keys = null;
			}
		}
		
		//=======================
		// Tick
		//=======================
		public override bool tick()
		{
			// Default
			KeyState tempOld = state;
			state = defaultState;
			
			// Keys
			if ( keys != null && keys.Length > 0 )
			{
				KeyState tempState;
				if ( isMatchingRequired ) // all Keys have to match
				{
					tempState = keys[0].state;
					for ( int i = ( keys.Length - 1 ); i >= 1; --i )
					{
						if ( keys[i].state != tempState )
						{
							break;
						}
						else if ( i == 1 )
						{
							state = tempState;
						}
					}
				}
				else // first Key to differ from default
				{
					for ( int i = ( keys.Length - 1 ); i >= 0; --i )
					{
						tempState = keys[i].state;
						if ( tempState != state )
						{
							state = tempState;
							break;
						}
					}
				}
			}
			
			// Fire event if continuous or changed
			if ( isContinuous || state != tempOld )
			{
				if ( onStateChanged != null )
				{
					onStateChanged( this );
				}
				
				return true;
			}
			
			return false;
		}
	}
}