using UnityEngine;
using System;

namespace PeenToys
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Spawns footstep prefabs at a location</summary>
	[AddComponentMenu( "PeenToys/Spawning/Footstepper" )]
	public class Footstepper : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Array that can be used to store different kinds of foot-steps (i.e., left and right feet)</summary>
		public Footstep[] footstepTypes;
		
		//=======================
		// Footstep
		//=======================
		/// <summary>Call designed to be used by animations to spawn a foot-step</summary>
		/// <param name="tIndex">Index of the footstep type to use</param>
		public virtual void onFootStep( int tIndex )
		{
			if ( tIndex >= 0 )
			{
				spawnStep( (uint)tIndex );
			}
		}
		
		/// <summary>Spawns a single footstep using the type at element </summary>
		/// <param name="tIndex">Index of the footstep type to use</param>
		/// <returns>Spawned object of the footprint if successful</returns>
		public virtual GameObject spawnStep( uint tIndex )
		{
			if ( footstepTypes != null && tIndex < footstepTypes.Length )
			{
				// Spawn
				GameObject tempStep = null;
				if ( footstepTypes[ tIndex ].prefab != null && footstepTypes[ tIndex ].spawnPoint != null )
				{
					tempStep = UnityEngine.Object.Instantiate( footstepTypes[ tIndex ].prefab, ( footstepTypes[ tIndex ].spawnPoint.position + footstepTypes[ tIndex ].positionOffset ), Quaternion.Euler( footstepTypes[ tIndex ].spawnPoint.eulerAngles + footstepTypes[ tIndex ].rotationOffset ) ) as GameObject;
					if ( tempStep != null && footstepTypes[ tIndex ].parent != null )
					{
						tempStep.transform.SetParent( footstepTypes[ tIndex ].parent, false );
					}
				}
				
				// Audio
				if ( footstepTypes[ tIndex ].audio != null )
				{
					footstepTypes[ tIndex ].audio.Stop();
					footstepTypes[ tIndex ].audio.Play();
				}
				
				return tempStep;
			}
			
			return null;
		}
	}
}