﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DependencyAnalyser.Utilities
{
    [ValueConversion(typeof (bool), typeof (Visibility))]
    public class BooleanInvisibileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof (Visibility))
                throw new InvalidOperationException("The target must be a Visibility");

            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
