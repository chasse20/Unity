using UnityEngine;
using System;

namespace PeenToys
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Relays 3D trigger events into a <see cref="UnityEngine.Events.UnityEvent"/> via the inspector</summary>
	[AddComponentMenu( "PeenToys/Physics/Trigger 3D" )]
	[DisallowMultipleComponent]
	public class Trigger3D : Collisioner<EventTrigger3D>
	{
		//=======================
		// Trigger
		//=======================
		/// <summary>Callback for an enter event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollider"/>Touched collider</param>
		public virtual void OnTriggerEnter( Collider tCollider )
		{
			if ( isEnterEvent )
			{
				_onEnter.Invoke( this, tCollider );
			}
		}
		
		/// <summary>Callback for a stay event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollider"/>Touched collider</param>
		public virtual void OnTriggerStay( Collider tCollider )
		{
			if ( isStayEvent )
			{
				_onStay.Invoke( this, tCollider );
			}
		}
		
		/// <summary>Callback for an exit event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollider"/>Touched collider</param>
		public virtual void OnTriggerExit( Collider tCollider )
		{
			if ( isExitEvent )
			{
				_onExit.Invoke( this, tCollider );
			}
		}
	}
}