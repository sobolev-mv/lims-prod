using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ParamChrOptDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ParamChrOptDataTable(string tblName) : base()
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

        col = new DataColumn("Thickness", typeof(double), null, MappingType.Element)
        {
          AllowDBNull = false,
          AutoIncrement = false,
          AutoIncrementStep = -1
        };
        this.Columns.Add(col);

        col = new DataColumn("MinVal", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("MaxVal", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("LogVal", typeof(int), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName,
          new[] { this.Columns["ParamId"], this.Columns["Thickness"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_PARAM_CHR_OPT", tblName);
        dtm.ColumnMappings.Add("PARAM_ID", "ParamId");
        dtm.ColumnMappings.Add("THICKNESS", "Thickness");
        dtm.ColumnMappings.Add("MIN_VAL", "MinVal");
        dtm.ColumnMappings.Add("MAX_VAL", "MaxVal");
        dtm.ColumnMappings.Add("LOG_VAL", "LogVal");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT PARAM_ID, THICKNESS, MIN_VAL, MAX_VAL, LOG_VAL " +
                        "FROM VIZ_PRN.QMF_PARAM_CHR_OPT " +
                        "WHERE PARAM_ID = :PPARAM_ID " +
                        "ORDER BY THICKNESS",
          CommandType = CommandType.Text
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
        adapter.SelectCommand.Parameters.Add(param);


        //Insert Command
        adapter.InsertCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "INSERT INTO VIZ_PRN.QMF_PARAM_CHR_OPT(PARAM_ID, THICKNESS, MIN_VAL, MAX_VAL, LOG_VAL) " +
            "VALUES(:PPARAM_ID, :PTHICKNESS, :PMIN_VAL, :PMAX_VAL, :PLOG_VAL)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
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
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PTHICKNESS",
          SourceColumn = "THICKNESS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PMIN_VAL",
          SourceColumn = "MIN_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PMAX_VAL",
          SourceColumn = "MAX_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int32,
          OracleDbType = OracleDbType.Integer,
          Direction = ParameterDirection.Input,
          ParameterName = "PLOG_VAL",
          SourceColumn = "LOG_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        //Update Command
        adapter.UpdateCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "UPDATE VIZ_PRN.QMF_PARAM_CHR_OPT SET THICKNESS = :PTHICKNESS, MIN_VAL = :PMIN_VAL, MAX_VAL = :PMAX_VAL, LOG_VAL = :PLOG_VAL " +
            "WHERE (PARAM_ID = :Original_PARAM_ID) AND (THICKNESS = :Original_THICKNESS)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PTHICKNESS",
          SourceColumn = "THICKNESS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PMIN_VAL",
          SourceColumn = "MIN_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PMAX_VAL",
          SourceColumn = "MAX_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int32,
          OracleDbType = OracleDbType.Integer,
          Direction = ParameterDirection.Input,
          ParameterName = "PLOG_VAL",
          SourceColumn = "LOG_VAL",
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
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_THICKNESS",
          SourceColumn = "THICKNESS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        //DeleteCommand
        adapter.DeleteCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "DELETE VIZ_PRN.QMF_PARAM_CHR_OPT WHERE (PARAM_ID = :Original_PARAM_ID) AND (THICKNESS = :Original_THICKNESS)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

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
        adapter.DeleteCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_THICKNESS",
          SourceColumn = "THICKNESS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.DeleteCommand.Parameters.Add(param);
      }

      public int LoadData(Int64 paramId)
      {
        var lstPrmValue = new List<Object> { paramId };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }

      public int SaveData()
      {
        return Odac.SaveChangedData(this, adapter);
      }
    }
  }
}