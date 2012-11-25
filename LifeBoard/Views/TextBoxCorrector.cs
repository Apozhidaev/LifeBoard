using System;
using System.Windows.Controls;
using System.Windows;

namespace LifeBoard.Views
{
    public class TextBoxCorrector
    {
        public static bool GetIsObserveSelectionChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsObserveSelectionChangedProperty);
        }

        public static void SetIsObserveSelectionChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsObserveSelectionChangedProperty, value);
        }

        public static int GetSelectionStart(DependencyObject obj)
        {
            return (int)obj.GetValue(SelectionStartProperty);
        }

        public static void SetSelectionStart(DependencyObject obj, int value)
        {
            obj.SetValue(SelectionStartProperty, value);
        }

        public static int GetSelectionLength(DependencyObject obj)
        {
            return (int)obj.GetValue(SelectionLengthProperty);
        }

        public static void SetSelectionLength(DependencyObject obj, int value)
        {
            obj.SetValue(SelectionLengthProperty, value);
        }

        public static readonly DependencyProperty IsObserveSelectionChangedProperty =
            DependencyProperty.RegisterAttached("IsObserveSelectionChanged", typeof(bool), typeof(TextBoxCorrector), new FrameworkPropertyMetadata(false, OnIsObserveSelectionChangedPropertyChanged));

        public static void OnIsObserveSelectionChangedPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentException("The dependency property can only be attached to a TextBox", "sender");

            if ((bool)e.NewValue)
                textBox.SelectionChanged += OnTextBoxSelectionChanged;
            else if (!(bool)e.NewValue)
                textBox.SelectionChanged -= OnTextBoxSelectionChanged;
        }

        public static readonly DependencyProperty SelectionStartProperty =
            DependencyProperty.RegisterAttached("SelectionStart", typeof(int), typeof(TextBoxCorrector), new FrameworkPropertyMetadata(0, OnSelectionStartPropertyChanged));

        public static void OnSelectionStartPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentException("The dependency property can only be attached to a TextBox", "sender");

            int value = (int)e.NewValue;
            if (textBox.SelectionStart != value)
            {
                textBox.SelectionStart = (int) e.NewValue;
                textBox.Focus();
            }
        }

        public static readonly DependencyProperty SelectionLengthProperty =
            DependencyProperty.RegisterAttached("SelectionLength", typeof(int), typeof(TextBoxCorrector));

        private static void OnTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SetValue(SelectionStartProperty, textBox.SelectionStart);
            textBox.SetValue(SelectionLengthProperty, textBox.SelectionLength);
        }
    }
}
