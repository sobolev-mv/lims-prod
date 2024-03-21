namespace Viz.WrkModule.RptOoAndPp
{
  public static class ModuleConst
  {
    public const string ModuleId = "178001";

    //Группы 1ур.
    internal enum AccL1Gr
    {
      TurnoverNzp = 33001,
      Pj4Ts = 33002
    }

    //Группы 2ур.
    internal enum AccL2Gr
    {
      ShiftRptUo = 13000,
      ReasonSettleMetal = 13001,
      IsolFinCut2Strann = 13002,
      StateWhsUoEdit = 13003,
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
    public const string TurnoverNzpSource = "\\Xlt\\Viz.WrkModule.RptOoAndPp.TurnoverNzp.xltx";
    public const string TurnoverNzpDest = "\\Viz.WrkModule.RptOoAndPp.TurnoverNzp.xlsx";

    public const string Pj4TsSource = "\\Xlt\\Viz.WrkModule.RptOoAndPp.Pj4Ts.xltx";
    public const string Pj4TsDest = "\\Viz.WrkModule.RptOoAndPp.Pj4Ts.xlsx";

  }
}
