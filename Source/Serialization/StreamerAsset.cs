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
	public class StreamerAsset : Streamer
	{
		//=======================
		// Variables
		//=======================
		public TextAsset asset;
		
		//=======================
		// Stream
		//=======================
		public override bool read( Action<Stream> tHandler )
		{
			if ( tHandler != null && asset != null )
			{
				using ( MemoryStream tempStream = new MemoryStream( asset.bytes ) )
				{
					tHandler( tempStream );
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool write( Action<Stream> tHandler )
		{
			// Writeable only if editor
			#if UNITY_EDITOR
				if ( tHandler != null && asset != null )
				{
					using ( FileStream tempStream = new FileStream( AssetDatabase.GetAssetPath( asset ), FileMode.OpenOrCreate, FileAccess.Write ) ) // assumes permissions
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
			asset = null;
			return true;
		}
	}
}