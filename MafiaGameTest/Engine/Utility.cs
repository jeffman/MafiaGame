using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;

namespace MafiaGameTest.Engine
{
    public static class Utility
    {
        public static class People
        {
            public static Person Alice { get; } = new Person("Alice");
            public static Person Bob { get; } = new Person("Bob");
            public static Person Chris { get; } = new Person("Chris");
            public static Person Denise { get; } = new Person("Denise");
            public static Person Gabby { get; } = new Person("Gabby");
            public static Person James { get; } = new Person("James");
            public static Person Kelly { get; } = new Person("Kelly");
            public static Person Larry { get; } = new Person("Larry");
            public static Person Millhouse { get; } = new Person("Millhouse");
            public static Person Bud { get; } = new Person("Bud");
            public static Person Buzz { get; } = new Person("Buzz");
            public static Person Brock { get; } = new Person("Brock");
        }

        public static IEnumerable<object[]> AsMemberData(this IEnumerable<object> data)
        {
            return data.Select(d => new[] { d });
        }
    }
}
