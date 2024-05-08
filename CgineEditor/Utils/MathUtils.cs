using System;
using System.Collections.Generic;
using System.Text;

namespace CgineEditor.Utils
{
    public static class MathUtil
    {
        public static float Eplison => 0.00001f;

        //extension method,adding new method to a class without modify the class
        public static bool IsTheSameAs(this float value, float other)
        {
            return Math.Abs(value - other) < Eplison;
        }

        public static bool isTheSameAs(this float? value, float? other)
        {
            if (!value.HasValue || !other.HasValue) return false;
            return Math.Abs(value.Value - other.Value) < Eplison;
        }

    }
}
