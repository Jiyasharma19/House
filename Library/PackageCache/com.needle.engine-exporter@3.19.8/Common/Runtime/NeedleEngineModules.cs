using UnityEngine;

namespace Needle.Engine
{
	public enum PhysicsEngine
	{
		None = 0,
		Auto = 1,
		Rapier = 2,
	}
	
	public class NeedleEngineModules : MonoBehaviour
	{
		public PhysicsEngine PhysicsEngine = PhysicsEngine.Auto;
	}
	
	internal class PhysicsConfig : IBuildConfigProperty
	{
		public string Key => "useRapier";
		
		public object GetValue(string projectDirectory)
		{
			var mod = Object.FindObjectOfType<NeedleEngineModules>();
			if (mod)
			{
				if (mod.PhysicsEngine == PhysicsEngine.Auto)
				{
					if(UsePhysicsAuto()) return true;
				}
				
				return mod.PhysicsEngine == PhysicsEngine.Rapier;
			}
			return true;
		}

		public static bool UsePhysicsAuto()
		{
			// TODO: this doesnt check if any referenced prefab or scene has a collider or rigidbody
			if (Object.FindObjectOfType<Collider>()) return true;
			if (Object.FindObjectOfType<Rigidbody>()) return true;
			return false;
		}
	}
}