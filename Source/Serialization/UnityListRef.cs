using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class UnityListRef<T> : UnityCollection<UnityEngine.Object> where T : class
	{
		//=======================
		// Variables
		//=======================
		public List<T> list;
		
		//=======================
		// Destructor
		//=======================
		~UnityListRef()
		{
			if ( list != null )
			{
				list.Clear();
				list = null;
			}
		}
		
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
				list = new List<T>();
				T tempObject;
				int tempListLength = _values.Count;
				for ( int i = 0; i < tempListLength; ++i )
				{
					tempObject = _values[i] as T;
					if ( tempObject != null )
					{
						list.Add( tempObject );
					}
				}
			}
			
			base.OnAfterDeserialize();
		}
	}
}