using System;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class InfluenceDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public InfluenceDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("ParamId", typeof(Int64), null, MappingType.Element)
        {
          AllowDBNull = false,
          AutoIncrement = false,
          AutoIncrementStep = -1
        };
        this.Columns.Add(col);

        col = new DataColumn("IndicatorId", typeof(Int64), null, MappingType.Element)
        {
          AllowDBNull = false,
          AutoIncrement = false,
          AutoIncrementStep = -1
        };
        this.Columns.Add(col);

        col = new DataColumn("ValInfluence", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName,
          new[] { this.Columns["ParamId"], this.Columns["IndicatorId"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_INFLUENCE", tblName);
        dtm.ColumnMappings.Add("PARAM_ID", "ParamId");
        dtm.ColumnMappings.Add("INDICATOR_ID", "IndicatorId");
        dtm.ColumnMappings.Add("VAL_INFLUENCE", "ValInfluence");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT PARAM_ID, INDICATOR_ID, VAL_INFLUENCE FROM VIZ_PRN.QMF_INFLUENCE ORDER BY INDICATOR_ID",
          CommandType = CommandType.Text
        };

        //Insert Command
        adapter.InsertCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "INSERT INTO VIZ_PRN.QMF_INFLUENCE(PARAM_ID, INDICATOR_ID, VAL_INFLUENCE) " +
            "VALUES(:PPARAM_ID, :PINDICATOR_ID, :PVAL_INFLUENCE)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        var param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          ParameterName = "PPARAM_ID",
          SourceColumn = "PARAM_ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          ParameterName = "PINDICATOR_ID",
          SourceColumn = "INDICATOR_ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PVAL_INFLUENCE",
          SourceColumn = "VAL_INFLUENCE",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        //Update Command
        adapter.UpdateCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "UPDATE VIZ_PRN.QMF_INFLUENCE SET VAL_INFLUENCE = :PVAL_INFLUENCE " +
            "WHERE (PARAM_ID = :Original_PARAM_ID) AND (INDICATOR_ID = :Original_INDICATOR_ID)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PVAL_INFLUENCE",
          SourceColumn = "VAL_INFLUENCE",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_PARAM_ID",
          SourceColumn = "PARAM_ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_INDICATOR_ID",
          SourceColumn = "INDICATOR_ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);
      }

      public int LoadData()
      {
        return Odac.LoadDataTable(this, adapter, true, null);
      }

      public int SaveData()
      {
        return Odac.SaveChangedData(this, adapter);
      }
    }
  }
}