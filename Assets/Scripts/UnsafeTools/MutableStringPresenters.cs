using System;
using System.Text;
using Asteroids.Configuration;
using Asteroids.Tools;
using UnityEngine;

namespace Asteroids.UnsafeTools
{
    public class Vector2MutableStringPresenter
    {
        private readonly MutableString _mutableString;
        private readonly NumericFormat _numericFormat;

        private readonly int _horizontalAxisTextIndex;
        private readonly int _verticalAxisTextIndex;

        public Vector2MutableStringPresenter(NumericFormat numericFormat)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(FormattingConstants.HorizontalAxisText);
            _horizontalAxisTextIndex = stringBuilder.Length;
            stringBuilder.Append(new string(FormattingConstants.EndStringCharacter, numericFormat.MaximumLength));
            stringBuilder.Append(FormattingConstants.AxisSeparatorText);
            stringBuilder.Append(FormattingConstants.VerticalAxisText);
            _verticalAxisTextIndex = stringBuilder.Length;
            stringBuilder.Append(new string(FormattingConstants.EndStringCharacter, numericFormat.MaximumLength));

            _mutableString = new MutableString(stringBuilder.ToString());
            _numericFormat = numericFormat;
        }

        public void UpdateContent(Vector2 value)
        {
            value.x.TryFormat(new Span<char>(_mutableString.Content, _horizontalAxisTextIndex, _numericFormat.MaximumLength), _numericFormat.FormatString);
            value.y.TryFormat(new Span<char>(_mutableString.Content, _verticalAxisTextIndex, _numericFormat.MaximumLength), _numericFormat.FormatString);
        }

        public override string ToString()
        {
            return _mutableString.ToString();
        }
    }

    public class IntegerMutableStringPresenter
    {
        private readonly MutableString _mutableString;
        private readonly NumericFormat _numericFormat;

        public IntegerMutableStringPresenter(NumericFormat numericFormat)
        {
            _mutableString = new MutableString(numericFormat.MaximumLength + 1);
            _numericFormat = numericFormat;
        }

        public void UpdateContent(int value)
        {
            value.TryFormat(_mutableString.Content, _numericFormat.FormatString);
            _mutableString.Content[^1] = FormattingConstants.EndStringCharacter;
        }

        public override string ToString()
        {
            return _mutableString.ToString();
        }
    }

    public class SingleMutableStringPresenter
    {
        private readonly MutableString _mutableString;
        private readonly NumericFormat _numericFormat;

        public SingleMutableStringPresenter(NumericFormat numericFormat)
        {
            _mutableString = new MutableString(numericFormat.MaximumLength + 1);
            _numericFormat = numericFormat;
        }

        public void UpdateContent(float value)
        {
            value.TryFormat(_mutableString.Content, _numericFormat.FormatString);
            _mutableString.Content[^1] = FormattingConstants.EndStringCharacter;
        }

        public override string ToString()
        {
            return _mutableString.ToString();
        }
    }
}
