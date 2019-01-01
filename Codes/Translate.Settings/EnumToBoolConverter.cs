using System;
using System.Windows;
using System.Windows.Data;

namespace Translate.Settings
{
    public class EnumBooleanConverter : IValueConverter
    {
        private TranslateResultShowType _target;

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            this._target = (TranslateResultShowType)value;

            string parameterString = parameter as string;
            if (parameterString == null || value == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value) || ((int)parameterValue & (int)value) == (int)parameterValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            // return Enum.Parse(targetType, parameterString);

             return this._target ^= (TranslateResultShowType)Enum.Parse(targetType, parameterString);
        }
        #endregion
    }
}