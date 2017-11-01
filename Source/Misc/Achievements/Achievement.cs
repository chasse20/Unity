using System;
using UnityEngine;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Individual achievement data</summary> 
	[Serializable]
	public class Achievement
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Date of successful achievement in <see cref="System.DateTime.Ticks"/> since 0AD, negative values indicate achievement not completed</summary>
		[SerializeField]
		protected long _date = -1;
	
		//=======================
		// Constructor
		//=======================
		public Achievement()
		{
		}
		
		public Achievement( long tDate )
		{
			_date = tDate;
		}
		
		//=======================
		// Date
		//=======================
		public virtual long date
		{
			get
			{
				return _date;
			}
			set
			{
				setDate( value );
			}
		}
		
		/// <summary>Sets the date of achievement</summary>
		/// <param name="tDate"/>Time achieved in <see cref="DateTime.Ticks"/> since 0AD</param>
		/// <returns>True if not already set and is successful</returns>
		public virtual bool setDate( long tDate )
		{
			if ( tDate != _date )
			{
				_date = tDate;
				return true;
			}
			
			return false;
		}
		
		/// <summary>Automatically sets the achievement date to the current time</summary>
		public virtual void complete()
		{
			setDate( DateTime.Now.Ticks );
		}
	}
}