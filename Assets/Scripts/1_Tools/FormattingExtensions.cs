using System;
using Asteroids.Configuration;

namespace Asteroids.Tools
{
    public static class FormattingExtensions
    {
        public static void TryFormat(this float value, Span<char> destination, string format)
        {
            value.TryFormat(destination, out var charsWritten, format);
            for (var i = charsWritten; i < destination.Length; i++)
            {
                destination[i] = FormattingConstants.SpaceCharacter;
            }
        }

        public static void TryFormat(this int value, Span<char> destination, string format)
        {
            value.TryFormat(destination, out var charsWritten, format);
            for (var i = charsWritten; i < destination.Length; i++)
            {
                destination[i] = FormattingConstants.SpaceCharacter;
            }
        }
    }
}
