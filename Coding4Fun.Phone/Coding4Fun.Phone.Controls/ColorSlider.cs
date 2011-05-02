﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using SilverlightColorPicker;

// This is a heavily modified version based on ColorSlider in their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Phone.Controls
{
    public class ColorSlider : ColorMonitorBaseControl
    {
        bool _isFirstLoad = true;

        const int GradientStops = 6;
        const double NegatedGradientStops = 1 / (float)GradientStops;

        const double HueSelectorSize = 24;
        double _rectHueMonitorSize = 180;

        #region controls on template
        protected Grid Body;
        private const string BodyName = "Body";

        protected Rectangle SelectedColor;
        private const string SelectedColorName = "SelectedColor";
        
        protected Grid GradientBody;
        private const string GradientBodyName = "GradientBody";

        protected Rectangle Gradient;
        private const string GradientName = "Gradient";

        protected Grid HueSelector;
        private const string HueSelectorName = "HueSelector";
        #endregion

        public ColorSlider()
        {
            DefaultStyleKey = typeof(ColorSlider);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Body = GetTemplateChild(BodyName) as Grid;
            GradientBody = GetTemplateChild(GradientBodyName) as Grid;
            HueSelector = GetTemplateChild(HueSelectorName) as Grid;

            SelectedColor = GetTemplateChild(SelectedColorName) as Rectangle;
            Gradient = GetTemplateChild(GradientName) as Rectangle;

            SizeChanged += UserControl_SizeChanged;
        }

        protected internal override void UpdateSample(double x, double y)
        {
            var position = (Orientation == Orientation.Horizontal) ? x : y;

            var offset = CheckMarginBound(position, _rectHueMonitorSize - HueSelectorSize);
            position = CheckMarginBound(position, _rectHueMonitorSize - 1);

            MarginOffset = (Orientation == Orientation.Vertical) ? new Thickness(0, offset, 0, 0) : new Thickness(offset, 0, 0, 0);

            var huePos = (int)(position / _rectHueMonitorSize * 255) * GradientStops;

            ColorChanging(ColorSpace.GetColorFromPosition(huePos));
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustLayoutBasedOnOrientation();
        }

        #region dependency properties
        internal Thickness MarginOffset
        {
            get { return (Thickness)GetValue(MarginOffsetProperty); }
            set { SetValue(MarginOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarginOffset.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty MarginOffsetProperty =
            DependencyProperty.Register("MarginOffset", typeof(Thickness), typeof(ColorSlider), new PropertyMetadata(new Thickness(0)));

        public bool IsColorVisible
        {
            get { return (bool)GetValue(IsColorVisibleProperty); }
            set { SetValue(IsColorVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsColorVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsColorVisibleProperty =
            DependencyProperty.Register("IsColorVisible", typeof(bool), typeof(ColorSlider), new PropertyMetadata(true, OnIsColorVisibleChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ColorSlider), new PropertyMetadata(Orientation.Vertical, OnOrientationPropertyChanged));
        #endregion

        private static void OnIsColorVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as ColorSlider;

            if (slider != null)
                slider.AdjustLayoutBasedOnOrientation();
        }

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as ColorSlider;
            
            if (slider != null)
                slider.AdjustLayoutBasedOnOrientation();
        }

        private void AdjustLayoutBasedOnOrientation()
        {
            if (Gradient == null ||
                Body == null ||
                SelectedColor == null ||
                GradientBody == null)
                return;

            var isVert = Orientation == Orientation.Vertical;

            #region LinearGradientBrush calculations
            var brush = new LinearGradientBrush();

            //<GradientStop Offset="0.00" Color="#ffff0000"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 0, Color = Color.FromArgb(255, 255, 0, 0) });
            //<GradientStop Offset="0.166666" Color="#ffffff00"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 1, Color = Color.FromArgb(255, 255, 255, 0) });
            //<GradientStop Offset="0.333333" Color="#ff00ff00"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 2, Color = Color.FromArgb(255, 0, 255, 0) });
            //<GradientStop Offset="0.50" Color="#ff00ffff"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 3, Color = Color.FromArgb(255, 0, 255, 255) });
            //<GradientStop Offset="0.666666" Color="#ff0000ff"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 4, Color = Color.FromArgb(255, 0, 0, 255) });
            //<GradientStop Offset="0.833333" Color="#ffff00ff"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 5, Color = Color.FromArgb(255, 255, 0, 255) });
            //<GradientStop Offset="1.00" Color="#ffff0000"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 6, Color = Color.FromArgb(255, 255, 0, 0) });

            brush.EndPoint = isVert ? new Point(0, 1) : new Point(1, 0);

            Gradient.Fill = brush;
            #endregion

            Body.RowDefinitions.Clear();
            Body.ColumnDefinitions.Clear();

            if (isVert)
            {
                Body.RowDefinitions.Add(new RowDefinition());
                Body.RowDefinitions.Add(new RowDefinition());
            }
            else
            {
                Body.ColumnDefinitions.Add(new ColumnDefinition());
                Body.ColumnDefinitions.Add(new ColumnDefinition());
            }

            GradientBody.SetValue(Grid.RowProperty, 0);
            GradientBody.SetValue(Grid.ColumnProperty, 0);

            HueSelector.VerticalAlignment = isVert ? VerticalAlignment.Top : VerticalAlignment.Stretch;
            HueSelector.HorizontalAlignment = isVert ? HorizontalAlignment.Stretch : HorizontalAlignment.Left;
            HueSelector.Height = isVert ? HueSelectorSize : double.NaN;
            HueSelector.Width = isVert ? double.NaN : HueSelectorSize;

            SelectedColor.SetValue(Grid.RowProperty, isVert ? 1 : 0);
            SelectedColor.SetValue(Grid.ColumnProperty, isVert ? 0 : 1);

            var colorMonitorWidth = ColorMonitor.ActualWidth;
            var colorMonitorHeight = ColorMonitor.ActualHeight;

            var squareSize = isVert ? colorMonitorWidth : colorMonitorHeight;
            _rectHueMonitorSize = isVert ? colorMonitorHeight : colorMonitorWidth;

            SelectedColor.Height = SelectedColor.Width = squareSize;

            if (isVert)
            {
                Body.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                Body.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                Body.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                Body.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Auto);
            }

            SelectedColor.Visibility = (IsColorVisible) ? Visibility.Visible : Visibility.Collapsed;

            if (IsColorVisible)
            {
                _rectHueMonitorSize -= squareSize;
            }

            if (_isFirstLoad)
            {
                var size = _rectHueMonitorSize / 3.0;
                UpdateSample(size, size);
                _isFirstLoad = false;
            }

        }
    }
}