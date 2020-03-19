﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AlbumCreator.WpfElements.Converters
{
    public class EnumToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null || !(value is Enum))
                return Visibility.Collapsed;
            string State = value.ToString();
            string parameterString = parameter.ToString();

            foreach (string state in parameterString.Split(','))
            {
                if (State.Equals(state))
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
