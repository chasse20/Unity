using UnityEditor;
using System;

namespace PeenScreen
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Draws the inspector for the <see cref="ScreenManager"/> with the <see cref="ScreenManager._splitScreenMode"/> field as an enumerator</summary>
	[CustomEditor( typeof( ScreenManager ), true )]
	public class EditorScreenManager : Editor
	{
		//=======================
		// Variables
		//=======================
		/// <summary>SerializedProperty for the split-screen mode</summary>
		protected SerializedProperty splitScreenMode;
		
		//=======================
		// GUI
		//=======================
		/// <summary>Sets the property reference of the <see cref="splitScreenMode"/></summary>
		protected virtual void OnEnable()
		{
			splitScreenMode = serializedObject.FindProperty( "_splitScreenMode" );
		}
		
		/// <summary>Draws the enumerator field for the <see cref="splitScreenMode"/></summary>
		public override void OnInspectorGUI()
		{
			// Inheritance
			base.OnInspectorGUI();
			
			// Draw split-screen mode enumerator
			int tempValue = splitScreenMode.intValue;
			splitScreenMode.intValue = (int)( (SplitScreenMode)EditorGUILayout.EnumPopup( "Split Screen Mode", (SplitScreenMode)tempValue ) );
			if ( splitScreenMode.intValue != tempValue )
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}