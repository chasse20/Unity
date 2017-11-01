using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class UnityHashSetRef<T> : UnityHashSet<T,UnityEngine.Object> where T : class
	{		
		//=======================
		// Serialization
		//=======================		
		public override void OnBeforeSerialize()
		{
			// Filter
			if ( _values != null )
			{
				for ( int i = ( _values.Count - 1 ); i >= 0; --i )
				{
					if ( _values[i] != null && !( _values[i] is T ) )
					{
						_values.RemoveAt( i );
					}
				}
			}
		}
		
		public override void OnAfterDeserialize()
		{
			if ( _values != null && _values.Count > 0 )
			{
				hashSet = new HashSet<T>();
				T tempObject;
				for ( int i = ( _values.Count - 1 ); i >= 0; --i )
				{
					tempObject = _values[i] as T;
					if ( tempObject != null )
					{
						hashSet.Add( tempObject );
					}
				}
			}
			
			base.OnAfterDeserialize();
		}
	}
}