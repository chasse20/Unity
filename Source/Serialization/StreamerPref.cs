using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class StreamerPref : Streamer
	{
		//=======================
		// Variables
		//=======================
		public string key;
		public bool isSavedOnWrite;

		//=======================
		// Stream
		//=======================		
		public override bool read( Action<Stream> tHandler )
		{
			if ( tHandler != null && !String.IsNullOrEmpty( key ) && PlayerPrefs.HasKey( key ) )
			{
				using ( MemoryStream tempStream = new MemoryStream( Convert.FromBase64String( PlayerPrefs.GetString( key ) ) ) )
				{
					tHandler( tempStream );
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool write( Action<Stream> tHandler )
		{
			if ( tHandler != null && !String.IsNullOrEmpty( key ) )
			{
				// Convert byte stream to string
				using ( MemoryStream tempStream = new MemoryStream() )
				{
					tHandler( tempStream );
					PlayerPrefs.SetString( key, Convert.ToBase64String( tempStream.ToArray() ) );
				}
					
				// Force save when completed
				if ( isSavedOnWrite )
				{
					PlayerPrefs.Save();
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool delete()
		{
			// Delete and check if force saved
			if ( !String.IsNullOrEmpty( key ) )
			{
				PlayerPrefs.DeleteKey( key );
				
				if ( isSavedOnWrite )
				{
					PlayerPrefs.Save();
				}
				
				return true;
			}
			
			return false;
		}
	}
}