using UnityEngine;
using System;
using System.Collections.Generic;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class UnityDictionary<K,V,B> : UnityCollection<B> where B : Bind<K,V>
	{
		//=======================
		// Variables
		//=======================
		public Dictionary<K,V> dictionary;
		
		//=======================
		// Destructor
		//=======================
		~UnityDictionary()
		{
			if ( dictionary != null )
			{
				dictionary.Clear();
				dictionary = null;
			}
		}
		
		//=======================
		// Serialization
		//=======================
		public override void OnAfterDeserialize()
		{
			if ( _values != null && _values.Count > 0 )
			{
				dictionary = new Dictionary<K,V>();
				for ( int i = ( _values.Count - 1 ); i >= 0; --i )
				{
					dictionary[ _values[i].key ] = _values[i].value;
				}
			}
			
			base.OnAfterDeserialize();
		}
	}
}