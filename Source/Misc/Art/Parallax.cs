using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Parallaxes this object along the local X and Y axes relative to a target</summary>
	[AddComponentMenu( "PeenToys/Art/Parallax" )]
	public class Parallax : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Target transform for parallaxing relative to</summary>
		[SerializeField]
		protected Transform _target;
		/// <summary>Previous target position used for optimization</summary>
		[NonSerialized]
		public Vector2 lastTargetPosition;
		/// <summary>Parallax speed along the local X axis</summary>
		[SerializeField]
		protected float _speedX;
		/// <summary>Parallax speed along the local Y axis</summary>
		[SerializeField]
		protected float _speedY;
		/// <summary>Original local position of this transform</summary>
		protected Vector2 _origin;
		/// <summary>Original local difference between target and this transform</summary>
		protected Vector2 _targetOffset;
	
		//=======================
		// Initialization
		//=======================
		/// <summary>Sets the initial <see cref="_origin"/> value</summary>
		protected virtual void Awake()
		{
			_origin = transform.localPosition;
		}
		
		/// <summary>Initializes the target and pre-applies the parallax</summary>
		protected virtual void Start()
		{
			effectsTarget( _target );
			effectsSpeedX( _speedX );
		}
		
		//=======================
		// Tick
		//=======================
		/// <summary>Tries to enable only if the <see cref="_target"/> is set and speeds are not 0</summary>
		public virtual bool enable()
		{
			enabled = _target != null && ( _speedX != 0 || _speedY != 0 );
			return enabled;
		}
		
		/// <summary>Updates the parallax position if the <see cref="_target"/> moved</summary>
		protected virtual void Update()
		{
			if ( _target == null )
			{
				enabled = false;
			}
			else
			{
				Vector2 tempPosition = transform.parent == null ? (Vector2)_target.position : (Vector2)transform.parent.InverseTransformPoint( _target.position );
				if ( tempPosition != lastTargetPosition )
				{
					lastTargetPosition = tempPosition;
					updatePosition();
				}
			}
		}
		
		/// <summary>Calculates and applies the parallax position</summary>
		protected virtual void updatePosition()
		{
			if ( _target != null )
			{
				Vector3 tempPosition = new Vector3( _origin.x, _origin.y, transform.localPosition.z );
				
				if ( _speedX != 0 )
				{
					tempPosition.x += _speedX * ( _origin.x - lastTargetPosition.x + _targetOffset.x );
				}
				if ( _speedY != 0 )
				{
					tempPosition.y += _speedY * ( _origin.y - lastTargetPosition.y + _targetOffset.y );
				}
				
				transform.localPosition = tempPosition;
			}
		}
		
		//=======================
		// Target
		//=======================
		public virtual Transform target
		{
			get
			{
				return _target;
			}
			set
			{
				setTarget( value );
			}
		}
		
		/// <summary>Sets the relative target</summary>
		/// <param name="tTarget">New target</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setTarget( Transform tTarget )
		{
			if ( tTarget != _target )
			{
				Transform tempOld = _target;
				_target = tTarget;
				effectsTarget( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Sets the offset, tries to enable the parallaxing tick</summary>
		/// <param name="tOld">Previous target</param>
		protected virtual void effectsTarget( Transform tOld )
		{
			if ( _target == null )
			{
				lastTargetPosition = Vector2.zero;
			}
			else
			{
				lastTargetPosition = transform.parent == null ? (Vector2)_target.position : (Vector2)transform.parent.InverseTransformPoint( _target.position );
				setTargetOffset( lastTargetPosition - _origin );
			}
			
			if ( _target != tOld )
			{
				enable();
			}
		}
		
		//=======================
		// Origin
		//=======================
		public virtual Vector2 origin
		{
			get
			{
				return _origin;
			}
			set
			{
				setOrigin( value );
			}
		}
		
		/// <summary>Sets the local origin point</summary>
		/// <param name="tOrigin">New origin point</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setOrigin( Vector2 tOrigin )
		{
			if ( tOrigin != _origin )
			{
				Vector2 tempOld = _origin;
				_origin = tOrigin;
				effectsOrigin( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the parallax position</summary>
		/// <param name="tOld">Previous origin point</param>
		protected virtual void effectsOrigin( Vector2 tOld )
		{
			updatePosition();
		}
		
		//=======================
		// Target Offset
		//=======================
		public virtual Vector2 targetOffset
		{
			get
			{
				return _targetOffset;
			}
			set
			{
				setTargetOffset( value );
			}
		}
		
		/// <summary>Sets the local target offset</summary>
		/// <param name="tOffset">New target offset</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setTargetOffset( Vector2 tOffset )
		{
			if ( tOffset != _targetOffset )
			{
				Vector2 tempOld = _targetOffset;
				_targetOffset = tOffset;
				effectsTargetOffset( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the parallax position</summary>
		/// <param name="tOld">Previous target offset</param>
		protected virtual void effectsTargetOffset( Vector2 tOld )
		{
			updatePosition();
		}
		
		//=======================
		// Speed
		//=======================
		public virtual float speedX
		{
			get
			{
				return _speedX;
			}
			set
			{
				setSpeedX( value );
			}
		}
		
		/// <summary>Sets the X parallax speed</summary>
		/// <param name="tSpeed">New X speed</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setSpeedX( float tSpeed )
		{
			if ( tSpeed != _speedX )
			{
				float tempOld = _speedX;
				_speedX = tSpeed;
				effectsSpeedX( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the parallax position</summary>
		/// <param name="tOld">Previous target offset</param>
		protected virtual void effectsSpeedX( float tOld )
		{
			updatePosition();
		}
		
		public virtual float speedY
		{
			get
			{
				return _speedY;
			}
			set
			{
				setSpeedY( value );
			}
		}
		
		/// <summary>Sets the Y parallax speed</summary>
		/// <param name="tSpeed">New Y speed</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setSpeedY( float tSpeed )
		{
			if ( tSpeed != _speedY )
			{
				float tempOld = _speedY;
				_speedY = tSpeed;
				effectsSpeedY( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Updates the parallax position</summary>
		/// <param name="tOld">Previous target offset</param>
		protected virtual void effectsSpeedY( float tOld )
		{
			updatePosition();
		}
	}
}