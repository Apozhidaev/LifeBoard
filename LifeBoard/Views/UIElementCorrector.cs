using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LifeBoard.Views
{
    /// <summary>
    /// Enum FixScrolling
    /// </summary>
    public enum FixScrolling
    {
        /// <summary>
        /// The no fix
        /// </summary>
        NoFix = 0,
        /// <summary>
        /// The fix
        /// </summary>
        Fix = 1,
        /// <summary>
        /// The fix no focus
        /// </summary>
        FixNoFocus = 2
    }

    /// <summary>
    /// Class UIElementCorrector
    /// </summary>
    public class UIElementCorrector
    {
        /// <summary>
        /// The fix scrolling property
        /// </summary>
        public static readonly DependencyProperty FixScrollingProperty =
            DependencyProperty.RegisterAttached("FixScrolling", typeof (FixScrolling), typeof (UIElementCorrector),
                                                new FrameworkPropertyMetadata(FixScrolling.NoFix,
                                                                              OnFixScrollingPropertyChanged));

        /// <summary>
        /// The double click command property
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof (ICommand), typeof (UIElementCorrector),
                                                new FrameworkPropertyMetadata(null, OnDoubleClickCommandPropertyChanged));

        /// <summary>
        /// The double click command parameter property
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommandParameter", typeof (object),
                                                typeof (UIElementCorrector), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the fix scrolling.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>FixScrolling.</returns>
        public static FixScrolling GetFixScrolling(DependencyObject obj)
        {
            return (FixScrolling) obj.GetValue(FixScrollingProperty);
        }

        /// <summary>
        /// Sets the fix scrolling.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetFixScrolling(DependencyObject obj, FixScrolling value)
        {
            obj.SetValue(FixScrollingProperty, value);
        }

        /// <summary>
        /// Called when [fix scrolling property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentException">The dependency property can only be attached to a UIElement;sender</exception>
        public static void OnFixScrollingPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var listBox = sender as UIElement;
            if (listBox == null)
                throw new ArgumentException("The dependency property can only be attached to a UIElement", "sender");

            if ((FixScrolling) e.NewValue == FixScrolling.FixNoFocus)
            {
                listBox.PreviewMouseWheel += FocusHandlePreviewMouseWheel;
            }
            else if ((FixScrolling) e.NewValue == FixScrolling.Fix)
            {
                listBox.PreviewMouseWheel += HandlePreviewMouseWheel;
            }
            else
            {
                listBox.PreviewMouseWheel -= FocusHandlePreviewMouseWheel;
                listBox.PreviewMouseWheel -= HandlePreviewMouseWheel;
            }
        }

        /// <summary>
        /// Focuses the handle preview mouse wheel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs" /> instance containing the event data.</param>
        private static void FocusHandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var uiElement = (UIElement) sender;
            if (!uiElement.IsKeyboardFocusWithin)
            {
                HandlePreviewMouseWheel(sender, e);
            }
        }

        /// <summary>
        /// Handles the preview mouse wheel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs" /> instance containing the event data.</param>
        private static void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            var parent = (UIElement) ((FrameworkElement) sender).Parent;
            parent.RaiseEvent(eventArg);
        }


        /// <summary>
        /// Gets the double click command.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>ICommand.</returns>
        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(DoubleClickCommandProperty);
        }

        /// <summary>
        /// Sets the double click command.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }

        /// <summary>
        /// Called when [double click command property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentException">The dependency property can only be attached to a Control;sender</exception>
        public static void OnDoubleClickCommandPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
                throw new ArgumentException("The dependency property can only be attached to a Control", "sender");

            var command = e.NewValue as ICommand;

            if (command != null)
            {
                control.MouseDoubleClick += HandleDoubleClick;
            }
            else
            {
                control.MouseDoubleClick -= HandleDoubleClick;
            }
        }

        /// <summary>
        /// Handles the double click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private static void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var control = (DependencyObject) sender;
            GetDoubleClickCommand(control).Execute(GetDoubleClickCommandParameter(control));
        }

        /// <summary>
        /// Gets the double click command parameter.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.Object.</returns>
        public static object GetDoubleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(DoubleClickCommandParameterProperty);
        }

        /// <summary>
        /// Sets the double click command parameter.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetDoubleClickCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DoubleClickCommandParameterProperty, value);
        }
    }
}