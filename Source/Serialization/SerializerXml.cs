using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	[ExecuteInEditMode]
	public class SerializerXml : Serializer<ISerializableXml,UnityListRefXml>
	{
		//=======================
		// Variables
		//=======================
		public Encoding encoding = Encoding.UTF8;

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
					// Settings
					XmlReaderSettings tempSettings = new XmlReaderSettings();
					tempSettings.IgnoreComments = true;
					
					// Read
					using ( XmlReader tempReader = XmlReader.Create( tStream, tempSettings ) )
					{
						if ( tempReader.Read() && tempReader.Read() && tempReader.Read() ) // expects root node afer xml header (i.e., <root>)
						{
							float tempVersion;
							if ( Single.TryParse( tempReader[ "version" ], out tempVersion ) )
							{
								for ( int i = 0; i < tempListLength; ++i )
								{
									if ( _serializables.list[i] != null )
									{
										_serializables.list[i].read( this, tempReader, tempVersion );
									}
								}
							}
							else
							{
								Debug.Log( "XML DID NOT INCLUDE VALID VERSION ROOT!", this );
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
					// Settings
					XmlWriterSettings tempSettings = new XmlWriterSettings();
					tempSettings.Indent = true;
					tempSettings.IndentChars = "\t";
					tempSettings.OmitXmlDeclaration = true;
					tempSettings.Encoding = encoding;

					// Write
					using ( XmlWriter tempWriter = XmlWriter.Create( tStream, tempSettings ) )
					{
						tempWriter.WriteStartDocument();
						tempWriter.WriteStartElement( "root" );
						tempWriter.WriteAttributeString( "version", version.ToString() );
						tempWriter.WriteEndElement();

						for ( int i = 0; i < tempListLength; ++i )
						{
							if ( _serializables.list[i] != null )
							{
								_serializables.list[i].write( this, tempWriter );
							}
						}
						
						tempWriter.WriteEndDocument();
						tempWriter.Flush();
					}
				}
			}
		}
	}
}