using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Provides axis status</summary>
	[Serializable]
	public class Axis : IAxis
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Instance of a raw axis data</summary>
		public IAxis raw;
		/// <summary>Simulated axis value</summary>
		public float value;
		/// <summary>Minimum threshold for raw axis input to be accepted</summary>
		public float deadZone;
		/// <summary>Sensitivity multiplier for raw axis input and change velocity</summary>
		public float sensitivity = 1;
		/// <summary>Velocity for going back to 0</summary>
		public float gravity;
		/// <summary>Velocity for going towards the target value</summary>
		public float velocity;
		/// <summary>Whether the raw axis input should be considered inverted</summary>
		public bool isInverted;
		/// <summary>Whether to use the <see cref="gravity"/> velocity to return to 0 before going to an opposite target, false will use the regular <see cref="velocity"/></summary>
		public bool isSnapped;
	
		//=======================
		// Constructor
		//=======================
		public Axis( IAxis tRaw, float tSensitivity = 1, float tDeadZone = 0, float tVelocity = 0, float tGravity = 0, bool tIsInverted = false, bool tIsSnapped = false )
		{
			raw = tRaw;
			sensitivity = tSensitivity;
			deadZone = tDeadZone;
			gravity = tGravity;
			velocity = tVelocity;
			isInverted = tIsInverted;
			isSnapped = tIsSnapped;
		}
		
		//=======================
		// Tick
		//=======================
		/// <summary>Calculates inversion, dead-zone and velocity of the axis <see cref="value"/></summary>
		public virtual float state
		{
			get
			{
				if ( raw == null )
				{
					value = 0;
				}
				else
				{
					// Inversion and Dead Zone
					float tempTarget = isInverted ? -raw.state : raw.state;
					if ( deadZone > 0 && Mathf.Abs( tempTarget ) <= deadZone )
					{
						tempTarget = 0;
					}
					else
					{
						tempTarget *= sensitivity;
					}
					
					// Rise
					if ( tempTarget > value )
					{
						if ( gravity > 0 && ( tempTarget == 0 || ( isSnapped && tempTarget > 0 && value < 0 ) ) )
						{
							value += gravity * Time.deltaTime;
							if ( value > 0 )
							{
								value = tempTarget == 0 ? 0 : velocity * ( value / gravity ); // calculate velocity overage via remainder of time
							}
						}
						else if ( velocity > 0 )
						{
							value += velocity * Time.deltaTime;
							if ( value > tempTarget )
							{
								value = tempTarget;
							}
						}
						else
						{
							value = tempTarget;
						}
					}
					// Fall
					if ( tempTarget < value )
					{
						if ( gravity > 0 && ( tempTarget == 0 || ( isSnapped && tempTarget < 0 && value > 0 ) ) )
						{
							value -= gravity * sensitivity * Time.deltaTime;
							if ( value < 0 )
							{
								value = tempTarget == 0 ? 0 : -velocity * ( value / gravity ); // calculate velocity overage via remainder of time
							}
						}
						else if ( velocity > 0 )
						{
							value -= velocity * sensitivity * Time.deltaTime;
							if ( value < tempTarget )
							{
								value = tempTarget;
							}
						}
						else
						{
							value = tempTarget;
						}
					}
				}
				
				return value;
			}
		}
	}
}