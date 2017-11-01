using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Provides a raw axis value using Unity's <see cref="UnityEngine.Input"/> system</summary>
	[Serializable]
	public class AxisUnity : IAxis
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Name of the axis defined in the InputManager</summary>
		public string axisName;
	
		//=======================
		// Constructor
		//=======================
		public AxisUnity( string tAxisName )
		{
			axisName = tAxisName;
		}
	
		//=======================
		// Input
		//=======================
		/// <summary>Determines the raw axis value using Unity's <see cref="UnityEngine.Input"/> system</summary>
		public virtual float state
		{
			get
			{
				return Input.GetAxisRaw( axisName );
			}
		}
	}
}