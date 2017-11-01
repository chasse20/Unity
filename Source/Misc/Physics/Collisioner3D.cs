using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Relays 3D collision events into a <see cref="UnityEngine.Events.UnityEvent"/> via the inspector</summary>
	[AddComponentMenu( "PeenToys/Physics/Collisioner 3D" )]
	[DisallowMultipleComponent]
	public class Collisioner3D : Collisioner<EventCollisioner3D>
	{		
		//=======================
		// Collision
		//=======================
		/// <summary>Callback for an enter event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollision"/>Collision data</param>
		public virtual void OnCollisionEnter( Collision tCollision )
		{
			if ( isEnterEvent )
			{
				_onEnter.Invoke( this, tCollision );
			}
		}
		
		/// <summary>Callback for a stay event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollision"/>Collision data</param>
		public virtual void OnCollisionStay( Collision tCollision )
		{
			if ( isStayEvent )
			{
				_onStay.Invoke( this, tCollision );
			}
		}
		
		/// <summary>Callback for an exit event, relayed by the corresponding <see cref="UnityEngine.Events.UnityEvent"/> if toggled</summary>
		/// <param name="tCollision"/>Collision data</param>
		public virtual void OnCollisionExit( Collision tCollision )
		{
			if ( isExitEvent )
			{
				_onExit.Invoke( this, tCollision );
			}
		}
	}
}