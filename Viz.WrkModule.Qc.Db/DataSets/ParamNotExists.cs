using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ParamNotExistsDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ParamNotExistsDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("GroupId", typeof(int), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("GroupName", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("ParamId", typeof(Int64), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("ParamName", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["GroupId"], this.Columns["ParamId"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.V_QMF_STS_NEPARAM", tblName);
        dtm.ColumnMappings.Add("GROUP_ID", "GroupId");
        dtm.ColumnMappings.Add("GROUP_NAME", "GroupName");
        dtm.ColumnMappings.Add("PARAM_ID", "ParamId");
        dtm.ColumnMappings.Add("PARAM_NAME", "ParamName");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME FROM VIZ_PRN.V_QMF_STS_NEPARAM",
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
