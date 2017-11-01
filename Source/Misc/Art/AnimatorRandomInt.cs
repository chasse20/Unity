using System;
using UnityEngine;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary><see cref="UnityEngine.StateMachineBehaviour"/> that randomizes an integer parameter when a new state is entered</summary>
	public class AnimatorRandomInt : StateMachineBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Integer parameter name</summary>
		public string parameter;
		/// <summary>Random number minimum (inclusive)</summary>
		public int min;
		/// <summary>Random number maximum (exclusive)</summary>
		public int max;
	
		//=======================
		// Members
		//=======================
		/// <summary>Randomizes the integer value upon entering an animator state</summary>
		/// <param name="tAnimator"><see cref="UnityEngine.Animator"/> instance that called this method</summary>
		/// <param name="tPathHash">Hash of the state path</summary>
		public override void OnStateMachineEnter( Animator tAnimator, int tPathHash )
		{
			if ( !String.IsNullOrEmpty( parameter ) )
			{
				tAnimator.SetInteger( parameter, UnityEngine.Random.Range( min, ( max + 1 ) ) );
			}
		}
	}
}