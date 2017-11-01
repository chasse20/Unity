using System;

namespace PeenIn
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Provides a raw axis value using positive and negative <see cref="IKey"/>s</summary>
	[Serializable]
	public class AxisKeyed : IAxis
	{
		//=======================
		// Variables
		//=======================
		/// <summary><see cref="IKey"/>s that represent a positive value when pressed or held</summary>
		public IKey[] keysPositive;
		/// <summary><see cref="IKey"/>s that represent a negative value when pressed or held</summary>
		public IKey[] keysNegative;
		/// <summary>Bias for determining how to blend both positive and negative <see cref="IKey"/>s together</summary>
		public KeyBias bias;
	
		//=======================
		// Constructor
		//=======================
		public AxisKeyed( IKey[] tKeysPositive, IKey[] tKeysNegative, KeyBias tBias = KeyBias.ShortCircuit )
		{
			keysPositive = tKeysPositive;
			keysNegative = tKeysNegative;
			bias = tBias;
		}
		
		//=======================
		// Destructor
		//=======================
		~AxisKeyed()
		{
			// Clear memory
			if ( keysPositive != null )
			{
				Array.Clear( keysPositive, 0, keysPositive.Length );
				keysPositive = null;
			}
			if ( keysNegative != null )
			{
				Array.Clear( keysNegative, 0, keysNegative.Length );
				keysNegative = null;
			}
		}
	
		//=======================
		// Input
		//=======================
		/// <summary>Calculates the raw axis value using the positive and negative input <see cref="IKey"/>s</summary>
		public virtual float state
		{
			get
			{
				// Positive Keys
				float tempRaw = 0;
				if ( keysPositive != null )
				{
					for ( int i = ( keysPositive.Length - 1 ); i >= 0; --i )
					{
						if ( keysPositive[i].state <= KeyState.Down )
						{
							if ( bias == KeyBias.Additive )
							{
								tempRaw += 1;
							}
							else if ( bias == KeyBias.Positive )
							{
								return 1;
							}
							else
							{
								tempRaw += 1;
								break;
							}
						}
					}
				}
				
				// Negative Keys
				if ( keysNegative != null )
				{
					for ( int i = ( keysNegative.Length - 1 ); i >= 0; --i )
					{
						if ( keysNegative[i].state <= KeyState.Down )
						{
							if ( bias == KeyBias.Additive )
							{
								tempRaw -= 1;
							}
							else if ( bias == KeyBias.Negative )
							{
								return -1;
							}
							else
							{
								tempRaw -= 1;
								break;
							}
						}
					}
				}
				
				return tempRaw;
			}
		}
	}
}