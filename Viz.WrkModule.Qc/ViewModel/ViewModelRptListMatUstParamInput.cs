using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Viz.WrkModule.Qc.Db.Dto;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace Viz.WrkModule.Qc
{
  public class ViewModelRptListMatUstParamInput
  {
    #region Fields
    private readonly Window dlgWindow;
    private readonly Window mainWindow;
    #endregion

    #region Public Property
    public virtual DtoRptListMatUstParamInput DtoParam { get; set; }
    #endregion

    #region Protected Method
    #endregion

    #region Private Method
    private void WinActivated(object sender, EventArgs e)
    {
      
    }

    private void WinClose(object sender, EventArgs e)
    {
      
    }
    #endregion

    #region Constructor

    public ViewModelRptListMatUstParamInput(Window dlgWindow, Object mainWindow, DtoRptListMatUstParamInput dtoParam)
    {
      this.dlgWindow = dlgWindow;
      this.mainWindow = mainWindow as Window;
      this.DtoParam = dtoParam;
      this.dlgWindow.Activated += WinActivated;
      this.dlgWindow.Closed += WinClose;
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
