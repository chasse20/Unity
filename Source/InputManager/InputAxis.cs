using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles an axis input</summary>
	[Serializable]
	public class InputAxis : Input<float>
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Simulated axes</summary>
		public IAxis[] axes;
		/// <summary>Determines how to handle the blending of <see cref="axes"/></summary>
		public AxisBlendMode blend;
		/// <summary>Clamping value to use for the final state of this axis</summary>
		public float clamp;
		/// <summary>Event that fires during state changes or if <see cref="isContinuous">is true</summary>
		public event Action<InputAxis> onStateChanged;
		
		//=======================
		// Constructor
		//=======================
		public InputAxis( IAxis[] tAxes, AxisBlendMode tBlend = AxisBlendMode.Add, float tClamp = Mathf.Infinity, bool tIsContinuous = false ) : base( tIsContinuous )
		{
			axes = tAxes;
			blend = tBlend;
			clamp = tClamp;
		}
		
		//=======================
		// Destructor
		//=======================
		~InputAxis()
		{
			// Clear memory
			onStateChanged = null;
			if ( axes != null )
			{
				Array.Clear( axes, 0, axes.Length );
				axes = null;
			}
		}
		
		//=======================
		// Tick
		//=======================
		public override bool tick()
		{
			// Defaults
			float tempOld = state;
			state = defaultState;
			
			// Axes
			if ( axes != null )
			{
				switch ( blend )
				{
					case AxisBlendMode.Add: // uses the added value of the axes
						float tempAdded = 0;
						for ( int i = ( axes.Length - 1 ); i >= 0; --i )
						{
							tempAdded += axes[i].state;
						}
						
						state = Mathf.Clamp( tempAdded, -clamp, clamp );
						break;
					case AxisBlendMode.Average: // uses the average value of the axes
						int tempListLength = axes.Length;
						float tempAverage = 0;
						for ( int i = ( tempListLength - 1 ); i >= 0; --i )
						{
							tempAverage += axes[i].state;
						}
						
						state = Mathf.Clamp( ( tempAverage / tempListLength ), -clamp, clamp );
						break;
					case AxisBlendMode.Max: // uses the axis with the largest value
						int tempMax = axes.Length - 1;
						if ( tempMax >= 0 )
						{
							float tempState = Mathf.Abs( axes[ tempMax ].state );
							float tempMaxState = tempState;
							for ( int i = ( tempMax - 1 ); i >= 0; --i )
							{
								tempState = Mathf.Abs( axes[i].state );
								if ( tempState > tempMaxState )
								{
									tempMaxState= tempState;
								}
							}
							
							state = Mathf.Clamp( tempMaxState, -clamp, clamp );
						}
						break;
					case AxisBlendMode.Min: // uses the axis with the smallest value
						int tempMin = axes.Length - 1;
						if ( tempMin >= 0 )
						{
							float tempState = Mathf.Abs( axes[ tempMin ].state );
							float tempMinState = tempState;
							for ( int i = ( tempMin - 1 ); i >= 0; --i )
							{
								tempState = Mathf.Abs( axes[i].state );
								if ( tempState < tempMinState )
								{
									tempMinState = tempState;
								}
							}
							
							state = Mathf.Clamp( tempMinState, -clamp, clamp );
						}
						break;
					default:
						break;
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