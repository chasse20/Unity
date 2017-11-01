using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public abstract class UnityHashSet<T,L> : UnityCollection<L>
	{
		//=======================
		// Variables
		//=======================
		public HashSet<T> hashSet;
		
		//=======================
		// Destructor
		//=======================
		~UnityHashSet()
		{
			if ( hashSet != null )
			{
				hashSet.Clear();
				hashSet = null;
			}
		}
	}
	
	//##########################
	// Class Declaration
	//##########################
	public class UnityHashSet<T> : UnityHashSet<T,T>
	{
		//=======================
		// Serialization
		//=======================		
		public override void OnAfterDeserialize()
		{
			if ( _values != null && _values.Count > 0 )
			{
				hashSet = new HashSet<T>( _values );
			}

			base.OnAfterDeserialize();
		}
	}
}