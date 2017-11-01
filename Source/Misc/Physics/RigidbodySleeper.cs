using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Wrapper for toggling the sleeping state of a <see cref="UnityEngine.Rigidbody"/></summary>
	[AddComponentMenu( "PeenToys/Physics/Rigidbody Sleeper" )]
	[RequireComponent( typeof( Rigidbody ) )]
	public class RigidbodySleeper : MonoBehaviour
	{	
		//=======================
		// Variables
		//=======================
		/// <summary>Sleeping state of the attached <see cref="UnityEngine.Rigidbody"/></summary>
		[SerializeField]
		protected bool _isSleeping;
		/// <summary>Will destroy this component at the start if true</summary>
		public bool isDestroyedOnStart = true;
		
		//=======================
		// Initialization
		//=======================
		/// <summary>Initializes the sleeping state, destroys itself if specified</summary>
		protected virtual void Start()
		{
			effectsSleeping( _isSleeping );
			
			if ( isDestroyedOnStart )
			{
				Destroy( this );
			}
		}
		
		//=======================
		// Sleeping
		//=======================
		public virtual bool isSleeping
		{
			get
			{
				return _isSleeping;
			}
			set
			{
				setSleeping( value );
			}
		}
		
		/// <summary>Sets the sleeping state</summary>
		/// <param name="tIsSleeping">Sleeping state to set to</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setSleeping( bool tIsSleeping )
		{
			if ( tIsSleeping != _isSleeping )
			{
				_isSleeping = tIsSleeping;
				effectsSleeping( !_isSleeping );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Toggles sleeping state in attached <see cref="UnityEngine.Rigidbody"/></summary>
		/// <param name="tOld">Previous state</param>
		protected virtual void effectsSleeping( bool tOld )
		{
			Rigidbody tempBody = GetComponent<Rigidbody>();
			if ( tempBody != null )
			{
				if ( _isSleeping )
				{
					tempBody.Sleep();
				}
				else
				{
					tempBody.WakeUp();
				}
			}
		}
	}
}