using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ResultFcastDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ResultFcastDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("LocNum", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("AnnealingLot", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("AnnealingLotSeq", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("NameInd", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("NameAgTyp", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("LfqVal", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("LhqVal", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Tou", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("CfCastQ", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["LocNum"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.V_QMF_RESULT_LFCASTQ", tblName);
        dtm.ColumnMappings.Add("LOCNUM", "LocNum");
        dtm.ColumnMappings.Add("ANNEALINGLOT", "AnnealingLot");
        dtm.ColumnMappings.Add("ANNEALINGLOT_SEQ", "AnnealingLotSeq");
        dtm.ColumnMappings.Add("NAMEIND", "NameInd");
        dtm.ColumnMappings.Add("NAMEAGTYP", "NameAgTyp");
        dtm.ColumnMappings.Add("LFQ_VAL", "LfqVal");
        dtm.ColumnMappings.Add("LHQ_VAL", "LhqVal");
        dtm.ColumnMappings.Add("TOU", "Tou");
        dtm.ColumnMappings.Add("CFCASTQ", "CfCastQ");

        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT LOCNUM, ANNEALINGLOT, ANNEALINGLOT_SEQ, NAMEIND, NAMEAGTYP, LFQ_VAL, LHQ_VAL, TOU, CFCASTQ FROM VIZ_PRN.V_QMF_RESULT_LFCASTQ ORDER BY 2, 3",
          CommandType = CommandType.Text
        };

      }

      public int LoadData()
      {
        return Odac.LoadDataTable(this, adapter, true, null);
      }


    }


  }
}
