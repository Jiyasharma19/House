using UnityEngine;
using UnityEngine.Serialization;

namespace Needle.Engine.Components
{
	[HelpURL(Constants.DocumentationUrl)]
	public class DragControls : MonoBehaviour
	{
		[Tooltip("When enabled DragControls will show lines to visualize the plane")]
		public bool showGizmo = true;
		[Tooltip("When enabled DragControls will drag vertically when viewed from a low angle")]
		public bool useViewAngle = true;
	}
}