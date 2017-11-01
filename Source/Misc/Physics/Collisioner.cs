using UnityEngine;
using UnityEngine.Events;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Relays various physics events into a <see cref="UnityEngine.Events.UnityEvent"/> via the inspector</summary>
	public abstract class Collisioner<T> : MonoBehaviour where T : UnityEventBase
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Toggles relaying of the physics enter event</summary>
		public bool isEnterEvent;
		/// <summary>Relayed <see cref="UnityEngine.Events.UnityEvent"/> associated with the physics enter event</summary>
		[SerializeField]
		protected T _onEnter;
		/// <summary>Toggles relaying of the physics stay event</summary>
		public bool isStayEvent;
		/// <summary>Relayed <see cref="UnityEngine.Events.UnityEvent"/> associated with the physics stay event</summary>
		[SerializeField]
		protected T _onStay;
		/// <summary>Toggles relaying of the physics exit event</summary>
		public bool isExitEvent;
		/// <summary>Relayed <see cref="UnityEngine.Events.UnityEvent"/> associated with the physics exit event</summary>
		[SerializeField]
		protected T _onExit;
		
		//=======================
		// Deconstruction
		//=======================
		/// <summary>Clears all events from memory</summary>
		protected virtual void OnDestroy()
		{
			_onEnter.RemoveAllListeners();
			_onStay.RemoveAllListeners();
			_onExit.RemoveAllListeners();
		}
		
		//=======================
		// Accessors
		//=======================
		public virtual T onEnter
		{
			get
			{
				return _onEnter;
			}
		}
		
		public virtual T onStay
		{
			get
			{
				return _onStay;
			}
		}
		
		public virtual T onExit
		{
			get
			{
				return _onExit;
			}
		}
	}
}