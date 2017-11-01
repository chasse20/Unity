using UnityEngine;
using System;

namespace PeenScreen
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Basic <see cref="Rig"/> for handling an array of cameras</summary>
	[AddComponentMenu( "PeenScreen/Rig Basic" )]
	public class RigBasic : Rig
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Array of cameras</summary>
		public Camera[] cameras;
		
		//=======================
		// Destruction
		//=======================
		/// <summary>Clear memory</summary>
		protected virtual void OnDestroy()
		{
			if ( cameras != null )
			{
				Array.Clear( cameras, 0, cameras.Length );
			}
		}
		
		//=======================
		// Type
		//=======================
		public override string type
		{
			get
			{
				return "";
			}
		}
		
		//=======================
		// Rect
		//=======================
		public override bool setRect( Rect tScreenRect )
		{
			if ( cameras != null && cameras.Length > 0 )
			{
				for ( int i = ( cameras.Length - 1 ); i >= 0; --i )
				{
					cameras[i].rect = tScreenRect;
				}
				
				return true;
			}
			
			return false;
		}
	}
}