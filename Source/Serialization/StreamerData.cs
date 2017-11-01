using UnityEngine;
using System;
using System.IO;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class StreamerData : Streamer // persistent data
	{
		//=======================
		// Variables
		//=======================
		public string fileName;
		
		//=======================
		// Stream
		//=======================
		public virtual string filePath
		{
			get
			{
				return Application.persistentDataPath + "/" + fileName;
			}
		}
		
		public override bool read( Action<Stream> tHandler )
		{
			if ( tHandler != null && !String.IsNullOrEmpty( fileName ) )
			{
				using ( FileStream tempStream = new FileStream( filePath, FileMode.Open, FileAccess.Read ) ) // assumes permissions
				{
					tHandler( tempStream );
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool write( Action<Stream> tHandler )
		{
			if ( tHandler != null && !String.IsNullOrEmpty( fileName ) )
			{
				using ( FileStream tempStream = new FileStream( filePath, FileMode.OpenOrCreate, FileAccess.Write ) ) // assumes permissions
				{
					tempStream.SetLength( 0 );
					tHandler( tempStream );
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool delete()
		{
			// Delete file
			if ( !String.IsNullOrEmpty( fileName ) )
			{
				string tempPath = filePath;
				if ( File.Exists( tempPath ) )
				{
					File.Delete( tempPath );
					return true;
				}
			}
			
			return false;
		}
	}
}