﻿using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using DevExpress.XtraPrinting;
using Viz.WrkModule.Qc.Db.DataSets;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Ribbon;
using Smv.Utils;
using Viz.DbApp.Psi;
using Viz.WrkModule.Qc.Db.Dto;



namespace Viz.WrkModule.Qc
{
  public class ViewModelQc
  {
    #region Fields

    private readonly UserControl usrControl;
    private readonly Window mainWindow;
    private readonly RibbonControl rcMain;
    private readonly BarManager bmMain;
    private readonly DsQc dsQc = new DsQc();
    private readonly DXTabControl tcMain;
    private readonly GridControl gcRef;
    private GridControl gcParamChr;
    private GridControl gcParamChrOpt;
    private GridControl gcParamLnk;
    private GridControl gcFocused;
    private GridControl gcProtCalcUst;

    private ChartControl chartSts;
    private readonly ProgressBarEdit pgbWait;
    private ModuleConst.TypeReferences crTypeRef;
    private DataRow paramDataRow = null;
    private int prevMasterRowHandle = -1;
    private Int64 paramIdKeyVal;
    private double tmpUstGrpDouble;
    private double tmpDffGrpDouble;

    private readonly bool accCmdEditReference;

    #endregion

    #region Public Property

    public virtual string LocNum { get; set; }
    public virtual DateTime DateFrom { get; set; }
    public virtual DateTime DateTo { get; set; }
    public virtual DataTable TypeUst => this.dsQc.TypeUst;
    public virtual Int32 TypeUstId { get; set; }
    public virtual string AgTyp { get; set; }
    public virtual DataTable AgTypTable => this.dsQc.AgTyp;
    public virtual string Agr { get; set; }
    public virtual DataTable Agregate => this.dsQc.Agregate;
    public virtual DataTable Brigade => this.dsQc.Brigade;
    public virtual DataTable ProtCalcUst => this.dsQc.ProtCalcUst;
    public virtual Int32 Brig { get; set; }
    public virtual double ?ResUstGrp { get; set; } = null;
    public virtual string LabelHeaderResUstGrp { get; set; }
    public virtual string LabelResUstGrp { get; set; }
    public virtual double? ResUstDff { get; set; } = null;
    //public virtual string LabelHeaderResUstDff { get; set; }
    public virtual Boolean IsEnableCbAgTyp { get; set; }
    public virtual Boolean IsEnableCbAgr { get; set; }
    public virtual Boolean IsEnableCbBrg { get; set; }
    public virtual Boolean IsControlEnabled { get; set; } = true;

    //Прогнозное качество
    public virtual string CaptionParamInFq { get; set; }
    public virtual string ParamInFq { get; set; }
    public virtual Boolean IsEnableParamInFq { get; set; } = true;
    public virtual Int32 TypeFqId { get; set; } = (int)ModuleConst.TypeFqGrp.Coil;
    public virtual DataTable TypeFqTable => this.dsQc.TypeFq;
    public virtual DataTable AgTypFqTable => this.dsQc.AgTypNzp;
    public virtual string AgTypFq { get; set; } = "0000";
    public virtual DataTable TypeIndFqTable => this.dsQc.TypeIndFq;
    public virtual Int32 TypeIndFqId { get; set; }
    public virtual Boolean IsEnableCbAgTypFq { get; set; }
    public virtual DataTable ResultFcastTable => this.dsQc.ResultFcast;
    //public virtual decimal ?ResForecast { get; set; } = null;
    public virtual string LabelHeaderResForecast { get; set; }
    public virtual string LabelResForecast { get; set; }
    public virtual DataTable ResultFcastAllTable => this.dsQc.ResultFcastAll;
    #endregion

    #region Protected Method
    protected void OnTypeUstIdChanged()
    {
      switch ((ModuleConst.TypeUstGrp)TypeUstId)
      {
        case ModuleConst.TypeUstGrp.Agregate:
          IsEnableCbAgTyp = IsEnableCbAgr = IsEnableCbBrg = true;
          break;
        case ModuleConst.TypeUstGrp.AgTyp:
          IsEnableCbAgTyp = true;
          IsEnableCbAgr = IsEnableCbBrg = false;
          break;
        case ModuleConst.TypeUstGrp.WorkShop:
          IsEnableCbAgTyp = IsEnableCbAgr = IsEnableCbBrg = false;
          break;
        default:
          return;
      }
    }

    protected void OnAgTypChanged()
    {
      Agr = String.Empty;
      this.dsQc.Agregate.LoadData(AgTyp);
    }

    protected void OnTypeFqIdChanged()
    {
      if ((ModuleConst.TypeFqGrp)TypeFqId == ModuleConst.TypeFqGrp.CoilsNzp)
      {
        ParamInFq = string.Empty;
        IsEnableParamInFq = false;
        AgTypFq = "0000";
        IsEnableCbAgTypFq = true;
        CaptionParamInFq = String.Empty;
      }
      else
      {
        IsEnableParamInFq = true;
        IsEnableCbAgTypFq = false;
        CaptionParamInFq = Db.Utils.GetNameTypeForecast(TypeFqId) + ":";
      }
    }

    #endregion

    #region Private Method
    private void RibbonSelectedPageChanged(object sender, RibbonPropertyChangedEventArgs e)
    {
      if (e.OldValue == null)
        return;

      if (e.NewValue == null)
        return;

      if ((e.OldValue as FrameworkContentElement).Tag == null)
        return;

      if ((e.NewValue as FrameworkContentElement).Tag == null)
        return;

      tcMain.SelectedIndex = Convert.ToInt32((e.NewValue as FrameworkContentElement).Tag);
    }

    private void ParamItemChanged(object sender, CurrentItemChangedEventArgs args)
    {
      //btnXSamplesRowChanged.CommandParameter = (sender as DevExpress.Xpf.Grid.GridViewBase).Grid.GetRow(e.RowData.RowHandle.Value);
      if (args.NewItem != null)
      {
        paramDataRow = (args.NewItem as DataRowView).Row;
        paramIdKeyVal = Convert.ToInt64(this.paramDataRow["Id"]);
        this.dsQc.ParamChr.LoadData(paramIdKeyVal);
        this.dsQc.ParamChrOpt.LoadData(paramIdKeyVal);
        this.dsQc.ParamLnk.LoadData(paramIdKeyVal);
      }
      else
        paramDataRow = null;
    }

    private void MasterRowExpanded(object sender, RowEventArgs e)
    {
      GridControl gcDetail = (sender as GridControl).GetDetail(e.RowHandle) as GridControl;

      if ((prevMasterRowHandle >= 0) && e.RowHandle != prevMasterRowHandle)
        (sender as GridControl).CollapseMasterRow(prevMasterRowHandle);

      gcDetail.ItemsSource = dsQc.ParamChr;
      prevMasterRowHandle = e.RowHandle;
    }

    private void FocusedViewChanged(object sender, FocusedViewChangedEventArgs e)
    {
      var detailGrid = (e.NewView.DataControl).OwnerDetailDescriptor as DataControlDetailDescriptor;

      if (detailGrid == null)
      {
        gcFocused = null;
        return;
      }

      gcFocused = e.NewView.DataControl as GridControl;

      var tag = (ModuleConst.TypeParamsGc)Convert.ToInt32(detailGrid.DataControl.Tag);

      switch (tag)
      {
        case ModuleConst.TypeParamsGc.GcParamChrOpt:
          e.NewView.DataControl.ItemsSource = dsQc.ParamChrOpt;
          break;
        case ModuleConst.TypeParamsGc.GcParamLnk:
          e.NewView.DataControl.ItemsSource = dsQc.ParamLnk;
          break;
      }
    }

    private void ParamChrNewRow(object sender, DataTableNewRowEventArgs e)
    {
      e.Row["ParamId"] = paramIdKeyVal;
    }


    void CreateGroupParamRef()
    {
      (gcRef.View as TableView).AllowMasterDetail = true;
      gcRef.ItemsSource = dsQc.ParamGroup;
      var col = new GridColumn()
      {
        FieldName = "Id",
        Header = "ID",
      };

      TextEditSettings textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "d",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;

      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "Name",
        Header = "Наименование"
      };
      gcRef.Columns.Add(col);
    }

    void CreateParamRef()
    {
      //Обновляем группы параметров
      dsQc.ParamGroup.LoadData();

      this.gcRef.CurrentItemChanged += ParamItemChanged;
      this.gcRef.MasterRowExpanded += MasterRowExpanded;
      this.gcRef.View.FocusedViewChanged += FocusedViewChanged;

      gcRef.ItemsSource = dsQc.Param;
      var col = new GridColumn()
      {
        FieldName = "Id",
        Header = "ID"
      };
      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "GroupId",
        Header = "Группа параметров"
      };

      var lookUpSettings = new LookUpEditSettings
      {
        StyleSettings = new SearchLookUpEditStyleSettings(),
        DisplayMember = "Name",
        ValueMember = "Id",
        ItemsSource = dsQc.ParamGroup
      };
      col.EditSettings = lookUpSettings;
      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "Name",
        Header = "Наименование"
      };
      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "InCalc",
        Header = "Участвует в расчете ДЗ"
      };

      var checkSettings = new CheckEditSettings();
      col.EditSettings = checkSettings;
      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "InCalcOp",
        Header = "Участвует в расчете ОЗ"
      };
      checkSettings = new CheckEditSettings();
      col.EditSettings = checkSettings;
      gcRef.Columns.Add(col);


      //Создаем Detail Grids
      DataControlDetailDescriptor dataControlDetail1 = new DataControlDetailDescriptor();
      //dataControlDetail1.ItemsSourcePath = "ParamChr";
      gcParamChr = new GridControl();
      gcParamChr.Tag = 1;
      dataControlDetail1.DataControl = gcParamChr;
      gcParamChr.View.DetailHeaderContent = "Допустимые значения параметров";
      gcParamChr.View.AllowEditing = accCmdEditReference;
      (gcParamChr.View as TableView).ShowGroupPanel = false;
      (gcParamChr.View as TableView).NewItemRowPosition = NewItemRowPosition.Bottom;
      (gcParamChr.View as TableView).NavigationStyle = GridViewNavigationStyle.Cell;
     
      col = new GridColumn()
      {
        FieldName = "ParamId",
        Header = "ID",
        ReadOnly = true
      };
      gcParamChr.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "Thickness",
        Header = "Толщина"
      };
      lookUpSettings = new LookUpEditSettings
      {
        StyleSettings = new SearchLookUpEditStyleSettings(),
        DisplayMember = "TextDispaly",
        ValueMember = "Thickness",
        ItemsSource = dsQc.Thickness
      };
      col.EditSettings = lookUpSettings;
      gcParamChr.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "MinVal",
        Header = "Мин. значение"
      };
      var textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n4",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcParamChr.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "MaxVal",
        Header = "Макс. значение"
      };
      textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n4",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcParamChr.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "LogVal",
        Header = "Логическое значение"
      };
      checkSettings = new CheckEditSettings();
      col.EditSettings = checkSettings;
      gcParamChr.Columns.Add(col);
      
      DataControlDetailDescriptor dataControlDetail2 = new DataControlDetailDescriptor();
      //dataControlDetail.ItemsSourcePath = "Orders";
      gcParamChrOpt = new GridControl();
      gcParamChrOpt.Tag = 2;
      dataControlDetail2.DataControl = gcParamChrOpt;
      gcParamChrOpt.View.DetailHeaderContent = "Оптимальные значения параметров";
      gcParamChrOpt.View.AllowEditing = accCmdEditReference;
      (gcParamChrOpt.View as TableView).ShowGroupPanel = false;
      (gcParamChrOpt.View as TableView).NewItemRowPosition = NewItemRowPosition.Bottom;


      col = new GridColumn()
      {
        FieldName = "ParamId",
        Header = "ID",
        ReadOnly = true
      };
      gcParamChrOpt.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "Thickness",
        Header = "Толщина"
      };
      lookUpSettings = new LookUpEditSettings
      {
        StyleSettings = new SearchLookUpEditStyleSettings(),
        DisplayMember = "TextDispaly",
        ValueMember = "Thickness",
        ItemsSource = dsQc.Thickness
      };
      col.EditSettings = lookUpSettings;
      gcParamChrOpt.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "MinVal",
        Header = "Мин. значение"
      };
      textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n4",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcParamChrOpt.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "MaxVal",
        Header = "Макс. значение"
      };
      textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n4",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcParamChrOpt.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "LogVal",
        Header = "Логическое значение"
      };
      checkSettings = new CheckEditSettings();
      col.EditSettings = checkSettings;
      gcParamChrOpt.Columns.Add(col);

      DataControlDetailDescriptor dataControlDetail3 = new DataControlDetailDescriptor();
      //dataControlDetail.ItemsSourcePath = "Orders";
      gcParamLnk = new GridControl();
      gcParamLnk.Tag = 3;
      dataControlDetail3.DataControl = gcParamLnk;
      gcParamLnk.View.DetailHeaderContent = "Зависимость параметров";
      gcParamLnk.View.AllowEditing = accCmdEditReference;
      (gcParamLnk.View as TableView).ShowGroupPanel = false;
      (gcParamLnk.View as TableView).NewItemRowPosition = NewItemRowPosition.Bottom;

      col = new GridColumn()
      {
        FieldName = "ParamId",
        Header = "ID",
        ReadOnly = true
      };
      gcParamLnk.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "ParamIdLnk",
        Header = "Параметр"
      };

      lookUpSettings = new LookUpEditSettings
      {
        StyleSettings = new SearchLookUpEditStyleSettings(),
        DisplayMember = "Name",
        ValueMember = "Id",
        ItemsSource = dsQc.Param
      };
      col.EditSettings = lookUpSettings;
      gcParamLnk.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "CofLnk",
        Header = "Влияние"
      };

      textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n3",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcParamLnk.Columns.Add(col);

      //ContentDetailDescriptor contentDetail = new ContentDetailDescriptor();
      //contentDetail.ContentTemplate = (DataTemplate)FindResource("EmployeeNotes");
      //contentDetail.HeaderContent = "Notes";

      TabViewDetailDescriptor tabDetail = new TabViewDetailDescriptor();
      tabDetail.DetailDescriptors.Add(dataControlDetail1);
      tabDetail.DetailDescriptors.Add(dataControlDetail2);
      tabDetail.DetailDescriptors.Add(dataControlDetail3);
      //tabDetail.DetailDescriptors.Add(contentDetail);

      gcRef.DetailDescriptor = tabDetail;
    }

    void CreateQmIndicatorRef()
    {
      gcRef.ItemsSource = dsQc.QmIndicator;
      var col = new GridColumn()
      {
        FieldName = "Id",
        Header = "ID",
      };

      TextEditSettings textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "d",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;

      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "Name",
        Header = "Наименование"
      };
      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "Tou",
        Header = "ТОУ"
      };

      textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n3",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcRef.Columns.Add(col);

    }

    void CreateInfluenceRef()
    {
      //Обновляем параметры и показатели
      dsQc.ParamGroup.LoadData();
      dsQc.QmIndicator.LoadData();

      gcRef.ItemsSource = dsQc.Influence;


      var col = new GridColumn()
      {
        FieldName = "ParamId",
        Header = "Параметр"
      };

      var lookUpSettings = new LookUpEditSettings
      {
        StyleSettings = new SearchLookUpEditStyleSettings(),
        DisplayMember = "Name",
        ValueMember = "Id",
        ItemsSource = dsQc.Param
      };
      col.EditSettings = lookUpSettings;
      gcRef.Columns.Add(col);


      col = new GridColumn()
      {
        FieldName = "IndicatorId",
        Header = "Показатель качества"
      };

      lookUpSettings = new LookUpEditSettings
      {
        StyleSettings = new SearchLookUpEditStyleSettings(),
        DisplayMember = "Name",
        ValueMember = "Id",
        ItemsSource = dsQc.QmIndicator
      };
      col.EditSettings = lookUpSettings;
      gcRef.Columns.Add(col);

      col = new GridColumn()
      {
        FieldName = "ValInfluence",
        Header = "Воздействие"
      };

      TextEditSettings textSetinngs = new TextEditSettings
      {
        MaskType = MaskType.Numeric,
        Mask = "n3",
        MaskIgnoreBlank = false,
        MaskUseAsDisplayFormat = true,
      };
      col.EditSettings = textSetinngs;
      gcRef.Columns.Add(col);

    }

    /*Для выполнения операции в другом потоке*/
    private void StartWaitPgb()
    {
      pgbWait.StyleSettings = new ProgressBarMarqueeStyleSettings();
      (((ProgressBarMarqueeStyleSettings)pgbWait.StyleSettings)).AccelerateRatio = 10;
    }

    private void EndWaitPgb()
    {
      this.pgbWait.StyleSettings = new ProgressBarStyleSettings();
    }

    private void TaskCalcUst4LocNum(Object state)
    {
      dsQc.UstTrendQuality.Rows.Clear();
      LabelHeaderResUstGrp = LabelResUstGrp = null;
      ResUstGrp = ResUstDff = null;
      Db.Utils.CalcParam4LocNum(ModuleConst.CS_TypeClcParamVld, LocNum);
      dsQc.UstTrendQuality.LoadData(ModuleConst.CS_TypeClcParamVld, LocNum);
      dsQc.UstTrendDff.LoadData(ModuleConst.CS_TypeClcParamVld, LocNum);
      dsQc.ProtCalcUst.LoadData();

    }

    private void CreateTrendUst(string strTitle, DataTable dataSourceTrend, string strLegend, Color trendColor)
    {
      chartSts.BeginInit();

      if (!string.IsNullOrEmpty(strTitle))
        chartSts.Titles.Add(new Title()
                                {
                                  Content = strTitle,
                                  HorizontalAlignment = HorizontalAlignment.Center
                                }
                           );

      if (chartSts.Legends.Count == 0)
        chartSts.Legends.Add(new Legend
                                 {
                                   HorizontalPosition = HorizontalPosition.Center,
                                   VerticalPosition = VerticalPosition.BottomOutside,
                                   Orientation = Orientation.Horizontal
                                 }
                            );

      if (chartSts.Diagram == null)
        chartSts.Diagram = new XYDiagram2D
        {
          AxisY = new AxisY2D
          {
            GridLinesVisible = true,
            GridLinesMinorVisible = true,
            VisualRange = new DevExpress.Xpf.Charts.Range
            {
              MinValue = 0,
              MaxValue = 1
            }
          },
          AxisX = new AxisX2D()
          {
            GridLinesVisible = true,
            GridLinesMinorVisible = true,
            VisualRange = new DevExpress.Xpf.Charts.Range()
          }
        };

      //LineSeries2D s;
      //s.Points[0].Brush 

      var colorizer = new DevExpress.Xpf.Charts.RangeSegmentColorizer
      {
        RangeStops = new DoubleCollection() { 0, 2 },
        Palette = new DevExpress.Xpf.Charts.CustomPalette
        {
          Colors = { trendColor }
        },
        ShowInLegend = false
      };
      
      var lineSeries2D = new LineSeries2D
      {
        Label = new SeriesLabel
        {
          FontSize = 16
        },
        LabelsVisibility = true,
        ValueScaleType = ScaleType.Numerical,
        MarkerVisible = true,
        ValueDataMember = "RatioSts",
        ArgumentDataMember = "NameGroup",
        DataSource = dataSourceTrend,
        DisplayName = strLegend,
        SegmentColorizer = colorizer
      };

      chartSts.Diagram.Series.Add(lineSeries2D);
      chartSts.EndInit();
    }
    private void ClearUstChart()
    {
      foreach (var legend in chartSts.Legends)
        legend.Visible = false;

      chartSts.Diagram?.Series.Clear();
      chartSts.Titles.Clear();
      chartSts.Legend?.Items.Clear();
      chartSts.Legends.Clear();
      chartSts.Diagram = null;
    }
    private void AfterTaskEndCalcUst4LocNum(Task obj)
    {
      this.usrControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() =>
      {
        tcMain.SelectedIndex = 1;

        //Db.Utils.CalcParam4LocNum(ModuleConst.CS_TypeClcParamVld, LocNum);
        //dsQc.Sts.LoadData(ModuleConst.CS_TypeClcParamVld, LocNum);

        if (dsQc.UstTrendQuality.Rows.Count == 0)
        {
          DXMessageBox.Show(Application.Current.Windows[0], "Данные по материалу отсутствуют.\r\nМатериал не найден или кон. толщина не равна 0.23, 0.27, 0.30, 0.35", "Нет данных", MessageBoxButton.OK, MessageBoxImage.Warning);
          EndWaitPgb();
          IsControlEnabled = true;
          return;
        }

        double ustAll = Db.Utils.GetUst4LocNum(ModuleConst.CS_TypeClcParamVld, LocNum);
        double ustDff = Db.Utils.GetDff4LocNum(ModuleConst.CS_TypeClcParamVld, LocNum);

        //Важен порядок отрисовки трендов!
        CreateTrendUst(null, dsQc.UstTrendDff, "Коэфф наполнения данных (КНД)", Colors.Blue);
        var strTitle = $"Лок. №: {LocNum}     УСТ общий: {ustAll}     КНД общий: {ustDff}";
        CreateTrendUst(strTitle, dsQc.UstTrendQuality, "Уровень соблюдения технологии (УСТ)", Colors.Red);
       
        

        gcProtCalcUst.ItemsSource = ProtCalcUst;

        LabelHeaderResUstGrp = "УСТ общий:";
        LabelResUstGrp = null;
        ResUstGrp = ustAll;
        ResUstDff = ustDff;

        EndWaitPgb();
        IsControlEnabled = true;
        CommandManager.InvalidateRequerySuggested();
      }));
    }

    public void TaskCalcUstGrp(Object state)
    {
      dsQc.UstTrendQuality.Rows.Clear();
      LabelHeaderResUstGrp = LabelResUstGrp = null;
      ResUstGrp = ResUstDff = null;
      string tmpAgTyp;


      switch ((ModuleConst.TypeUstGrp)TypeUstId)
      {
        case ModuleConst.TypeUstGrp.Agregate:
          Db.Utils.CalcParam4AgTypAgr(ModuleConst.CS_TypeClcParamVld, DateFrom, DateTo, AgTyp, Agr, Brig);
          dsQc.ProtCalcUst.LoadData(AgTyp);
          tmpAgTyp = AgTyp;
          break;
        case ModuleConst.TypeUstGrp.AgTyp:
          Db.Utils.CalcParam4AgTypAgr(ModuleConst.CS_TypeClcParamVld, DateFrom, DateTo, AgTyp, null, 0);
          dsQc.ProtCalcUst.LoadData(AgTyp);
          tmpAgTyp = AgTyp;
          break;
        case ModuleConst.TypeUstGrp.WorkShop:
          Db.Utils.CalcParam4WorkShop(ModuleConst.CS_TypeClcParamVld, DateFrom, DateTo);
          dsQc.ProtCalcUst.LoadData4WorkShop();
          tmpAgTyp = null;
          break;
        default:
          return;
      }
      
      tmpUstGrpDouble = Db.Utils.GetUst4AgTypAgr(ModuleConst.CS_TypeClcParamVld);
      tmpDffGrpDouble = Db.Utils.GetDff4Grp(tmpAgTyp);
    }

    private void AfterTaskEndCalcUstGrp(Task obj)
    {
      this.usrControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() =>
      {
        tcMain.SelectedIndex = 1;
        CreateLabelResUstGrp();
        ResUstGrp = tmpUstGrpDouble;
        ResUstDff = tmpDffGrpDouble;
        gcProtCalcUst.ItemsSource = ProtCalcUst;
        EndWaitPgb();
        IsControlEnabled = true;
        CommandManager.InvalidateRequerySuggested();
      }));
    }

    private void CreateLabelResUstGrp()
    {
      LabelHeaderResUstGrp = $"Расчет за период с { DateFrom: dd.MM.yyyy} по { DateTo: dd.MM.yyyy}: ";

      switch ((ModuleConst.TypeUstGrp)TypeUstId)
      {
        case ModuleConst.TypeUstGrp.Agregate:
          LabelResUstGrp = Db.Utils.GetNameTypeUst(TypeUstId) + " ● " +
                           Db.Utils.GetNameAgTyp(AgTyp) + " ● " + Db.Utils.GetNameAgregate(AgTyp, Agr) +
                           " ● " + Db.Utils.GetNameBrigade(Brig);
          break;
        case ModuleConst.TypeUstGrp.AgTyp:
          LabelResUstGrp = Db.Utils.GetNameTypeUst(TypeUstId) + " ● " +
                           Db.Utils.GetNameAgTyp(AgTyp);
          break;
        case ModuleConst.TypeUstGrp.WorkShop:
          LabelResUstGrp = Db.Utils.GetNameTypeUst(TypeUstId);
          break;
      }
    }

    public void TaskGenRptGnrUst(Object state)
    {
      var dtoRpt = state as DtoRptGnrUstParamInput;

      switch ((ModuleConst.TypeUstGrp)TypeUstId)
      {
        case ModuleConst.TypeUstGrp.Agregate:
          dtoRpt.AgTyp = null;
          Db.Reports.ReportGeneralUst.CreateGeneralUstAgregate(dtoRpt);
          break;
        case ModuleConst.TypeUstGrp.AgTyp:
          dtoRpt.Agr = null;
          Db.Reports.ReportGeneralUst.CreateGeneralUstAgTyp(dtoRpt);
          break;
        case ModuleConst.TypeUstGrp.WorkShop:
          dtoRpt.AgTyp = null;
          dtoRpt.Agr = null;
          Db.Reports.ReportGeneralUst.CreateGeneralUstWs(dtoRpt);
          break;
        default:
          return;
      }

      //tmpUstGrpDouble = Db.Utils.GetUst4AgTypAgr(ModuleConst.CS_TypeClcParamVld);
      //tmpDffGrpDouble = Db.Utils.GetDff4Grp(tmpAgTyp);
    }

    private void AfterTaskEndGenRptGnrUst(Task obj)
    {
      this.usrControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() =>
      {
        tcMain.SelectedIndex = 1;
        EndWaitPgb();
        IsControlEnabled = true;
        CommandManager.InvalidateRequerySuggested();
      }));
    }

    public void TaskGenListMatUst(Object state)
    {
      var dtoRpt = state as DtoRptListMatUstParamInput;
      Db.Reports.ReportListMatUst.CreateListMatUst(dtoRpt);
      //tmpUstGrpDouble = Db.Utils.GetUst4AgTypAgr(ModuleConst.CS_TypeClcParamVld);
      //tmpDffGrpDouble = Db.Utils.GetDff4Grp(tmpAgTyp);
    }
    

    public void TaskCalcForecastQualityCoil(Object state)
    {
      Db.Utils.CalcForecastQualityCoil(ParamInFq, TypeIndFqId);
      CreateLabelResForecast();
      //ResForecast = Db.Utils.GetResForecast() as decimal?;
    }

    private void AfterTaskCalcForecastQualityCoil(Task obj)
    {
      this.usrControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() =>
      {
        tcMain.SelectedIndex = 2;
        EndWaitPgb();
        IsControlEnabled = true;
        dsQc.ResultFcast.LoadData();
        dsQc.ResultFcastAll.LoadData();
        CommandManager.InvalidateRequerySuggested();
      }));
    }

    public void TaskCalcForecastQualityAnLot(Object state)
    {
      Db.Utils.CalcForecastQualityAnLot(ParamInFq, TypeIndFqId);
      CreateLabelResForecast();
      //ResForecast = Db.Utils.GetResForecast() as decimal?;
    }

    public void TaskCalcForecastQualityListAnLot(Object state)
    {
      Db.Utils.CalcForecastQualityListAnLot(ParamInFq, TypeIndFqId);
      CreateLabelResForecast();
      //ResForecast = Db.Utils.GetResForecast() as decimal?;
    }

    public void TaskCalcForecastQualityCoilsNzp(Object state)
    {
      Db.Utils.CalcForecastQualityCoilsNzp(AgTypFq, TypeIndFqId);
      CreateLabelResForecast();
      //ResForecast = Db.Utils.GetResForecast() as decimal?;
    }

    private void CreateLabelResForecast()
    {
      if (((ModuleConst.TypeFqGrp) TypeFqId == ModuleConst.TypeFqGrp.Coil) ||
         ((ModuleConst.TypeFqGrp)TypeFqId == ModuleConst.TypeFqGrp.Lot) ||
         ((ModuleConst.TypeFqGrp)TypeFqId == ModuleConst.TypeFqGrp.ListLot))

        LabelResForecast = Db.Utils.GetNameTypeForecast(TypeFqId) + " ● " + Db.Utils.GetNameTypeIndForecast(TypeIndFqId);
      else if ((ModuleConst.TypeFqGrp)TypeFqId == ModuleConst.TypeFqGrp.CoilsNzp)
        LabelResForecast = Db.Utils.GetNameTypeForecast(TypeFqId) + " ● " + Db.Utils.GetNameTypeIndForecast(TypeIndFqId) + " ● " + Db.Utils.GetNameAgTypForecast(AgTypFq);

      LabelHeaderResForecast = ModuleConst.CS_LabelHeaderResForecast;
    }
    #endregion

    #region Constructor

    public ViewModelQc(UserControl control, Object mainWindow)
    {
      usrControl = control;
      this.mainWindow = mainWindow as Window;

      rcMain = LogicalTreeHelper.FindLogicalNode(mainWindow as Window, "rcMain") as RibbonControl;
      rcMain.SelectedPageChanged += RibbonSelectedPageChanged;
      bmMain = LogicalTreeHelper.FindLogicalNode(mainWindow as Window, "bmMain") as BarManager;


      tcMain = LogicalTreeHelper.FindLogicalNode(this.usrControl, "tcMain") as DXTabControl;
      gcRef = LogicalTreeHelper.FindLogicalNode(this.usrControl, "GcRef") as GridControl;
      gcProtCalcUst = LogicalTreeHelper.FindLogicalNode(this.usrControl, "GcProtCalcUst") as GridControl;
      /*
      if (this.dbgMaterial != null)
        this.dbgMaterial.CurrentItemChanged += CurrentItemChanged;
      */

      chartSts = LogicalTreeHelper.FindLogicalNode(control, "ChartSts") as ChartControl;
      pgbWait = LogicalTreeHelper.FindLogicalNode(this.usrControl, "PgbWait") as ProgressBarEdit;
      dsQc.ParamGroup.LoadData();
      dsQc.Param.LoadData();
      dsQc.QmIndicator.LoadData();
      dsQc.Influence.LoadData();
      dsQc.Thickness.LoadData();
      dsQc.TypeUst.LoadData();
      dsQc.AgTyp.LoadData();
      dsQc.Brigade.LoadData();
      dsQc.TypeFq.LoadData();
      dsQc.TypeIndFq.LoadData();
      dsQc.AgTypNzp.LoadData4Nzp();

      dsQc.ParamChr.TableNewRow += ParamChrNewRow;
      dsQc.ParamChrOpt.TableNewRow += ParamChrNewRow;
      dsQc.ParamLnk.TableNewRow += ParamChrNewRow;

      DateFrom = DateTo = DateTime.Today;

      accCmdEditReference = Permission.GetPermissionForModuleUif(ModuleConst.AccCmdEditReference, ModuleConst.ModuleId);
      gcRef.View.AllowEditing = accCmdEditReference;

      CaptionParamInFq = Db.Utils.GetNameTypeForecast(TypeFqId) + ":";
    }


    #endregion

    #region Command

    public void SelectTypeRef(Object param)
    {
      gcRef.Columns.Clear();
      this.gcRef.CurrentItemChanged -= ParamItemChanged;
      this.gcRef.MasterRowExpanded -= MasterRowExpanded;
      this.gcRef.View.FocusedViewChanged -= FocusedViewChanged;

      //(gcRef.DetailDescriptor as TabViewDetailDescriptor)?.DetailDescriptors.Clear();
      gcRef.DetailDescriptor = null;


      crTypeRef = (ModuleConst.TypeReferences)Convert.ToInt32(param);

      switch (crTypeRef)
      {
        case ModuleConst.TypeReferences.GroupParam:
          CreateGroupParamRef();
          break;
        case ModuleConst.TypeReferences.Param:
          CreateParamRef();
          break;
        case ModuleConst.TypeReferences.QmIndicator:
          CreateQmIndicatorRef();
          break;
        case ModuleConst.TypeReferences.Influence:
          CreateInfluenceRef();
          break;
      }
    }

    /*
    public bool CanShowDefectMap()
    {
      return true;
    }
    */

    public void SaveData()
    {
      switch (crTypeRef)
      {
        case ModuleConst.TypeReferences.GroupParam:
          dsQc.ParamGroup.SaveData();
          break;
        case ModuleConst.TypeReferences.Param:
          dsQc.Param.SaveData();
          dsQc.ParamChr.SaveData();
          dsQc.ParamChrOpt.SaveData();
          dsQc.ParamLnk.SaveData();
          break;
        case ModuleConst.TypeReferences.QmIndicator:
          dsQc.QmIndicator.SaveData();
          break;
        case ModuleConst.TypeReferences.Influence:
          dsQc.Influence.SaveData();
          break;
      }
    }

    public bool CanSaveData()
    {
      return dsQc.HasChanges() && IsControlEnabled && accCmdEditReference;
      ;
    }

    public void DeleteData()
    {
      if (crTypeRef != ModuleConst.TypeReferences.Param)
        (gcRef.View as TableView).DeleteRow(gcRef.View.FocusedRowHandle);
      else if ((gcFocused == null) && (crTypeRef == ModuleConst.TypeReferences.Param))
        (gcRef.View as TableView).DeleteRow(gcRef.View.FocusedRowHandle);
      else if ((gcFocused != null) && (crTypeRef == ModuleConst.TypeReferences.Param))
        (gcFocused.View as TableView).DeleteRow(gcFocused.View.FocusedRowHandle);
    }

    public bool CanDeleteData()
    {
      if ((gcRef.View.IsFocusedView) && (gcRef.View.FocusedRowHandle >= 0) &&
          (crTypeRef != ModuleConst.TypeReferences.Param))
        return true && IsControlEnabled && accCmdEditReference;
      else if ((gcFocused == null) && (gcRef.View.IsFocusedView) && (gcRef.View.FocusedRowHandle >= 0) &&
               (crTypeRef == ModuleConst.TypeReferences.Param))
        return true && IsControlEnabled && accCmdEditReference;
      else if ((gcFocused != null) && (gcFocused.View.IsFocusedView) && (gcFocused.View.FocusedRowHandle >= 0) &&
               (crTypeRef == ModuleConst.TypeReferences.Param))
        return true && IsControlEnabled && accCmdEditReference;
      else
        return false;
    }

    public void ReportParam()
    {
      Db.Utils.ParamRpt();
    }

    public bool CanReportParam()
    {
      return IsControlEnabled;
    }

    public void CalcUst4LocNum()
    {
      ClearUstChart();
      IsControlEnabled = false;
      StartWaitPgb();
      dsQc.ProtCalcUst.Rows.Clear();
      gcProtCalcUst.ItemsSource = null;
      var task = Task.Factory.StartNew(TaskCalcUst4LocNum, null).ContinueWith(AfterTaskEndCalcUst4LocNum);
    }

    public bool CanCalcUst4LocNum()
    {
      return (!String.IsNullOrEmpty(this.LocNum)) && IsControlEnabled;
    }

    public void ExportStsToGraphFile()
    {
      var sfd = new SaveFileDialog
      {
        OverwritePrompt = false,
        AddExtension = true,
        DefaultExt = ".png",
        Filter = "png file (.png)|*.png"
      };

      if (sfd.ShowDialog().GetValueOrDefault() != true)
        return;

      if (File.Exists(sfd.FileName))
      {
        DxInfo.ShowDxBoxInfo("Файл", "Файл: " + sfd.FileName + " уже существует!", MessageBoxImage.Error);
        return;
      }

      var imgExportOption = new ImageExportOptions()
      {
        ExportMode = ImageExportMode.SingleFile,
        Format = System.Drawing.Imaging.ImageFormat.Png
      };

      chartSts.ExportToImage(sfd.FileName, imgExportOption);
    }

    public bool CanExportStsToGraphFile()
    {
      return (dsQc.UstTrendQuality.Rows.Count > 0) && IsControlEnabled;
    }

    public void CalcUstGrp()
    {
      dsQc.ProtCalcUst.Rows.Clear();
      gcProtCalcUst.ItemsSource = null;

      ClearUstChart();

      IsControlEnabled = false;
      StartWaitPgb();
      var task = Task.Factory.StartNew(TaskCalcUstGrp, null).ContinueWith(AfterTaskEndCalcUstGrp);
    }

    public bool CanCalcUstGrp()
    {
      if ((ModuleConst.TypeUstGrp)TypeUstId == ModuleConst.TypeUstGrp.WorkShop)
        return true && IsControlEnabled;

      if (((ModuleConst.TypeUstGrp)TypeUstId == ModuleConst.TypeUstGrp.AgTyp) && (!String.IsNullOrEmpty(AgTyp)))
        return true && IsControlEnabled;

      if (((ModuleConst.TypeUstGrp)TypeUstId == ModuleConst.TypeUstGrp.Agregate) && (!String.IsNullOrEmpty(AgTyp)) && (!String.IsNullOrEmpty(Agr)))
        return true && IsControlEnabled;
      
      return false; 
    }

    public void ExportProtCalcUstGrp()
    {
      var sfd = new SaveFileDialog
      {
        OverwritePrompt = false,
        AddExtension = true,
        DefaultExt = ".xlsx",
        Filter = "xlsx file (.xlsx)|*.xlsx"
      };

      if (sfd.ShowDialog().GetValueOrDefault() != true)
        return;

      if (File.Exists(sfd.FileName))
        DxInfo.ShowDxBoxInfo("Файл", "Файл: " + sfd.FileName + " уже существует и будет перезаписан!", MessageBoxImage.Warning);
        
      

      gcProtCalcUst.View.ExportToXlsx(sfd.FileName);
      DxInfo.ShowDxBoxInfo("Экспорт данных", "Экспорт данных в файл: " + sfd.FileName + " успешно выполнен", MessageBoxImage.Information);
    }

    public bool CanExportProtCalcUstGrp()
    {
      return (IsControlEnabled && (dsQc.ProtCalcUst.Rows.Count > 0));
    }

    public void CalcForecastQuality()
    {
      LabelHeaderResForecast = LabelResForecast = null;
      //ResForecast = null;

      tcMain.SelectedIndex = 2;
      IsControlEnabled = false;
      dsQc.ResultFcast.Rows.Clear();
      dsQc.ResultFcastAll.Rows.Clear();
      StartWaitPgb();
      
      switch ((ModuleConst.TypeFqGrp)TypeFqId)
      {
        case ModuleConst.TypeFqGrp.Coil:
          var task = Task.Factory.StartNew(TaskCalcForecastQualityCoil, null).ContinueWith(AfterTaskCalcForecastQualityCoil);
          break;
        case ModuleConst.TypeFqGrp.Lot:
          task = Task.Factory.StartNew(TaskCalcForecastQualityAnLot, null).ContinueWith(AfterTaskCalcForecastQualityCoil);
          break;
        case ModuleConst.TypeFqGrp.ListLot:
          task = Task.Factory.StartNew(TaskCalcForecastQualityListAnLot, null).ContinueWith(AfterTaskCalcForecastQualityCoil);
          break;
        case ModuleConst.TypeFqGrp.CoilsNzp:
          task = Task.Factory.StartNew(TaskCalcForecastQualityCoilsNzp, null).ContinueWith(AfterTaskCalcForecastQualityCoil);
          break;
      }
    }

    public bool CanCalcForecastQuality()
    {
      return true;
    }
    public void RptGnrUst()
    {
      var dtoRpt = new DtoRptGnrUstParamInput()
      {
        DtThickness = dsQc.Thickness,
        DateFrom = this.DateFrom,
        DateTo = this.DateTo,
        AgTyp = this.AgTyp,
        Agr = this.Agr
      };
      
      var wnd = new ViewRptGnrUstParamInput(this.mainWindow, dtoRpt);
      if (!wnd.ShowDialog().GetValueOrDefault())
        return;
      
      IsControlEnabled = false;
      StartWaitPgb();
      var task = Task.Factory.StartNew(TaskGenRptGnrUst, dtoRpt).ContinueWith(AfterTaskEndGenRptGnrUst);
      
    }
    public bool CanRptGnrUst()
    {
      return CanCalcUstGrp();
    }

    public void RptListMatUst()
    {
      var dtoRpt = new DtoRptListMatUstParamInput();
      
      var wnd = new ViewRptListMatUstParamInput(this.mainWindow, dtoRpt);
      if (!wnd.ShowDialog().GetValueOrDefault())
        return;

      IsControlEnabled = false;
      StartWaitPgb();
      var task = Task.Factory.StartNew(TaskGenListMatUst, dtoRpt).ContinueWith(AfterTaskEndGenRptGnrUst);
      
    }
    public bool CanRptListMatUst()
    {
      return true;
    }

    #endregion

  }

  }
