using CrystalDecisions.CrystalReports.Engine;
using Phony.Kernel;
using Phony.Persistence;
using System.Data;
using System.Linq;

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
                DataTable tb = new DataTable();
                Reports.SalesBillA7 b = new Reports.SalesBillA7();
                using (var db = new PhonyDbContext())
                {
                    DataSet1 ds = new DataSet1();
                    b.SetDataSource(db.Bills.ToList());
                }                  
                Report = b;
            }
        }
    }
}