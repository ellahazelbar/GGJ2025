using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
	static class GameObject
	{
		public static IEnumerable<UnityEngine.Transform> GetTransformsInChildrenExcludingParent(this UnityEngine.Transform parent)
		{
			return parent.GetComponentsInChildren<UnityEngine.Transform>().Where(o => o != parent).ToArray();
		}

		public static IEnumerable<UnityEngine.Transform> GetChildren(this UnityEngine.Transform parent)
		{
			for (var i = 0; i < parent.childCount; i++)
				yield return parent.GetChild(i);
		}
	}
}