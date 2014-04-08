using UnityEngine;
using System.Collections;

namespace OneDescript {
	public class DescriptorComponent : MonoBehaviour {
		public DescriptorGroup group {get; private set;}

		public DescriptorComponent() : base() {
			group = new DescriptorGroup();
			group[0] = new Descriptor();
		}

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}