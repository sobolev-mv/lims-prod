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


namespace Viz.WrkModule.RptOoAndPp
{
  public class ViewModelRptOoAndPp
  {
    #region Fields
    private readonly UserControl usrControl;
    private readonly DsRptOoAndPp dsRptOoAndPp = new DsRptOoAndPp();
    private GridControl gcTrnVal;
    #endregion

    #region Public Property
    public DateTime DateBegin { get; set; }
    public DataTable TrnNzpDataSet => this.dsRptOoAndPp.TrnNzp;
    #endregion

    #region Protected Method

    #endregion

    #region Private Method

    #endregion

    #region Constructor

    public ViewModelRptOoAndPp(UserControl control, Object mainWindow)
    {
      usrControl = control;

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
      dsRptOoAndPp.TrnNzp.LoadData(Convert.ToString(param));
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

    #endregion

  }
}
