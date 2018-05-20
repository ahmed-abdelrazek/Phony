using Phony.Kernel;

namespace Phony.ViewModel
{
    public class SalesBillsViewerVM : CommonBase
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument _report;

        public CrystalDecisions.CrystalReports.Engine.ReportDocument Report
        {
            get => _report;
            set
            {
                if (value != _report)
                {
                    _report = value;
                    RaisePropertyChanged();
                }
            }
        }

        public SalesBillsViewerVM()
        {
            if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
            {
                Report = new Reports.SalesBillA4();
            }
            else
            {
                Report = new Reports.SalesBillA7();
            }
        }
    }
}