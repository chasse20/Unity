using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Provides touch status using Unity's <see cref="UnityEngine.Input"/> system for mobile</summary>
	[Serializable]
	public class TouchMobile : ITouch
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Specific finger to gather state data from</summary>
		public int fingerID;
		/// <summary>Output state data that is populated during the last tick</summary>
		public UnityEngine.Touch touch;
	
		//=======================
		// Constructor
		//=======================
		public TouchMobile( int tFingerID )
		{
			fingerID = tFingerID;
		}
	
		//=======================
		// Input
		//=======================
		/// <summary>Touch position of the latest tick</summary>
		public virtual Vector2 position
		{
			get
			{
				return touch.position;
			}
		}
		
		/// <summary>Change in position since previous tick</summary>
		public virtual Vector2 deltaPosition
		{
			get
			{
				return touch.deltaPosition;
			}
		}
		
		/// <summary>Time passed since previous tick</summary>
		public virtual float deltaTime
		{
			get
			{
				return touch.deltaTime;
			}
		}
		
		/// <summary>Number of registered taps in a row</summary>
		public virtual int tapCount
		{
			get
			{
				return touch.tapCount;
			}
		}
		
		/// <summary>Populates touch state and determines current <see cref="UnityEngine.TouchPhase"/> using Unity's <see cref="UnityEngine.Input"/> system</summary>
		public virtual TouchPhase state
		{
			get
			{
				if ( fingerID >= 0 && fingerID < Input.touchCount )
				{
					touch = Input.GetTouch( fingerID );
					return touch.phase;
				}
				
				return TouchPhase.Canceled;
			}
		}
	}
}