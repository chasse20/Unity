using UnityEditor;
using UnityEngine;
using System.Text;
using System.IO;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Editor Utility for locking an object's transform to the view</summary>
	public static class UtilityViewLock
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Active state</summary>
		private static bool isSelectionLockedToView;
		/// <summary>Last position of the view camera for optimization</summary>
		private static Vector3 lastSceneCameraPosition;
		/// <summary>Last rotation of the view camera for optimization</summary>
		private static Quaternion lastSceneCameraRotation;
		
		//=======================
		// View Lock
		//=======================
		/// <summary>Validating function for the editor menu</summary>
		/// <returns>True if an object is selected</returns>
		[MenuItem( "GameObject/Lock To View %#t", true )]
		private static bool ValidateLockToView()
		{
			return Selection.activeGameObject != null;
		}
		
		/// <summary>Function for the editor menu that toggles the tick state</summary>
		[MenuItem( "GameObject/Lock To View %#t", false )]
		public static void LockToView()
		{
			isSelectionLockedToView = !isSelectionLockedToView;
			if ( isSelectionLockedToView )
			{
				EditorApplication.update += OnLockToViewTick;
			}
			else
			{
				EditorApplication.update -= OnLockToViewTick;
			}
		}
		
		/// <summary>Ticks each frame to move the selected object to the view's transform, disabled if the object is deselected</summary>
		private static void OnLockToViewTick()
		{
			// Disable if nothing selected
			if ( Selection.activeGameObject == null )
			{
				isSelectionLockedToView = false;
				EditorApplication.update -= OnLockToViewTick;
			}
			// Have selected follow viewport camera
			else if ( Camera.current != null )
			{
				Transform tempCameraTransform = Camera.current.transform;
				if ( tempCameraTransform.position != lastSceneCameraPosition )
				{
					lastSceneCameraPosition = tempCameraTransform.position;
					Selection.activeGameObject.transform.position = tempCameraTransform.position;
				}
				
				if ( tempCameraTransform.rotation != lastSceneCameraRotation )
				{
					lastSceneCameraRotation = tempCameraTransform.rotation;
					Selection.activeGameObject.transform.rotation = tempCameraTransform.rotation;
				}
			}
		}
	}
}