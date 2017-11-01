using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public class StreamerRaw : Streamer
	{
		//=======================
		// Variables
		//=======================
		public string raw;
		
		//=======================
		// Stream
		//=======================		
		public override bool read( Action<Stream> tHandler )
		{
			if ( tHandler != null && !String.IsNullOrEmpty( raw ) )
			{
				using ( MemoryStream tempStream = new MemoryStream( Convert.FromBase64String( raw ) ) )
				{
					tHandler( tempStream );
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool write( Action<Stream> tHandler )
		{
			if ( tHandler != null )
			{
				using ( MemoryStream tempStream = new MemoryStream() )
				{
					tHandler( tempStream );
					raw = Convert.ToBase64String( tempStream.ToArray() );
				}
				
				return true;
			}
			
			return false;
		}
		
		public override bool delete()
		{
			raw = null;
			return true;
		}
	}
}