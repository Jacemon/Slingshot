using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tools
{
    public static class PointsGenerators
    {
        public static List<Vector2> GetRandomPointsByFunction(Func<Vector2> generationFunction,
            int amount, float spaceBetween, int maxTries = 200)
        {
            List<Vector2> existingCoordinates = new();
            var remainingAmount = amount;
            var remainingTries = maxTries;

            while (remainingAmount > 0 && remainingTries > 0)
            {
                var newCoordinate = generationFunction.Invoke();

                remainingTries--;
                if (!existingCoordinates.TrueForAll(c => Vector2.Distance(c, newCoordinate) > spaceBetween)) continue;

                remainingTries = maxTries;
                remainingAmount--;
                existingCoordinates.Add(newCoordinate);
            }

            return existingCoordinates;
        }

        // One random point

        public static Vector2 GetRandomRectanglePoint(Rect rectangle)
        {
            return new Vector2(
                Random.Range(rectangle.x, rectangle.x + rectangle.width),
                Random.Range(rectangle.y, rectangle.y + rectangle.height)
            );
        }

        public static Vector2 GetRandomEllipsePoint(Vector2 center, float semiMinor, float semiMajor)
        {
            var t = Random.Range(0f, Mathf.PI * 2f);
            var x = Random.Range(0f, semiMinor);
            var y = Random.Range(0f, semiMajor);
            return new Vector2(x * Mathf.Cos(t), y * Mathf.Sin(t)) + center;
        }

        // Several random points

        public static List<Vector2> GetRandomRectanglePoints(Rect rectangle,
            int amount, float spaceBetween, int maxTries = 200)
        {
            return GetRandomPointsByFunction(() =>
                GetRandomRectanglePoint(rectangle), amount, spaceBetween, maxTries);
        }

        public static List<Vector2> GetRandomEllipsePoints(Vector2 center, float semiMinor, float semiMajor,
            int amount, float spaceBetween, int maxTries = 200)
        {
            return GetRandomPointsByFunction(() =>
                GetRandomEllipsePoint(center, semiMinor, semiMajor), amount, spaceBetween, maxTries);
        }
    }
}