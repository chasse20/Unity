using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Attribute for forcing an object field to be a specific type</summary>
	public class AttributeRef : PropertyAttribute
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Expected type of object to force</summary>
		public Type type;
		
		//=======================
		// Initialization
		//=======================
		public AttributeRef( Type tType )
		{
			type = tType;
		}
	}
}