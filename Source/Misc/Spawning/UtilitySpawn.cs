using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Utility for spawning objects and components</summary>
	public static class UtilitySpawn
	{
		//=======================
		// Functions
		//=======================
		/// <summary>Spawns a <see cref="UnityEngine.GameObject"/> from a prefab</summary>
		/// <param name="tPrefab"/>Prefab instance</param>
		/// <param name="tParent"/>Optional parent transform</param>
		/// <param name="tSpawnPoint"/>Optional spawn point</param>
		/// <param name="tLocation"/>Optional location, treated as offset if <paramref name="tSpawnPoint"/> is specified</param>
		/// <param name="tRotation"/>Optional rotation, treated as offset if <paramref name="tSpawnPoint"/> is specified</param>
		/// <returns>GameObject instance if successful</returns>
		public static GameObject SpawnObject( GameObject tPrefab, Transform tParent = null, Transform tSpawnPoint = null, Vector3 tLocation = default( Vector3 ), Vector3 tRotation = default( Vector3 ) )
		{
			if ( tPrefab != null )
			{
				// Try to grab random Marker location if specified, treat Location/Rotation as offsets to Marker
				if ( tSpawnPoint != null )
				{
					tLocation += tSpawnPoint.position;
					tRotation += tSpawnPoint.eulerAngles;
				}
				
				// Create at location and parent
				GameObject tempSpawned = UnityEngine.Object.Instantiate( tPrefab, tLocation, Quaternion.Euler( tRotation ) ) as GameObject;
				if ( tempSpawned.transform != null && tParent != null && tParent != tempSpawned.transform )
				{
					tempSpawned.transform.SetParent( tParent );
				}
				
				return tempSpawned;
			}
			
			return null;
		}
		
		/// <summary>Spawns a typed <see cref="UnityEngine.Component"/> and its attached <see cref="UnityEngine.GameObject"/> from a prefab</summary>
		/// <param name="tPrefab"/>Prefab instance</param>
		/// <param name="tParent"/>Optional parent transform</param>
		/// <param name="tSpawnPoint"/>Optional spawn point</param>
		/// <param name="tLocation"/>Optional location, treated as offset if <paramref name="tSpawnPoint"/> is specified</param>
		/// <param name="tRotation"/>Optional rotation, treated as offset if <paramref name="tSpawnPoint"/> is specified</param>
		/// <returns>Typed component instance if successful</returns>
		public static T SpawnComponent<T>( T tPrefab, Transform tParent = null, Transform tSpawnPoint = null, Vector3 tLocation = default( Vector3 ), Vector3 tRotation = default( Vector3 ) ) where T : Component
		{
			// Check if prefab is set
			if ( tPrefab != null )
			{
				return SpawnObject( tPrefab.gameObject, tParent, tSpawnPoint, tLocation, tRotation ).GetComponent<T>();
			}
			
			return null;
		}
	}
}