using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Drawer for displaying an enum property as a mask pop-up field</summary>
	[CustomPropertyDrawer( typeof( AttributeMask ), true )]
	public class DrawerAttributeMask : PropertyDrawer
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
			if ( tProperty.propertyType == SerializedPropertyType.Enum )
			{
				AttributeMask tempAttribute = attribute as AttributeMask;
				EditorGUI.BeginProperty( tPosition, tLabel, tProperty );
				tProperty.intValue = (int)Convert.ChangeType( EditorGUI.EnumMaskField( tPosition, tLabel, Enum.ToObject( tempAttribute.type, tProperty.intValue ) as Enum ), tempAttribute.type );
				EditorGUI.EndProperty();
			}
		}
	}
}