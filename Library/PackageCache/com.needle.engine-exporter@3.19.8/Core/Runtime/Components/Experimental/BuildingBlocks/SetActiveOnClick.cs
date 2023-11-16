using System.Linq;
using UnityEngine;

namespace Needle.Engine.Components
{
	[AddComponentMenu(USDZExporter.ComponentMenuPrefix + "Set Active on Click" + USDZExporter.ComponentMenuTags)]
	public class SetActiveOnClick : MonoBehaviour
	{
		public Transform target;
		public bool toggleOnClick = false;
		public bool hideSelf = true;
		public bool targetState = true;

		private void OnDrawGizmosSelected()
		{
			if (!target) return;
			DrawLineAndBounds(transform, target);
		}

		internal static void DrawLineAndBounds(Transform from, Transform target)
		{
			if (!from || !target) return;
			
			var c = Gizmos.color;
			Gizmos.color = new Color(1,1,1,0.3f);
			DrawBounds(target, out var bounds);
			Gizmos.DrawLine(from.position, bounds.center);
			Gizmos.color = c;
		}
		
		internal static void DrawBounds(Transform target, out Bounds bounds)
		{
			if (!target) {
				bounds = new Bounds();
				return;
			}
			
			bounds = new Bounds(target.position, Vector3.zero);

			// get bounds of target
			var renderers = target.GetComponentsInChildren<Renderer>();
			if (!renderers.Any()) return;
			
			bounds = renderers[0].bounds;
			for (var i = 1; i < renderers.Length; i++)
				bounds.Encapsulate(renderers[i].bounds);
			
			// draw bounds
			Gizmos.DrawWireCube(bounds.center, bounds.size * 1.15f);
		}
	}
}