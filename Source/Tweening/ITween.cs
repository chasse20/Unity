using System;
using UnityEngine;

namespace PeenTween
{
	//##########################
	// Interface Declaration
	//##########################
	public interface ITween
	{
		TweenTick tick( float tDeltaTime );
		TimeMode timeMode { get; set; }
		Component owner { get; set; }
		void reverse();
	}
}