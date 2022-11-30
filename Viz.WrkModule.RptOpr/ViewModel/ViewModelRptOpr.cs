using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Smv.MVVM.Commands;
using Smv.MVVM.ViewModels;
using Smv.Utils;
using Smv.Xls;
using Viz.DbApp.Psi;
using Viz.WrkModule.RptOpr.Db;
using Viz.WrkModule.RptOpr.Db.DataSets;

namespace Viz.WrkModule.RptOpr
{
  internal sealed class ViewModelRptOpr : ViewModelBase
  {
    #region Fields
    private readonly XlsInstanceBackgroundReport rpt;
    private readonly UserControl usrControl;
    private readonly Object param;
    //private readonly LayoutGroup lg;
    private readonly DsRptOpr dsRptOpr = new DsRptOpr(); 
    private DateTime dateBegin;
    private DateTime dateEnd;
    private DataRowView selFinishApr;
    private string teamFinishApr;
    private string shiftMasterFinishApr;
    private string topWorkerFinishApr;
    private string typeShiftFinishApr;
    private Boolean isLogInfo;

    //Поля для фильтра отчета "Обработка на ЛК и АПР"
    private Boolean isGroupParam1F1;
    private decimal p1750_023LstF1;
    private decimal p1750_027LstF1;
    private decimal p1750_030LstF1;
    private decimal b800LstF1;
    private int     qntWeldsF1;
    private int     kesiAvgF1;
    private string  cat1F1 = "N";
    private string  cat2F1 = "N";
    private string  cat3F1 = "N";
    private string  catWcF1 = "N";
    private string  adgInF1 = "O";
    private string  adgOutF1 = "G";
    private decimal coffWave1F1;
    private decimal coffWave2F1;
    private decimal heightWave1F1;
    private decimal heightWave2F1;

    //Поля для фильтра отчета "Причины осевшего металла на УО"
    private Boolean isGroupDateRangeAvoF2;
    private Boolean isGroupDateRangeUoF2;
    private DateTime dateIncomplProd1;
    private DateTime dateIncomplProd2;
    private DateTime dateRangeBeginAvoF2;
    private DateTime dateRangeEndAvoF2;
    private DateTime dateRangeBeginUoF2;
    private DateTime dateRangeEndUoF2;



    private string  clsNoPloskF1;
    private DataRowView selTargetNextProcItemF1;
    private int? idtargetNextProcF1;

    //Поля для фильтра отчета "Средневзвешанная ширина"
    private Boolean isFill1StSheet;
    private Boolean isTypeProdF3;
    private int? idTypeProdF3;
    private DataRowView selTypeProdItemF3;
    private Boolean isThicknessF3;
    private int? idThicknessF3;
    private DataRowView selThicknessItemF3;
    private Boolean isSortF3;
    private int? idSortF3;
    private DataRowView selSortItemF3;

    private DateTime dateBeginQuart;
    private DateTime dateEndQuart;
    //private string typeUm = "R";
    private decimal rkTotal;
    private decimal rkLng;

    //Поля для отчета "Состояние складов СГП УО" 
    private DateTime dateStateView;
    private GridControl gcStateWhsUo;

    //Поля для отчета "Сменный рапорт ПУ" 
    private DataRowView selFinishRoll;
    private string teamFinishRoll;
    private string shiftMasterFinishRoll;
    private string topWorkerFinishRoll;
    private string typeShiftFinishRoll;

    //Поля для отчета "Устраненные дефекты на АВО"
    private string locNum;

    //Поля для отчета "История обработки рулонов"
    private string listAnLot;

    #endregion

    #region Public Property
    public DateTime DateBegin
    {
      get { return dateBegin; }
      set{
        if (value == dateBegin) return;
        dateBegin = value;
        OnPropertyChanged("DateBegin");
      }
    }

    public DateTime DateEnd
    {
      get { return dateEnd; }
      set
      {
        if (value == dateEnd) return;
        dateEnd = value;
        OnPropertyChanged("DateEnd");
      }
    }

    public DataTable LstFinishApr => dsRptOpr.LstFinishApr;

    public DataRowView SelFinishAprItem
    {
      get { return selFinishApr; }
      set{
        if (Equals(value, selFinishApr)) return;
        selFinishApr = value;
        OnPropertyChanged("SelFinishApr");
      }
    }

    public string TeamFinishApr
    {
      get { return teamFinishApr; }
      set{
        if (Equals(value, teamFinishApr)) return;
        teamFinishApr = value;
        OnPropertyChanged("TeamFinishApr");
      }
    }

    public string ShiftMasterFinishApr
    {
      get { return shiftMasterFinishApr; }
      set{
        if (Equals(value, shiftMasterFinishApr)) return;
        shiftMasterFinishApr = value;
        OnPropertyChanged("ShiftMasterFinishApr");
      }
    }

    public string TopWorkerFinishApr
    {
      get { return topWorkerFinishApr; }
      set{
        if (Equals(value, topWorkerFinishApr)) return;
        topWorkerFinishApr = value;
        OnPropertyChanged("TopWorkerFinishApr");
      }
    }

    public string TypeShiftFinishApr
    {
      get { return typeShiftFinishApr; }
      set{
        if (Equals(value, typeShiftFinishApr)) return;
        typeShiftFinishApr = value;
        OnPropertyChanged("TypeShiftFinishApr");
      }
    }

    public Boolean IsLogInfo
    {
      get { return isLogInfo; }
      set
      {
        if (Equals(value, isLogInfo)) return;
        isLogInfo = value;
        OnPropertyChanged("IsLogInfo");
      }
    }

    //Поля для фильтра отчета "Обработка на ЛК и АПР" 
    public Boolean IsGroupParam1F1
    {
      get { return isGroupParam1F1; }
      set
      {
        if (value == isGroupParam1F1) return;
        isGroupParam1F1 = value;
        OnPropertyChanged("IsGroupParam1F1");
      }
    }
    
    public decimal P1750_023LstF1
    {
      get { return p1750_023LstF1; }
      set
      {
        if (value == p1750_023LstF1) return;
        p1750_023LstF1 = value;
        OnPropertyChanged("P1750_023LstF1");
      }
    }

    public decimal P1750_027LstF1
    {
      get { return p1750_027LstF1; }
      set
      {
        if (value == p1750_027LstF1) return;
        p1750_027LstF1 = value;
        OnPropertyChanged("P1750_027LstF1");
      }
    }

    public decimal P1750_030LstF1
    {
      get { return p1750_030LstF1; }
      set
      {
        if (value == p1750_030LstF1) return;
        p1750_030LstF1 = value;
        OnPropertyChanged("P1750_030LstF1");
      }
    }

    public decimal B800LstF1
    {
      get { return b800LstF1; }
      set
      {
        if (value == b800LstF1) return;
        b800LstF1 = value;
        OnPropertyChanged("B800LstF1");
      }
    }

    public int KesiAvgF1
    {
      get { return kesiAvgF1; }
      set
      {
        if (value == kesiAvgF1) return;
        kesiAvgF1 = value;
        OnPropertyChanged("KesiAvgF1");
      }
    }

    public int QntWeldsF1
    {
      get { return qntWeldsF1; }
      set
      {
        if (value == qntWeldsF1) return;
        qntWeldsF1 = value;
        OnPropertyChanged("QntWeldsF1");
      }
    }

    public string Cat1F1
    {
      get { return cat1F1; }
      set
      {
        if (value == cat1F1) return;
        cat1F1 = value;
        OnPropertyChanged("Cat1F1");
      }
    }

    public string Cat2F1
    {
      get { return cat2F1; }
      set
      {
        if (value == cat2F1) return;
        cat2F1 = value;
        OnPropertyChanged("Cat2F1");
      }
    }

    public string Cat3F1
    {
      get { return cat3F1; }
      set
      {
        if (value == cat3F1) return;
        cat3F1 = value;
        OnPropertyChanged("Cat3F1");
      }
    }

    public string CatWcF1
    {
      get { return catWcF1; }
      set
      {
        if (value == catWcF1) return;
        catWcF1 = value;
        OnPropertyChanged("CatWcF1");
      }
    }

    public string AdgInF1
    {
      get { return adgInF1; }
      set
      {
        if (value == adgInF1) return;
        adgInF1 = value;
        OnPropertyChanged("AdgInF1");
      }
    }

    public string AdgOutF1
    {
      get { return adgOutF1; }
      set
      {
        if (value == adgOutF1) return;
        adgOutF1 = value;
        OnPropertyChanged("AdgOutF1");
      }
    }

    public decimal CoffWave1F1
    {
      get { return coffWave1F1; }
      set
      {
        if (value == coffWave1F1) return;
        coffWave1F1 = value;
        OnPropertyChanged("CoffWave1F1");
      }
    }

    public decimal CoffWave2F1
    {
      get { return coffWave2F1; }
      set
      {
        if (value == coffWave2F1) return;
        coffWave2F1 = value;
        OnPropertyChanged("CoffWave2F1");
      }
    }

    public decimal HeightWave1F1
    {
      get { return heightWave1F1; }
      set
      {
        if (value == heightWave1F1) return;
        heightWave1F1 = value;
        OnPropertyChanged("HeightWave1F1");
      }
    }

    public decimal HeightWave2F1
    {
      get { return heightWave2F1; }
      set
      {
        if (value == heightWave2F1) return;
        heightWave2F1 = value;
        OnPropertyChanged("HeightWave2F1");
      }
    }

    public string ClsNoPloskF1
    {
      get { return clsNoPloskF1; }
      set
      {
        if (value == clsNoPloskF1) return;
        clsNoPloskF1 = value;
        OnPropertyChanged("ClsNoPloskF1");
      }
    }
 

    public DataTable LstTrgtNextProc
    {
      get { return dsRptOpr.LstTrgtNextProc; }
    }

    public DataRowView SelTargetNextProcItemF1
    {
      get { return selTargetNextProcItemF1; }
      set
      {
        if (Equals(value, selTargetNextProcItemF1)) return;
        selTargetNextProcItemF1 = value;
        OnPropertyChanged("SelTargetNextProcItemF1");
      }
    }
    
    public int? IdtargetNextProcF1
    {
      get { return idtargetNextProcF1; }
      set
      {
        if (value == idtargetNextProcF1) return;
        idtargetNextProcF1 = value;
        OnPropertyChanged("IdtargetNextProcF1");
      }
    }

    //Поля для фильтра отчета "Причины осевшего металла на УО"
    public Boolean IsGroupDateRangeAvoF2
    {
      get { return isGroupDateRangeAvoF2; }
      set{
        if (value == isGroupDateRangeAvoF2) return;
        isGroupDateRangeAvoF2 = value;
        OnPropertyChanged("IsGroupDateRangeAvoF2");
      }
    }

    public Boolean IsGroupDateRangeUoF2
    {
      get { return isGroupDateRangeUoF2; }
      set{
        if (value == isGroupDateRangeUoF2) return;
        isGroupDateRangeUoF2 = value;
        OnPropertyChanged("IsGroupDateRangeUoF2");
      }
    }

    public DateTime DateIncomplProd1
    {
      get { return dateIncomplProd1; }
      set
      {
        if (value == dateIncomplProd1) return;
        dateIncomplProd1 = value;
        OnPropertyChanged("DateIncomplProd1");
      }
    }

    public DateTime DateIncomplProd2
    {
      get { return dateIncomplProd2; }
      set
      {
        if (value == dateIncomplProd2) return;
        dateIncomplProd2 = value;
        OnPropertyChanged("DateIncomplProd2");
      }
    }
    /*
    private DateTime dateRangeEndAvoF2;
    private DateTime dateRangeBeginUoF2;
    private DateTime dateRangeEndUoF2;


  */
    public DateTime DateRangeBeginAvoF2
    {
      get { return dateRangeBeginAvoF2; }
      set
      {
        if (value == dateRangeBeginAvoF2) return;
        dateRangeBeginAvoF2 = value;
        OnPropertyChanged("DateRangeBeginAvoF2");
      }
    }

    public DateTime DateRangeEndAvoF2
    {
      get { return dateRangeEndAvoF2; }
      set{
        if (value == dateRangeEndAvoF2) return;
        dateRangeEndAvoF2 = value;
        OnPropertyChanged("DateRangeEndAvoF2");
      }
    }
    public DateTime DateRangeBeginUoF2
    {
      get { return dateRangeBeginUoF2; }
      set{
        if (value == dateRangeBeginUoF2) return;
        dateRangeBeginUoF2 = value;
        OnPropertyChanged("DateRangeBeginUoF2");
      }
    }

    public DateTime DateRangeEndUoF2
    {
      get { return dateRangeEndUoF2; }
      set{
        if (value == dateRangeEndUoF2) return;
        dateRangeEndUoF2 = value;
        OnPropertyChanged("DateRangeEndUoF2");
      }
    }

    //Поля для фильтра отчета "Средневзвешанная ширина"
    public Boolean IsFill1StSheet
    {
      get { return isFill1StSheet; }
      set{
        if (value == isFill1StSheet) return;
        isFill1StSheet = value;
        OnPropertyChanged("IsFill1StSheet");
      }
    }
   
    public Boolean IsTypeProdF3
    {
      get { return isTypeProdF3; }
      set{
        if (value == isTypeProdF3) return;
        isTypeProdF3 = value;
        OnPropertyChanged("IsTypeProdF3");
      }
    }

    public DataTable LstTypeProd
    {
      get { return dsRptOpr.LstTypeProd; }
    }

    public DataTable LstThickness
    {
      get { return dsRptOpr.LstThickness; }
    }

    public DataTable LstSort
    {
      get { return dsRptOpr.LstSort; }
    }


    public int? IdTypeProdF3
    {
      get { return idTypeProdF3; }
      set{
        if (value == idTypeProdF3) return;
        idTypeProdF3 = value;
        OnPropertyChanged("IdTypeProdF3");
      }
    }
    
    public DataRowView SelTypeProdItemF3
    {
      get { return selTypeProdItemF3; }
      set{
        if (Equals(value, selTypeProdItemF3)) return;
        selTypeProdItemF3 = value;
        OnPropertyChanged("SelTypeProdItemF3");
      }
    }
    
    public Boolean IsThicknessF3
    {
      get { return isThicknessF3; }
      set{
        if (value == isThicknessF3) return;
        isThicknessF3 = value;
        OnPropertyChanged("IsThicknessF3");
      }
    }
    
    public int? IdThicknessF3
    {
      get { return idThicknessF3; }
      set{
        if (value == idThicknessF3) return;
        idThicknessF3 = value;
        OnPropertyChanged("IdThicknessF3");
      }
    }
    
    public DataRowView SelThicknessItemF3
    {
      get { return selThicknessItemF3; }
      set
      {
        if (Equals(value, selThicknessItemF3)) return;
        selThicknessItemF3 = value;
        OnPropertyChanged("SelThicknessItemF3");
      }
    }
    
    public Boolean IsSortF3
    {
      get { return isSortF3; }
      set{
        if (value == isSortF3) return;
        isSortF3 = value;
        OnPropertyChanged("IsSortF3");
      }
    }
    
    public int? IdSortF3
    {
      get { return idSortF3; }
      set{
        if (value == idSortF3) return;
        idSortF3 = value;
        OnPropertyChanged("IdSortF3");
      }
    }
    
    public DataRowView SelSortItemF3
    {
      get { return selSortItemF3; }
      set{
        if (Equals(value, selSortItemF3)) return;
        selSortItemF3 = value;
        OnPropertyChanged("SelSortItemF3");
      }
    }

    public DateTime DateBeginQuart
    {
      get { return dateBeginQuart; }
      set
      {
        if (value == dateBeginQuart) return;
        dateBeginQuart = value;
        base.OnPropertyChanged("DateBeginQuart");
      }
    }

    public DateTime DateEndQuart
    {
      get { return dateEndQuart; }
      set
      {
        if (value == dateEndQuart) return;
        dateEndQuart = value;
        base.OnPropertyChanged("DateEndQuart");
      }
    }

    public decimal RkTotal
    {
      get { return rkTotal; }
      set
      {
        if (value == rkTotal) return;
        rkTotal = value;
        OnPropertyChanged("RkTotal");
      }
    }

    public decimal RkLng
    {
      get { return rkLng; }
      set
      {
        if (value == rkLng) return;
        rkLng = value;
        OnPropertyChanged("RkLng");
      }
    }

    public DateTime DateStateView
    {
      get { return dateStateView; }
      set
      {
        if (value == dateStateView) return;
        dateStateView = value;
        OnPropertyChanged("DateStateView");
      }
    }

    public DataTable StateWhsUo
    {
      get { return dsRptOpr.StateWhsUo; }
    }

    //Свойства для отчета "Сменный рапорт ПУ"
    public DataRowView SelFinishRollItem
    {
      get { return selFinishRoll; }
      set
      {
        if (Equals(value, selFinishRoll)) return;
        selFinishRoll = value;
        OnPropertyChanged("SelFinishRoll");
      }
    }

    public string TeamFinishRoll
    {
      get { return teamFinishRoll; }
      set
      {
        if (Equals(value, teamFinishRoll)) return;
        teamFinishRoll = value;
        OnPropertyChanged("TeamFinishRoll");
      }
    }

    public string ShiftMasterFinishRoll
    {
      get { return shiftMasterFinishRoll; }
      set
      {
        if (Equals(value, shiftMasterFinishRoll)) return;
        shiftMasterFinishRoll = value;
        OnPropertyChanged("ShiftMasterFinishRoll");
      }
    }

    public string TopWorkerFinishRoll
    {
      get { return topWorkerFinishRoll; }
      set
      {
        if (Equals(value, topWorkerFinishRoll)) return;
        topWorkerFinishRoll = value;
        OnPropertyChanged("TopWorkerFinishRoll");
      }
    }

    public string TypeShiftFinishRoll
    {
      get { return typeShiftFinishRoll; }
      set
      {
        if (Equals(value, typeShiftFinishRoll)) return;
        typeShiftFinishRoll = value;
        OnPropertyChanged("TypeShiftFinishRoll");
      }
    }
    public DataTable LstFinishRoll => dsRptOpr.LstFinishRoll;

    //Поля для отчета "Устраненные дефекты на АВО"
    
    public string LocNum
    {
      get { return locNum; }
      set
      {
        if (Equals(value, locNum)) return;
        locNum = value;
        OnPropertyChanged("LocNum");
      }
    }

    //Поля для отчета "История обработки рулонов"
    public string ListAnLot
    {
      get { return listAnLot; }
      set
      {
        if (Equals(value, listAnLot)) return;
        listAnLot = value;
        OnPropertyChanged("ListAnLot");
      }
    }
    #endregion

    #region Private Method
    private void RunXlsRptCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      GC.Collect();
      var barEditItem = param as BarEditItem;
      if (barEditItem != null)
        barEditItem.IsVisible = false;
    }

    private void LayoutGroupExpanded(object sender, EventArgs e)
    {
      var layoutGroup = sender as LayoutGroup;
      if (layoutGroup != null)
      {
        var i = Convert.ToInt32(layoutGroup.Tag);
        switch (i)
        {
          case 0:
            LgExpanded0();
            break;
          case 1:
            this.LgExpanded1();
            break;
          case 2:
            break;
          case 3:
            break;
          case 4:
            LgExpanded4();
            break;
          case 5:
            break;
          case 6:
            break;
          case 7:
            RkTotal = Convert.ToDecimal(DbUtils.GetTotalRk(1));
            RkLng = Convert.ToDecimal(DbUtils.GetLngRk(1));
            DateBeginQuart = DbUtils.GetDateBeginQuart(3);
            DateEndQuart = DbUtils.GetDateEndQuart(3);
            break;
          case 8:
            LgExpanded8();
            break;

        }
      }
    }
    private void LgExpanded8()
    {
      //if (dsRptOpr.LstFinishApr.Rows.Count == 0)
        //dsRptOpr.LstFinishApr.LoadData(18);

      var cbe = LogicalTreeHelper.FindLogicalNode(usrControl, "cbeLstFinishRoll") as ComboBoxEdit;
      if (cbe != null)
        cbe.SelectedIndex = 0;
    }

    private void LgExpanded0()
    {
      if (dsRptOpr.LstFinishApr.Rows.Count == 0)
        dsRptOpr.LstFinishApr.LoadData(18);

      var cbe = LogicalTreeHelper.FindLogicalNode(usrControl, "cbeLstFinishApr") as ComboBoxEdit;
      if (cbe != null)
        cbe.SelectedIndex = 0;
    }

    private void LgExpanded1()
    {
      if (dsRptOpr.LstTrgtNextProc.Rows.Count == 0)
        dsRptOpr.LstTrgtNextProc.LoadData(19);

      ClsNoPloskF1 = "1";
      IdtargetNextProcF1 = 3;
    }

    private void LgExpanded4(){
      if (dsRptOpr.LstTypeProd.Rows.Count == 0)
        dsRptOpr.LstTypeProd.LoadData(20);

      if (dsRptOpr.LstThickness.Rows.Count == 0)
        dsRptOpr.LstThickness.LoadData(12);

      if (dsRptOpr.LstSort.Rows.Count == 0)
        dsRptOpr.LstSort.LoadData(21);

      IdSortF3 = IdThicknessF3 = IdTypeProdF3 = 0;
    }

    #endregion

    #region Constructor
    internal ViewModelRptOpr(UserControl control, Object Param)
    {
      param = Param;
      rpt = new XlsInstanceBackgroundReport();
      usrControl = control;
      DateBegin = DateEnd = DateIncomplProd1 = DateIncomplProd2 = DateRangeBeginAvoF2 = DateRangeEndAvoF2 = DateRangeBeginUoF2 = DateRangeEndUoF2 = DateStateView = DateTime.Today;
      TeamFinishApr = TeamFinishRoll = "1";
      TypeShiftFinishApr = TypeShiftFinishRoll = "Д";
      gcStateWhsUo = LogicalTreeHelper.FindLogicalNode(usrControl, "GcStateWhsUo") as GridControl;

      //Группы 1-уровня
      foreach (int i in Enum.GetValues(typeof(ModuleConst.AccL1Gr))){
        var lg = LogicalTreeHelper.FindLogicalNode(usrControl, "L1Grp_" + i.ToString(CultureInfo.InvariantCulture)) as LayoutGroup;

        if (lg != null){
          if (Permission.GetPermissionForModuleUif2(i, ModuleConst.ModuleId)){
            lg.Visibility = Visibility.Visible;
            lg.Expanded += LayoutGroupExpanded;
            //this.lg.Collapsed += LayoutGroupCollapsed;
          }
          else
            lg.Visibility = Visibility.Collapsed;

        }
      }

      //Группы 2-уровня
      foreach (int i in Enum.GetValues(typeof(ModuleConst.AccL2Gr))){
        var uie = LogicalTreeHelper.FindLogicalNode(usrControl, "L2Grp_" + i.ToString(CultureInfo.InvariantCulture)) as UIElement;

        if (uie != null){
          if (Permission.GetPermissionForModuleUif2(i, ModuleConst.ModuleId))
            uie.Visibility = Visibility.Visible;
          else
            uie.Visibility = Visibility.Collapsed;
        }
      }

      //Делаем controls невидимыми
      foreach (int i in Enum.GetValues(typeof(ModuleConst.AccRunControl))){
        var btn = LogicalTreeHelper.FindLogicalNode(usrControl, "b" + "_" + i) as UIElement;

        if (btn != null){
          if (Permission.GetPermissionForModuleUif2(i, ModuleConst.ModuleId))
            btn.Visibility = Visibility.Visible;
          else
            btn.Visibility = Visibility.Collapsed;
        }
      }

      //DxInfo.ShowDxBoxInfo("Внимание", "Файлы созданных отчетов будут находиться в папке Документы (Мои документы)!", MessageBoxImage.Information);
      dsRptOpr.LstFinishRoll.LoadData(24);
      
    }
    #endregion Constructor

    #region Command
    private DelegateCommand<Object> loadFromTxtFileCommand;
    private DelegateCommand<Object> shiftRptFinishCommand;
    private DelegateCommand<Object> procLaserAndAprCommand;
    private DelegateCommand<Object> reasonSettleMetalCommand;
    private DelegateCommand<Object> isolFinCut2StrannCommand;
    private DelegateCommand<Object> wghtAvrWidthCommand;
    private DelegateCommand<Object> cuttingMatScrapUoCommand;
    private DelegateCommand<Object> apr8MatOutCommand;
    private DelegateCommand<Object> reasonOfStripBreakageRmAreaCommand;
    private DelegateCommand<Object> qualityIndsUo1Command;
    private DelegateCommand<Object> thickness2ndCutCommand;
    private DelegateCommand<Object> diffCertCommand;
    private DelegateCommand<Object> refRolInExplt;
    private DelegateCommand<Object> outOfServiceMillRolls;
    private DelegateCommand<Object> resultTargetValue;
    private DelegateCommand<Object> sgpAndPsCommand;
    private DelegateCommand<Object> trimAlongUoCommand;
    private DelegateCommand<Object> selectTypeUmCommand;
    private DelegateCommand<Object> monitorDefLngTrimCommand;
    private DelegateCommand<Object> aooMetSleeveCommand;
    private DelegateCommand<Object> stateWhsUoViewCommand;
    private DelegateCommand<Object> stateWhsUoSaveCommand;
    private DelegateCommand<Object> stateWhsUoDeleteCommand;
    private DelegateCommand<Object> stateWhsUoCommand;
    private DelegateCommand<Object> shiftRptRollCommand;
    private DelegateCommand<Object> eliminateDefAvoCommand;
    private DelegateCommand<Object> histCoilProcCommand;
    public ICommand LoadFromTxtFileCommand
    {
      get { return loadFromTxtFileCommand ?? (loadFromTxtFileCommand = new DelegateCommand<Object>(ExecuteLoadFromTxtFile, CanExecuteLoadFromTxtFile)); }
    }

    private void ExecuteLoadFromTxtFile(Object parameter)
    {
      this.ListAnLot = Etc.GetStringWithDelimFromTxtFile(Encoding.GetEncoding("windows-1251"), ",");
    }

    private bool CanExecuteLoadFromTxtFile(Object parameter)
    {
      return true;
    }

    public ICommand ShiftRptFinishCommand => shiftRptFinishCommand ?? (shiftRptFinishCommand = new DelegateCommand<Object>(ExecuteShiftRptFinish, CanExecuteShiftRptFinish));

    private void ExecuteShiftRptFinish(Object parameter)
    {
      string src = Etc.StartPath + ModuleConst.ShiftRptFinishAprSource;
      string dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.ShiftRptFinishAprDest;

      string apr = Convert.ToString(SelFinishAprItem.Row["StrSql"]);
      string aprLabel = Convert.ToString(SelFinishAprItem.Row["StrDlg"]);

      var rptParam = new ShiftRptFinishRptParam(src, dst)
         {
           DateBegin = DateBegin,
           FinishApr = apr,
           FinishAprLabel = aprLabel,
           TeamFinishApr = TeamFinishApr,
           ShiftMasterFinishApr = ShiftMasterFinishApr,
           TopWorkerFinishApr = TopWorkerFinishApr,
           TypeShiftFinishApr = TypeShiftFinishApr,
           IsLogInfo = IsLogInfo
         };

      var sp = new ShiftRptFinish()
      {
        //IdReport = (int)ModuleConst.AccRunControl.ShiftRptUo,
        //ConnectToTargetDb = DbSelector.ConnectToTargetDb,
        //GetCurrentDbAlias = DbSelector.GetCurrentDbAlias
      };

      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }

    private bool CanExecuteShiftRptFinish(Object parameter)
    {
      return true;
    }
    
    public ICommand ProcLaserAndAprCommand => procLaserAndAprCommand ?? (procLaserAndAprCommand = new DelegateCommand<Object>(ExecuteProcLaserAndApr, CanExecuteProcLaserAndApr));

    private void ExecuteProcLaserAndApr(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.ProcLaserAndAprSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.ProcLaserAndAprDest;

      var rptParam = new ProcLaserAndAprRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
        IsGroupParam1F1 = IsGroupParam1F1,
        P1750023LstF1 = P1750_023LstF1,
        P1750027LstF1 = P1750_027LstF1,
        P1750030LstF1 = P1750_030LstF1,
        B800LstF1 = B800LstF1,
        QntWeldsF1 = QntWeldsF1,
        KesiAvgF1 = KesiAvgF1,
        Cat1F1 = Cat1F1,
        Cat2F1 = Cat2F1,
        Cat3F1 = Cat3F1,
        CatWcF1 = CatWcF1,
        AdgInF1 = AdgInF1,
        AdgOutF1 = AdgOutF1,
        CoffWave1F1 = CoffWave1F1,
        CoffWave2F1 = CoffWave2F1,
        HeightWave1F1 = HeightWave1F1,
        HeightWave2F1 = HeightWave2F1,
        ClsNoPloskF1 = ClsNoPloskF1,
        TargetNextProcF1 = Convert.ToString(SelTargetNextProcItemF1.Row["StrDlg"])
      };

      var sp = new ProcLaserAndApr();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }

    private bool CanExecuteProcLaserAndApr(Object parameter)
    {
      return true;
    }
    
    public ICommand ReasonSettleMetalCommand => reasonSettleMetalCommand ?? (reasonSettleMetalCommand = new DelegateCommand<Object>(ExecuteReasonSettleMetal, CanExecuteReasonSettleMetal));

    private void ExecuteReasonSettleMetal(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.ReasonSettleMetalSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.ReasonSettleMetalDest;

      var rptParam = new ReasonSettleMetalRptParam(src, dst)
      {
        IsGroupDateRangeAvoF2 = this.IsGroupDateRangeAvoF2,
        IsGroupDateRangeUoF2 = this.IsGroupDateRangeUoF2,
        DateIncomplProd1 = this.DateIncomplProd1,
        DateIncomplProd2 = this.DateIncomplProd2,
        DateRangeBeginAvoF2 = this.DateRangeBeginAvoF2,
        DateRangeEndAvoF2 = this.DateRangeEndAvoF2,
        DateRangeBeginUoF2 = this.DateRangeBeginUoF2,
        DateRangeEndUoF2 = this.DateRangeEndUoF2
      };

      var sp = new ReasonSettleMetal();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteReasonSettleMetal(Object parameter)
    {
      return true;
    }
    
    public ICommand IsolFinCut2StrannCommand => isolFinCut2StrannCommand ?? (isolFinCut2StrannCommand = new DelegateCommand<Object>(ExecuteIsolFinCut2Strann, CanExecuteIsolFinCut2Strann));

    private void ExecuteIsolFinCut2Strann(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.IsolFinCut2StrannSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.IsolFinCut2StrannDest;

      var rptParam = new IsolFinCut2StrannRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };

      var sp = new IsolFinCut2Strann();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteIsolFinCut2Strann(Object parameter)
    {
      return true;
    }
    
    public ICommand CuttingMatScrapUoCommand => cuttingMatScrapUoCommand ?? (cuttingMatScrapUoCommand = new DelegateCommand<Object>(ExecuteCuttingMatScrapUo, CanExecuteCuttingMatScrapUo));

    private void ExecuteCuttingMatScrapUo(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.CuttingMatScrapUoSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.CuttingMatScrapUoDest;

      var rptParam = new CuttingMatScrapUoRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };

      var sp = new CuttingMatScrapUo();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteCuttingMatScrapUo(Object parameter)
    {
      return true;
    }
    
    public ICommand WghtAvrWidthCommand => wghtAvrWidthCommand ?? (wghtAvrWidthCommand = new DelegateCommand<Object>(ExecuteWghtAvrWidth, CanExecuteWghtAvrWidth));

    private void ExecuteWghtAvrWidth(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.WghtAvrWidthSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.WghtAvrWidthDest;

      var rptParam = new WghtAvrWidthRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
        IsFill1StSheet = IsFill1StSheet,
        IsTypeProdF3 = IsTypeProdF3,
        TypeProdSqlStrF3 = Convert.ToString(SelTypeProdItemF3.Row["StrSql"]),
        IsThicknessF3 = IsThicknessF3,
        ThicknessSqlStrF3 = Convert.ToString(SelThicknessItemF3.Row["StrSql"]),
        IsSortF3 = IsSortF3,
        SortSqlStrF3 = Convert.ToString(SelSortItemF3.Row["StrSql"])
      };
      var sp = new WghtAvrWidth();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteWghtAvrWidth(Object parameter)
    {
      return true;
    }
    
    public ICommand Apr8MatOutCommand => apr8MatOutCommand ?? (apr8MatOutCommand = new DelegateCommand<Object>(ExecuteApr8MatOut, CanExecuteApr8MatOut));

    private void ExecuteApr8MatOut(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.Apr8MatOutSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.Apr8MatOutDest;

      var rptParam = new Apr8MatOutRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new Apr8MatOut();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteApr8MatOut(Object parameter)
    {
      return true;
    }
    
    public ICommand ReasonOfStripBreakageRmAreaCommand => reasonOfStripBreakageRmAreaCommand ?? (reasonOfStripBreakageRmAreaCommand = new DelegateCommand<Object>(ExecuteReasonOfStripBreakageRmArea, CanExecuteReasonOfStripBreakageRmArea));

    private void ExecuteReasonOfStripBreakageRmArea(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.ReasonOfStripBreakageRmAreaSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.ReasonOfStripBreakageRmAreaDest;

      var rptParam = new ReasonOfStripBreakageRmAreaRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new ReasonOfStripBreakageRmArea();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteReasonOfStripBreakageRmArea(Object parameter)
    {
      return true;
    }
    
    public ICommand QualityIndsUo1Command => qualityIndsUo1Command ?? (qualityIndsUo1Command = new DelegateCommand<Object>(ExecuteQualityIndsUo1, CanExecuteQualityIndsUo1));

    private void ExecuteQualityIndsUo1(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.QualityIndsUo1Source;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.QualityIndsUo1Dest;

      var rptParam = new QualityIndsUo1RptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new QualityIndsUo1();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteQualityIndsUo1(Object parameter)
    {
      return true;
    }
    
    public ICommand Thickness2ndCutCommand => thickness2ndCutCommand ?? (thickness2ndCutCommand = new DelegateCommand<Object>(ExecuteThickness2ndCut, CanExecuteThickness2ndCut));

    private void ExecuteThickness2ndCut(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.Thickness2ndCutSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.Thickness2ndCutDest;

      var rptParam = new Thickness2ndCutRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new Thickness2ndCut();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteThickness2ndCut(Object parameter)
    {
      return true;
    }
    
    public ICommand DiffCertCommand => diffCertCommand ?? (diffCertCommand = new DelegateCommand<Object>(ExecuteDiffCert, CanExecuteDiffCert));

    private void ExecuteDiffCert(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.DiffCertSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.DiffCertDest;

      var rptParam = new DiffCertRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new DiffCert();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteDiffCert(Object parameter)
    {
      return true;
    }
    
    public ICommand RefRolInExpltCommand => refRolInExplt ?? (refRolInExplt = new DelegateCommand<Object>(ExecuteRefRolInExplt, CanExecuteRefRolInExplt));

    private void ExecuteRefRolInExplt(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.RefRolInExpltSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.RefRolInExpltDest;

      var rptParam = new RefRolInExpltRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new RefRolInExplt();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteRefRolInExplt(Object parameter)
    {
      return true;
    }
    
    public ICommand OutOfServiceMillRollsCommand => outOfServiceMillRolls ?? (outOfServiceMillRolls = new DelegateCommand<Object>(ExecuteOutOfServiceMillRolls, CanExecuteOutOfServiceMillRolls));

    private void ExecuteOutOfServiceMillRolls(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.OutOfServiceMillRollsSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.OutOfServiceMillRollsDest;

      var rptParam = new OutOfServiceMillRollsRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new OutOfServiceMillRolls();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteOutOfServiceMillRolls(Object parameter)
    {
      return true;
    }
    
    public ICommand ResultTargetValueCommand => resultTargetValue ?? (resultTargetValue = new DelegateCommand<Object>(ExecuteResultTargetValue, CanExecuteResultTargetValue));

    private void ExecuteResultTargetValue(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.ResultTargetValueSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.ResultTargetValueDest;

      var rptParam = new ResultTargetValueRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };
      var sp = new ResultTargetValue();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteResultTargetValue(Object parameter)
    {
      return true;
    }
    

    public ICommand SgpAndPsCommand => sgpAndPsCommand ?? (sgpAndPsCommand = new DelegateCommand<Object>(ExecuteSgpAndPs, CanExecuteSgpAndPs));

    private void ExecuteSgpAndPs(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.SgpAndPsSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.SgpAndPsDest;

      var rptParam = new SgpAndPsRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
        TypeShiftFinishApr = this.TypeShiftFinishApr,
        TypeRpt = Convert.ToInt32(parameter)
      };
      var sp = new SgpAndPs();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteSgpAndPs(Object parameter)
    {
      return true;
    }
    
    public ICommand TrimAlongUoCommand => trimAlongUoCommand ?? (trimAlongUoCommand = new DelegateCommand<Object>(ExecuteTrimAlongUo, CanExecuteTrimAlongUo));

    private void ExecuteTrimAlongUo(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.TrimAlongUoSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.TrimAlongUoDest;

      var rptParam = new TrimAlongUoRptParam(src, dst)
      {
        DateBegin = DateBegin,
        DateEnd = DateEnd,
      };

      var sp = new TrimAlongUo();
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;

    }

    private bool CanExecuteTrimAlongUo(Object parameter)
    {
      return true;
    }

    public ICommand SelectTypeUmCommand
    {
      get { return selectTypeUmCommand ?? (selectTypeUmCommand = new DelegateCommand<Object>(ExecuteSelectTypeUm, CanExecuteSelectTypeUm)); }
    }

    private void ExecuteSelectTypeUm(Object parameter)
    {
      //typeUm = Convert.ToString(parameter);
    }

    private bool CanExecuteSelectTypeUm(Object parameter)
    {
      return true;
    }

    public ICommand MonitorDefLngTrimCommand => monitorDefLngTrimCommand ?? (monitorDefLngTrimCommand = new DelegateCommand<Object>(ExecuteMonitorLngDefTrim, CanExecuteMonitorLngDefLngTrim));
    private void ExecuteMonitorLngDefTrim(Object parameter)
    {
      string src;
      string dst;

      //MessageBox.Show("111");
      //return;

      src = Etc.StartPath + ModuleConst.MonitorLngTrimUoShiftRkSource;
      dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.MonitorLngTrimUoShiftRkDest;
 
        
      var rptParam = new MonitorDefLngTrimUoRptParam(src, dst)
      {
        DateBegin = this.DateBeginQuart,
        DateEnd = this.DateEndQuart,
        RkTotal = this.RkTotal,
        RkPlan = this.RkLng,
        DateMon = this.DateBegin,
        Shift = this.TypeShiftFinishApr
      };
      
      DbUtils.SaveDateQuart(3, this.DateBeginQuart, this.DateEndQuart);
      DbUtils.SaveRk(1,RkTotal, RkLng);
      
      var sp = new MonitorDefLngTrimUo();
      Boolean res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);
      if (!res) return;
      

      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }
    private bool CanExecuteMonitorLngDefLngTrim(Object parameter)
    {
      return true;
    }
    
    public ICommand AooMetSleeveCommand => aooMetSleeveCommand ?? (aooMetSleeveCommand = new DelegateCommand<Object>(ExecuteAooMetSleeve, CanExecuteAooMetSleeve));
    private void ExecuteAooMetSleeve(Object parameter)
    {
      string src;
      string dst;

      src = Etc.StartPath + ModuleConst.AooMetSleeveSource;
      dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.AooMetSleeveDest;

      var rptParam = new AooMetSleeveRptParam(src, dst);

      var sp = new AooMetSleeve();
      Boolean res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);
      if (!res) return;
      
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }
    private bool CanExecuteAooMetSleeve(Object parameter)
    {
      return true;
    }
    
    public ICommand StateWhsUoViewCommand => stateWhsUoViewCommand ?? (stateWhsUoViewCommand = new DelegateCommand<Object>(ExecuteStateWhsUoView, CanExecuteStateWhsUoView));
    private void ExecuteStateWhsUoView(Object parameter)
    {
      dsRptOpr.StateWhsUo.LoadData(DateStateView);
    }
    private bool CanExecuteStateWhsUoView(Object parameter)
    {
      return true;
    }
    
    public ICommand StateWhsUoSaveCommand => stateWhsUoSaveCommand ?? (stateWhsUoSaveCommand = new DelegateCommand<Object>(ExecuteStateWhsUoSave, CanExecuteStateWhsUoSave));
    private void ExecuteStateWhsUoSave(Object parameter)
    {
      (gcStateWhsUo.View as TableView)?.UpdateRow();
      dsRptOpr.StateWhsUo.SaveData();
    }
    private bool CanExecuteStateWhsUoSave(Object parameter)
    {
      return true;
    }
    
    public ICommand StateWhsUoDeleteCommand => stateWhsUoDeleteCommand ?? (stateWhsUoDeleteCommand = new DelegateCommand<Object>(ExecuteStateWhsUoDelete, CanExecuteStateWhsUoDelete));
    private void ExecuteStateWhsUoDelete(Object parameter)
    {
      (gcStateWhsUo.View as TableView)?.DeleteRow(gcStateWhsUo.View.FocusedRowHandle);
      dsRptOpr.StateWhsUo.SaveData();
    }
    private bool CanExecuteStateWhsUoDelete(Object parameter)
    {
      return dsRptOpr.StateWhsUo.Rows.Count > 0;
    }

    public ICommand StateWhsUoCommand => stateWhsUoCommand ?? (stateWhsUoCommand = new DelegateCommand<Object>(ExecuteStateWhsUo, CanExecuteStateWhsUo));
    private void ExecuteStateWhsUo(Object parameter)
    {
      var src = Etc.StartPath + ModuleConst.StateWhsUoSource;
      var dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.StateWhsUoDest;

      var rptParam = new StateWhsUoRptParam(src, dst)
      {
        DateBegin = new DateTime(DateStateView.Year, DateStateView.Month, 1)
      };

      var sp = new StateWhsUo();
      Boolean res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);
      if (!res) return;

      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }
    private bool CanExecuteStateWhsUo(Object parameter)
    {
      return true;
    }

    public ICommand ShiftRptRollCommand => shiftRptRollCommand ?? (shiftRptRollCommand = new DelegateCommand<Object>(ExecuteShiftRptRoll, CanExecuteShiftRptRoll));

    private void ExecuteShiftRptRoll(Object parameter)
    {
      string src = Etc.StartPath + ModuleConst.ShiftRptRollSource;
      string dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.ShiftRptRollDest;

      string rollUnit = Convert.ToString(SelFinishRollItem.Row["StrSql"]);
      string rollLabel = Convert.ToString(SelFinishRollItem.Row["StrDlg"]);

      var rptParam = new ShiftRptRollRptParam(src, dst)
      {
        DateBegin = DateBegin,
        Roll = rollUnit,
        RollLabel = rollLabel,
        TeamRoll = TeamFinishRoll,
        ShiftMasterRoll = ShiftMasterFinishRoll,
        TopWorkerRoll = TopWorkerFinishRoll,
        ShiftTypeRoll = TypeShiftFinishRoll
      };

      var sp = new ShiftRptRoll();
 
      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }

    private bool CanExecuteShiftRptRoll(Object parameter)
    {
      return true;
    }
    
    public ICommand EliminateDefAvoCommand => eliminateDefAvoCommand ?? (eliminateDefAvoCommand = new DelegateCommand<Object>(ExecuteEliminateDefAvo, CanExecuteEliminateDefAvo));

    private void ExecuteEliminateDefAvo(Object parameter)
    {
      string src = Etc.StartPath + ModuleConst.EliminateDefAvoSource;
      string dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.EliminateDefAvoDest;
      
      var rptParam = new EliminateDefAvoRptParam(src, dst)
      {
        LocNum = this.LocNum
      };

      var sp = new EliminateDefAvo();

      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }

    private bool CanExecuteEliminateDefAvo(Object parameter)
    {
      return true;
    }
        
    public ICommand HistCoilProcCommand => histCoilProcCommand ?? (histCoilProcCommand = new DelegateCommand<Object>(ExecuteHistCoilProc, CanExecuteHistCoilProc));

    private void ExecuteHistCoilProc(Object parameter)
    {
      string src = Etc.StartPath + ModuleConst.HistCoilProcSource;
      string dst = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ModuleConst.HistCoilProcDest;

      var rptParam = new HistCoilProcRptParam(src, dst)
      {
        ListAnLot = this.ListAnLot
      };

      var sp = new HistCoilProc();

      var res = sp.RunXls(rpt, RunXlsRptCompleted, rptParam);

      if (!res) return;
      var barEditItem = param as BarEditItem;
      if (barEditItem != null) barEditItem.IsVisible = true;
    }

    private bool CanExecuteHistCoilProc(Object parameter)
    {
      return true;
    }

    #endregion
  }
}
