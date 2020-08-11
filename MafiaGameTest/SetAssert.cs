using MafiaGame.Utility;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace MafiaGameTest
{
    public static class SetAssert
    {
        /// <summary>
        /// Asserts that two unordered sets contain the same elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Equivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var actualNotInExpected = actual.Except(expected);
            foreach (var element in actualNotInExpected)
            {
                throw new XunitException(
                    GetExpectedAndActualString(expected, actual)
                        .Append($"Unexpected element: {element}")
                        .ToString());
            }

            var expectedNotInActual = expected.Except(actual);
            foreach (var element in expectedNotInActual)
            {
                throw new XunitException(
                    GetExpectedAndActualString(expected, actual)
                        .Append($"Missing element: {element}")
                        .ToString());
            }
        }

        public static void Equivalent<T>(IEnumerable<T> actual, params T[] expected)
            => Equivalent(expected, actual);

        public static void Count<T>(int expected, IEnumerable<T> actual)
        {
            int actualCount = actual.Count();
            if (actualCount != expected)
            {
                throw new XunitException(
                    new StringBuilder()
                        .AppendLine($"Expected count: {expected}")
                        .Append($"Actual count:  {actualCount}")
                        .ToString());
            }
        }

        public static void Empty<T>(IEnumerable<T> actual)
            => Assert.Empty(actual);

        private static StringBuilder GetExpectedAndActualString<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            => new StringBuilder()
                    .Append($"Expected: ")
                    .AppendJoin(", ", expected.OrderBy(e => e))
                    .AppendLine()
                    .Append($"Actual:   ")
                    .AppendJoin(", ", actual.OrderBy(a => a))
                    .AppendLine();
    }
}
