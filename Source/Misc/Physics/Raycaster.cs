using UnityEngine;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Raycasts (relative to this transform's forward) against 3D colliders and checks if hitting when ticked</summary>
	[AddComponentMenu( "PeenToys/Physics/Raycaster" )]
	public class Raycaster : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Layers to raycast against</summary>
		public LayerMask layers;
		/// <summary>Maximum distance of the raycast</summary>
		public float castDistance = 1000;
		/// <summary>Whether a raycast hit during the tick</summary>
		protected bool _isHit;
		/// <summary><see cref="UnityEngine.RaycastHit"/> data during the tick</summary>
		[NonSerialized]
		public RaycastHit hitData;
	
		//=======================
		// Tick
		//=======================		
		/// <summary>Raycasts each frame to check hit status (disabled this component if not used)</summary>
		protected virtual void Update()
		{
			setHit( Physics.Raycast( transform.position, transform.forward, out hitData, castDistance, layers ) );
		}
		
		//=======================
		// Hit
		//=======================
		public virtual bool isHit
		{
			get
			{
				return _isHit;
			}
			set
			{
				setHit( value );
			}
		}
		
		/// <summary>Sets the hit state</summary>
		/// <param name="tIsHit">Hit state to set to</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setHit( bool tIsHit )
		{
			if ( tIsHit != _isHit )
			{
				_isHit = tIsHit;
				
				return true;
			}
			
			return false;
		}
	
		//=======================
		// Raycast
		//=======================
		/// <summary>Performs raycast using a start vector offset</summary>
		/// <param name="tStart">Offset vector that gets converted into this transform's coordinate space (drops the Z)</param>
		/// <param name="tHit">Output <see cref="UnityEngine.RaycastHit"/></param>
		/// <returns>True if successful raycast hit</summary>
		public virtual bool raycast( Vector3 tStart, out RaycastHit tHit )
		{
			tStart = transform.InverseTransformPoint( tStart );
			tStart.z = 0;
			
			if ( Physics.Raycast( transform.TransformPoint( tStart ), transform.forward, out tHit, castDistance, layers ) )
			{
				return true;
			}
			
			return false;
		}
		
		/// <summary>Returns a normalized depth value with 1 considered to be at the origin and 0 at <see cref="castDistance"/></summary>
		/// <param name="tDistance">Hit distance, assumed to be less than <see cref="castDistance"/></param>
		/// <returns>Normalized value of <paramref name="tDistance"/></summary>
		public virtual float getNormalizedDepth( float tDistance )
		{
			return 1 - ( tDistance / castDistance );
		}
		
		/// <summary>Tries to perform a raycast and positions a <paramref name="tTransform"/> to the hit location</summary>
		/// <param name="tTransform">Transform to position</param>
		/// <returns>True if successful</summary>
		public virtual bool ground( Transform tTransform )
		{
			if ( tTransform != null )
			{
				RaycastHit tempHit;
				if ( raycast( tTransform.position, out tempHit ) )
				{
					tTransform.position = tempHit.point;
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>Tries to perform a raycasts for multiple <paramref name="tTransform"/>s and moves them to their respective hit locations</summary>
		/// <param name="tTransforms">Transforms to position</param>
		public virtual void ground( Transform[] tTransforms )
		{
			if ( tTransforms != null )
			{
				for ( int i = ( tTransforms.Length - 1 ); i >= 0; --i )
				{
					if ( tTransforms[i] != transform )
					{
						ground( tTransforms[i] );
					}
				}
			}
		}
	}
}