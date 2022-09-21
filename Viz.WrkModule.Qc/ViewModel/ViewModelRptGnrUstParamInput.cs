using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Viz.WrkModule.Qc.Dto;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace Viz.WrkModule.Qc
{
  public class ViewModelRptGnrUstParamInput
  {
    #region Fields
    private readonly Window dlgWindow;
    private readonly Window mainWindow;
    private readonly ComboBoxEdit cbeThicknessNominal;
    private readonly ComboBoxEdit cbeAdgIn;
    #endregion

    #region Public Property
    public virtual DtoRptGnrUstParamInput DtoParam { get; set; }
    #endregion

    #region Protected Method
    #endregion

    #region Private Method
    private void WinActivated(object sender, EventArgs e)
    {
      cbeThicknessNominal.SelectedIndex = 0;
    }

    private void WinClose(object sender, EventArgs e)
    {
      DtoParam.AdgIn = cbeAdgIn.DisplayText;
    }
    #endregion

    #region Constructor

    public ViewModelRptGnrUstParamInput(Window dlgWindow, Object mainWindow, DtoRptGnrUstParamInput dtoParam)
    {
      this.dlgWindow = dlgWindow;
      this.mainWindow = mainWindow as Window;
      this.DtoParam = dtoParam;
      this.dlgWindow.Activated += WinActivated;
      this.dlgWindow.Closed += WinClose;
      cbeThicknessNominal = LogicalTreeHelper.FindLogicalNode(this.dlgWindow, "CbeFinalThickness") as ComboBoxEdit;
      cbeAdgIn = LogicalTreeHelper.FindLogicalNode(this.dlgWindow, "CbeAdgIn") as ComboBoxEdit;
    }

    #endregion

    #region Command
    public void CloseOkWindow()
    {
      this.dlgWindow.DialogResult = true;
      this.dlgWindow.Close();
    }
    public bool CanCloseOkWindow()
    {
      return true;
    }
    #endregion

  }
}
