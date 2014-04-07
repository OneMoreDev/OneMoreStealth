using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

//https://docs.google.com/document/d/1I-lmpBienTGslAQr8rBo_cxRngqxZvRurx9mZSrMEss/edit#


namespace OneDescript {
	public class OneDescriptor {
		public Int16 ID {
			get; set;
		}
		private Dictionary<string, OValue> properties;
		public OneDescriptor() {
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
		private List<OneDescriptor> blocks;
		public DescriptorGroup() {
			blocks = new List<OneDescriptor>();
		}
		public OneDescriptor this[Int16 ID] {
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

		public static DescriptorGroup Deserialize(BinaryReader reader) {
			Action skipToBlock = () => {
				while (reader.ReadByte() != 0x3C) {}
				return;
			};

			while (reader.BaseStream.Position != reader.BaseStream.Length) {
				skipToBlock();

			}
		}

		public static void Serialize(DescriptorGroup group, BinaryWriter writer) {

		}
	}
}