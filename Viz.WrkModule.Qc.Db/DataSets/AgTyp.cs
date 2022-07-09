using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class AgTypDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public AgTypDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("AgTyp", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("Name", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["AgTyp"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_PARAM_GROUP", tblName);
        dtm.ColumnMappings.Add("AGTYP", "AgTyp");
        dtm.ColumnMappings.Add("NAME", "Name");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT AGTYP, NAME FROM VIZ_PRN.QMF_PARAM_GROUP ORDER BY ID",
          CommandType = CommandType.Text
        };

      }

      public int LoadData()
      {
        return Odac.LoadDataTable(this, adapter, true, null);
      }

      public int LoadData4Nzp()
      {
        adapter.SelectCommand.CommandText = "SELECT AGTYP, NAME FROM VIZ_PRN.V_QMF_AGTYP_NZP ORDER BY ID";
        return Odac.LoadDataTable(this, adapter, true, null);
      }

    }
    
  }
}
