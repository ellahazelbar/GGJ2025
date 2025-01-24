using UnityEngine;
using Unity.Burst;

namespace Extensions
{
	static class Mask
	{
		[BurstCompile]
		public static bool Contains(this LayerMask mask, int layer) => mask == (mask | layer.ToMask());

		/// <summary>
		/// Converts an index to a mask.
		/// Useful when trying to convert layer to LayerMask or NavMeshAreas to NavMesh masks.
		/// </summary>
		/// <param name="index">Index to convert</param>
		/// <returns>The created mask.</returns>
		[BurstCompile]
		public static int ToMask(this int index) => (1 << index);

		public static int LayerIndex(this LayerMask mask) => Mathf.RoundToInt(Mathf.Log(mask.value, 2));
	}
}