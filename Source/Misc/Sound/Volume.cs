using UnityEngine;
using UnityEngine.Audio;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Adjusts volume for a mixing group via <see cref="UnityEngine.Audio.AudioMixer"/> parameters</summary>
	[AddComponentMenu( "PeenToys/Sound/Volume" )]
	public class Volume : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Normalized volume from specified min to max decibels</summary>
		[SerializeField]
		[Range( 0, 1 )]
		protected float _normalizedVolume = 1;
		/// <summary><see cref="UnityEngine.Audio.AudioMixer"/> reference to adjust</summary>
		[SerializeField]
		public AudioMixer _mixer;
		/// <summary>Array of parameters to apply to the <see cref="UnityEngine.Audio.AudioMixer"/></summary>
		public string[] parameters;
		/// <summary>Muted decibel level that represents 0 when normalized</summary>
		public float mutedDecibels = -80;
		/// <summary>Minimum decibel level that represents the smallest value when normalized</summary>
		public float minDecibels = -20;
		/// <summary>Maximum decibel level that represents 1 when normalized</summary>
		public float maxDecibels;
		
		//=======================
		// Initialization
		//=======================	
		/// <summary>Initializes the volume and applies to the mixer</summary>
		protected virtual void Start()
		{
			effectsNormalizedVolume( _normalizedVolume );
		}
		
		//=======================
		// Mixer
		//=======================
		public virtual AudioMixer mixer
		{
			get
			{
				return _mixer;
			}
			set
			{
				setMixer( value );
			}
		}
		
		/// <summary>Sets the <see cref="UnityEngine.Audio.AudioMixer"/></summary>
		/// <param name="tMixer">Mixer to set</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setMixer( AudioMixer tMixer )
		{
			if ( tMixer != _mixer )
			{
				AudioMixer tempOld = _mixer;
				_mixer = tMixer;
				effectsMixer( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Applies the volume adjustment to the <see cref="UnityEngine.Audio.AudioMixer"/></summary>
		/// <param name="tOld">Previous mixer</param>
		protected virtual void effectsMixer( AudioMixer tOld )
		{
			if ( _mixer != null && parameters != null )
			{
				for ( int i = ( parameters.Length - 1 ); i >= 0; --i )
				{
					_mixer.SetFloat( parameters[i], volume );
				}
			}
		}
		
		//=======================
		// Volume
		//=======================
		public virtual float normalizedVolume
		{
			get
			{
				return _normalizedVolume;
			}
			set
			{
				setNormalizedVolume( value );
			}
		}
		
		/// <summary>Sets the normalized volume</summary>
		/// <param name="tVolume">Normalized volume</param>
		/// <returns>True if successfully changed</returns>
		public virtual bool setNormalizedVolume( float tVolume )
		{
			tVolume = Mathf.Clamp( tVolume, 0, 1 );
			if ( tVolume != _normalizedVolume )
			{
				float tempOld = _normalizedVolume;
				_normalizedVolume = tVolume;
				effectsNormalizedVolume( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>Applies the volume adjustment to the <see cref="UnityEngine.Audio.AudioMixer"/></summary>
		/// <param name="tOld">Previous volume</param>
		protected virtual void effectsNormalizedVolume( float tOld )
		{
			if ( _mixer != null && parameters != null )
			{
				for ( int i = ( parameters.Length - 1 ); i >= 0; --i )
				{
					_mixer.SetFloat( parameters[i], volume );
				}
			}
		}
		
		/// <summary>Calculate and returns the actual volume in decibels</summary>
		public virtual float volume
		{
			get
			{
				return _normalizedVolume <= 0 ? mutedDecibels : ( Mathf.Clamp( _normalizedVolume, 0, 1 ) * ( maxDecibels - minDecibels ) + minDecibels );
			}
		}
	}
}