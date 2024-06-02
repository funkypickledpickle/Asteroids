using Asteroids.Configuration;

namespace Asteroids.UnsafeTools
{
    public class MutableString
    {
        private string _targetString;

        public char[] Content { get; }

        public MutableString(int length)
        {
            _targetString = new string(FormattingConstants.EndStringCharacter, length);
            Content = new char[length];
        }

        private MutableString(string initialString, int length)
        {
            _targetString = new string(FormattingConstants.EndStringCharacter, length);
            Content = initialString.ToCharArray();
        }

        public MutableString(string initialString) : this(initialString, initialString.Length) { }

        public override string ToString()
        {
            unsafe
            {
                fixed (char* targetStringPointer = _targetString)
                {
                    for (int i = 0; i < Content.Length; i++)
                    {
                        targetStringPointer[i] = Content[i];
                    }
                }
            }

            return _targetString;
        }
    }
}
