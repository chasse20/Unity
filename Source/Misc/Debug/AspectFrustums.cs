#pragma warning disable 108

using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Renders multiple view frustums when expanded in the hierarchy</summary>
	[AddComponentMenu( "PeenToys/Debug/Aspect Frustums" )]
	public class AspectFrustums : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>View frustums to render; preset with 4:3, 16:9 and 16:10</summary>
		public AspectFrustum[] frustums = new AspectFrustum[] { new AspectFrustum( 4, 3, Color.red ), new AspectFrustum( 16, 9, Color.green ), new AspectFrustum( 16, 10, Color.cyan ) };
		/// <summary>View camera, defaults to scene camera if null</summary>
		public Camera camera;
		/// <summary>Caps the far clipping plane of the camera</summary>
		public float farClipLimit = Mathf.Infinity;
	
		//=======================
		// Destruction
		//=======================
		/// <summary>Clear frustums from memory</summary>
		protected virtual void OnDestroy()
		{
			if ( frustums != null )
			{
				Array.Clear( frustums, 0, frustums.Length );
				frustums = null;
			}
		}
	
		//=======================
		// Editor Tick
		//=======================
		/// <summary>Renders all view frustums</summary>
		protected void OnDrawGizmos()
		{
			if ( frustums != null && frustums.Length > 0 )
			{
				// Determine view camera
				Camera tempCamera = camera;
				if ( tempCamera == null )
				{
					tempCamera = Camera.current;
				}
				
				// Render
				if ( tempCamera != null )
				{
					Color32 tempOldColor = Gizmos.color;
					Matrix4x4 tempOldMatrix = Gizmos.matrix;
					Gizmos.matrix = tempCamera.transform.localToWorldMatrix;
					float tempFarClip;
					
					for ( int i = ( frustums.Length - 1 ); i >= 0; --i )
					{
						if ( frustums[i].height != 0 )
						{
							Gizmos.color = frustums[i].color;
							
							tempFarClip = tempCamera.farClipPlane;
							if ( farClipLimit < Mathf.Infinity )
							{
								tempFarClip = farClipLimit;
							}
							
							if ( tempCamera.orthographic )
							{
								Gizmos.DrawWireCube( new Vector3( 0, 0, ( ( tempFarClip - tempCamera.nearClipPlane ) * 0.5f ) ), new Vector3( ( tempCamera.orthographicSize * 2 * frustums[i].aspectRatio ), ( tempCamera.orthographicSize * 2 ), ( tempFarClip - tempCamera.nearClipPlane ) ) );
							}
							else
							{
								Gizmos.DrawFrustum( tempCamera.transform.position, tempCamera.fieldOfView, tempCamera.nearClipPlane, tempFarClip, frustums[i].aspectRatio );
							}
						}
					}
					
					Gizmos.color = tempOldColor;
					Gizmos.matrix = tempOldMatrix;
				}
			}
		}
	}
}