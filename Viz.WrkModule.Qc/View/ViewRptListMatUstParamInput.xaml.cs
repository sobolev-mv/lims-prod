using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.POCO;
using Viz.WrkModule.Qc.Db.Dto;


namespace Viz.WrkModule.Qc
{
  /// <summary>
  /// Interaction logic for ViewRptGnrUstParamInput.xaml
  /// </summary>
  public partial class ViewRptListMatUstParamInput
  {
    public ViewRptListMatUstParamInput(Object mainWnd, DtoRptListMatUstParamInput dtoParam)
    {
      InitializeComponent();
      this.DataContext = ViewModelSource.Create(() => new ViewModelRptListMatUstParamInput(this, mainWnd, dtoParam));
    }
  }
}
