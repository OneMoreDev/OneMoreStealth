using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

//Specification: https://docs.google.com/document/d/1I-lmpBienTGslAQr8rBo_cxRngqxZvRurx9mZSrMEss/edit#
using System.Collections;
using UnityEngine;


namespace OneDescript {
	/// <summary>
	/// Holds a arbirtrary number of properties accessable via an indexer.
	/// </summary>
	/// <example>
	/// Descriptor desc = new Descriptor();
	/// desc["number"] = new OValue(OValueType.INT, 1337);
	/// desc["somethingElse"] = new OValue(OValueType.STRING, "herpty derp");
	/// </example>
	public class Descriptor : IEnumerable<KeyValuePair<string, OValue>> {
		public int ID {
			get; set;
		}
		private Dictionary<string, OValue> properties;
		public Descriptor() {
			properties = new Dictionary<string, OValue>();
		}
		/// <summary>
		/// Gets or sets a property value.
		/// </summary>
		/// <param name="property">The property key.</param>
		public OValue this[string property] {
			get {
				return properties[property];
			}
			set {
				properties[property] = value;
			}
		}

		/// <summary>
		/// Remove the specified property.
		/// </summary>
		/// <param name="property">Property key to remove.</param>
		public void Remove(string property) {
			properties.Remove(property);
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

	/// <summary>
	/// Holds an arbitrary amount of <see cref="Descriptor"/>s, accessible from the indexer.
	/// </summary>
	public class DescriptorGroup : IEnumerable<Descriptor> {
		private List<Descriptor> blocks;
		public int Count {
			get {
				return blocks.Count;
			}
		}
		public DescriptorGroup() {
			blocks = new List<Descriptor>();
		}
		/// <summary>
		/// Gets or sets the descriptor on the specified index.
		/// IDs are automatically adjusted to reflect this.
		/// </summary>
		/// <param name="ID">ID of the descriptor</param>
		public Descriptor this[int ID] {
			get {
				return blocks.First(desc => {return desc.ID == ID;});
			}
			set {
				value.ID = ID;
				blocks.Add(value);
			}
		}

		/// <summary>
		/// Remove the specified descriptor.
		/// </summary>
		/// <param name="desc">Descriptor to remove.</param>
		public void Remove(Descriptor desc) {
			blocks.Remove(desc);
		}
		/// <summary>
		/// Removes the descriptor with the given id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void RemoveAt(int id) {
			blocks.Remove(this[id]);
		}

		/// <summary>
		/// Analogous to the indexer.
		/// </summary>
		/// <returns>The descriptor.</returns>
		/// <param name="reference">Reference.</param>
		public Descriptor FetchReference(int reference) {
			return this[reference];
		}

		/// <summary>
		/// Dereference the specified references.
		/// </summary>
		/// <param name="refs">References.</param>
		public List<Descriptor> Dereference(IEnumerable<int> refs) {
			List<Descriptor> descriptors = new List<Descriptor> ();
			foreach (Descriptor reference in refs.Select(refer => this[refer])) {
				descriptors.Add(reference);		
			}
			return descriptors;
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

		private static readonly byte BLOCK_INITIALIZER = 0x3C;
		private static readonly byte BLOCK_FINALIZER = 0xC3;

		/// <summary>
		/// Deserialize the specified input stream into a DescriptorGroup. The stream must be seekable.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static DescriptorGroup Deserialize(Stream stream) {
			using (BinaryReader reader = new BinaryReader(stream)) {
				DescriptorGroup group = new DescriptorGroup();
				while (reader.BaseStream.Position != reader.BaseStream.Length) {
					reader.ReadByte(); //read block initiator
					Descriptor block = new Descriptor();
					block.ID = reader.ReadInt32();
					while (reader.PeekByte() != BLOCK_FINALIZER) {
						string key = reader.ReadString();
						byte rawType = reader.ReadByte();
						OValueType type = OValueType.FromByte(rawType);
						object data = null;
						if (type.Equals(OValueType.INT)) {
							data = reader.ReadInt32();
						} else if (type.Equals(OValueType.FLOAT)) {
							data = reader.ReadSingle();
						} else if (type.Equals(OValueType.STRING)) {
							data = reader.ReadString();
						} else if (type.Equals(OValueType.REFLIST)) {
							int length = reader.ReadInt32();
							List<int> refs = new List<int>();
							for (int i = 0; i < length; i++) {
								refs.Add(reader.ReadInt32());
							}
							data = refs;
						}
						OValue value = new OValue(type, data);
						block[key] = value;
					}
					reader.ReadByte(); //read block finalizer
					group[block.ID] = block;
				}
				return group;
			}
		}

		/// <summary>
		/// Serialize the specified group to the given output stream.
		/// </summary>
		/// <param name="group">Group.</param>
		/// <param name="stream">Stream.</param>
		public static void Serialize(DescriptorGroup group, Stream stream) {
			using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8)) {
				foreach (Descriptor descriptor in group) {
					writer.Write(BLOCK_INITIALIZER);
					writer.Write(descriptor.ID);
					foreach (KeyValuePair<string, OValue> kvp in descriptor) {
						writer.Write(kvp.Key);
						OValue value = kvp.Value;
						writer.Write(value.type.typeByte);
						if (value.type.Equals(OValueType.INT)) {
							writer.Write((int)value.GetValue(value.type));
						} else if (value.type.Equals(OValueType.FLOAT)) {
							writer.Write((float)value.GetValue(value.type));
						} else if (value.type.Equals(OValueType.STRING)) {
							writer.Write((string)value.GetValue(value.type));
						} else if (value.type.Equals(OValueType.REFLIST)) {
							List<int> references = (List<int>)value.GetValue(OValueType.REFLIST);
							writer.Write(references.Count);
							for (int i = 0; i < references.Count; i++) {
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