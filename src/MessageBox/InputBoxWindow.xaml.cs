using System;
using System.Windows;

namespace Phony.MessageBox
{
    /// <summary>
    /// Interaction logic for InputBoxWindow.xaml
    /// </summary>
    public partial class InputBoxWindow : IDisposable
    {
        public MessageBoxResult Result { get; set; }

        public InputBoxWindow()
        {
            InitializeComponent();
            Result = MessageBoxResult.Cancel;
        }
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            InputTxt.Text = string.Empty;
            Close();
        }

        public void Dispose()
        {
            Close();
        }

        private void BtnCopyMessage_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(TxtMessage.Text);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        private void TitleBackgroundPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
