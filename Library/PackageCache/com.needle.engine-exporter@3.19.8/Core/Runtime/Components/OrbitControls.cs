using UnityEngine;
using UnityEngine.Animations;

namespace Needle.Engine.Components
{
	[HelpURL(Constants.DocumentationUrl)]
	public class OrbitControls : MonoBehaviour
	{
		public bool enableRotate = true;
        public bool autoRotate = false;
		public float autoRotateSpeed = .2f;
		[Tooltip("When enabled the scene content will be automatically fit into the view")]
		public bool autoFit = false;
		public bool enableKeys = false;
		public bool enableDamping = true;
		[Range(0.001f, 1), Tooltip("Low values translate to more damping")]
		public float dampingFactor = .1f;
		public bool enableZoom = true;
		public float minZoom = 0;
		public float maxZoom = float.PositiveInfinity; 
		public bool enablePan = true;
		public LookAtConstraint lookAtConstraint;
		public bool middleClickToFocus = true;
		public bool doubleClickToFocus = true;


		private void OnEnable()
		{
			
		}
	}
}