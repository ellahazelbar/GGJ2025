using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

namespace Extensions
{
	static class Math
	{
		public const float DEGREES = 360f;
		public const float HALF_ROTATION = DEGREES/2f;

		public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
		{
			return new(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
		}

		public static Vector3 ClampEuler(this Vector3 value, Vector3 min, Vector3 max)
		{
			return new(ClampEuler(value.x, min.x, max.x), ClampEuler(value.y, min.y, max.y), ClampEuler(value.z, min.z, max.z));
		}

		public static float ClampEuler(float value, float min, float max)
		{
			min = min.ToPositiveAngle();
			max = max.ToPositiveAngle();
			value = value.ToPositiveAngle();
			if (min > max)
			{
				min -= DEGREES;
				if (value > HALF_ROTATION)
					value -= DEGREES;
			}
			return Mathf.Clamp(value, min, max);
		}

		/// <summary>
		/// Clamps <paramref name="value"/> so that its <see cref="Quaternion.eulerAngles"/> is between <paramref name="min"/> and <paramref name="max"/>.
		/// </summary>
		/// <param name="value">Quaternion to clamp.</param>
		/// <param name="min">Min euler angles in signed degrees.</param>
		/// <param name="max">Max euler angles in signed degrees.</param>
		/// <returns><paramref name="value"/> clamped between <paramref name="min"/> and <paramref name="max"/>.</returns>
		public static Quaternion ClampQuaternion(Quaternion value, Vector3 min, Vector3 max)
		{
			return Quaternion.Euler(ClampEuler(value.eulerAngles, min, max));
		}

		/// <summary>
		/// Forces <paramref name="angleInDegrees"/> to a positive (0 to 360) angle.
		/// </summary>
		/// <returns><paramref name="angleInDegrees"/> in positive degrees.</returns>
		public static float ToPositiveAngle(this float angleInDegrees)
		{
			return Mathf.Repeat(angleInDegrees, DEGREES);
		}

		/// <summary>
		/// Forces <paramref name="angleInDegrees"/> to a signed (-180 to +180) angle.
		/// </summary>
		/// <returns><paramref name="angleInDegrees"/> in signed degrees.</returns>
		public static float ToSignedAngle(this float angleInDegrees)
		{
			return (angleInDegrees + HALF_ROTATION).ToPositiveAngle() - HALF_ROTATION;
		}

		/// <returns><paramref name="eulerAngles"/> in positive degrees.</returns>
		public static Vector3 ToPositiveAngle(this Vector3 eulerAngles)
		{
			return new(ToPositiveAngle(eulerAngles.x), ToPositiveAngle(eulerAngles.y), ToPositiveAngle(eulerAngles.z));
		}

		/// <returns><paramref name="eulerAngles"/> in signed degrees.</returns>
		public static Vector3 ToSignedAngle(this Vector3 eulerAngles)
		{
			return new(ToSignedAngle(eulerAngles.x), ToSignedAngle(eulerAngles.y), ToSignedAngle(eulerAngles.z));
		}

		public static bool IsRightOf(this UnityEngine.Transform target, UnityEngine.Transform origin) => Cross((Vector2)origin.up, Transform.Direction(origin.position, target.position)) > 0f;

		[BurstCompile]
		public static float Cross(float2 A, float2 B) => A.y * B.x - A.x * B.y;

		[BurstCompile]
		public static float TriangleWave(float f) => 2 * math.abs(f - math.floor(f + 0.5f)) - 1;

		[BurstCompile]
		public static float SquareWave(float f) => 2 * (2 * math.floor(f) - math.floor(2 * f)) + 1;

		/// <param name="rect">Rectangle to lerp over.</param>
		/// <param name="t">Relative point on the perimeter.</param>
		/// <returns>Point on <paramref name="rect"/>'s perimeter corresponding to <paramref name="t"/>, starting at the top-left corner and moving clockwise.</returns>
		public static Vector2 LerpPerimeter(this Rect rect, float t)
		{
			Vector2 center = rect.center;
			rect.center = Vector2.zero;
			float halfPerimeter = rect.width + rect.height;
			float perimeter = halfPerimeter * 2f;
			t = Mathf.Repeat(t, 1f) * perimeter;
			bool overHalf = t >= halfPerimeter;
			t = Mathf.Repeat(t, halfPerimeter);
			bool isHeight = t >= rect.width;
			Vector2 perimeterPoint;
			if (isHeight)
				perimeterPoint = new(rect.xMax, rect.yMax - (t - rect.width));
			else
				perimeterPoint = new(rect.xMin + t, rect.yMax);
			if (overHalf)
				perimeterPoint = new(-perimeterPoint.x, -perimeterPoint.y);
			return center + perimeterPoint;
		}
	}
}
