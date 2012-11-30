using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace LifeBoard.Views
{
    public enum FixScrolling
    {
        NoFix = 0,
        Fix = 1,
        FixNoFocus = 2
    }

    public class UIElementCorrector
    {
        public static FixScrolling GetFixScrolling(DependencyObject obj)
        {
            return (FixScrolling)obj.GetValue(FixScrollingProperty);
        }

        public static void SetFixScrolling(DependencyObject obj, FixScrolling value)
        {
            obj.SetValue(FixScrollingProperty, value);
        }

        public static readonly DependencyProperty FixScrollingProperty =
            DependencyProperty.RegisterAttached("FixScrolling", typeof(FixScrolling), typeof(UIElementCorrector), new FrameworkPropertyMetadata(FixScrolling.NoFix, OnFixScrollingPropertyChanged));

        public static void OnFixScrollingPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var listBox = sender as UIElement;
            if (listBox == null)
                throw new ArgumentException("The dependency property can only be attached to a UIElement", "sender");

            if ((FixScrolling)e.NewValue == FixScrolling.FixNoFocus)
            {
                listBox.PreviewMouseWheel += FocusHandlePreviewMouseWheel;
            }
            else if ((FixScrolling)e.NewValue == FixScrolling.Fix)
            {
                listBox.PreviewMouseWheel += HandlePreviewMouseWheel;
            }
            else
            {
                listBox.PreviewMouseWheel -= FocusHandlePreviewMouseWheel;
                listBox.PreviewMouseWheel -= HandlePreviewMouseWheel;
            }
                
        }

        private static void FocusHandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var uiElement = (UIElement)sender;
            if (!uiElement.IsKeyboardFocusWithin)
            {
                HandlePreviewMouseWheel(sender, e);
            }
        }

        private static void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            var parent = (UIElement)((FrameworkElement)sender).Parent;
            parent.RaiseEvent(eventArg);
        }



        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(UIElementCorrector), new FrameworkPropertyMetadata(null, OnDoubleClickCommandPropertyChanged));

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

        private static void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var control = (DependencyObject) sender;
            GetDoubleClickCommand(control).Execute(GetDoubleClickCommandParameter(control));
        }

        public static object GetDoubleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(DoubleClickCommandParameterProperty);
        }

        public static void SetDoubleClickCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DoubleClickCommandParameterProperty, value);
        }

        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommandParameter", typeof(object), typeof(UIElementCorrector), new FrameworkPropertyMetadata(null));


    }
}
