using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Extensions
{
	static class AI
	{
		private static readonly NavMeshPath _path = new();

		public static float NavMeshCalculateDistance(Vector3 sourcePosition, Vector3 targetPosition, int areaMask)
		{
			_path.ClearCorners();
			if (NavMesh.CalculatePath(sourcePosition, targetPosition, areaMask, _path))
				return _path.NavMeshPathTotalDistance();
			else
				return 0f;
		}

		public static bool TryNavMeshGetPath(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, out IReadOnlyList<Vector3> path)
		{
			_path.ClearCorners();
			if (NavMesh.CalculatePath(sourcePosition, targetPosition, areaMask, _path))
			{
				path = _path.corners;
				return true;
			}
			path = null;
			return false;
		}

		public static float NavMeshPathTotalDistance(this NavMeshPath path)
		{
			var corners = path.corners;
			float sum = 0;
			for (int i = 1; i < corners.Length; i++)
				sum += Vector2.Distance(corners[i-1], corners[i]);
			return sum;
		}
	}
}
