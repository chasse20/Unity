using UnityEngine;
using UnityEditor;
using System;
using System.Text;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	[CustomPropertyDrawer( typeof( AttributeRef ), true )]
	public class DrawerAttributeRef : PropertyDrawer
	{
		//=======================
		// GUI
		//=======================
		/// <summary>Draws an object-field of the specified <see cref="PeenToys.AttributeRef"/> type</summary>
		/// <param name="tPosition">Inspector GUI position</param>
		/// <param name="tProperty"><see cref="UnityEditor.SerializedProperty"/> reference</param>
		/// <param name="tLabel">Inspector GUI label</param>
		public override void OnGUI( Rect tPosition, SerializedProperty tProperty, GUIContent tLabel )
		{
			EditorGUI.BeginProperty( tPosition, tLabel, tProperty );
			tProperty.objectReferenceValue = EditorGUI.ObjectField( tPosition, tLabel, tProperty.objectReferenceValue, ( attribute as AttributeRef ).type, false );
			EditorGUI.EndProperty();
		}
	}
}