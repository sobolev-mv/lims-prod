﻿namespace Viz.WrkModule.RptOpr
{
  public static class ModuleConst
  {
    public const string ModuleId = "173001";

    //Группы 1ур.
    internal enum AccL1Gr
    {
      ShiftRptUo = 10001,
      ProcLaserAndApr  = 10002,
      ReasonSettleMetal = 10003,
      IsolFinCut2Strann = 10004,
      WghtAvrWidth = 10005,
      Equipment = 10006,
      PackUo = 10007,
      MonitorTrimUoShift = 10008,
      StateWhsUo = 10009,
      ShiftRptRoll = 10010,
      EliminateDefAvo = 10011,
      HistCoilProc = 10012
    }

    //Группы 2ур.
    internal enum AccL2Gr
    {
      ShiftRptUo = 13000,
      ReasonSettleMetal = 13001,
      IsolFinCut2Strann = 13002,
      StateWhsUoEdit = 13003
    }

    //Кнопки запуска отчетов
    internal enum AccRunControl
    {
      ShiftRptUo = 16000,
      ProcLaserAndApr = 16001,
      ReasonSettleMetal = 16002,
      IsolFinCut2Strann = 16003,
      WghtAvrWidth = 16004,
      CuttingMatScrapUo = 16005,
      Apr8MatOut = 16006,
      ReasonOfStripBreakageRmArea = 16007,
      QualityIndsUo1 = 16008,
      Thickness2ndCut = 16009,
      DiffCert = 16010,
      RefRolInExplt = 16011,
      OutOfServiceMillRolls = 16012,
      ResultTargetValue = 16013,
      SgpAndPsToGp = 16014,
      SgpAndPsRepSGp = 16015,
      TrimAlongUo = 16016,
      MonitorTrimUoShift = 16017,
      AooMetSleeve = 16018,
      AllowMonitorTrimUoShift = 16019,
      CrossTrimUo = 16020
    }

    public const string ScriptsFolder = "\\Scripts";
    //public const string ShiftRptFinishAprLaserSource = "\\Xlt\\Viz.WrkModule.RptOpr-ShiftRptFinishLaser.xltx";
    //public const string ShiftRptFinishAprLaserDest = "\\Viz.WrkModule.RptOpr-ShiftRptFinishLaser.xlsx";
    //public const string ShiftRptFinishApr12Source = "\\Xlt\\Viz.WrkModule.RptOpr-ShiftRptFinishApr12.xltx";
    //public const string ShiftRptFinishApr12Dest = "\\Viz.WrkModule.RptOpr-ShiftRptFinishApr12.xlsx";
    //public const string ShiftRptFinishAprOtherSource = "\\Xlt\\Viz.WrkModule.RptOpr-ShiftRptFinishOther.xltx";
    //public const string ShiftRptFinishAprOtherDest = "\\Viz.WrkModule.RptOpr-ShiftRptFinishOther.xlsx";
    public const string ProcLaserAndAprSource = "\\Xlt\\Viz.WrkModule.RptOpr-ProcLaserAndApr.xltx";
    public const string ProcLaserAndAprDest = "\\Viz.WrkModule.RptOpr-ProcLaserAndApr.xlsx";
    public const string ReasonSettleMetalSource = "\\Xlt\\Viz.WrkModule.RptOpr-ReasonSettleMetal.xltx";
    public const string ReasonSettleMetalDest = "\\Viz.WrkModule.RptOpr-ReasonSettleMetal.xlsx";
    public const string IsolFinCut2StrannSource = "\\Xlt\\Viz.WrkModule.RptOpr-IsolFincut2Strann.xltx";
    public const string IsolFinCut2StrannDest = "\\Viz.WrkModule.RptOpr-IsolFincut2Strann.xlsx";
    public const string WghtAvrWidthSource = "\\Xlt\\Viz.WrkModule.RptOpr-WghtAvrWidth.xltx";
    public const string WghtAvrWidthDest = "\\Viz.WrkModule.RptOpr-WghtAvrWidth.xlsx";
    public const string CuttingMatScrapUoSource = "\\Xlt\\Viz.WrkModule.RptOpr-CuttingMatScrapUo.xltx";
    public const string CuttingMatScrapUoDest = "\\Viz.WrkModule.RptOpr-CuttingMatScrapUo.xlsx";
    public const string ShiftRptFinishAprSource = "\\Xlt\\Viz.WrkModule.RptOpr-ShiftRptFinishAll.xltx";
    public const string ShiftRptFinishAprDest = "\\Viz.WrkModule.RptOpr-ShiftRptFinishAll.xlsx";
    public const string Apr8MatOutSource = "\\Xlt\\Viz.WrkModule.RptOpr-Apr8MatOut.xltx";
    public const string Apr8MatOutDest = "\\Viz.WrkModule.RptOpr-Apr8MatOut.xlsx";
    public const string ReasonOfStripBreakageRmAreaSource = "\\Xlt\\Viz.WrkModule.RptOpr-ReasonOfStripBreakageRmArea.xltx";
    public const string ReasonOfStripBreakageRmAreaDest = "\\Viz.WrkModule.RptOpr-ReasonOfStripBreakageRmArea.xlsx";
    public const string QualityIndsUo1Source = "\\Xlt\\Viz.WrkModule.RptOpr-QualityIndsUo1.xltx";
    public const string QualityIndsUo1Dest = "\\Viz.WrkModule.RptOpr-QualityIndsUo1.xlsx";
    public const string Thickness2ndCutSource = "\\Xlt\\Viz.WrkModule.RptOpr-Thickness2ndCut.xltx";
    public const string Thickness2ndCutDest = "\\Viz.WrkModule.RptOpr-Thickness2ndCut.xlsx";
    public const string DiffCertSource = "\\Xlt\\Viz.WrkModule.RptOpr-DiffCert.xltx";
    public const string DiffCertDest = "\\Viz.WrkModule.RptOpr-DiffCert.xlsx";
    public const string RefRolInExpltSource = "\\Xlt\\Viz.WrkModule.RptOpr-RefRolInExplt.xltx";
    public const string RefRolInExpltDest = "\\Viz.WrkModule.RptOpr-RefRolInExplt.xlsx";
    public const string OutOfServiceMillRollsSource = "\\Xlt\\Viz.WrkModule.RptOpr-OutOfServiceMillRolls.xltx";
    public const string OutOfServiceMillRollsDest = "\\Viz.WrkModule.RptOpr-OutOfServiceMillRolls.xlsx";
    public const string ResultTargetValueSource = "\\Xlt\\Viz.WrkModule.RptOpr-ResultTargetValue.xltx";
    public const string ResultTargetValueDest = "\\Viz.WrkModule.RptOpr-ResultTargetValue.xlsx";
    public const string SgpAndPsSource = "\\Xlt\\Viz.WrkModule.RptOpr-SgpAndPs.xltx";
    public const string SgpAndPsDest = "\\Viz.WrkModule.RptOpr-SgpAndPs.xlsx";
    public const string TrimAlongUoSource = "\\Xlt\\Viz.WrkModule.RptOpr-TrimAlongUo.xltx";
    public const string TrimAlongUoDest = "\\Viz.WrkModule.RptOpr-TrimAlongUo.xlsx";

    public const string MonitorLngTrimUoShiftRkSource = "\\Xlt\\Viz.WrkModule.RptOpr-MonitorTrimUoShiftRk.xltx";
    public const string MonitorLngTrimUoShiftRkDest = "\\Viz.WrkModule.RptOpr-MonitorTrimUoShiftRk.xlsx";
    public const string AooMetSleeveSource = "\\Xlt\\Viz.WrkModule.RptOpr-AooMetSleeve.xltx";
    public const string AooMetSleeveDest = "\\Viz.WrkModule.RptOpr-AooMetSleeve.xlsx";

    public const string StateWhsUoSource = "\\Xlt\\Viz.WrkModule.RptOpr-StateWhsUo.xltx";
    public const string StateWhsUoDest = "\\Viz.WrkModule.RptOpr-StateWhsUo.xlsx";

    public const string ShiftRptRollSource = "\\Xlt\\Viz.WrkModule.RptOpr-ShiftRptRoll.xltx";
    public const string ShiftRptRollDest = "\\Viz.WrkModule.RptOpr-ShiftRptRoll.xlsx";
    
    public const string EliminateDefAvoSource = "\\Xlt\\Viz.WrkModule.RptOpr-EliminateDefAvo.xltx";
    public const string EliminateDefAvoDest = "\\Viz.WrkModule.RptOpr-EliminateDefAvo.xlsx";

    public const string HistCoilProcSource = "\\Xlt\\Viz.WrkModule.RptOpr-HistCoilProc.xltx";
    public const string HistCoilProcDest = "\\Viz.WrkModule.RptOpr-HistCoilProc.xlsx";

    public const string QiUo1Source = "\\Xlt\\Viz.WrkModule.RptOpr-QiUo1.xltx";
    public const string QiUo1ProcDest = "\\Viz.WrkModule.RptOpr-QiUo1.xlsx";

    public const string QiExUo1Source = "\\Xlt\\Viz.WrkModule.RptOpr-QiExUo1.xltx";
    public const string QiExUo1ProcDest = "\\Viz.WrkModule.RptOpr-QiExUo1.xlsx";

    public const string CrossTrimUoSource = "\\Xlt\\Viz.WrkModule.RptOpr-CrossTrimUo.xltx";
    public const string CrossTrimUoDest = "\\Viz.WrkModule.RptOpr-CrossTrimUo.xlsx";

  }
}
