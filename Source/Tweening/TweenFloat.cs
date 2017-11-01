using System;
using UnityEngine;

namespace PeenTween
{
	//##########################
	// Class Declaration
	//##########################
	public class TweenFloat : Tween<float>
	{		
		//=======================
		// Constructor
		//=======================
		public TweenFloat( float tDuration, float tFrom, float tTo ) : base( tDuration, tFrom, tTo )
		{
		}
		
		//=======================
		// Tween
		//=======================
		public override float result
		{
			get
			{
				if ( elapsed > 0 )
				{
					switch ( easing )
					{
						case EasingType.ElasticIn:
							return Easing.ElasticIn( elapsed, duration, from, to );
						case EasingType.ElasticOut:
							return Easing.ElasticOut( elapsed, duration, from, to );
						case EasingType.ElasticInOut:
							return Easing.ElasticInOut( elapsed, duration, from, to );
						default:
							float tempProgress = elapsed / duration;
							if ( tempProgress < 1 )
							{
								switch ( easing )
								{
									case EasingType.Linear:
										return Easing.Linear( tempProgress, from, to );
									case EasingType.QuadraticIn:
										return Easing.QuadraticIn( tempProgress, from, to );
									case EasingType.QuadraticOut:
										return Easing.QuadraticOut( tempProgress, from, to );
									case EasingType.QuadraticInOut:
										return Easing.QuadraticInOut( tempProgress, from, to );
									case EasingType.CubicIn:
										return Easing.CubicIn( tempProgress, from, to );
									case EasingType.CubicOut:
										return Easing.CubicOut( tempProgress, from, to );
									case EasingType.CubicInOut:
										return Easing.CubicInOut( tempProgress, from, to );
									case EasingType.QuarticIn:
										return Easing.QuarticIn( tempProgress, from, to );
									case EasingType.QuarticOut:
										return Easing.QuarticOut( tempProgress, from, to );
									case EasingType.QuarticInOut:
										return Easing.QuarticInOut( tempProgress, from, to );
									case EasingType.QuinticIn:
										return Easing.QuinticIn( tempProgress, from, to );
									case EasingType.QuinticOut:
										return Easing.QuinticOut( tempProgress, from, to );
									case EasingType.QuinticInOut:
										return Easing.QuinticInOut( tempProgress, from, to );
									case EasingType.SineIn:
										return Easing.SineIn( tempProgress, from, to );
									case EasingType.SineOut:
										return Easing.SineOut( tempProgress, from, to );
									case EasingType.SineInOut:
										return Easing.SineInOut( tempProgress, from, to );
									case EasingType.ExponentialIn:
										return Easing.ExponentialIn( tempProgress, from, to );
									case EasingType.ExponentialOut:
										return Easing.ExponentialOut( tempProgress, from, to );
									case EasingType.ExponentialInOut:
										return Easing.ExponentialInOut( tempProgress, from, to );
									case EasingType.CircularIn:
										return Easing.CircularIn( tempProgress, from, to );
									case EasingType.CircularOut:
										return Easing.CircularOut( tempProgress, from, to );
									case EasingType.CircularInOut:
										return Easing.CircularInOut( tempProgress, from, to );
									case EasingType.BackIn:
										return Easing.BackIn( tempProgress, from, to );
									case EasingType.BackOut:
										return Easing.BackOut( tempProgress, from, to );
									case EasingType.BackInOut:
										return Easing.BackInOut( tempProgress, from, to );
									case EasingType.BounceIn:
										return Easing.BounceIn( tempProgress, from, to );
									case EasingType.BounceOut:
										return Easing.BounceOut( tempProgress, from, to );
									case EasingType.BounceInOut:
										return Easing.BounceInOut( tempProgress, from, to );
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