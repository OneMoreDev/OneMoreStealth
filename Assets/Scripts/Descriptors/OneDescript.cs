using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

//https://docs.google.com/document/d/1I-lmpBienTGslAQr8rBo_cxRngqxZvRurx9mZSrMEss/edit#


namespace OneDescript {
	public class Descriptor {
		public Int16 ID {
			get; set;
		}
		private Dictionary<string, OValue> properties;
		public Descriptor() {
			properties = new Dictionary<string, OValue>();
		}

		public OValue this[string property] {
			get {
				return properties[property];
			}
			set {
				properties[property] = value;
			}
		}
	}

	public class DescriptorGroup {
		private List<Descriptor> blocks;
		public DescriptorGroup() {
			blocks = new List<Descriptor>();
		}
		public Descriptor this[Int16 ID] {
			get {
				return blocks.First(desc => desc.ID == ID);
			}
			set {
				value.ID = ID;
				blocks.Remove(this[ID]);
				blocks.Add(value);
			}
		}
	}

	public static class OneDescriptorSerializer {
		public class BinaryParsingException : FormatException {}

		public static DescriptorGroup Deserialize(Stream stream) {
			BinaryReader reader = new BinaryReader(stream);
			Action skipToBlock = () => {
				while (reader.ReadByte() != 0x3C) {}
				return;
			};
			DescriptorGroup group = new DescriptorGroup();
			while (reader.BaseStream.Position != reader.BaseStream.Length) {
				skipToBlock(); //go to the start of the next block
				Descriptor block = new Descriptor();
				block.ID = reader.ReadInt16(); //read its id
				while (reader.PeekByte() != 0xC3) {
					string key = reader.ReadString();
					OValueType type = OValueType.FromByte(reader.ReadByte());
					object data = null;
					if (type.Equals(OValueType.INT)) {
						data = reader.ReadInt32();
					} else if (type.Equals(OValueType.FLOAT)) {
						data = reader.ReadSingle();
					} else if (type.Equals(OValueType.STRING)) {
						data = reader.ReadString();
					} else if (type.Equals(OValueType.REFLIST)) {
						List<short> refs = new List<short>();
						while (reader.ReadBoolean()) {
							refs.Add(reader.ReadInt16());
						}
						data = refs;
					}
					OValue value = new OValue(type, data);
					block[key] = value;
				}
			}

			//Added by Sebastian Lawe - This was keeping the project from compiling. okay :P
			return null;
		}

		public static void Serialize(DescriptorGroup group, Stream stream) {
			BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);

		}
	}

	public static class BinaryReaderExtensions {
		public static byte PeekByte(this BinaryReader reader) {
			byte data = reader.ReadByte();
			reader.BaseStream.Seek(-1, SeekOrigin.Current);
			return data;
		}
	}
}