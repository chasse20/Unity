using UnityEngine;
using UnityEditor;
using System;
using System.Text;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Drawer for displaying a string property as a scene object field</summary>
	[CustomPropertyDrawer( typeof( AttributeScene ), true )]
	public class DrawerAttributeScene : PropertyDrawer
	{
		//=======================
		// GUI
		//=======================
		/// <summary>Displays the enumerator property as a mask pop-up field in the inspector</summary>
		/// <param name="tPosition">Inspector GUI position</param>
		/// <param name="tProperty"><see cref="UnityEditor.SerializedProperty"/> reference</param>
		/// <param name="tLabel">Inspector GUI label</param>
		public override void OnGUI( Rect tPosition, SerializedProperty tProperty, GUIContent tLabel )
		{
			// Try to load scene
			EditorGUI.BeginProperty( tPosition, tLabel, tProperty );
			SceneAsset tempScene = null;
			if ( !String.IsNullOrEmpty( tProperty.stringValue ) )
			{
				StringBuilder tempString = new StringBuilder( "Assets/" );
				tempString.Append( tProperty.stringValue );
				tempString.Append( ".unity" );
				tempScene = AssetDatabase.LoadAssetAtPath<SceneAsset>( tempString.ToString() );
			}
			
			// Convert string to scene path if changed
			EditorGUI.BeginChangeCheck();
			tempScene = EditorGUI.ObjectField( tPosition, tLabel, tempScene, typeof( SceneAsset ), false ) as SceneAsset;
			if ( EditorGUI.EndChangeCheck() )
			{
				if ( tempScene == null )
				{
					tProperty.stringValue = null;
				}
				else
				{
					string tempPath = AssetDatabase.GetAssetPath( tempScene );
					tProperty.stringValue = tempPath.Substring( 7, ( tempPath.Length - 13 ) ); // skip "Assets/" at beginning and remove ".unity" at the end
				}
			}
			EditorGUI.EndProperty();
		}
	}
}