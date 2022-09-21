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
using Viz.WrkModule.Qc.Dto;

namespace Viz.WrkModule.Qc
{
  /// <summary>
  /// Interaction logic for ViewRptGnrUstParamInput.xaml
  /// </summary>
  public partial class ViewRptGnrUstParamInput
  {
    public ViewRptGnrUstParamInput(Object mainWnd, DtoRptGnrUstParamInput dtoParam)
    {
      InitializeComponent();
      this.DataContext = ViewModelSource.Create(() => new ViewModelRptGnrUstParamInput(this, mainWnd, dtoParam));
    }
  }
}
