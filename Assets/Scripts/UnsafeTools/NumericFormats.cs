namespace Asteroids.UnsafeTools
{
    public abstract class NumericFormat
    {
        public readonly string FormatString;
        public readonly int MaximumLength;

        public NumericFormat(string formatString, int maximumLength)
        {
            FormatString = formatString;
            MaximumLength = maximumLength;
        }
    }

    public class GenericFormat : NumericFormat
    {
        private const int _additionStringLength = 4;

        public GenericFormat(int maxValueLength) : base($"G{maxValueLength}", maxValueLength + _additionStringLength) { }
    }

    public static class NumericFormats
    {
        public static readonly GenericFormat G4 = new GenericFormat(4);
    }
}
