using System;
using UnityEngine;

namespace PeenTween
{
	//##########################
	// Class Declaration
	//##########################
	public class TweenVector3Transform : TweenVector3
	{
		//=======================
		// Variables
		//=======================
		public Transform transform;
		public TransformMode mode;
		
		//=======================
		// Constructor
		//=======================
		public TweenVector3Transform( Transform tTransform, float tDuration, Vector3 tFrom, Vector3 tTo ) : base( tDuration, tFrom, tTo )
		{
			transform = tTransform;
		}
		
		//=======================
		// Tween
		//=======================
		public override TweenTick tick( float tDeltaTime )
		{
			// Apply Transforms
			switch ( base.tick( tDeltaTime ) )
			{
				case TweenTick.Ticked:
					if ( transform != null )
					{
						switch ( mode )
						{
							case TransformMode.Move:
								transform.position = result;
								break;
							case TransformMode.MoveLocal:
								transform.localPosition = result;
								break;
							case TransformMode.Rotate:
								transform.rotation = Quaternion.Euler( result );
								break;
							case TransformMode.RotateLocal:
								transform.localRotation = Quaternion.Euler( result );
								break;
							default: // scale
								transform.localScale = result;
								break;
						}
					}
					return TweenTick.Ticked;
				case TweenTick.Completed:
					if ( transform != null )
					{
						switch ( mode )
						{
							case TransformMode.Move:
								transform.position = to;
								break;
							case TransformMode.MoveLocal:
								transform.localPosition = to;
								break;
							case TransformMode.Rotate:
								transform.rotation = Quaternion.Euler( to );
								break;
							case TransformMode.RotateLocal:
								transform.localRotation = Quaternion.Euler( to );
								break;
							default: // scale
								transform.localScale = to;
								break;
						}
					}
					return TweenTick.Completed;
				default:
					break;
			}
			
			return TweenTick.Ticking;
		}
	}
}