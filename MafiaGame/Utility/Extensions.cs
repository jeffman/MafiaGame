using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MafiaGame.Utility
{
    public static class Extensions
    {
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            if (source is ReadOnlyCollection<T> readOnly)
                return readOnly;
            return new ReadOnlyCollection<T>(source.ToList());
        }

        public static TEnum AsDefinedOrThrow<TEnum>(this TEnum value) where TEnum : Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
                return value;
            throw new ArgumentOutOfRangeException();
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T element)
        {
            return source.Except(new[] { element });
        }
    }
}
