using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class AgregateDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public AgregateDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Agr", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("Name", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["Agr"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_AGREGATE", tblName);
        dtm.ColumnMappings.Add("AGR", "Agr");
        dtm.ColumnMappings.Add("NAME", "Name");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT AGR, NAME FROM VIZ_PRN.QMF_AGREGATE WHERE AGTYP = :PAGTYP",
          CommandType = CommandType.Text
        };

        var prm = new OracleParameter
        {
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          ParameterName = "PAGTYP"
        };
        adapter.SelectCommand.Parameters.Add(prm);

      }

      public int LoadData(string agTyp)
      {
        var lstPrmValue = new List<Object> { agTyp };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }

    }

  }
}
