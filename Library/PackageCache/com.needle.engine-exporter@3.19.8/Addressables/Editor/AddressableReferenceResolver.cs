#if HAS_ADDRESSABLES

using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Needle.Engine.Components;
using Needle.Engine.Core.References.ReferenceResolvers;
using Needle.Engine.Gltf;
using UnityEngine;

namespace Needle.Engine.Addressables
{
	[UsedImplicitly]
	public class AddressableReferenceResolver : GltfExtensionHandlerBase, IValueResolver
	{
		public bool TryGetValue(IExportContext ctx, object instance, MemberInfo member, ref object value)
		{
			return Handle(ctx, instance, ref value);
		}

		private static bool Handle(IExportContext context, object owner, ref object result)
		{
			var path = default(string);
			if (result is UnityEngine.AddressableAssets.AssetReference ar)
			{
				if (TryExportAssetReference(ar, owner, context, ref path))
				{
					result = path;
					return true;
				}
			}
			else if (result is IList<UnityEngine.AddressableAssets.AssetReference> list)
			{
				var exportedList = new List<string>();
				foreach (var e in list)
				{
					path = default;
					if (TryExportAssetReference(e, owner, context, ref path))
						exportedList.Add(path);
				}
				if (exportedList.Count > 0)
				{
					result = exportedList;
					return true;
				}
			}
			return false;
		}

		private static bool TryExportAssetReference(UnityEngine.AddressableAssets.AssetReference ar, object owner, IExportContext context, ref string res)
		{
			var asset = ar.editorAsset;
			var go = asset as GameObject;
			if (!asset || !go)
			{
				res = null;
				return false;
			}
			var exp = default(IExportableObject);
			var addedExportable = false;
			try
			{
				// for addressables ensure that the thing is exportable
				if (!go.TryGetComponent(out exp))
				{
					var obj = go.AddComponent<GltfObject>();
					exp = obj;
					addedExportable = true;
				}
				return GltfReferenceResolver.ExportReferencedObject(owner, go, go, exp, context, ref res);
			}
			finally
			{
				if (addedExportable && exp is Object addedObject)
				{
					Object.DestroyImmediate(addedObject, true);
				}
			}
		}
	}
}

#endif