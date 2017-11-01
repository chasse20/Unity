using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Attribute for displaying an enumerator as maskable pop-up field</summary>
	public class AttributeMask : PropertyAttribute
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Enumerator type</summary>
		public Type type;
		
		//=======================
		// Initialization
		//=======================
		public AttributeMask( Type tType )
		{
			type= tType;
		}
	}
}