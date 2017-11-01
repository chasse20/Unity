using System;
using UnityEngine;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Provides key status using Unity's <see cref="UnityEngine.Input"/> system</summary>
	[Serializable]
	public class KeyPC : IKey
	{
		//=======================
		// Variables
		//=======================
		/// <summary><see cref="UnityEngine.KeyCode"/> of the desired PC key</summary>
		public KeyCode key;
	
		//=======================
		// Constructor
		//=======================
		public KeyPC( KeyCode tKeyCode )
		{
			key = tKeyCode;
		}
	
		//=======================
		// State
		//=======================
		/// <summary>Calculates the <see cref="KeyState"/></summary>
		public virtual KeyState state
		{
			get
			{
				if ( Input.GetKey( key ) )
				{
					return Input.GetKeyDown( key ) ? KeyState.Pressed : KeyState.Down;
				}
				else if ( Input.GetKeyUp( key ) )
				{
					return KeyState.Released;
				}
				
				return KeyState.Up;
			}
		}
	}
}