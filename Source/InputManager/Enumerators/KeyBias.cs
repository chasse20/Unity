using System;

namespace PeenIn
{
	//##########################
	// Enum Declaration
	//##########################
	/// <summary>Enumerator for bias when dealing with multiple <see cref="Key"/>s</summary>
	public enum KeyBias
	{
		ShortCircuit,
		Additive,
		Positive,
		Negative
	}
}