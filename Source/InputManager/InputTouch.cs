using System;
using UnityEngine;

namespace PeenIn
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles a touch input</summary>
	public class InputTouch : Input<TouchPhase>
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Simulated touches</summary>
		public ITouch[] touches;
		/// <summary>If true, requires ALL <see cref="touches"/> to have the same state value in order for the <see cref="state"/> to change</summary>
		public bool isMatchingRequired;
		/// <summary>Event that fires during state changes or if <see cref="isContinuous">is true</summary>
		public event Action<InputTouch> onStateChanged;
		
		//=======================
		// Constructor
		//=======================
		public InputTouch( ITouch[] tTouches, bool tIsMatchingRequired = false, TouchPhase tDefaultState = TouchPhase.Canceled, bool tIsContinuous = false ) : base( tIsContinuous )
		{
			touches = tTouches;
			isMatchingRequired = tIsMatchingRequired;
			defaultState = tDefaultState;
		}
		
		//=======================
		// Destructor
		//=======================
		~InputTouch()
		{
			// Clear memory
			onStateChanged = null;
			if ( touches != null )
			{
				Array.Clear( touches, 0, touches.Length );
				touches = null;
			}
		}
		
		//=======================
		// Tick
		//=======================
		public override bool tick()
		{
			// Default
			TouchPhase tempOld = state;
			state = defaultState;
			
			// Touches
			if ( touches != null && touches.Length > 0 )
			{
				TouchPhase tempPhase;
				if ( isMatchingRequired ) // all Touches have to match
				{
					tempPhase = touches[0].state;
					for ( int i = ( touches.Length - 1 ); i >= 1; --i )
					{
						if ( touches[i].state != tempPhase )
						{
							break;
						}
						else if ( i == 1 )
						{
							state = tempPhase;
						}
					}
				}
				else // first Touch to differ from default
				{
					for ( int i = ( touches.Length - 1 ); i >= 0; --i )
					{
						tempPhase = touches[i].state;
						if ( tempPhase != state )
						{
							state = tempPhase;
							break;
						}
					}
				}
			}
			
			// Fire event if continuous or changed
			if ( isContinuous || state != tempOld || state == TouchPhase.Moved ) // also returns true if moved
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