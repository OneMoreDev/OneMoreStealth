using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

//Specification: https://docs.google.com/document/d/1I-lmpBienTGslAQr8rBo_cxRngqxZvRurx9mZSrMEss/edit#
using System.Collections;
using UnityEngine;


namespace OneDescript {
	public class Descriptor : IEnumerable<KeyValuePair<string, OValue>> {
		public int ID {
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

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<string, OValue>> GetEnumerator() {
			return properties.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator() {
			return properties.GetEnumerator();
		}

		#endregion
	}

	public class DescriptorGroup : IEnumerable<Descriptor> {
		private List<Descriptor> blocks;
		public DescriptorGroup() {
			blocks = new List<Descriptor>();
		}
		public Descriptor this[int ID] {
			get {
				return blocks.First(desc => {return desc.ID == ID;});
			}
			set {
				value.ID = ID;
				blocks.Add(value);
			}
		}

		#region IEnumerable implementation
		IEnumerator<Descriptor> IEnumerable<Descriptor>.GetEnumerator() {
			return blocks.GetEnumerator();
		}
		#endregion
		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator() {
			return blocks.GetEnumerator();
		}
		#endregion
	}

	public static class OneDescriptorSerializer {
		public class BinaryParsingException : FormatException {}

		public static readonly byte BLOCK_INITIALIZER = 0x3C;
		public static readonly byte BLOCK_FINALIZER = 0xC3;

		public static DescriptorGroup Deserialize(Stream stream) {
			using (BinaryReader reader = new BinaryReader(stream)) {
				Action skipToBlock = () => {
					while (reader.ReadByte() != BLOCK_INITIALIZER) {}
					return;
				};
				DescriptorGroup group = new DescriptorGroup();
				while (reader.BaseStream.Position != reader.BaseStream.Length) {
					skipToBlock();
					Descriptor block = new Descriptor();
					block.ID = reader.ReadInt32();
					while (reader.PeekByte() != BLOCK_FINALIZER) {
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
							List<int> refs = new List<int>();
							while (reader.ReadBoolean()) {
								refs.Add(reader.ReadInt32());
							}
							data = refs;
						}
						OValue value = new OValue(type, data);
						block[key] = value;
					}
					group[block.ID] = block;
				}
				return group;
			}
		}

		public static void Serialize(DescriptorGroup group, Stream stream) {
			using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8)) {
				foreach (Descriptor descriptor in group) {
					writer.Write(BLOCK_INITIALIZER);
					writer.Write(descriptor.ID);
					foreach (KeyValuePair<string, OValue> kvp in descriptor) {
						writer.Write(kvp.Key);
						OValue value = kvp.Value;
						writer.Write(value.type.typeByte);
						Debug.Log(value.type.typeByte);
						if (value.type.Equals(OValueType.INT)) {
							writer.Write((int)value.GetValue(value.type));
						} else if (value.type.Equals(OValueType.FLOAT)) {
							writer.Write((float)value.GetValue(value.type));
						} else if (value.type.Equals(OValueType.STRING)) {
							writer.Write((string)value.GetValue(value.type));
						} else if (value.type.Equals(OValueType.REFLIST)) {
							List<int> references = (List<int>)value.GetValue(OValueType.REFLIST);
							for (int i = 0; i < references.Count; i++) {
								bool hasNext = i+1 == references.Count;
								writer.Write(hasNext);
								writer.Write(references[i]);
							}
						}
					}
					writer.Write(BLOCK_FINALIZER);
				}
			}
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