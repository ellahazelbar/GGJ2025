using UnityEngine;
using MoreMountains.Tools;

namespace Extensions
{
	static class Transform
	{
		public static Vector2 Direction(Vector2 source, Vector2 target) => target - source;

		public static Vector2 Direction(this UnityEngine.Transform source, Vector2 target) => Direction(source.position, target);

		public static Vector2 Direction(this UnityEngine.Transform source, UnityEngine.Transform target) => Direction(source.position, target.position);

		public static void LookAt2D(this UnityEngine.Transform transform, Vector2 target) => transform.right = Direction(transform.position, target);

		public static void LookAt2D(this UnityEngine.Transform transform, UnityEngine.Transform target) => LookAt2D(transform, target.position);

		/// <summary>
		/// Changes the rotation of <paramref name="transform"/>, and flips it when <paramref name="direction"/> becomes negative.
		/// </summary>
		/// <param name="transform">Target Transform.</param>
		/// <param name="direction">Direction to look to.</param>
		/// <param name="maxDelta">Max angular delta.</param>
		/// <param name="defaultXScale">Default X scale to use.</param>
		public static void LookAt2DFlip(this UnityEngine.Transform transform, Vector2 direction, float maxDelta = 360f, float defaultXScale = 1f)
		{
			if (direction.magnitude < 0.001f)
				return;
			float polarity = direction.x < 0 ? -1f : 1f;
			transform.eulerAngles = GetRotation();
			transform.localScale = GetScale();

			Vector3 GetRotation() => transform.eulerAngles.MMSetZ(Mathf.MoveTowardsAngle(transform.eulerAngles.z, direction.DegFromDirection(), maxDelta * polarity * Time.deltaTime));

			Vector3 GetScale() => transform.localScale.MMSetX(defaultXScale * polarity);
		}

		public static Vector2 Rotate2DDeg(this Vector2 vector, float degrees) => vector.Rotate2DRad(degrees * Mathf.Deg2Rad);

		public static Vector2 Rotate2DRad(this Vector2 vector, float radians)
		{
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);
			return new((cos * vector.x) - (sin * vector.y), (sin * vector.x) + (cos * vector.y));
		}

		public static Vector2 DirectionFromDeg(float degrees) => DirectionFromRad(degrees * Mathf.Deg2Rad);

		public static Vector2 DirectionFromRad(float radians) => new(Mathf.Cos(radians), Mathf.Sin(radians));

		public static float DegFromDirection(this Vector2 direction) => Vector2.SignedAngle(Vector2.right, direction);

		public static float RadFromDirection(this Vector2 direction) => DegFromDirection(direction) * Mathf.Deg2Rad;
	}
}
