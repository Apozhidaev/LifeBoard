using System;
using System.Windows;
using System.Windows.Controls;

namespace LifeBoard.Views
{
    /// <summary>
    /// Class TextBoxCorrector
    /// </summary>
    public class TextBoxCorrector
    {
        /// <summary>
        /// The is observe selection changed property
        /// </summary>
        public static readonly DependencyProperty IsObserveSelectionChangedProperty =
            DependencyProperty.RegisterAttached("IsObserveSelectionChanged", typeof (bool), typeof (TextBoxCorrector),
                                                new FrameworkPropertyMetadata(false,
                                                                              OnIsObserveSelectionChangedPropertyChanged));

        /// <summary>
        /// The selection start property
        /// </summary>
        public static readonly DependencyProperty SelectionStartProperty =
            DependencyProperty.RegisterAttached("SelectionStart", typeof (int), typeof (TextBoxCorrector),
                                                new FrameworkPropertyMetadata(0, OnSelectionStartPropertyChanged));

        /// <summary>
        /// The selection length property
        /// </summary>
        public static readonly DependencyProperty SelectionLengthProperty =
            DependencyProperty.RegisterAttached("SelectionLength", typeof (int), typeof (TextBoxCorrector));

        /// <summary>
        /// Gets the is observe selection changed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool GetIsObserveSelectionChanged(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsObserveSelectionChangedProperty);
        }

        /// <summary>
        /// Sets the is observe selection changed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetIsObserveSelectionChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsObserveSelectionChangedProperty, value);
        }

        /// <summary>
        /// Gets the selection start.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.Int32.</returns>
        public static int GetSelectionStart(DependencyObject obj)
        {
            return (int) obj.GetValue(SelectionStartProperty);
        }

        /// <summary>
        /// Sets the selection start.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionStart(DependencyObject obj, int value)
        {
            obj.SetValue(SelectionStartProperty, value);
        }

        /// <summary>
        /// Gets the length of the selection.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.Int32.</returns>
        public static int GetSelectionLength(DependencyObject obj)
        {
            return (int) obj.GetValue(SelectionLengthProperty);
        }

        /// <summary>
        /// Sets the length of the selection.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionLength(DependencyObject obj, int value)
        {
            obj.SetValue(SelectionLengthProperty, value);
        }

        /// <summary>
        /// Called when [is observe selection changed property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentException">The dependency property can only be attached to a TextBox;sender</exception>
        public static void OnIsObserveSelectionChangedPropertyChanged(object sender,
                                                                      DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentException("The dependency property can only be attached to a TextBox", "sender");

            if ((bool) e.NewValue)
                textBox.SelectionChanged += OnTextBoxSelectionChanged;
            else if (!(bool) e.NewValue)
                textBox.SelectionChanged -= OnTextBoxSelectionChanged;
        }

        /// <summary>
        /// Called when [selection start property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentException">The dependency property can only be attached to a TextBox;sender</exception>
        public static void OnSelectionStartPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentException("The dependency property can only be attached to a TextBox", "sender");

            var value = (int) e.NewValue;
            if (textBox.SelectionStart != value)
            {
                textBox.SelectionStart = (int) e.NewValue;
                textBox.Focus();
            }
        }

        /// <summary>
        /// Called when [text box selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private static void OnTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.SetValue(SelectionStartProperty, textBox.SelectionStart);
            textBox.SetValue(SelectionLengthProperty, textBox.SelectionLength);
        }
    }
}