﻿using System;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class LinearCurve
    {
        public AnimationCurve curve;
        public int cornerCount;
        [Header("y = Kx + B")]
        public int k;
        public int b;

        public void Rebuild()
        {
            curve = new AnimationCurve();
            for (var i = 0; i < cornerCount; i++)
            {
                curve.AddKey(i, i * k + b);
            }
        }

        public void AddCorner()
        {
            cornerCount++;
            curve.AddKey(cornerCount, cornerCount * k + b);
        }

        public void RemoveCorner()
        {
            cornerCount--;
            curve.RemoveKey(cornerCount);
        }

        public int Evaluate(int x)
        {
            return (int)curve.Evaluate(x);
        }

        public int ForceEvaluate(int x)
        {
            while (curve.length - 1 < x)
            {
                AddCorner();
            }

            return Evaluate(x);
        }
    }
}