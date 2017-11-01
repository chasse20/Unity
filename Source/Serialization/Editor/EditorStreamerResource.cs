using UnityEngine;
using UnityEditor;
using System;
using System.Text;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	[CustomEditor( typeof( StreamerResource ), true )]
	public class EditorStreamerResource : Editor
	{	
		//=======================
		// GUI
		//=======================
		public override void OnInspectorGUI()
		{		
			// Inheritance
			base.OnInspectorGUI();
			
			// Resource input
			StreamerResource tempStreamer = target as StreamerResource;
			TextAsset tempResource = EditorGUILayout.ObjectField( "Resource", tempStreamer.resource, typeof( TextAsset ), false ) as TextAsset;
			if ( tempResource != tempStreamer.resource )
			{
				if ( tempResource == null )
				{
					tempStreamer.resource = null;
					tempStreamer.path = null;
				}
				else
				{
					// Check to see if it is an actual resource
					string tempPath = AssetDatabase.GetAssetPath( tempResource );
					int tempPathStart = tempPath.IndexOf( "/Resources/", StringComparison.OrdinalIgnoreCase ); // 11 chars
					if ( tempPathStart >= 0 )
					{
						tempStreamer.resource = tempResource;
						tempStreamer.path = tempPath.Substring( tempPathStart + 11, ( tempPath.IndexOf( '.' ) - tempPathStart - 11 ) ); // truncate "/Resources/" at beginning and extension at end
					}
					else
					{
						tempStreamer.resource = null;
						tempStreamer.path = null;
					}
				}
			}
			
			// Delete button
			if ( GUILayout.Button( "Delete Source" ) && UnityEditor.EditorUtility.DisplayDialog( "Delete Source?", "Delete Source? This will remove the source data completely.", "Okay", "Cancel" ) )
			{
				tempStreamer.delete();
			}
		}
	}
}