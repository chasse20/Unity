using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Provides raw touch status using the mouse</summary>
	[Serializable]
	public class TouchMouse : ITouch
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Minimum position change required for the touch to be considered moving</summary>
		public float moveThreshold;
		/// <summary>Current position of the mouse</summary>
		protected Vector2 _position;
		/// <summary>Current change in position of the mouse</summary>
		protected Vector2 _deltaPosition;
		/// <summary>Time passed since last tick</summary>
		protected float _deltaTime;
		/// <summary>Time of last tick</summary>
		public float lastChangeTime;
		/// <summary>Maximum delay allowed to consider successive clicks to be part of a tap count</summary>
		public float tapDelay;
		/// <summary>Counter used to determine when the clicks are no longer considered taps</summary>
		public float tapCounter;
		/// <summary>Number of successive taps made before a sequence expires</summary>
		protected int _tapCount;
	
		//=======================
		// Constructor
		//=======================
		public TouchMouse( float tMoveThreshold = 0, float tTapDelay = 0.25f )
		{
			moveThreshold = tMoveThreshold;
			tapDelay = tTapDelay;
		}
	
		//=======================
		// Input
		//=======================
		/// <summary>Touch position of the latest tick</summary>
		public virtual Vector2 position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}
		
		/// <summary>Change in position since previous tick</summary>
		public virtual Vector2 deltaPosition
		{
			get
			{
				return _deltaPosition;
			}
			set
			{
				_deltaPosition = value;
			}
		}
		
		/// <summary>Time passed since previous tick</summary>
		public virtual float deltaTime
		{
			get
			{
				return _deltaTime;
			}
			set
			{
				_deltaTime = value;
			}
		}
		
		/// <summary>Number of registered taps in a row</summary>
		public virtual int tapCount
		{
			get
			{
				return _tapCount;
			}
			set
			{
				_tapCount = value;
			}
		}
		
		/// <summary>Populates touch state and determines current <see cref="UnityEngine.TouchPhase"/> using the mouse</summary>
		public virtual TouchPhase state
		{
			get
			{
				// Determine state
				TouchPhase tempState;
				Vector2 tempDeltaPosition = (Vector2)Input.mousePosition - _position;
				_position = Input.mousePosition;
				_deltaPosition = tempDeltaPosition;
				_deltaTime = Time.realtimeSinceStartup - lastChangeTime;
				lastChangeTime = Time.realtimeSinceStartup;
				
				if ( Input.GetKey( KeyCode.Mouse0 ) )
				{
					if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
					{
						tempState = TouchPhase.Began;
						tapCounter = tapDelay;
						++_tapCount;
					}
					else if ( moveThreshold >= 0 && ( tempDeltaPosition == Vector2.zero || tempDeltaPosition.magnitude <= moveThreshold ) )
					{
						tempState = TouchPhase.Stationary;
					}
					else
					{
						tempState = TouchPhase.Moved;
					}
				}
				else if ( Input.GetKeyUp( KeyCode.Mouse0 ) )
				{
					tempState = TouchPhase.Ended;
				}
				else
				{
					tempState = TouchPhase.Canceled;
				}
				
				// Tap
				if ( tapCounter > 0 )
				{
					tapCounter -= _deltaTime;
					if ( tapCounter <= 0 && _tapCount != 0 )
					{
						_tapCount = 0;
					}
				}
				
				return tempState;
			}
		}
	}
}