using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class TypeIndFqDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public TypeIndFqDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Id", typeof(Int32), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("Name", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["Id"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.V_QMF_TYPEIND_FQ", tblName);
        dtm.ColumnMappings.Add("ID", "Id");
        dtm.ColumnMappings.Add("NAME", "Name");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT ID, NAME FROM VIZ_PRN.V_QMF_TYPEIND_FQ ORDER BY 1",
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
