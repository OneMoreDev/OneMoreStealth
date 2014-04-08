using UnityEngine;
using System.Collections;

namespace OneDescript {
	/// <summary>
	/// This class makes possible the edition of descriptors 
	/// right from the Inspector, and also allows referencing
	/// such descriptors to other classes.
	/// </summary>
	public class DescriptorComponent : MonoBehaviour {
		/// <summary>
		/// The descriptor group represented by this component.
		/// </summary>
		/// <value>The group.</value>
		public DescriptorGroup group {get; set;}

		public DescriptorComponent() : base() {
			group = new DescriptorGroup();
		}
	}
}