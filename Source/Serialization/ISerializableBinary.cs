using System;
using System.IO;

namespace PeenRetain
{
	//##########################
	// Interface Declaration
	//##########################
	public interface ISerializableBinary
	{
		void read( SerializerBinary tSerializer, BinaryReader tReader, float tVersion );
		void write( SerializerBinary tSerializer, BinaryWriter tWriter );
	}
}