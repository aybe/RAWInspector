using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace RAWInspector.Classes
{
    [PublicAPI]
    public struct Pattern : IReadOnlyList<byte>
    {
        public static Pattern CafeBabe { get; } = new Pattern("CAFEBABE", new byte[] {0xCA, 0xFE, 0xBA, 0xBE});

        public static Pattern DeadBabe { get; } = new Pattern("DEADBABE", new byte[] {0xDE, 0xAD, 0xBA, 0xBE});

        public static Pattern DeadBeef { get; } = new Pattern("DEADBEEF", new byte[] {0xDE, 0xAD, 0xBE, 0xEF});

        public static Pattern DeadCode { get; } = new Pattern("DEADCODE", new byte[] {0xDE, 0xAD, 0xC0, 0xDE});

        public static Pattern DeadFeed { get; } = new Pattern("DEADFEED", new byte[] {0xDE, 0xAD, 0xFE, 0xED});

        public static Pattern FaceFeed { get; } = new Pattern("FACEFEED", new byte[] {0xFA, 0xCE, 0xFE, 0xED});

        private byte[] Bytes { get; }

        public string Name { get; }

        public Pattern([NotNull] string name, [NotNull] byte[] bytes)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>) Bytes).GetEnumerator();
        }

        public override string ToString()
        {
            return Name;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Bytes.GetEnumerator();
        }

        public int Count => Bytes.Length;

        public byte this[int index] => Bytes[index];
    }
}