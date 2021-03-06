﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
    public partial class Timespan : PhoneApplicationPage
    {
        public Timespan()
        {
            InitializeComponent();
            DataContext = this;

            foo.Value = TimeSpan5Min;
        }

        public TimeSpan TimeSpan5Min { get { return TimeSpan.FromMinutes(5); } }
        public TimeSpan TimeSpan10Min { get { return TimeSpan.FromMinutes(10); } }
        public TimeSpan TimeSpan30Min { get { return TimeSpan.FromMinutes(30); } }
        public TimeSpan TimeSpan2Hour { get { return TimeSpan.FromHours(2); } }

        private void SetLanguage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                var culture = button.Content as string;

                if (culture != null)
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
                }
            }
        }

		private void LaunchPickerClick(object sender, RoutedEventArgs e)
		{
			foo.OpenPicker();
		}

		private void TimeSpanPicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<TimeSpan> e)
		{
			resultBlock.Text = e.NewValue.ToString();
		}
    }
}