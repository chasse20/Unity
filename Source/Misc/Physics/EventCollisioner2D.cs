using UnityEngine;
using System;
using UnityEngine.Events;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary><see cref="UnityEngine.Events.UnityEvent"/> wrapper for 2D collisions</summary>
	[Serializable]
	public class EventCollisioner2D : UnityEvent<Collisioner2D,Collision2D>
	{
	}
}