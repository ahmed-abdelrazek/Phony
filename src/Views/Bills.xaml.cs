using MahApps.Metro.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Phony.Views
{
    /// <summary>
    /// Interaction logic for Bills.xaml
    /// </summary>
    public partial class Bills : MetroWindow
    {
        public Bills()
        {
            InitializeComponent();
        }

        private void ClientComboBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (!string.IsNullOrWhiteSpace(comboBox.Text) || comboBox.IsDropDownOpen)
                return;

            // Let the focus/click finish before opening the dropdown.
            comboBox.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                if (comboBox.IsKeyboardFocusWithin && !comboBox.IsDropDownOpen
                    && string.IsNullOrWhiteSpace(comboBox.Text))
                {
                    comboBox.SetCurrentValue(ComboBox.IsDropDownOpenProperty, true);
                }
            }));
        }
    }
}
