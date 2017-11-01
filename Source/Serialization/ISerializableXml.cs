using System;
using System.Xml;

namespace PeenRetain
{
	//##########################
	// Interface Declaration
	//##########################
	public interface ISerializableXml
	{
		void read( SerializerXml tSerializer, XmlReader tReader, float tVersion );
		void write( SerializerXml tSerializer, XmlWriter tWriter );
	}
}