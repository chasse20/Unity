using UnityEngine;
using UnityEditor;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	[CustomEditor( typeof( Streamer ), true )]
	public class EditorStreamer : Editor
	{	
		//=======================
		// GUI
		//=======================
		public override void OnInspectorGUI()
		{
			// Inheritance
			base.OnInspectorGUI();
			
			// Delete button
			if ( GUILayout.Button( "Delete Source" ) && UnityEditor.EditorUtility.DisplayDialog( "Delete Source?", "Delete Source? This will remove the source data completely.", "Okay", "Cancel" ) )
			{
				( target as Streamer ).delete();
			}
		}
	}
}