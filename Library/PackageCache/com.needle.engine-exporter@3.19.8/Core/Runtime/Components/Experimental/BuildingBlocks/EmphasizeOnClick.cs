using UnityEngine;

namespace Needle.Engine.Components
{
    [AddComponentMenu(USDZExporter.USDZOnlyMenuPrefix + "Emphasize on Click" + USDZExporter.USDZOnlyMenuTags)]
    public class EmphasizeOnClick : MonoBehaviour
    {
        public GameObject target;
        public float duration = 0.5f;
        // public float moveDistance = 0.5f; // not supported in QuickLook right now
        public MotionType motionType = MotionType.Bounce;
        
        private void OnDrawGizmosSelected()
        {
            if (!target || transform == target.transform) return;
            SetActiveOnClick.DrawLineAndBounds(transform, target.transform);
        }
    }

    public enum MotionType
    {
        Pop = 0,
        Blink = 1,
        Bounce = 2,
        Flip = 3,
        Float = 4,
        Jiggle = 5,
        Pulse = 6,
        Spin = 7,
    }
}