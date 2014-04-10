using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;
using JsonFx.Json;
using JsonFx;
using System.Collections;
using UnityEngine;


namespace OneDescript {
	//DEPRECATED:Specification: https://docs.google.com/document/d/1I-lmpBienTGslAQr8rBo_cxRngqxZvRurx9mZSrMEss/edit#
	//Now using epic JSON! YEEY
	public static class OneDescriptorSerializer {
		private static JsonWriterSettings writeSettings = new JsonWriterSettings();
		private static JsonReaderSettings readSettings = new JsonReaderSettings();

		public static T Deserialize<T>(Stream stream) {
			JsonDataReader reader = new JsonDataReader(readSettings);

			return (T)reader.Deserialize(new StreamReader(stream), typeof(T));
		}

		public static void Serialize(object data, Stream stream) {
			JsonDataWriter writer = new JsonDataWriter(writeSettings);
			writer.Serialize(new StreamWriter(stream), data);
		}
	}
}