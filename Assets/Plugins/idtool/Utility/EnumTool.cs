using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace idbrii.lib.util
{
    public static class EnumTool
    {
        public static IEnumerable GetNames<EnumType>()
        {
            return Enum.GetValues(typeof(EnumType));
        }

        public static int GetLength<EnumType>()
        {
            // http://www.csharp411.com/c-count-items-in-an-enum/
            // > you can also use the Enum.GetValues method to count the items in
            // > an enumeration, however GetNames is 10X faster than GetValues!
            // > Perhaps this performance difference is the result of boxing and
            // > unboxing of integer values in the Array for Enum.GetValues?
            // For some reason I can't get Count of IEnumerable.
            return Enum.GetValues(typeof(EnumType)).Length;
        }

    }
}
