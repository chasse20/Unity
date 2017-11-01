using System;
using UnityEngine;

namespace PeenTween
{
	//##########################
	// Class Declaration
	//##########################
	public class TweenVector2 : Tween<Vector2>
	{		
		//=======================
		// Constructor
		//=======================
		public TweenVector2( float tDuration, Vector2 tFrom, Vector2 tTo ) : base( tDuration, tFrom, tTo )
		{
		}
		
		//=======================
		// Tween
		//=======================
		public override Vector2 result
		{
			get
			{
				if ( elapsed > 0 )
				{
					switch ( easing )
					{
						case EasingType.ElasticIn:
							return new Vector2( Easing.ElasticIn( elapsed, duration, from.x, to.x ), Easing.ElasticIn( elapsed, duration, from.y, to.y ) );
						case EasingType.ElasticOut:
							return new Vector2( Easing.ElasticOut( elapsed, duration, from.x, to.x ), Easing.ElasticOut( elapsed, duration, from.y, to.y ) );
						case EasingType.ElasticInOut:
							return new Vector2( Easing.ElasticInOut( elapsed, duration, from.x, to.x ), Easing.ElasticInOut( elapsed, duration, from.y, to.y ) );
						default:
							float tempProgress = elapsed / duration;
							if ( tempProgress < 1 )
							{
								switch ( easing )
								{
									case EasingType.Linear:
										return new Vector2( Easing.Linear( tempProgress, from.x, to.x ), Easing.Linear( tempProgress, from.y, to.y ) );
									case EasingType.QuadraticIn:
										return new Vector2( Easing.QuadraticIn( tempProgress, from.x, to.x ), Easing.QuadraticIn( tempProgress, from.y, to.y ) );
									case EasingType.QuadraticOut:
										return new Vector2( Easing.QuadraticOut( tempProgress, from.x, to.x ), Easing.QuadraticOut( tempProgress, from.y, to.y ) );
									case EasingType.QuadraticInOut:
										return new Vector2( Easing.QuadraticInOut( tempProgress, from.x, to.x ), Easing.QuadraticInOut( tempProgress, from.y, to.y ) );
									case EasingType.CubicIn:
										return new Vector2( Easing.CubicIn( tempProgress, from.x, to.x ), Easing.CubicIn( tempProgress, from.y, to.y ) );
									case EasingType.CubicOut:
										return new Vector2( Easing.CubicOut( tempProgress, from.x, to.x ), Easing.CubicOut( tempProgress, from.y, to.y ) );
									case EasingType.CubicInOut:
										return new Vector2( Easing.CubicInOut( tempProgress, from.x, to.x ), Easing.CubicInOut( tempProgress, from.y, to.y ) );
									case EasingType.QuarticIn:
										return new Vector2( Easing.QuarticIn( tempProgress, from.x, to.x ), Easing.QuarticIn( tempProgress, from.y, to.y ) );
									case EasingType.QuarticOut:
										return new Vector2( Easing.QuarticOut( tempProgress, from.x, to.x ), Easing.QuarticOut( tempProgress, from.y, to.y ) );
									case EasingType.QuarticInOut:
										return new Vector2( Easing.QuarticInOut( tempProgress, from.x, to.x ), Easing.QuarticInOut( tempProgress, from.y, to.y ) );
									case EasingType.QuinticIn:
										return new Vector2( Easing.QuinticIn( tempProgress, from.x, to.x ), Easing.QuinticIn( tempProgress, from.y, to.y ) );
									case EasingType.QuinticOut:
										return new Vector2( Easing.QuinticOut( tempProgress, from.x, to.x ), Easing.QuinticOut( tempProgress, from.y, to.y ) );
									case EasingType.QuinticInOut:
										return new Vector2( Easing.QuinticInOut( tempProgress, from.x, to.x ), Easing.QuinticInOut( tempProgress, from.y, to.y ) );
									case EasingType.SineIn:
										return new Vector2( Easing.SineIn( tempProgress, from.x, to.x ), Easing.SineIn( tempProgress, from.y, to.y ) );
									case EasingType.SineOut:
										return new Vector2( Easing.SineOut( tempProgress, from.x, to.x ), Easing.SineOut( tempProgress, from.y, to.y ) );
									case EasingType.SineInOut:
										return new Vector2( Easing.SineInOut( tempProgress, from.x, to.x ), Easing.SineInOut( tempProgress, from.y, to.y ) );
									case EasingType.ExponentialIn:
										return new Vector2( Easing.ExponentialIn( tempProgress, from.x, to.x ), Easing.ExponentialIn( tempProgress, from.y, to.y ) );
									case EasingType.ExponentialOut:
										return new Vector2( Easing.ExponentialOut( tempProgress, from.x, to.x ), Easing.ExponentialOut( tempProgress, from.y, to.y ) );
									case EasingType.ExponentialInOut:
										return new Vector2( Easing.ExponentialInOut( tempProgress, from.x, to.x ), Easing.ExponentialInOut( tempProgress, from.y, to.y ) );
									case EasingType.CircularIn:
										return new Vector2( Easing.CircularIn( tempProgress, from.x, to.x ), Easing.CircularIn( tempProgress, from.y, to.y ) );
									case EasingType.CircularOut:
										return new Vector2( Easing.CircularOut( tempProgress, from.x, to.x ), Easing.CircularOut( tempProgress, from.y, to.y ) );
									case EasingType.CircularInOut:
										return new Vector2( Easing.CircularInOut( tempProgress, from.x, to.x ), Easing.CircularInOut( tempProgress, from.y, to.y ) );
									case EasingType.BackIn:
										return new Vector2( Easing.BackIn( tempProgress, from.x, to.x ), Easing.BackIn( tempProgress, from.y, to.y ) );
									case EasingType.BackOut:
										return new Vector2( Easing.BackOut( tempProgress, from.x, to.x ), Easing.BackOut( tempProgress, from.y, to.y ) );
									case EasingType.BackInOut:
										return new Vector2( Easing.BackInOut( tempProgress, from.x, to.x ), Easing.BackInOut( tempProgress, from.y, to.y ) );
									case EasingType.BounceIn:
										return new Vector2( Easing.BounceIn( tempProgress, from.x, to.x ), Easing.BounceIn( tempProgress, from.y, to.y ) );
									case EasingType.BounceOut:
										return new Vector2( Easing.BounceOut( tempProgress, from.x, to.x ), Easing.BounceOut( tempProgress, from.y, to.y ) );
									case EasingType.BounceInOut:
										return new Vector2( Easing.BounceInOut( tempProgress, from.x, to.x ), Easing.BounceInOut( tempProgress, from.y, to.y ) );
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