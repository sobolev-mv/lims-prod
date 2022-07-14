using System;
using System.Data;
using System.Collections.Generic;
using Smv.Data.Oracle;
using Devart.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc : DataSet
  {
    public ParamGroupDataTable ParamGroup { get; private set; }
    public ParamDataTable Param { get; private set; }
    public QmIndicatorDataTable QmIndicator { get; private set; }
    public InfluenceDataTable Influence { get; private set; }
    public ParamChrDataTable ParamChr { get; private set; }
    public ThicknessDataTable Thickness { get; private set; }
    public ParamChrOptDataTable ParamChrOpt { get; private set; }
    public ParamLnkDataTable ParamLnk { get; private set; }
    public StsDataTable Sts { get; private set; }
    public TypeUstDataTable TypeUst { get; private set; }
    public AgTypDataTable AgTyp { get; private set; }
    public AgregateDataTable Agregate { get; private set; }
    public BrigadeDataTable  Brigade { get; private set; }
    public TypeFqDataTable TypeFq { get; private set; }
    public TypeIndFqDataTable TypeIndFq  { get; private set; }
    public ResultFcastDataTable ResultFcast { get; private set; }
    public AgTypDataTable AgTypNzp { get; private set; }
    public ResultFcastAllDataTable ResultFcastAll { get; private set; }
    public ParamNotExistsDataTable ParamNotExists { get; private set; }

    public DsQc() : base()
    {
      this.DataSetName = "DsQc";

      this.ParamGroup = new ParamGroupDataTable("ParamGroup");
      this.Tables.Add(this.ParamGroup);

      this.Param = new ParamDataTable("Param");
      this.Tables.Add(this.Param);

      this.QmIndicator = new QmIndicatorDataTable("QmIndicator");
      this.Tables.Add(this.QmIndicator);

      this.Influence = new InfluenceDataTable("Influence");
      this.Tables.Add(this.Influence);

      this.ParamChr = new ParamChrDataTable("ParamChr");
      this.Tables.Add(this.ParamChr);

      this.Thickness = new ThicknessDataTable("Thickness");
      this.Tables.Add(this.Thickness);

      this.ParamChrOpt = new ParamChrOptDataTable("ParamChrOpt");
      this.Tables.Add(this.ParamChrOpt);

      this.ParamLnk = new ParamLnkDataTable("ParamLnk");
      this.Tables.Add(this.ParamLnk);

      this.Sts = new StsDataTable("Sts");
      this.Tables.Add(this.Sts);

      this.TypeUst = new TypeUstDataTable("TypeUst");
      this.Tables.Add(this.TypeUst);

      this.AgTyp = new AgTypDataTable("AgTyp");
      this.Tables.Add(this.AgTyp);

      this.Agregate = new AgregateDataTable("Agregate");
      this.Tables.Add(this.Agregate);

      this.Brigade = new BrigadeDataTable("Brigade");
      this.Tables.Add(this.Brigade);

      this.TypeFq = new TypeFqDataTable("TypeFq");
      this.Tables.Add(this.TypeFq);

      this.TypeIndFq = new TypeIndFqDataTable("TypeIndFq");
      this.Tables.Add(this.TypeIndFq);

      this.ResultFcast = new ResultFcastDataTable("ResultFcast");
      this.Tables.Add(this.ResultFcast);

      this.AgTypNzp = new AgTypDataTable("AgTypNzp");
      this.Tables.Add(this.AgTypNzp);

      this.ResultFcastAll = new ResultFcastAllDataTable("ResultFcastAll");
      this.Tables.Add(this.ResultFcastAll);

      this.ParamNotExists = new ParamNotExistsDataTable("ParamNotExists");
      this.Tables.Add(this.ParamNotExists);

    }
  }

}
