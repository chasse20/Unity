using UnityEngine;
using System;
using System.Collections.Generic;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Manages all <see cref="Achievement"/>s</summary>
	[AddComponentMenu( "PeenToys/Achievements/Achievement Manager" )]
	public class AchievementManager : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Array of <see cref="Achievement"/>s being managed</summary> 
		[SerializeField]
		protected Achievement[] _achievements;

		//=======================
		// Destruction
		//=======================
		/// <summary>Clear achievements from memory</summary>
		protected virtual void OnDestroy()
		{
			if ( _achievements != null )
			{
				Array.Clear( _achievements, 0, _achievements.Length );
				_achievements = null;
			}
		}
		
		//=======================
		// Achievements
		//=======================
		public virtual List<Achievement> achievements
		{
			get
			{
				return _achievements == null ? null : new List<Achievement>( _achievements );
			}
		}
		
		/// <summary>Returns the instance of <see cref="Achievement"/>s from a specific index</summary>
		/// <param name="tIndex">Index to retrieve</param>
		/// <param name="tAchievement">Achievement found at <paramref name="tIndex"/></param>
		/// <returns>True if successfully found</returns>
		public virtual bool getAchievement( uint tIndex, out Achievement tAchievement )
		{
			if ( _achievements != null && tIndex < _achievements.Length )
			{
				tAchievement = _achievements[ tIndex ];
				return true;
			}
			
			tAchievement = null;
			return false;
		}
	}
}