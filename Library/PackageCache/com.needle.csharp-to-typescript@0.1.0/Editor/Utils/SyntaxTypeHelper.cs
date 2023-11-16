#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Needle.Engine.Codegen.Utils
{
	public static class SyntaxTypeHelper
	{
		public static string Parse(TypeSyntax type, out string serializableType, IImportHandler importHandler)
		{
			string typeName = null!;
			serializableType = null!;

			switch (type)
			{
				case PredefinedTypeSyntax predefinedTypeSyntax:
					switch (predefinedTypeSyntax.Keyword.Kind())
					{
						case SyntaxKind.UIntKeyword:
						case SyntaxKind.IntKeyword:
						case SyntaxKind.FloatKeyword:
						case SyntaxKind.DoubleKeyword:
							typeName = "number";
							break;
						case SyntaxKind.ULongKeyword:
						case SyntaxKind.LongKeyword:
							typeName = "BigInt";
							break;
						case SyntaxKind.CharKeyword:
						case SyntaxKind.StringKeyword:
							typeName = "string";
							break;
						case SyntaxKind.BoolKeyword:
							typeName = "boolean";
							break;
					}
					break;

				case IdentifierNameSyntax identifierNameSyntax:
					typeName = ToNeedleEngineType(identifierNameSyntax.Identifier.Text, importHandler,
						out serializableType);
					break;

				case QualifiedNameSyntax qualifiedNameSyntax:
					typeName = ToNeedleEngineType(qualifiedNameSyntax.ToString(), importHandler, out serializableType);
					break;

				case ArrayTypeSyntax arrayTypeSyntax:
					var arrayType = Parse(arrayTypeSyntax.ElementType, out serializableType, importHandler);
					typeName = "Array<" + arrayType + ">";
					break;

				case GenericNameSyntax genericNameSyntax:
					switch (genericNameSyntax.Identifier.Text)
					{
						case "UnityEvent":
							typeName = ToNeedleEngineType("UnityEvent", importHandler, out serializableType);
							break;
						case "IEnumerable":
						case "IList":
						case "ICollection":
						case "List":
							var genericType = Parse(genericNameSyntax.TypeArgumentList.Arguments[0],
								out serializableType, importHandler);
							typeName = "Array<" + genericType + ">";
							break;
						case "HashSet":
							var hashSetType = Parse(genericNameSyntax.TypeArgumentList.Arguments[0],
								out serializableType, importHandler);
							typeName = "Set<" + hashSetType + ">";
							break;
						case "IDictionary":
						case "Dictionary":
							var keyType = Parse(genericNameSyntax.TypeArgumentList.Arguments[0], out _, importHandler);
							var valueType = Parse(genericNameSyntax.TypeArgumentList.Arguments[1], out _,
								importHandler);
							typeName = "Map<" + keyType + ", " + valueType + ">";
							break;
					}
					break;

				default:
					// unknown type
					break;
			}
			return typeName;
		}

		public static string ToNeedleEngineType(string name, IImportHandler importHandler, out string serializableType)
		{
			var typeName = name;
			serializableType = typeName;
			switch (typeName)
			{
				case "AssetReference":
				case "GameObject":
				case "Transform":
					typeName = "Object3D";
					serializableType = typeName;
					importHandler.RegisterImport(serializableType, "three");
					break;
				case "MonoBehaviour":
				case "Component":
					typeName = "Behaviour";
					serializableType = typeName;
					importHandler.RegisterImport(serializableType, "@needle-tools/engine");
					break;
				case "UnityEvent":
					typeName = "EventList";
					serializableType = typeName;
					importHandler.RegisterImport(serializableType, "@needle-tools/engine");
					break;
				case "Matrix4x4":
					typeName = "Matrix4";
					serializableType = typeName;
					RegisterThreeType(serializableType, importHandler);
					break;
				case "RenderTexture":
					typeName = "RenderTexture";
					serializableType = typeName;
					importHandler.RegisterImport(serializableType, "@needle-tools/engine");
					break;
				case "Texture2D":
					typeName = "Texture";
					serializableType = typeName;
					RegisterThreeType(serializableType, importHandler);
					break;
				case "Color":
					typeName = "RGBAColor";
					serializableType = typeName;
					importHandler.RegisterImport(serializableType, "@needle-tools/engine");
					break;
				case "Vector2":
				case "Vector3":
				case "Vector4":
				case "Quaternion":
				case "Mesh":
				case "Material":
					RegisterThreeType(serializableType, importHandler);
					break;
				default:
					break;
			}

			return typeName;
		}

		private static void RegisterThreeType(string type, IImportHandler importHandler)
		{
			importHandler.RegisterImport(type, "three");
		}
	}
}