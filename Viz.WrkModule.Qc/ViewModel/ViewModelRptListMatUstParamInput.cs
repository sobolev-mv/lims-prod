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
using Microsoft.Win32;

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

    public virtual string ListMatDelim { get; set; }
    #endregion

    #region Protected Method
    #endregion

    #region Private Method
    private string GetStringFromTxtFile()
    {
      var ofd = new OpenFileDialog { DefaultExt = ".txt", Filter = "text format (.txt)|*.txt" };
      return !ofd.ShowDialog().GetValueOrDefault() ? string.Empty : System.IO.File.ReadAllText(ofd.FileName, Encoding.GetEncoding(1251)).Replace(" ", "").Replace("\r\n", " ").Trim().Replace(" ", ",");
    }
    #endregion

    #region Constructor

    public ViewModelRptListMatUstParamInput(Window dlgWindow, Object mainWindow, DtoRptListMatUstParamInput dtoParam)
    {
      this.dlgWindow = dlgWindow;
      this.mainWindow = mainWindow as Window;
      this.DtoParam = dtoParam;

      DtoParam.UnitType = "C";
    }

    #endregion

    #region Command
    public void CloseOkWindow()
    {
      this.dlgWindow.DialogResult = true;
      DtoParam.ListMatStringDelim = ListMatDelim;
      this.dlgWindow.Close();
    }
    public bool CanCloseOkWindow()
    {
      return true;
    }

    public void LoadListMatFromTxtFile()
    {
      ListMatDelim = DtoParam.ListMatStringDelim = GetStringFromTxtFile();
    }
    public bool CanLoadListMatFromTxtFile()
    {
      return true;
    }

    public void SelectUnitType(Object parameter)
    {
      DtoParam.UnitType = Convert.ToString(parameter);
    }
    public bool CanSelectUnitType(Object parameter)
    {
      return true;
    }


    #endregion

  }
}
