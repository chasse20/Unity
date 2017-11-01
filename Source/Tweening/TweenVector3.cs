using System;
using UnityEngine;

namespace PeenTween
{
	//##########################
	// Class Declaration
	//##########################
	public class TweenVector3 : Tween<Vector3>
	{		
		//=======================
		// Constructor
		//=======================
		public TweenVector3( float tDuration, Vector3 tFrom, Vector3 tTo ) : base( tDuration, tFrom, tTo )
		{
		}
		
		//=======================
		// Tween
		//=======================
		public override Vector3 result
		{
			get
			{
				if ( elapsed > 0 )
				{
					switch ( easing )
					{
						case EasingType.ElasticIn:
							return new Vector3( Easing.ElasticIn( elapsed, duration, from.x, to.x ), Easing.ElasticIn( elapsed, duration, from.y, to.y ), Easing.ElasticIn( elapsed, duration, from.z, to.z ) );
						case EasingType.ElasticOut:
							return new Vector3( Easing.ElasticOut( elapsed, duration, from.x, to.x ), Easing.ElasticOut( elapsed, duration, from.y, to.y ), Easing.ElasticOut( elapsed, duration, from.z, to.z ) );
						case EasingType.ElasticInOut:
							return new Vector3( Easing.ElasticInOut( elapsed, duration, from.x, to.x ), Easing.ElasticInOut( elapsed, duration, from.y, to.y ), Easing.ElasticInOut( elapsed, duration, from.z, to.z ) );
						default:
							float tempProgress = elapsed / duration;
							if ( tempProgress < 1 )
							{
								switch ( easing )
								{
									case EasingType.Linear:
										return new Vector3( Easing.Linear( tempProgress, from.x, to.x ), Easing.Linear( tempProgress, from.y, to.y ), Easing.Linear( tempProgress, from.z, to.z ) );
									case EasingType.QuadraticIn:
										return new Vector3( Easing.QuadraticIn( tempProgress, from.x, to.x ), Easing.QuadraticIn( tempProgress, from.y, to.y ), Easing.QuadraticIn( tempProgress, from.z, to.z ) );
									case EasingType.QuadraticOut:
										return new Vector3( Easing.QuadraticOut( tempProgress, from.x, to.x ), Easing.QuadraticOut( tempProgress, from.y, to.y ), Easing.QuadraticOut( tempProgress, from.z, to.z ) );
									case EasingType.QuadraticInOut:
										return new Vector3( Easing.QuadraticInOut( tempProgress, from.x, to.x ), Easing.QuadraticInOut( tempProgress, from.y, to.y ), Easing.QuadraticInOut( tempProgress, from.z, to.z ) );
									case EasingType.CubicIn:
										return new Vector3( Easing.CubicIn( tempProgress, from.x, to.x ), Easing.CubicIn( tempProgress, from.y, to.y ), Easing.CubicIn( tempProgress, from.z, to.z ) );
									case EasingType.CubicOut:
										return new Vector3( Easing.CubicOut( tempProgress, from.x, to.x ), Easing.CubicOut( tempProgress, from.y, to.y ), Easing.CubicOut( tempProgress, from.z, to.z ) );
									case EasingType.CubicInOut:
										return new Vector3( Easing.CubicInOut( tempProgress, from.x, to.x ), Easing.CubicInOut( tempProgress, from.y, to.y ), Easing.CubicInOut( tempProgress, from.z, to.z ) );
									case EasingType.QuarticIn:
										return new Vector3( Easing.QuarticIn( tempProgress, from.x, to.x ), Easing.QuarticIn( tempProgress, from.y, to.y ), Easing.QuarticIn( tempProgress, from.z, to.z ) );
									case EasingType.QuarticOut:
										return new Vector3( Easing.QuarticOut( tempProgress, from.x, to.x ), Easing.QuarticOut( tempProgress, from.y, to.y ), Easing.QuarticOut( tempProgress, from.z, to.z ) );
									case EasingType.QuarticInOut:
										return new Vector3( Easing.QuarticInOut( tempProgress, from.x, to.x ), Easing.QuarticInOut( tempProgress, from.y, to.y ), Easing.QuarticInOut( tempProgress, from.z, to.z ) );
									case EasingType.QuinticIn:
										return new Vector3( Easing.QuinticIn( tempProgress, from.x, to.x ), Easing.QuinticIn( tempProgress, from.y, to.y ), Easing.QuinticIn( tempProgress, from.z, to.z ) );
									case EasingType.QuinticOut:
										return new Vector3( Easing.QuinticOut( tempProgress, from.x, to.x ), Easing.QuinticOut( tempProgress, from.y, to.y ), Easing.QuinticOut( tempProgress, from.z, to.z ) );
									case EasingType.QuinticInOut:
										return new Vector3( Easing.QuinticInOut( tempProgress, from.x, to.x ), Easing.QuinticInOut( tempProgress, from.y, to.y ), Easing.QuinticInOut( tempProgress, from.z, to.z ) );
									case EasingType.SineIn:
										return new Vector3( Easing.SineIn( tempProgress, from.x, to.x ), Easing.SineIn( tempProgress, from.y, to.y ), Easing.SineIn( tempProgress, from.z, to.z ) );
									case EasingType.SineOut:
										return new Vector3( Easing.SineOut( tempProgress, from.x, to.x ), Easing.SineOut( tempProgress, from.y, to.y ), Easing.SineOut( tempProgress, from.z, to.z ) );
									case EasingType.SineInOut:
										return new Vector3( Easing.SineInOut( tempProgress, from.x, to.x ), Easing.SineInOut( tempProgress, from.y, to.y ), Easing.SineInOut( tempProgress, from.z, to.z ) );
									case EasingType.ExponentialIn:
										return new Vector3( Easing.ExponentialIn( tempProgress, from.x, to.x ), Easing.ExponentialIn( tempProgress, from.y, to.y ), Easing.ExponentialIn( tempProgress, from.z, to.z ) );
									case EasingType.ExponentialOut:
										return new Vector3( Easing.ExponentialOut( tempProgress, from.x, to.x ), Easing.ExponentialOut( tempProgress, from.y, to.y ), Easing.ExponentialOut( tempProgress, from.z, to.z ) );
									case EasingType.ExponentialInOut:
										return new Vector3( Easing.ExponentialInOut( tempProgress, from.x, to.x ), Easing.ExponentialInOut( tempProgress, from.y, to.y ), Easing.ExponentialInOut( tempProgress, from.z, to.z ) );
									case EasingType.CircularIn:
										return new Vector3( Easing.CircularIn( tempProgress, from.x, to.x ), Easing.CircularIn( tempProgress, from.y, to.y ), Easing.CircularIn( tempProgress, from.z, to.z ) );
									case EasingType.CircularOut:
										return new Vector3( Easing.CircularOut( tempProgress, from.x, to.x ), Easing.CircularOut( tempProgress, from.y, to.y ), Easing.CircularOut( tempProgress, from.z, to.z ) );
									case EasingType.CircularInOut:
										return new Vector3( Easing.CircularInOut( tempProgress, from.x, to.x ), Easing.CircularInOut( tempProgress, from.y, to.y ), Easing.CircularInOut( tempProgress, from.z, to.z ) );
									case EasingType.BackIn:
										return new Vector3( Easing.BackIn( tempProgress, from.x, to.x ), Easing.BackIn( tempProgress, from.y, to.y ), Easing.BackIn( tempProgress, from.z, to.z ) );
									case EasingType.BackOut:
										return new Vector3( Easing.BackOut( tempProgress, from.x, to.x ), Easing.BackOut( tempProgress, from.y, to.y ), Easing.BackOut( tempProgress, from.z, to.z ) );
									case EasingType.BackInOut:
										return new Vector3( Easing.BackInOut( tempProgress, from.x, to.x ), Easing.BackInOut( tempProgress, from.y, to.y ), Easing.BackInOut( tempProgress, from.z, to.z ) );
									case EasingType.BounceIn:
										return new Vector3( Easing.BounceIn( tempProgress, from.x, to.x ), Easing.BounceIn( tempProgress, from.y, to.y ), Easing.BounceIn( tempProgress, from.z, to.z ) );
									case EasingType.BounceOut:
										return new Vector3( Easing.BounceOut( tempProgress, from.x, to.x ), Easing.BounceOut( tempProgress, from.y, to.y ), Easing.BounceOut( tempProgress, from.z, to.z ) );
									case EasingType.BounceInOut:
										return new Vector3( Easing.BounceInOut( tempProgress, from.x, to.x ), Easing.BounceInOut( tempProgress, from.y, to.y ), Easing.BounceInOut( tempProgress, from.z, to.z ) );
									default:
										break;
								}
							}
							
							return to;
					}
				}
				
				return from;
			}
		}
	}
}