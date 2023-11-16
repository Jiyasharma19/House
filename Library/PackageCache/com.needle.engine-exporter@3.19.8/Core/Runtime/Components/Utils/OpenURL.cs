using UnityEngine;

namespace Needle.Engine.Components
{
	public enum OpenURLMode
	{
		NewTab,
		SameTab,
		NewWindow
	}

	public class OpenURL : MonoBehaviour
	{
		public bool clickable = true;
		public string url = "https://needle.tools";
		public OpenURLMode mode;
		public void Open() {}
	}
}