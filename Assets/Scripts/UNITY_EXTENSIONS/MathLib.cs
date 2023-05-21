// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MathLib
using System;
using UnityEngine;

public static class MathLib
{
	public enum Axis
	{
		X,
		Y,
		Z
	}

	public const float Tau = (float)Math.PI * 2f;

	public const double DegToRad = Math.PI / 180.0;

	public const double RadToDeg = 180.0 / Math.PI;

	public static Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float projectileVelocity, Vector3 targetPosition, Vector3 targetVelocity)
	{
		Vector3 targetRelativePosition = targetPosition - shooterPosition;
		Vector3 vector = targetVelocity - shooterVelocity;
		float num = FirstOrderInterceptTime(projectileVelocity, targetRelativePosition, vector);
		return targetPosition + num * vector;
	}

	private static float FirstOrderInterceptTime(float projectileVelocity, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
	{
		float sqrMagnitude = targetRelativeVelocity.sqrMagnitude;
		if (sqrMagnitude < 0.001f)
		{
			return 0f;
		}
		float num = sqrMagnitude - projectileVelocity * projectileVelocity;
		if (Mathf.Abs(num) < 0.001f)
		{
			return Mathf.Max((0f - targetRelativePosition.sqrMagnitude) / (2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition)), 0f);
		}
		float num2 = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
		float sqrMagnitude2 = targetRelativePosition.sqrMagnitude;
		float num3 = num2 * num2 - 4f * num * sqrMagnitude2;
		if (num3 > 0f)
		{
			float num4 = (0f - num2 + Mathf.Sqrt(num3)) / (2f * num);
			float num5 = (0f - num2 - Mathf.Sqrt(num3)) / (2f * num);
			if (num4 > 0f)
			{
				if (num5 > 0f)
				{
					return Mathf.Min(num4, num5);
				}
				return num4;
			}
			return Mathf.Max(num5, 0f);
		}
		if (num3 < 0f)
		{
			return 0f;
		}
		return Mathf.Max((0f - num2) / (2f * num), 0f);
	}

	public static float Map(float v, float fromMin, float fromMax, float toMin, float toMax)
	{
		v = Mathf.Clamp(v, fromMin, fromMax);
		float num = v - fromMin;
		float num2 = fromMax - fromMin;
		float num3 = num / num2;
		return (toMax - toMin) * num3 + toMin;
	}

	public static float NormalizeAngle(float angle)
	{
		angle %= 360f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		return angle;
	}

	public static float SignedAngle(float angle)
	{
		angle = NormalizeAngle(angle);
		if (angle > 180f)
		{
			angle = 0f - (360f - angle);
		}
		return angle;
	}

	public static float ClampAngle(float angle, float from, float to)
	{
		angle = NormalizeAngle(angle);
		if (angle > 180f)
		{
			angle = 0f - (360f - angle);
		}
		angle = Mathf.Clamp(angle, from, to);
		if (angle < 0f)
		{
			angle = 360f + angle;
		}
		return angle;
	}

	public static bool Compare(float a, float b, float epsilon = 0.0001f)
	{
		return Mathf.Abs(a - b) <= epsilon;
	}

	public static bool Compare(Vector3 a, Vector3 b, float epsilon = 0.0001f)
	{
		if (Mathf.Abs(a.x - b.x) > epsilon)
		{
			return false;
		}
		if (Mathf.Abs(a.y - b.y) > epsilon)
		{
			return false;
		}
		if (Mathf.Abs(a.z - b.z) > epsilon)
		{
			return false;
		}
		return true;
	}

	public static bool CompareSign(float a, float b)
	{
		return Mathf.Approximately(Mathf.Sign(a), Mathf.Sign(b));
	}

	public static float LerpAngleUnclamped(float a, float b, float t)
	{
		float num = Mathf.Repeat(b - a, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return a + num * t;
	}

	public static Quaternion Damp(Quaternion a, Quaternion b, float smoothing, float deltaTime)
	{
		return Quaternion.Slerp(a, b, 1f - Mathf.Exp((0f - smoothing) * deltaTime));
	}

	public static Vector3 DampDirection(Vector3 a, Vector3 b, float smoothing, float deltaTime)
	{
		return Vector3.Slerp(a, b, 1f - Mathf.Exp((0f - smoothing) * deltaTime));
	}

	public static Vector3 Damp(Vector3 a, Vector3 b, float smoothing, float deltaTime)
	{
		return Vector3.Lerp(a, b, 1f - Mathf.Exp((0f - smoothing) * deltaTime));
	}

	public static Color Damp(Color a, Color b, float smoothing, float deltaTime)
	{
		return Color.Lerp(a, b, 1f - Mathf.Exp((0f - smoothing) * deltaTime));
	}

	public static float Damp(float a, float b, float smoothing, float deltaTime)
	{
		return Mathf.Lerp(a, b, 1f - Mathf.Exp((0f - smoothing) * deltaTime));
	}

	public static float DampAngle(float a, float b, float smoothing, float deltaTime)
	{
		return Mathf.LerpAngle(a, b, 1f - Mathf.Exp((0f - smoothing) * deltaTime));
	}

	public static float InverseLerp(Vector3 a, Vector3 b, Vector3 t)
	{
		Vector3 vector = b - a;
		return Mathf.Clamp01(Vector3.Dot(t - a, vector) / Vector3.Dot(vector, vector));
	}

	public static float InverseLerpUnclamped(Vector3 a, Vector3 b, Vector3 t)
	{
		Vector3 vector = b - a;
		return Vector3.Dot(t - a, vector) / Vector3.Dot(vector, vector);
	}

	public static Vector3 RandomOnTriangle(Vector3 a, Vector3 b, Vector3 c)
	{
		float value = UnityEngine.Random.value;
		float value2 = UnityEngine.Random.value;
		return (1f - Mathf.Sqrt(value)) * a + Mathf.Sqrt(value) * (1f - value2) * b + value2 * Mathf.Sqrt(value) * c;
	}

	public static bool PlanePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Vector3 plane1Normal, Vector3 plane1Position, Vector3 plane2Normal, Vector3 plane2Position)
	{
		linePoint = Vector3.zero;
		lineVec = Vector3.zero;
		lineVec = Vector3.Cross(plane1Normal, plane2Normal);
		Vector3 vector = Vector3.Cross(plane2Normal, lineVec);
		float num = Vector3.Dot(plane1Normal, vector);
		if (Mathf.Abs(num) > 0.006f)
		{
			Vector3 rhs = plane1Position - plane2Position;
			float num2 = Vector3.Dot(plane1Normal, rhs) / num;
			linePoint = plane2Position + num2 * vector;
			return true;
		}
		return false;
	}

	public static bool IntersectRaySphere(Vector3 start, Vector3 direction, Vector3 center, float radius, out Vector3 point)
	{
		point = Vector3.zero;
		Vector3 vector = start - center;
		float num = Vector3.Dot(vector, direction);
		float num2 = Vector3.Dot(vector, vector) - radius * radius;
		if (num2 > 0f && num > 0f)
		{
			return false;
		}
		float num3 = num * num - num2;
		if (num3 < 0f)
		{
			return false;
		}
		float a = 0f - num - Mathf.Sqrt(num3);
		a = Mathf.Max(a, 0f);
		point = start + a * direction;
		return true;
	}

	public static Vector3 ClosestPointOnLineSegment(Vector3 start, Vector3 end, Vector3 position)
	{
		Vector3 lhs = position - start;
		Vector3 vector = end - start;
		float value = Vector3.Dot(lhs, vector) / vector.sqrMagnitude;
		value = Mathf.Clamp01(value);
		return start + value * vector;
	}

	public static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
	{
		float num = SignedDistancePlanePoint(planeNormal, planePoint, point);
		num *= -1f;
		Vector3 vector = planeNormal.normalized * num;
		return point + vector;
	}

	public static float SignedDistancePlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
	{
		return Vector3.Dot(planeNormal, point - planePoint);
	}

	public static Vector3 CalculateCubicBezierPoint(Vector3 start, Vector3 end, Vector3 startHandle, Vector3 endHandle, float t)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float num4 = num3 * num;
		float num5 = num2 * t;
		return num4 * start + 3f * num3 * t * startHandle + 3f * num * num2 * endHandle + num5 * end;
	}

	public static float SmoothMin(float a, float b, float k)
	{
		k = Mathf.Max(0f, k);
		float num = Mathf.Max(0f, Mathf.Min(1f, (b - a + k) / (2f * k)));
		return a * num + b * (1f - num) - k * num * (1f - num);
	}

	public static float SmoothMax(float a, float b, float k)
	{
		k = Mathf.Min(0f, 0f - k);
		float num = Mathf.Max(0f, Mathf.Min(1f, (b - a + k) / (2f * k)));
		return a * num + b * (1f - num) - k * num * (1f - num);
	}

	public static bool DistanceCheck(Vector2 a, Vector2 b, float distance)
	{
		return (a - b).sqrMagnitude <= distance * distance;
	}

	public static bool DistanceCheck(Vector3 a, Vector3 b, float distance)
	{
		return (a - b).sqrMagnitude <= distance * distance;
	}

	public static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 vector = b - a;
		Vector3 vector2 = c - a;
		return new Vector3(vector.y * vector2.z - vector.z * vector2.y, vector.z * vector2.x - vector.x * vector2.z, vector.x * vector2.y - vector.y * vector2.x).normalized;
	}

	public static float ScreenSpaceDistance(Camera camera, Vector3 a, Vector3 b)
	{
		if (!camera)
		{
			return float.PositiveInfinity;
		}
		Vector3 b2 = new Vector3(1f / (float)camera.pixelWidth, 1f / (float)camera.pixelHeight, 0f);
		a = Vector3.Scale(camera.WorldToScreenPoint(a), b2);
		b = Vector3.Scale(camera.WorldToScreenPoint(b), b2);
		return Vector3.Distance(a, b);
	}

	public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
	{
		float num = Vector3.Dot(point - linePoint, lineVec);
		return linePoint + lineVec * num;
	}

	public static Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
	{
		Vector3 vector = ProjectPointOnLine(linePoint1, (linePoint2 - linePoint1).normalized, point);
		return PointOnWhichSideOfLineSegment(linePoint1, linePoint2, vector) switch
		{
			0 => vector, 
			1 => linePoint1, 
			2 => linePoint2, 
			_ => Vector3.zero, 
		};
	}

	public static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
	{
		Vector3 rhs = linePoint2 - linePoint1;
		Vector3 lhs = point - linePoint1;
		if (Vector3.Dot(lhs, rhs) > 0f)
		{
			if (lhs.magnitude <= rhs.magnitude)
			{
				return 0;
			}
			return 2;
		}
		return 1;
	}

	public static Vector3 GetRandomDirectionInCone(Vector3 axis, float maxAngle)
	{
		float num = Mathf.Tan((float)Math.PI / 180f * maxAngle / 2f);
		Vector2 vector = UnityEngine.Random.insideUnitCircle * num;
		return (axis + Quaternion.FromToRotation(Vector3.forward, axis) * new Vector3(vector.x, vector.y, 0f)).normalized;
	}

	public static Vector2 RotateClockwise(Vector2 direction, float angle)
	{
		angle *= (float)Math.PI / 180f;
		float num = Mathf.Sin(angle);
		float num2 = Mathf.Cos(angle);
		return new Vector2(direction.x * num2 - direction.y * num, direction.y * num2 + direction.x * num);
	}

	public static Vector3 GetRandomDirection2D(Axis axis = Axis.Y)
	{
		Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
		return axis switch
		{
			Axis.X => new Vector3(0f, normalized.x, normalized.y), 
			Axis.Y => new Vector3(normalized.x, 0f, normalized.y), 
			_ => new Vector3(normalized.x, normalized.y, 0f), 
		};
	}

	public static Vector3 GetRandomInsideCircle2D(Axis axis = Axis.Y)
	{
		Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
		return axis switch
		{
			Axis.X => new Vector3(0f, insideUnitCircle.x, insideUnitCircle.y), 
			Axis.Y => new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y), 
			_ => new Vector3(insideUnitCircle.x, insideUnitCircle.y, 0f), 
		};
	}

	public static bool CompareVelocities(Vector3 a, Vector3 b, float maxAngle, float maxSpeed)
	{
		float magnitude = a.magnitude;
		float magnitude2 = b.magnitude;
		if (Mathf.Abs(magnitude - magnitude2) > maxSpeed)
		{
			return false;
		}
		a /= magnitude;
		b /= magnitude2;
		if (Vector3.Angle(a, b) > maxAngle)
		{
			return false;
		}
		return true;
	}
}
