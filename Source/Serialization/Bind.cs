using System;
using UnityEngine;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class Bind<K,V>
	{
		//=======================
		// Variables
		//=======================
		public K key;
		public V value;
		
		//=======================
		// Constructor
		//=======================
		public Bind()
		{
		}
		
		public Bind( K tKey, V tValue )
		{
			key = tKey;
			value = tValue;
		}
	}
}