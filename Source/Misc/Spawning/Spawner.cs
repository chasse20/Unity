using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles both manual and automatic spawning via a delayed countdown</summary>
	[AddComponentMenu( "PeenToys/Spawning/Spawner" )]
	public class Spawner : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Prefab instance to spawn</summary>
		public GameObject prefab;
		/// <summary>Parent transform for the spawned instances, can be null</summary>
		public Transform parent;
		/// <summary>Spawn point</summary>
		public Transform point;
		/// <summary>Spawn location, treated as offset if spawn point is specified</summary>
		public Vector3 location;
		/// <summary>Spawn rotation, treated as offset if spawn point is specified</summary>
		public Vector3 rotation;
		/// <summary>Countdown to spawn: 0 treated as instant, negative is not spawned</summary>
		[SerializeField]
		protected float _counter;
		
		//=======================
		// Initialization
		//=======================
		/// <summary>Initializes the spawn countdown</summary>
		protected virtual void Start()
		{
			effectsCounter( _counter );
		}
		
		//=======================
		// Tick
		//=======================
		/// <summary>Tries to enable only if <see cref="_counter"/> is greater than 0</summary>
		public virtual bool enable()
		{
			enabled = _counter > 0;
			return enabled;
		}
		
		/// <summary>Counts down the spawn timer</summary>
		protected virtual void Update()
		{
			float tempCounter = _counter - Time.deltaTime;
			if ( tempCounter < 0 )
			{
				tempCounter = 0;
			}
			setCounter( tempCounter );
		}
			
		public virtual float counter
		{
			get
			{
				return _counter;
			}
			set
			{
				setCounter( value );
			}
		}
		
		/// <summary>Sets the spawn countdown</summary>
		/// <param name="tCounter">Countdown time</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setCounter( float tCounter )
		{
			if ( tCounter != _counter )
			{
				float tempOld = _counter;
				_counter = tCounter;
				effectsCounter( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Spawns an instance when the countdown reaches 0</summary>
		/// <param name="tOld">Previous countdown time</param>
		protected virtual void effectsCounter( float tOld )
		{
			if ( _counter == 0 )
			{
				spawn();
			}
			
			if ( _counter != tOld )
			{
				enable();
			}
		}
		
		//=======================
		// Spawn
		//=======================
		/// <summary>Spawns an instance</summary>
		/// <returns>Spawned instance</returns>
		public virtual GameObject spawn()
		{
			return UtilitySpawn.SpawnObject( prefab, parent, point, location, rotation );
		}
	}
}