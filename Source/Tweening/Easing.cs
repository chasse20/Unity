using System;
using UnityEngine;

/*TERMS OF USE - EASING EQUATIONS#

Open source under the BSD License.

Copyright (c)2001 Robert Penner
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace PeenTween
{
	//##########################
	// Class Declaration
	//##########################
	public static class Easing
	{
		//=======================
		// Linear
		//=======================
		public static float Linear( float tProgress, float tFrom, float tTo )
		{
			return tProgress * ( tTo - tFrom ) + tFrom;
		}
		
		//=======================
		// Quadratic
		//=======================
		public static float QuadraticIn( float tProgress, float tFrom, float tTo )
		{
			return tProgress * tProgress * ( tTo - tFrom ) + tFrom;
		}
		
		public static float QuadraticOut( float tProgress, float tFrom, float tTo )
		{
			return tProgress * ( tProgress - 2 ) * ( tFrom - tTo ) + tFrom;
		}
		
		public static float QuadraticInOut( float tProgress, float tFrom, float tTo )
		{
			tProgress *= 2;
			if ( tProgress < 1 )
			{
				return ( tProgress * tProgress * ( tTo - tFrom ) * 0.5f ) + tFrom;
			}
			
			--tProgress;
			return ( tProgress * ( tProgress - 2 ) - 1 ) * ( tFrom - tTo ) * 0.5f + tFrom;
		}
		
		//=======================
		// Cubic
		//=======================
		public static float CubicIn( float tProgress, float tFrom, float tTo )
		{
			return tProgress * tProgress * tProgress * ( tTo - tFrom ) + tFrom;
		}
		
		public static float CubicOut( float tProgress, float tFrom, float tTo )
		{
			--tProgress;
			return ( tProgress * tProgress * tProgress + 1 ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float CubicInOut( float tProgress, float tFrom, float tTo )
		{
			tProgress *= 2;
			if ( tProgress < 1 )
			{
				return tProgress * tProgress * tProgress * ( tTo - tFrom ) * 0.5f + tFrom;
			}
			
			tProgress -= 2;
			return ( tProgress * tProgress * tProgress + 2 ) * ( tTo - tFrom ) * 0.5f + tFrom;
		}
		
		//=======================
		// Quartic
		//=======================
		public static float QuarticIn( float tProgress, float tFrom, float tTo )
		{
			return tProgress * tProgress * tProgress * tProgress * ( tTo - tFrom ) + tFrom;
		}
		
		public static float QuarticOut( float tProgress, float tFrom, float tTo )
		{
			--tProgress;
			return ( tProgress * tProgress * tProgress * tProgress - 1 ) * ( tFrom - tTo ) + tFrom;
		}
		
		public static float QuarticInOut( float tProgress, float tFrom, float tTo )
		{
			tProgress *= 2;
			if ( tProgress < 1 )
			{
				return tProgress * tProgress * tProgress * tProgress * ( tTo - tFrom ) * 0.5f + tFrom;
			}
			
			tProgress -= 2;
			return ( tProgress * tProgress * tProgress * tProgress - 2 ) * ( tFrom - tTo ) * 0.5f + tFrom;
		}
		
		//=======================
		// Quintic
		//=======================
		public static float QuinticIn( float tProgress, float tFrom, float tTo )
		{
			return tProgress * tProgress * tProgress * tProgress * tProgress * ( tTo - tFrom ) + tFrom;
		}
		
		public static float QuinticOut( float tProgress, float tFrom, float tTo )
		{
			--tProgress;
			return ( tProgress * tProgress * tProgress * tProgress * tProgress + 1 ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float QuinticInOut( float tProgress, float tFrom, float tTo )
		{
			tProgress *= 2;
			if ( tProgress < 1 )
			{
				return tProgress * tProgress * tProgress * tProgress * tProgress * ( tTo - tFrom ) * 0.5f + tFrom;
			}
			
			tProgress -= 2;
			return ( tProgress * tProgress * tProgress * tProgress * tProgress + 2 ) * ( tTo - tFrom ) * 0.5f + tFrom;
		}
		
		//=======================
		// Sinusoidal
		//=======================
		public static float SineIn( float tProgress, float tFrom, float tTo )
		{
			tTo -= tFrom;
			return -tTo * Mathf.Cos( Mathf.PI * tProgress * 0.5f ) + tTo + tFrom;
		}
		
		public static float SineOut( float tProgress, float tFrom, float tTo )
		{
			return ( tTo - tFrom ) * Mathf.Sin( Mathf.PI * tProgress * 0.5f ) + tFrom;
		}
		
		public static float SineInOut( float tProgress, float tFrom, float tTo )
		{
			return ( Mathf.Cos( Mathf.PI * tProgress ) - 1 ) * ( tFrom - tTo ) * 0.5f + tFrom;
		}
		
		//=======================
		// Exponential
		//=======================
		public static float ExponentialIn( float tProgress, float tFrom, float tTo )
		{
			return Mathf.Pow( 2, 10 * ( tProgress - 1 ) ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float ExponentialOut( float tProgress, float tFrom, float tTo )
		{
			return ( 1 - Mathf.Pow( 2, -10 * tProgress ) ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float ExponentialInOut( float tProgress, float tFrom, float tTo )
		{
			tProgress *= 2;
			if ( tProgress < 1 )
			{
				return Mathf.Pow( 2, 10 * ( tProgress - 1 ) ) * ( tTo - tFrom ) * 0.5f + tFrom;
			}
			
			--tProgress;
			return ( 2 - Mathf.Pow( 2, -10 * tProgress ) ) * ( tTo - tFrom ) * 0.5f + tFrom;
		}
		
		//=======================
		// Circular
		//=======================
		public static float CircularIn( float tProgress, float tFrom, float tTo )
		{
			return ( Mathf.Sqrt( 1 - ( tProgress * tProgress ) ) - 1 ) * ( tFrom - tTo ) + tFrom;
		}
		
		public static float CircularOut( float tProgress, float tFrom, float tTo )
		{
			--tProgress;
			return Mathf.Sqrt( 1 - ( tProgress * tProgress ) ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float CircularInOut( float tProgress, float tFrom, float tTo )
		{
			tProgress *= 2;
			if ( tProgress < 1 )
			{
				return ( Mathf.Sqrt( 1 - ( tProgress * tProgress ) ) - 1 ) * ( tFrom - tTo ) * 0.5f + tFrom;
			}
			
			tProgress -= 2;
			return ( Mathf.Sqrt( 1 - ( tProgress * tProgress ) ) + 1 ) * ( tTo - tFrom ) * 0.5f + tFrom;
		}
		
		//=======================
		// Elastic
		//=======================
		public static float ElasticIn( float tElapsed, float tDuration, float tFrom, float tTo, float tAmplitude = 0, float tPeriod = 0 )
		{
			if ( tElapsed <= 0 || tDuration == 0 )
			{
				return tFrom;
			}
			tElapsed /= tDuration;
			if ( tElapsed >= 1 )
			{
				return tTo;
			}
			
			if ( tPeriod == 0 )
			{
				tPeriod = tDuration * 0.3f;
			}
			
			tTo -= tFrom;
			float tempSpring = tPeriod * 0.25f;
			if ( tAmplitude == 0 || tAmplitude < Mathf.Abs( tTo ) )
			{
				tAmplitude = tTo;
			}
			else
			{
				tempSpring = Mathf.Asin( tTo / tAmplitude ) * tPeriod / ( Mathf.PI * 2 );
			}
			
			--tElapsed;
			return -tAmplitude * Mathf.Pow( 2, 10 * tElapsed ) * Mathf.Sin( ( tElapsed * tDuration - tempSpring ) * 2 * Mathf.PI / tPeriod ) + tFrom; 
		}
		
		public static float ElasticOut( float tElapsed, float tDuration, float tFrom, float tTo, float tAmplitude = 0, float tPeriod = 0 )
		{
			if ( tElapsed <= 0 || tDuration == 0 )
			{
				return tFrom;
			}
			tElapsed /= tDuration;
			if ( tElapsed >= 1 )
			{
				return tTo;
			}
			
			if ( tPeriod == 0 )
			{
				tPeriod = tDuration * 0.3f;
			}
			
			tTo -= tFrom;
			float tempSpring = tPeriod * 0.25f;
			if ( tAmplitude == 0 || tAmplitude < Mathf.Abs( tTo ) )
			{
				tAmplitude = tTo;
			}
			else
			{
				tempSpring = Mathf.Asin( tTo / tAmplitude ) * tPeriod / ( Mathf.PI * 2 );
			}
			
			return tAmplitude * Mathf.Pow( 2, -10 * tElapsed ) * Mathf.Sin( ( tElapsed * tDuration - tempSpring ) * 2 * Mathf.PI / tPeriod ) + tFrom + tTo;
		}
		
		public static float ElasticInOut( float tElapsed, float tDuration, float tFrom, float tTo, float tAmplitude = 0, float tPeriod = 0 )
		{
			if ( tElapsed <= 0 || tDuration == 0 )
			{
				return tFrom;
			}
			tElapsed = ( tElapsed * 2 ) / tDuration;
			if ( tElapsed >= 2 )
			{
				return tTo;
			}
			
			if ( tPeriod == 0 )
			{
				tPeriod = tDuration * 0.45f;
			}
			
			tTo -= tFrom;
			float tempSpring = tPeriod * 0.25f;
			if ( tAmplitude == 0 || tAmplitude < Mathf.Abs( tTo ) )
			{
				tAmplitude = tTo;
			}
			else
			{
				tempSpring = Mathf.Asin( tTo / tAmplitude ) * tPeriod / ( Mathf.PI * 2 );
			}
			
			--tElapsed;
			if ( tElapsed < 0 )
			{
				return -0.5f * tAmplitude * Mathf.Pow( 2, 10 * tElapsed ) * Mathf.Sin( ( tElapsed * tDuration - tempSpring ) * 2 * Mathf.PI / tPeriod ) + tFrom;
			}
			
			return 0.5f * tAmplitude * Mathf.Pow( 2, -10 * tElapsed ) * Mathf.Sin( ( tElapsed * tDuration - tempSpring ) * 2 * Mathf.PI / tPeriod ) + tFrom + tTo;
		}
		
		//=======================
		// Back
		//=======================
		public static float BackIn( float tProgress, float tFrom, float tTo, float tAmplitude = 1.70158f )
		{
			return tProgress * tProgress * ( tProgress * ( tAmplitude + 1 ) - tAmplitude ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float BackOut( float tProgress, float tFrom, float tTo, float tAmplitude = 1.70158f )
		{
			--tProgress;
			return ( tProgress * tProgress * ( tProgress * ( tAmplitude + 1 ) + tAmplitude ) + 1 ) * ( tTo - tFrom ) + tFrom;
		}
		
		public static float BackInOut( float tProgress, float tFrom, float tTo, float tAmplitude = 1.70158f )
		{
			tProgress *= 2;
			tAmplitude *= 1.525f;
			if ( tProgress < 1 )
			{
				return tProgress * tProgress * ( tProgress * ( tAmplitude + 1 ) - tAmplitude ) * ( tTo - tFrom ) * 0.5f + tFrom;
			}
			
			tProgress -= 2;
			return ( tProgress * tProgress * ( tProgress * ( tAmplitude + 1 ) + tAmplitude ) + 2 ) * ( tTo - tFrom ) * 0.5f + tFrom;
		}
		
		//=======================
		// Bounce
		//=======================
		public static float BounceIn( float tProgress, float tFrom, float tTo )
		{
			return tTo - BounceOut( ( 1 - tProgress ), 0, ( tTo - tFrom ) );
		}
		
		public static float BounceOut( float tProgress, float tFrom, float tTo )
		{
			tTo -= tFrom;
			if ( tProgress < ( 1 / 2.75f ) )
			{
				return tProgress * tProgress * tTo * 7.5625f + tFrom;
			}
			else if ( tProgress < ( 2f / 2.75f ) )
			{
				tProgress -= 1.5f / 2.75f;
				return ( tProgress * tProgress * 7.5625f + 0.75f ) * tTo + tFrom;
			}
			else if ( tProgress < ( 2.5f / 2.75f ) )
			{
				tProgress -= 2.25f / 2.75f;
				return ( tProgress * tProgress * 7.5625f + 0.9375f ) * tTo + tFrom;
			}
			
			tProgress -= 2.625f / 2.75f;
			return ( tProgress * tProgress * 7.5625f + 0.984375f ) * tTo + tFrom;
		}
		
		public static float BounceInOut( float tProgress, float tFrom, float tTo )
		{
			tTo -= tFrom;
			if ( tProgress < 0.5f )
			{
				return BounceIn( ( tProgress * 2 ), 0, tTo ) * 0.5f + tFrom;
			}
			
			return BounceOut( ( tProgress * 2 - 1 ), 0, tTo ) * 0.5f + ( tTo * 0.5f ) + tFrom;
		}
	}
}