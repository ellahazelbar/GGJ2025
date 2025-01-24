using UnityEngine;

namespace Extensions
{
	static class Physics2D
	{
		public static Vector2 GetAverageContactPoint(this Collision2D collision)
		{
			var points = new ContactPoint2D[collision.contactCount];
			collision.GetContacts(points);
			Vector2 averagePoint = Vector2.zero;
			foreach (var point in points)
				averagePoint += point.point;
			averagePoint /= points.Length;
			return averagePoint;
		}

		public static float GetAverageImpulse(this Collision2D collision)
		{
			var points = new ContactPoint2D[collision.contactCount];
			collision.GetContacts(points);
			float averageImpulse = 0;
			foreach (var point in points)
				averageImpulse += point.normalImpulse;
			averageImpulse /= points.Length;
			return averageImpulse;
		}
	}
}