using System;
using System.Data;
using System.Collections.Generic;
using Smv.Data.Oracle;
using Devart.Data.Oracle;

namespace Viz.WrkModule.MapDefects.Db.DataSets
{
  public sealed partial class DsMapDef : DataSet
  {
    public MapDefDataTable MapDef {get; private set;}
    public LstDefZonesDataTable LstDefZones { get; private set; }
    public CutMatDataTable CutMat { get; private set; }
    public TrendDataTable Trend { get; private set; }

    public DsMapDef() : base()
    {
      this.DataSetName = "DsMapDef";

      this.MapDef = new MapDefDataTable();
      this.Tables.Add(this.MapDef);

      this.LstDefZones = new LstDefZonesDataTable();
      this.Tables.Add(this.LstDefZones);

      this.CutMat = new CutMatDataTable();
      this.Tables.Add(this.CutMat);

      this.Trend = new TrendDataTable();
      this.Tables.Add(this.Trend);
    }

  }
}
