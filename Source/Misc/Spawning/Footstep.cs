using UnityEngine;
using UnityEngine.Audio;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Individual footstep data</summary> 
	[Serializable]
	public class Footstep
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Prefab instance of a single step</summary>
		public GameObject prefab;
		/// <summary>Spawn location</summary>
		public Transform spawnPoint;
		/// <summary>Spawn parent</summary>
		public Transform parent;
		/// <summary>Audio to play when a step is made</summary>
		public AudioSource audio;
		/// <summary>Offset position from the spawn point</summary>
		public Vector3 positionOffset;
		/// <summary>Offset rotation from the spawn point</summary>
		public Vector3 rotationOffset;
	}
}