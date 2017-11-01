using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	[ExecuteInEditMode]
	public class SerializerBinary : Serializer<ISerializableBinary,UnityListRefBinary>
	{
		//=======================
		// Read
		//=======================
		public override void read( Stream tStream )
		{
			if ( tStream != null && _serializables != null )
			{
				int tempListLength = _serializables.list == null ? 0 : _serializables.list.Count;
				if ( tempListLength > 0 )
				{
					using ( BinaryReader tempReader = new BinaryReader( tStream ) )
					{ 
						if ( tStream.Length >= 4 ) // expects the first 4 bytes to be a version
						{
							float tempVersion = tempReader.ReadSingle();
							for ( int i = 0; i < tempListLength; ++i )
							{
								if ( _serializables.list[i] != null )
								{
									_serializables.list[i].read( this, tempReader, tempVersion );
								}
							}
						}
					}
				}
			}
		}
		
		//=======================
		// Write
		//=======================
		public override void write( Stream tStream )
		{
			if ( tStream != null && _serializables != null )
			{
				int tempListLength = _serializables.list == null ? 0 : _serializables.list.Count;
				if ( tempListLength > 0 )
				{
					using ( BinaryWriter tempWriter = new BinaryWriter( tStream ) )
					{
						tempWriter.Write( version );
						for ( int i = 0; i < tempListLength; ++i )
						{
							if ( _serializables.list[i] != null )
							{
								_serializables.list[i].write( this, tempWriter );
							}
						}
						tempWriter.Flush();
					}
				}
			}
		}
	}
}