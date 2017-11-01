using UnityEngine;
using UnityEditor;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	[CustomEditor( typeof( Serializer ), true )]
	public class EditorSerializer : Editor
	{
		//=======================
		// GUI
		//=======================
		public override void OnInspectorGUI()
		{
			// Inheritance
			base.OnInspectorGUI();
			
			// Buttons
			Serializer tempSerializer = target as Serializer;
			if ( GUILayout.Button( "Read" ) )
			{
				tempSerializer.read();
			}
			if ( GUILayout.Button( "Write" ) )
			{
				tempSerializer.write();
			}
		}
	}
}