#pragma warning disable 108

using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Acts a faux-collider that will attempt to keep this object inside of an orthogonal, non-rotated screen space</summary>
	[AddComponentMenu( "PeenToys/Physics/Camera Collider 2D" )]
	public class CameraCollider2D : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Camera reference, assumed perpendicular to the screen</summary>
		[SerializeField]
		protected Camera _camera;
		/// <summary>Position of the camera since last tick</summary>
		[NonSerialized]
		public Vector3 lastCameraPosition;
		/// <summary>Projection matrix of the camera since last tick</summary>
		[NonSerialized]
		public Matrix4x4 lastCameraProjectionMatrix;
		/// <summary>Position of this object since last tick</summary>
		[NonSerialized]
		public Vector3 lastPosition;
		/// <summary>Offset amount for the bottom-left of the screen</summary>
		[SerializeField]
		protected Vector2 _offsetBottomLeft;
		/// <summary>Offset amount for the top-right of the screen</summary>
		[SerializeField]
		protected Vector2 _offsetTopRight;
		/// <summary>Offsets treated as pixels if true, normalized value if not</summary>
		[SerializeField]
		protected bool _isOffsetInPixels;
		/// <summary>If set, attempts to maintain this object at the specified transform position</summary>
		public Transform defaultTarget;
		/// <summary>Collider that can be used to for checking bounds, uses this object's position if null</summary>
		public Collider2D collider;
		/// <summary>Projected bottom-left point of the screen</summary>
		[NonSerialized]
		public Vector2 worldBottomLeft;
		/// <summary>Projected top-right point of the screen</summary>
		[NonSerialized]
		public Vector2 worldTopRight;
	
		//=======================
		// Initialization
		//=======================
		/// <summary>Initializes the camera collider</summary>
		protected virtual void Start()
		{
			effectsCamera( _camera );
		}
		
		//=======================
		// Tick
		//=======================
		/// <summary>Tries to enable only if the <see cref="_camera"/> is set</summary>
		public virtual bool enable()
		{
			enabled = _camera != null;
			return enabled;
		}
		
		/// <summary>Updates the projected screen corners if the relative location of the camera has changed, and solves for collision</summary>
		protected virtual void Update()
		{
			if ( _camera == null )
			{
				enabled = false;
			}
			else
			{
				// Rebuild screen corners
				bool tempIsCameraChanged = _camera.transform.position != lastCameraPosition || _camera.projectionMatrix != lastCameraProjectionMatrix;
				if ( tempIsCameraChanged || transform.position.z != lastPosition.z )
				{
					if ( tempIsCameraChanged )
					{
						lastCameraPosition = _camera.transform.position;
						lastCameraProjectionMatrix = _camera.projectionMatrix;
					}
					lastPosition.z = transform.position.z;
					updateScreenCorners();
					tempIsCameraChanged = true;
				}
				
				// Check collision
				bool tempIsCollider = collider != null && collider.enabled;
				if ( tempIsCameraChanged || ( tempIsCollider && ( collider.transform.position.x != lastPosition.x || collider.transform.position.y != lastPosition.y ) ) || ( !tempIsCollider && ( transform.position.x != lastPosition.x || transform.position.y != lastPosition.y ) ) )
				{
					if ( tempIsCollider )
					{
						updateCollisionCollider();
						lastPosition.x = collider.transform.position.x;
						lastPosition.y = collider.transform.position.y;
					}
					else
					{
						updateCollisionPoint();
						lastPosition = transform.position;
					}
				}
			}
		}
		
		/// <summary>Rebuilds projected screen corner locations in world space</summary>
		protected virtual void updateScreenCorners()
		{
			if ( _camera != null )
			{
				float tempDistance = transform.position.z - _camera.transform.position.z;
				if ( _isOffsetInPixels )
				{
					worldBottomLeft = _camera.ScreenToWorldPoint( new Vector3( _offsetBottomLeft.x, _offsetBottomLeft.y, tempDistance ) );
					worldTopRight = _camera.ScreenToWorldPoint( new Vector3( ( Screen.width - _offsetTopRight.x ), ( Screen.height - _offsetTopRight.y ), tempDistance ) );
				}
				else
				{
					worldBottomLeft = _camera.ViewportToWorldPoint( new Vector3( _offsetBottomLeft.x, _offsetBottomLeft.y, tempDistance ) );
					worldTopRight = _camera.ViewportToWorldPoint( new Vector3( ( 1 - _offsetTopRight.x ), ( 1 - _offsetTopRight.y ), tempDistance ) );
				}
			}
		}
		
		/// <summary>Solves collision using a single point against the screen corners</summary>
		public virtual void updateCollisionPoint()
		{
			// No target
			Vector3 tempPosition = transform.position;
			if ( defaultTarget == null )
			{
				if ( tempPosition.x < worldBottomLeft.x )
				{
					tempPosition.x = worldBottomLeft.x;
				}
				else if ( tempPosition.x > worldTopRight.x )
				{
					tempPosition.x = worldTopRight.x;
				}
				
				if ( tempPosition.y < worldBottomLeft.y )
				{
					tempPosition.y = worldBottomLeft.y;
				}
				else if ( tempPosition.y > worldTopRight.y )
				{
					tempPosition.y = worldTopRight.y;
				}
			}
			// Try to move towards target
			else
			{
				if ( defaultTarget.position.x < worldBottomLeft.x )
				{
					tempPosition.x = worldBottomLeft.x;
				}
				else if ( defaultTarget.position.x > worldTopRight.x )
				{
					tempPosition.x = worldTopRight.x;
				}
				else
				{
					tempPosition.x = defaultTarget.position.x;
				}
				
				if ( defaultTarget.position.y < worldBottomLeft.y )
				{
					tempPosition.y = worldBottomLeft.x;
				}
				else if ( defaultTarget.position.y > worldTopRight.y )
				{
					tempPosition.y = worldTopRight.y;
				}
				else
				{
					tempPosition.y = defaultTarget.position.y;
				}
			}
			
			// Apply
			if ( tempPosition != transform.position )
			{
				transform.position = tempPosition;
			}
		}
		
		/// <summary>Solves collision using the bounds of the <see cref="collider"/> against the screen corners</summary>
		public virtual void updateCollisionCollider()
		{
			if ( collider != null )
			{
				// No target
				Vector3 tempPosition = transform.position;
				if ( defaultTarget == null )
				{
					if ( collider.bounds.min.x < worldBottomLeft.x )
					{
						tempPosition.x = worldBottomLeft.x + transform.position.x - collider.bounds.min.x;
					}
					else if ( collider.bounds.max.x > worldTopRight.x )
					{
						tempPosition.x = worldTopRight.x + transform.position.x - collider.bounds.max.x;
					}
					
					if ( collider.bounds.min.y < worldBottomLeft.y )
					{
						tempPosition.y = worldBottomLeft.y + transform.position.y - collider.bounds.min.y;
					}
					else if ( collider.bounds.max.y > worldTopRight.y )
					{
						tempPosition.y = worldTopRight.y + transform.position.y - collider.bounds.max.y;
					}
				}
				// Try to move towards target
				else
				{
					if ( ( defaultTarget.position.x - transform.position.x + collider.bounds.min.x ) < worldBottomLeft.x )
					{
						tempPosition.x = worldBottomLeft.x + transform.position.x - collider.bounds.min.x;
					}
					else if ( ( defaultTarget.position.x - transform.position.x + collider.bounds.max.x ) > worldTopRight.x )
					{
						tempPosition.x = worldTopRight.x + transform.position.x - collider.bounds.max.x;
					}
					else
					{
						tempPosition.x = defaultTarget.position.x;
					}
					
					if ( ( defaultTarget.position.y - transform.position.y + collider.bounds.min.y ) < worldBottomLeft.y )
					{
						tempPosition.y = worldBottomLeft.y + transform.position.y - collider.bounds.min.y;
					}
					else if ( ( defaultTarget.position.y - transform.position.y + collider.bounds.max.y ) > worldTopRight.y )
					{
						tempPosition.y = worldTopRight.y + transform.position.y - collider.bounds.max.y;
					}
					else
					{
						tempPosition.y = defaultTarget.position.y;
					}
				}
				
				// Apply
				if ( tempPosition != transform.position )
				{
					transform.position = tempPosition;
				}
			}
		}
		
		//=======================
		// Camera
		//=======================
		public virtual Camera camera
		{
			get
			{
				return _camera;
			}
			set
			{
				setCamera( value );
			}
		}
		
		/// <summary>Sets the camera</summary>
		/// <param name="tCamera">New camera</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setCamera( Camera tCamera )
		{
			if ( tCamera != _camera )
			{
				Camera tempOld = _camera;
				_camera = tCamera;
				effectsCamera( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the screen corners and attempt to enable</summary>
		/// <param name="tOld">Previous camera</param>
		protected virtual void effectsCamera( Camera tOld )
		{
			if ( _camera != null )
			{
				updateScreenCorners();
				lastCameraPosition = _camera.transform.position;
				lastCameraProjectionMatrix = _camera.projectionMatrix;
			}
			
			if ( _camera != tOld )
			{
				enable();
			}
		}
		
		//=======================
		// Offsets
		//=======================
		public virtual Vector2 offsetBottomLeft
		{
			get
			{
				return _offsetBottomLeft;
			}
			set
			{
				setOffsetBottomLeft( value );
			}
		}
		
		/// <summary>Sets the bottom-left offset</summary>
		/// <param name="tOffset">New offset</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setOffsetBottomLeft( Vector2 tOffset )
		{
			if ( tOffset != _offsetBottomLeft )
			{
				Vector3 tempOld = _offsetBottomLeft;
				_offsetBottomLeft = tOffset;
				effectsOffsetBottomLeft( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the screen corners</summary>
		/// <param name="tOld">Previous offset</param>
		protected virtual void effectsOffsetBottomLeft( Vector2 tOld )
		{
			updateScreenCorners();
		}
		
		public virtual Vector2 offsetTopRight
		{
			get
			{
				return _offsetTopRight;
			}
			set
			{
				setOffsetTopRight( value );
			}
		}
		
		/// <summary>Sets the top-right offset</summary>
		/// <param name="tOffset">New offset</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setOffsetTopRight( Vector2 tOffset )
		{
			if ( tOffset != _offsetTopRight )
			{
				Vector3 tempOld = _offsetTopRight;
				_offsetTopRight = tOffset;
				effectsOffsetTopRight( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the screen corners</summary>
		/// <param name="tOld">Previous offset</param>
		protected virtual void effectsOffsetTopRight( Vector2 tOld )
		{
			updateScreenCorners();
		}
		
		public virtual bool isOffsetInPixels
		{
			get
			{
				return _isOffsetInPixels;
			}
			set
			{
				setOffsetInPixels( value );
			}
		}
		
		/// <summary>Toggles if offsets should be measured in pixels or normalized screen space</summary>
		/// <param name="tIsPixels">True for pixel space</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setOffsetInPixels( bool tIsPixels )
		{
			if ( tIsPixels != _isOffsetInPixels )
			{
				_isOffsetInPixels = tIsPixels;
				effectsOffsetInPixels( !_isOffsetInPixels );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the screen corners</summary>
		/// <param name="tOld">Previous setting</param>
		protected virtual void effectsOffsetInPixels( bool tOld )
		{
			updateScreenCorners();
		}
	}
}