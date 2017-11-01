using UnityEngine;
using UnityEditor;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Draws the inspector for the <see cref="Raycaster"/> with a button for grounding all selected transforms</summary>
	[CustomEditor( typeof( Raycaster ), true )]
	public class EditorRaycaster : Editor
	{
		//=======================
		// GUI
		//=======================
		/// <summary>Draws the button for the grounding all selected transforms inside of the editor</summary>
		public override void OnInspectorGUI()
		{
			// Inheritance
			base.OnInspectorGUI();
			
			// Auto ground selected
			if ( GUILayout.Button( "Ground Selected" ) )
			{
				Raycaster tempCaster = target as Raycaster;
				for ( int i = ( Selection.transforms.Length - 1 ); i >= 0; --i )
				{
					if ( Selection.transforms[i] != tempCaster.transform )
					{
						Undo.RecordObject( Selection.transforms[i], "Ground Selected" );
						tempCaster.ground( Selection.transforms[i] );
					}
				}
			}
		}
	}
}