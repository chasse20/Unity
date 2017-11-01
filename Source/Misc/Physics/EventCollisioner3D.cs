using UnityEngine;
using System;
using UnityEngine.Events;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary><see cref="UnityEngine.Events.UnityEvent"/> wrapper for 3D collisions</summary>
	[Serializable]
	public class EventCollisioner3D : UnityEvent<Collisioner3D,Collision>
	{
	}
}