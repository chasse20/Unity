using UnityEngine;
using System;

namespace PeenScreen
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Container for cameras and any associated properties, used by the <see cref="View"/></summary>
	public abstract class Rig : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>True if this instance can be safely destroyed inside the <see cref="View.disableRenderable"/> method</summary>
		[NonSerialized]
		public bool isCloned;
		/// <summary>Primary audio listener</summary>
		public AudioListener listener;
		
		//=======================
		// Type
		//=======================
		/// <summary>Instance type that corresponds to VRSettings.supportedDevices</summary>
		public abstract string type { get; }
		
		//=======================
		// Rect
		//=======================
		/// <summary>Tries to apply the <paramref="tScreenRect"/> to this instance</summary>
		/// <param name="tScreenRect">Projection Rect</param>
		/// <returns>True if successfully applied</returns>
		public virtual bool setRect( Rect tScreenRect )
		{
			return false;
		}
	}
}