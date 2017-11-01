using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Interface Declaration
	//##########################
	public interface ITouch
	{
		TouchPhase state { get; }
		Vector2 position { get; }
		Vector2 deltaPosition { get; }
		float deltaTime { get; }
		int tapCount { get; }
	}
}