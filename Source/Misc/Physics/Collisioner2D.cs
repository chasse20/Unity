using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Relays 2D collision events into a <see cref="UnityEngine.Events.UnityEvent"/> via the inspector</summary>
	[AddComponentMenu( "PeenToys/Physics/Collisioner 2D" )]
	[DisallowMultipleComponent]
	public class Collisioner2D : Collisioner<EventCollisioner2D>
	{		
		//=======================
		// Collision
		//=======================
		/// <summary>Callback for an enter event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollision"/>Collision data</param>
		public virtual void OnCollisionEnter2D( Collision2D tCollision )
		{
			if ( isEnterEvent )
			{
				_onEnter.Invoke( this, tCollision );
			}
		}
		
		/// <summary>Callback for a stay event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollision"/>Collision data</param>
		public virtual void OnCollisionStay2D( Collision2D tCollision )
		{
			if ( isStayEvent )
			{
				_onStay.Invoke( this, tCollision );
			}
		}
		
		/// <summary>Callback for an exit event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollision"/>Collision data</param>
		public virtual void OnCollisionExit2D( Collision2D tCollision )
		{
			if ( isExitEvent )
			{
				_onExit.Invoke( this, tCollision );
			}
		}
	}
}