using UnityEditor;
using UnityEngine;
using System.Text;
using System;
using System.IO;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Editor Utility for handling <see cref="UnityEngine.ScriptableObject"/></summary>
	public static class UtilityScriptableObject
	{
		//=======================
		// ScriptableObject Factory
		//=======================
		/// <summary>Creates a new <see cref="UnityEngine.ScriptableObject"/> instance</summary>
		/// <param name="tName"/>Desired file-name of the instance, defaults to the class name if empty</param>
		/// <returns>Instance of a <see cref="UnityEngine.ScriptableObject"/> if successfully created</returns>
		public static T Create<T>( string tName = "" ) where T : ScriptableObject
		{
			// Determine output path, give priority to whatever is selected
			string tempPath = AssetDatabase.GetAssetPath( Selection.activeObject );
			StringBuilder tempString = new StringBuilder();
			if ( string.IsNullOrEmpty( tempPath ) )
			{
				tempString.Append( "Assets" );
			}
			else
			{
				tempString.Append( tempPath );
				if ( !string.IsNullOrEmpty( Path.GetExtension( tempPath ) ) )
				{
					tempString.Replace( Path.GetFileName( AssetDatabase.GetAssetPath( Selection.activeObject ) ), "" );
				}
			}
			tempString.Append( "/" );
			tempString.Append( String.IsNullOrEmpty( tName ) ? typeof(T).ToString() : tName );
			tempString.Append( ".asset" );
			
			// Create object and save
			T tempObject = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset( tempObject, AssetDatabase.GenerateUniqueAssetPath( tempString.ToString() ) );
			AssetDatabase.SaveAssets();
			UnityEditor.EditorUtility.FocusProjectWindow();
			Selection.activeObject = tempObject;
			
			return tempObject;
		}
	}
}