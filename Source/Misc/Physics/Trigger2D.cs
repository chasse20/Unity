using UnityEngine;
using System;

namespace PeenToys
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Relays 2D trigger events into a <see cref="UnityEngine.Events.UnityEvent"/> via the inspector</summary>
	[AddComponentMenu( "PeenToys/Physics/Trigger 2D" )]
	[DisallowMultipleComponent]
	public class Trigger2D : Collisioner<EventTrigger2D>
	{
		//=======================
		// Trigger
		//=======================
		/// <summary>Callback for an enter event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollider"/>Touched collider</param>
		public virtual void OnTriggerEnter2D( Collider2D tCollider )
		{
			if ( isEnterEvent )
			{
				_onEnter.Invoke( this, tCollider );
			}
		}
		
		/// <summary>Callback for a stay event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollider"/>Touched collider</param>
		public virtual void OnTriggerStay2D( Collider2D tCollider )
		{
			if ( isStayEvent )
			{
				_onStay.Invoke( this, tCollider );
			}
		}
		
		/// <summary>Callback for an exit event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollider"/>Touched collider</param>
		public virtual void OnTriggerExit2D( Collider2D tCollider )
		{
			if ( isExitEvent )
			{
				_onExit.Invoke( this, tCollider );
			}
		}
	}
}