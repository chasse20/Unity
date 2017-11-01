using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Handles reset functionality for the owning <see cref="UnityEngine.GameObject"/></summary>
	[AddComponentMenu( "PeenToys/Spawning/Resetter" )]
	public class Resetter : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>If true, will destroy this instance when reset</summary>
		public bool isDestroyedOnReset;
		/// <summary>If not null, will create a new instance prefab and destroys this one when reset</summary>
		public GameObject resetPrefab;
		/// <summary><see cref="UnityEngine.Transform"/> to reset to</summary>
		public Transform resetTransform;
		/// <summary>Original position to reset to</summary>
		public Vector3 originalPosition;
		/// <summary>Original rotation to reset to</summary>
		public Vector3 originalRotation;
		/// <summary>Original local scale to reset to</summary>
		public Vector3 originalLocalScale;
		
		//=======================
		// Initialization
		//=======================
		/// <summary>Stores the original transformations</summary>
		protected virtual void Awake()
		{
			originalPosition = transform.position;
			originalRotation = transform.rotation.eulerAngles;
			originalLocalScale = transform.localScale;
		}
		
		//=======================
		// Reset
		//=======================
		/// <summary>Either destroys this instance entirely, replaces it with a prefab, or resets its transform</summary>
		public virtual void reset()
		{
			// Destroy if specified
			if ( isDestroyedOnReset )
			{
				Destroy( gameObject );
			}
			// Replace with Prefab
			else if ( resetPrefab != null )
			{
				UtilitySpawn.SpawnObject( resetPrefab, transform.parent, resetTransform );
				Destroy( gameObject );
			}
			// Retransform
			else if ( resetTransform != null )
			{
				transform.position = resetTransform.position;
				transform.rotation = resetTransform.rotation;
				transform.localScale = resetTransform.localScale;
			}
			else
			{
				transform.position = originalPosition;
				transform.rotation = Quaternion.Euler( originalRotation );
				transform.localScale = originalLocalScale;
			}
		}
	}
}