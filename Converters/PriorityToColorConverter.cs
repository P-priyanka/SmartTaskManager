using System;
using System.Globalization;
using System.Windows.Data;
using SmartTaskManager.Models;

namespace SmartTaskManager.Converters
{
    // Returns text color for the priority badge (e.g. red for High)
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                if (priority == Priority.High) return "#C0392B";
                if (priority == Priority.Medium) return "#D68910";
                if (priority == Priority.Low) return "#1E8449";
            }
            return "#626567";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Returns background color for the priority badge
    public class PriorityToBgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                if (priority == Priority.High) return "#FDECEA";
                if (priority == Priority.Medium) return "#FEF9EC";
                if (priority == Priority.Low) return "#EAFAF1";
            }
            return "#F2F3F4";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Makes completed tasks look dimmer (0.5 opacity vs 1.0)
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted && isCompleted)
                return 0.5;
            return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Adds strikethrough to completed task titles
    public class BoolToStrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted && isCompleted)
                return System.Windows.TextDecorations.Strikethrough;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Returns a friendly due date label
    public class DueDateLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date.Date == DateTime.Today) return "Due Today";
                if (date.Date == DateTime.Today.AddDays(1)) return "Due Tomorrow";
                if (date.Date < DateTime.Today) return $"Overdue - {date:MMM d}";
                return $"Due {date:MMM d, yyyy}";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Returns red color if overdue, gray otherwise
    public class DueDateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date && date.Date < DateTime.Today)
                return "#E74C3C";
            return "#7F8C8D";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}