using System;
using System.Data;
using System.Collections.Generic;
using Smv.Data.Oracle;
using Devart.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ParamLnkDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ParamLnkDataTable(string tblName) : base()
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

        col = new DataColumn("ParamIdLnk", typeof(Int64), null, MappingType.Element)
        {
          AllowDBNull = false,
          AutoIncrement = false,
          AutoIncrementStep = -1
        };
        this.Columns.Add(col);

        col = new DataColumn("CofLnk", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName,
          new[] { this.Columns["ParamId"], this.Columns["ParamIdLnk"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_PARAM_LNK", tblName);
        dtm.ColumnMappings.Add("PARAM_ID", "ParamId");
        dtm.ColumnMappings.Add("PARAM_ID_LNK", "ParamIdLnk");
        dtm.ColumnMappings.Add("COF_LNK", "CofLnk");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT PARAM_ID, PARAM_ID_LNK, COF_LNK " +
                        "FROM VIZ_PRN.QMF_PARAM_LNK " +
                        "WHERE PARAM_ID = :PPARAM_ID " +
                        "ORDER BY PARAM_ID_LNK",
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
            "INSERT INTO VIZ_PRN.QMF_PARAM_LNK(PARAM_ID, PARAM_ID_LNK, COF_LNK) " +
            "VALUES(:PPARAM_ID, :PPARAM_ID_LNK, :PCOF_LNK)",
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
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          ParameterName = "PPARAM_ID_LNK",
          SourceColumn = "PARAM_ID_LNK",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);
        
        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PCOF_LNK",
          SourceColumn = "COF_LNK",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        //Update Command
        adapter.UpdateCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "UPDATE VIZ_PRN.QMF_PARAM_LNK SET PARAM_ID_LNK = :PPARAM_ID_LNK, COF_LNK = :PCOF_LNK " +
            "WHERE (PARAM_ID = :Original_PARAM_ID) AND (PARAM_ID_LNK = :Original_PARAM_ID_LNK)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          ParameterName = "PPARAM_ID_LNK",
          SourceColumn = "PARAM_ID_LNK",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);


        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PCOF_LNK",
          SourceColumn = "COF_LNK",
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
          ParameterName = "Original_PARAM_ID_LNK",
          SourceColumn = "PARAM_ID_LNK",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        //DeleteCommand
        adapter.DeleteCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "DELETE VIZ_PRN.QMF_PARAM_LNK WHERE (PARAM_ID = :Original_PARAM_ID) AND (PARAM_ID_LNK = :Original_PARAM_ID_LNK)",
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
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Int64,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_PARAM_ID_LNK",
          SourceColumn = "PARAM_ID_LNK",
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
