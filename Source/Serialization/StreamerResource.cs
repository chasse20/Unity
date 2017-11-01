#if UNITY_EDITOR
	using UnityEditor;
#endif
using UnityEngine;
using System;
using System.IO;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class StreamerResource : Streamer
	{
		//=======================
		// Variables
		//=======================
		#if UNITY_EDITOR
			[HideInInspector]
			public TextAsset resource;
		#endif
		[HideInInspector]
		public string path;
		
		//=======================
		// Stream
		//=======================
		public override bool read( Action<Stream> tHandler )
		{
			#if UNITY_EDITOR
				// Load directly if editor
				if ( tHandler != null && resource != null )
				{
					using ( MemoryStream tempStream = new MemoryStream( resource.bytes ) )
					{
						tHandler( tempStream );
					}
					
					return true;
				}
			#else
				// Load into memory if playing
				if ( tHandler != null && !String.IsNullOrEmpty( path ) )
				{
					TextAsset tempAsset = Resources.Load<TextAsset>( path );
					if ( tempAsset != null )
					{
						using ( MemoryStream tempStream = new MemoryStream( tempAsset.bytes ) )
						{
							tHandler( tempStream );
						}
						
						return true;
					}
				}
			#endif
			
			return false;
		}
		
		public override bool write( Action<Stream> tHandler )
		{
			// Writeable only if editor
			#if UNITY_EDITOR
				if ( tHandler != null && resource != null )
				{
					using ( FileStream tempStream = new FileStream( AssetDatabase.GetAssetPath( resource ), FileMode.OpenOrCreate, FileAccess.Write ) ) // assumes permissions
					{
						tempStream.SetLength( 0 );
						tHandler( tempStream );
					}
					
					return true;
				}
			#endif
			
			return false;
		}
		
		public override bool delete()
		{
			#if UNITY_EDITOR
				resource = null;
			#endif
			path = null;
			return true;
		}
	}
}