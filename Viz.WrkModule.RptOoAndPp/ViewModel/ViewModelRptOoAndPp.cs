using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Ribbon;
using Smv.Utils;
using DevExpress.Xpf.LayoutControl;
using System.Globalization;
using Viz.DbApp.Psi;
using Viz.WrkModule.RptOoAndPp.Db.DataSets;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Bars;
using System.ComponentModel;
using Viz.WrkModule.RptOoAndPp.Db;
using Smv.Xls;


namespace Viz.WrkModule.RptOoAndPp
{
  public class ViewModelRptOoAndPp
  {
    #region Fields
    private readonly XlsInstanceBackgroundReport rpt;
    private readonly UserControl usrControl;
    private readonly DsRptOoAndPp dsRptOoAndPp = new DsRptOoAndPp();
    private GridControl gcTrnVal;
    private readonly Object param;
    private string whsTurnNzp;
    #endregion

    #region Public Property
    public DateTime DateBegin { get; set; }
    public DataTable TrnNzpDataSet => this.dsRptOoAndPp.TrnNzp;
    #endregion

    #region Protected Method

    #endregion

    #region Private Method
    private void RunXlsRptCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      GC.Collect();
      var barEditItem = param as BarEditItem;
      if (barEditItem != null)
        barEditItem.IsVisible = false;
    }
    #endregion

    #region Constructor

    public ViewModelRptOoAndPp(UserControl control, Object mainWindow)
    {
      usrControl = control;
      param = mainWindow;
      rpt = new XlsInstanceBackgroundReport();

      //Группы 1-уровня
      foreach (int i in Enum.GetValues(typeof(ModuleConst.AccL1Gr)))
      {
        var lg = LogicalTreeHelper.FindLogicalNode(usrControl, "L1Grp_" + i.ToString(CultureInfo.InvariantCulture)) as LayoutGroup;

        if (lg != null)
        {
          if (Permission.GetPermissionForModuleUif2(i, ModuleConst.ModuleId))
          {
            lg.Visibility = Visibility.Visible;
            //lg.Expanded += LayoutGroupExpanded;
            //this.lg.Collapsed += LayoutGroupCollapsed;
          }
          else
            lg.Visibility = Visibility.Collapsed;

        }
      }

      DateBegin = DateTime.Today;
      gcTrnVal = LogicalTreeHelper.FindLogicalNode(usrControl, "GcTrnVal") as GridControl;
    }

    #endregion

    #region Command

    public void SelectWhs(Object param)
    {
      whsTurnNzp = Convert.ToString(param);
      dsRptOoAndPp.TrnNzp.LoadData(whsTurnNzp);
    }
    public void SaveTrnVal()
    {
      (gcTrnVal.View as TableView)?.UpdateRow();
      dsRptOoAndPp.TrnNzp.SaveData();
    }

    public void DeleteRowTrnVal()
    {
      (gcTrnVal.View as TableView)?.DeleteRow(gcTrnVal.View.FocusedRowHandle);
      dsRptOoAndPp.TrnNzp.SaveData();
    }

    public void TurnoverNzp()
    {
      var src = Etc.StartPath + ModuleConst.TurnoverNzpSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.TurnoverNzpDest;

      var rptParam = new TurnoverNzpRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateBegin,
        Whs = whsTurnNzp
      };

      var sp = new TurnoverNzp();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) 
        barEditItem.IsVisible = (barEditItem != null);
    }




    #endregion

  }
}
