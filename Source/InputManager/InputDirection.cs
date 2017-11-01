using System;
using UnityEngine;

namespace PeenIn
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles a directional input using two <see cref="InputAxis"/> to represent X and Y</summary>
	[Serializable]
	public class InputDirection : Input<Vector2>
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Axis input for the X direction</summary>
		public InputAxis axisX;
		/// <summary>Axis input for the Y direction</summary>
		public InputAxis axisY;
		/// <summary>Whether the direction should be normalized</summary>
		public bool isNormalized;
		/// <summary>Output scale of the direction</summary>
		public float scale;
		/// <summary>Event that fires during state changes or if <see cref="isContinuous">is true</summary>
		public event Action<InputDirection> onStateChanged;
		
		//=======================
		// Constructor
		//=======================
		public InputDirection( InputAxis tX, InputAxis tY, bool tIsNormalized = true, float tScale = 1, bool tIsContinuous = false ) : base( tIsContinuous )
		{
			axisX = tX;
			axisY = tY;
			isNormalized = tIsNormalized;
			scale = tScale;
		}
		
		//=======================
		// Destructor
		//=======================
		~InputDirection()
		{
			// Clear memory
			onStateChanged = null;
		}
		
		//=======================
		// Tick
		//=======================
		public override bool tick()
		{
			// Defaults
			onStateChanged = null;
			Vector2 tempOld = state;
			if ( axisX == null && axisY == null )
			{
				state = defaultState;
			}
			// Calculate
			else
			{
				bool tempIsChanged = false;
				if ( axisX == null )
				{
					state.x = defaultState.x;
				}
				else
				{
					tempIsChanged = axisX.tick();
					state.x = axisX.state;
				}
				
				if ( axisY == null )
				{
					state.y = defaultState.y;
				}
				else
				{
					tempIsChanged = axisY.tick() || tempIsChanged;
					state.y = axisY.state;
				}
				
				// Normalize and scale
				if ( tempIsChanged )
				{
					if ( isNormalized )
					{
						state.Normalize();
					}
					
					state *= scale;
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