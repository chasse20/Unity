using UnityEngine;
using System;
using System.IO;

namespace PeenRetain
{
	//##########################
	// Class Declaration
	//##########################
	public abstract class Streamer : MonoBehaviour
	{		
		//=======================
		// Stream
		//=======================
		public abstract bool read( Action<Stream> tHandler );
		public abstract bool write( Action<Stream> tHandler );
		public abstract bool delete();
	}
}