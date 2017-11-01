using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Pans the attached object towards a target</summary>
	[AddComponentMenu( "PeenToys/Art/Panner" )]
	public class Panner : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Target transform to pan towards</summary>
		[SerializeField]
		protected Transform _target;
		/// <summary>Offset position from the target</summary>
		public Vector3 offset;
		/// <summary><see cref="UnityEngine.AnimationCurve"/> for creating an acceleration effect to pan faster when the further away from the target</summary>
		public AnimationCurve speedPerDistance = AnimationCurve.Linear( 0, 1, 1, 1 );
	
		//=======================
		// Initialization
		//=======================
		/// <summary>Initializes the target</summary>
		protected virtual void Start()
		{
			effectsTarget( _target );
		}
		
		//=======================
		// Tick
		//=======================
		/// <summary>Tries to enable only if the <see cref="_target"/> is set</summary>
		public virtual bool enable()
		{
			enabled = _target != null;
			return enabled;
		}
		
		/// <summary>Moves the panner towards the target</summary>
		protected virtual void Update()
		{
			if ( _target == null )
			{
				enabled = false;
			}
			else
			{
				Vector3 tempTarget = panTarget;
				Vector3 tempDifference = tempTarget - panPosition;
				if ( tempDifference != Vector3.zero )
				{
					Vector3 tempVelocity = tempDifference.normalized * Time.deltaTime;
					if ( speedPerDistance != null )
					{
						tempVelocity *= speedPerDistance.Evaluate( tempDifference.magnitude );
					}

					if ( tempVelocity.magnitude >= tempDifference.magnitude )
					{
						transform.position = tempTarget;
					}
					else
					{
						transform.position += tempVelocity;
					}
				}
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
		
		/// <summary>Sets the target</summary>
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
		
		/// <summary>Tries to enable the panning tick</summary>
		protected virtual void effectsTarget( Transform tOld )
		{
			enable();
		}
		
		/// <summary>Target vector for the panner</summary>
		public virtual Vector3 panTarget
		{
			get
			{
				return _target == null ? offset : _target.position + offset;
			}
		}
		
		/// <summary>Position of the panner</summary>
		public virtual Vector3 panPosition
		{
			get
			{
				return transform.position;
			}
		}
	}
}