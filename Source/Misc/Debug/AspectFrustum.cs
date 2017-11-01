using System;
using UnityEngine;

namespace PeenToys
{
	//##########################
	// Struct Declaration
	//##########################
	/// <summary>View frustum data used by <see cref="AspectFrustums"/></summary>
	[Serializable]
	public class AspectFrustum
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Width aspect</summary>
		public float width;
		/// <summary>Height aspect</summary>
		public float height;
		/// <summary>Render color</summary>
		public Color32 color;
		
		//=======================
		// Constructor
		//=======================
		public AspectFrustum()
		{
		}
		
		public AspectFrustum( float tWidth, float tHeight, Color32 tColor )
		{
			width = tWidth;
			height = tHeight;
			color = tColor;
		}
		
		//=======================
		// Ratio
		//=======================
		public float aspectRatio
		{
			get
			{
				return height == 0 ? 0 : ( width / height );
			}
		}
	}
}