using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathStuff
{
    // всякие полезные штуки с математическими операциями
    public static class MathHelper
    {
        // не используется радиус круга для квадрата с половиной стороны которая один
        public const float circleRadiusModifier = 1.1283791671f;

        public static Vector3 RandomizedRotation(Vector3 localAngles)
        {
            return new Vector3(localAngles.x, RandomEulerAngle(), localAngles.z);
        }

        public static Quaternion RandomRotationAroundY()
        {
            return Quaternion.Euler(0f, RandomEulerAngle(), 0f);
        }

        public static Quaternion RandomRotationAroundYWithoutParentPrefabCorrect()
        {
            return Quaternion.Euler(-90f, RandomEulerAngle(), 0f);
        }

        public static float RandomEulerAngle()
        {
            return UnityEngine.Random.Range(0f, 360f);
        }

        public static float CircleRadius(float square)
        {
            return Mathf.Sqrt((square / Mathf.PI));
        }

        public static bool DistanceLessThanBothRadiuses(Vector3 firstPosition, float firstRadius, Vector3 secondPosition, float secondRadius)
        {
            float lineX = firstPosition.x - secondPosition.x;
            if (lineX < 0f)
            {
                lineX = -lineX;
            }
            float radiusSum = firstRadius + secondRadius;
            if (lineX > radiusSum)
            {
                return false;
            }
            float lineY = firstPosition.z - secondPosition.z;
            if (lineY < 0f)
            {
                lineY = -lineY;
            }
            if (lineY > radiusSum)
            {
                return false;
            }
            return (lineX * lineX + lineY * lineY < radiusSum * radiusSum);
            //return DistanceLessThanBothRadiuses(new Vector2(firstPosition.x, firstPosition.z), firstRadius, new Vector2(secondPosition.x, secondPosition.z), secondRadius);
        }

        // дистанция меньше чем сумма радусов объекты касаются друг друга немного (например круг чувственности викинга и круг зоны)
        public static bool DistanceLessThanBothRadiuses(Vector2 firstPosition, float firstRadius, Vector2 secondPosition, float secondRadius)
        {
            Vector2 line = firstPosition - secondPosition;
            float powMags = line.sqrMagnitude;

            float powRadiuses = ((firstRadius + secondRadius) * (firstRadius + secondRadius));

            return (powMags < powRadiuses);
        }

        // правда если хотя бы один из объектов задевает центр другого своим радиусом (например радуис свободной территории вокруг одного объекта пересекает центр другого)
        public static bool DistanceLessThanAnyRadius(Vector2 positionOne, float radiusOn, Vector2 positionTwo, float radiusTwo)
        {
            Vector2 line = positionOne - positionTwo;
            float powMags = line.sqrMagnitude;

            return ((powMags < radiusOn * radiusOn) || (powMags < radiusTwo * radiusTwo));
        }

        public static T[] GetEnumValues<T>() where T : struct
        {
            IList audioEvents;
            try
            {
                audioEvents = Enum.GetValues(typeof(T));
            }
            catch (ArgumentException e)
            {
                Debug.LogError("Это не enum " + typeof(T));
                return new T[0];
            }
            T[] values = new T[audioEvents.Count];
            for (int i = 0; i < audioEvents.Count; i++)
            {
                values[i] = (T)audioEvents[i];
            }
            return values;
        }

        public static string[] GetEnumToStringValues<T>() where T : struct
        {
            T[] enumValues = GetEnumValues<T>();
            string[] enumStringValues = new string[enumValues.Length];
            for (int i = 0; i < enumStringValues.Length; i++)
            {
                enumStringValues[i] = enumValues[i].ToString();
            }
            return enumStringValues;
        }

        public static float GetDistance2D(Vector3 firstPosition, Vector3 secondPosition)
        {
            return (new Vector2(firstPosition.x, firstPosition.z) - new Vector2(secondPosition.x, secondPosition.z)).magnitude;
        }

        public static float GetSqrDistance2D(Vector3 firstPosition, Vector3 secondPosition)
        {
            return (new Vector2(firstPosition.x, firstPosition.z) - new Vector2(secondPosition.x, secondPosition.z)).sqrMagnitude;
        }


        public static Vector3 RotatedVectorAroundYAxis(Vector3 vector, float eulerAngle)
        {
            Quaternion q = Quaternion.Euler(0f, eulerAngle, 0f);
            Vector3 rotatedVector = q * vector;
            return rotatedVector;
        }

        public static T NearestToAgent2D<T>(this IEnumerable<T> targets, Vector3 agent, Func<T, Vector3> getPosition)
        {
            float nearestDistance = float.MaxValue;
            T nearestTarget = default(T);
            foreach (var target in targets)
            {
                Vector3 targetPosition = getPosition(target);
                float sqrDistance2D = GetSqrDistance2D(targetPosition, agent);
                if (sqrDistance2D < nearestDistance)
                {
                    nearestTarget = target;
                    nearestDistance = sqrDistance2D;
                }
            }
            return nearestTarget;
        }
    }

    // класс для сравнения дистанций до игрока
    class DistanceComparer : IComparer<Vector3>
    {
        public int Compare(Vector3 x, Vector3 y)
        {
            return x.sqrMagnitude.CompareTo(y.sqrMagnitude);
        }
    }

    class Distance2DComparer : IComparer<Vector2>
    {
        public int Compare(Vector2 x, Vector2 y)
        {
            return x.sqrMagnitude.CompareTo(y.sqrMagnitude);
        }
    }
}
