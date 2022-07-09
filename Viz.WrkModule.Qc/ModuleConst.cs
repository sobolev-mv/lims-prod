namespace Viz.WrkModule.Qc
{

  public static class ModuleConst
  {
    public const string ModuleId = "177001";

    public enum TypeReferences
    {
      GroupParam = 0,
      Param = 1,
      QmIndicator = 2,
      Influence = 3
    };

    public enum TypeParamsGc
    {
      GcParam = 0,
      GcParamChr = 1,
      GcParamChrOpt = 2,
      GcParamLnk = 3
    };

    public enum TypeUstGrp
    {
      Agregate = 10,
      AgTyp = 20,
      WorkShop = 30
    };

    public enum TypeFqGrp
    {
      Coil = 1,
      Lot = 2,
      ListLot = 3,
      CoilsNzp = 4
    };

    //Группы 1ур.
    internal enum AccL1Gr
    {
      ShiftRptUo = 10001
    }

    //Группы 2ур.
    internal enum AccL2Gr
    {
      ShiftRptUo = 13000
    }

    //Кнопки запуска отчетов
    internal enum AccRunControl
    {
      ShiftRptUo = 16000
    }


    public const int AccCmdEditReference = 23001;

    public const string CS_TypeClcParamVld = "VLD";
    public const string CS_TypeClcParamOpt = "OPT";
    public const string CS_LabelHeaderResForecast = "Параметры расчета:";
  }
}
