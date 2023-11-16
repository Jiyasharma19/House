using UnityEngine;

namespace Needle.Engine
{
    public class WebXRPlaneTracking : MonoBehaviour
    {
        public GameObject @planeTemplate;
        [Tooltip("On Quest, Room Setup can be started automatically if no planes are detected. This is recommended as not everyone has Room Setup completed.")]
        public bool initiateRoomCaptureIfNoPlanes = true;
    }
}
