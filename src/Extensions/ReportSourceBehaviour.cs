namespace Phony.Extensions
{
    public static class ReportSourceBehaviour
    {
        public static readonly System.Windows.DependencyProperty ReportSourceProperty = System.Windows.DependencyProperty.RegisterAttached( "ReportSource", typeof(object), typeof(ReportSourceBehaviour), new System.Windows.PropertyMetadata(ReportSourceChanged));

        private static void ReportSourceChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var crviewer = d as SAPBusinessObjects.WPF.Viewer.CrystalReportsViewer;
            if (crviewer != null)
            {
                crviewer.ViewerCore.ReportSource = e.NewValue;
            }
        }

        public static void SetReportSource(System.Windows.DependencyObject target, object value)
        {
            target.SetValue(ReportSourceProperty, value);
        }

        public static object GetReportSource(System.Windows.DependencyObject target)
        {
            return target.GetValue(ReportSourceProperty);
        }
    }
}
