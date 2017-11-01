using UnityEngine;
using System;
using UnityEngine.Events;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary><see cref="UnityEngine.Events.UnityEvent"/> wrapper for 2D trigger events</summary>
	[Serializable]
	public class EventTrigger2D : UnityEvent<Trigger2D,Collider2D>
	{
	}
}