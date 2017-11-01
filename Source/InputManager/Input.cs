using System;

namespace PeenIn
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Abstract class for handling inputs</summary>
	public abstract class Input<T>
	{
		//=======================
		// Variables
		//=======================
		/// <summary>If true, forces the tick to return true regardless of state changes</summary>
		public bool isContinuous;
		/// <summary>Current state of the input</summary>
		public T state;
		/// <summary>Reset state of the input if nothing has changed</summary>
		public T defaultState;
		
		//=======================
		// Constructor
		//=======================
		public Input( bool tIsContinuous = false )
		{
			isContinuous = tIsContinuous;
		}
		
		//=======================
		// Tick
		//=======================
		/// <summary>Evaluates the state of the input</summary>
		/// <returns>True if the state changed or <see cref="isContinuous"/> is true</returns>
		public abstract bool tick();
	}
}