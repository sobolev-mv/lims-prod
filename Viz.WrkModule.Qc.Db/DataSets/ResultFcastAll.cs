using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ResultFcastAllDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ResultFcastAllDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("NameInd", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false
        };
        this.Columns.Add(col);

        col = new DataColumn("CfCastQIndicator", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["NameInd"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.V_QMF_RESULT_LFCASTQ_INDICATOR", tblName);
        dtm.ColumnMappings.Add("NAMEIND", "NameInd");
        dtm.ColumnMappings.Add("CFCASTQ_INDICATOR", "CfCastQIndicator");

        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT NAMEIND, CFCASTQ_INDICATOR FROM VIZ_PRN.V_QMF_RESULT_LFCASTQ_INDICATOR",
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
