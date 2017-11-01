using UnityEngine;
using System;
using UnityEngine.Events;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary><see cref="UnityEngine.Events.UnityEvent"/> wrapper for 3D trigger events</summary>
	[Serializable]
	public class EventTrigger3D : UnityEvent<Trigger3D,Collider>
	{
	}
}