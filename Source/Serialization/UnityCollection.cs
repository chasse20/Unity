using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public abstract class UnityCollection<T> : ISerializationCallbackReceiver
	{
		//=======================
		// Variables
		//=======================
		[SerializeField]
		protected List<T> _values;
		#if !UNITY_ENGINE
			protected int serializeCounter; // used to fix a bug since deserialization happens twice because Unity...
		#endif
		
		//=======================
		// Destructor
		//=======================
		~UnityCollection()
		{
			if ( _values != null )
			{
				_values.Clear();
				_values = null;
			}
		}
		
		//=======================
		// Serialization
		//=======================
		public virtual void OnBeforeSerialize()
		{
		}
		
		public virtual void OnAfterDeserialize()
		{
			#if !UNITY_EDITOR
				if ( _values != null )
				{
					if ( serializeCounter == 1 )
					{
						_values.Clear();
						_values = null;
					}
					++serializeCounter;
				}
			#endif
		}
	}
}