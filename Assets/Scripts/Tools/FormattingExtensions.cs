using System;
using Asteroids.Configuration;

namespace Asteroids.Tools
{
    public static class FormattingExtensions
    {
        public static void TryFormat(this float value, Span<char> destination, string format)
        {
            value.TryFormat(destination, out int charsWritten, format);
            for (int i = charsWritten; i < destination.Length; i++)
            {
                destination[i] = FormattingConstants.SpaceCharacter;
            }
        }

        public static void TryFormat(this int value, Span<char> destination, string format)
        {
            value.TryFormat(destination, out int charsWritten, format);
            for (int i = charsWritten; i < destination.Length; i++)
            {
                destination[i] = FormattingConstants.SpaceCharacter;
            }
        }
    }
}
