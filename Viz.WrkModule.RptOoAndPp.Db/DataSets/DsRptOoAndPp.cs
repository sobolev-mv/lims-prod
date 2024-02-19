using System;
using System.Data;
using System.Collections.Generic;
using Smv.Data.Oracle;
using Devart.Data.Oracle;

namespace Viz.WrkModule.RptOoAndPp.Db.DataSets
{
  public sealed class DsRptOoAndPp : DataSet
  {
    public TrnNzpDataTable TrnNzp { get; private set; }

    public DsRptOoAndPp() : base()
    {
      this.DataSetName = "DsRptOoAndPp";

      this.TrnNzp = new TrnNzpDataTable("TrnNzp");
      this.Tables.Add(this.TrnNzp);
    }

    public sealed class TrnNzpDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public TrnNzpDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Whs", typeof(string), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("DateBegin", typeof(DateTime), null, MappingType.Element) { AllowDBNull = false }; 
        this.Columns.Add(col);

        col = new DataColumn("DateEnd", typeof(DateTime), null, MappingType.Element) { AllowDBNull = false }; 
        this.Columns.Add(col);

        col = new DataColumn("TurnVal", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["Whs"], this.Columns["DateBegin"], this.Columns["DateEnd"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.DG_TURNOVER_NZP", tblName);
        dtm.ColumnMappings.Add("WHS", "Whs");
        dtm.ColumnMappings.Add("DATE_BEGIN", "DateBegin");
        dtm.ColumnMappings.Add("DATE_END", "DateEnd");
        dtm.ColumnMappings.Add("TURN_VAL", "TurnVal");

        adapter.TableMappings.Add(dtm);

        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT WHS, DATE_BEGIN, DATE_END, TURN_VAL FROM VIZ_PRN.DG_TURNOVER_NZP WHERE WHS = :PWHS ORDER BY 2",
          CommandType = CommandType.Text
        };

        var param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "PWHS",
          SourceColumn = "WHS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.SelectCommand.Parameters.Add(param);

        //Insert Command
        adapter.InsertCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "INSERT INTO VIZ_PRN.DG_TURNOVER_NZP(WHS, DATE_BEGIN, DATE_END, TURN_VAL) " +
            "VALUES(:PWHS, :PDATE_BEGIN, :PDATE_END, :PTURN_VAL)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "PWHS",
          SourceColumn = "WHS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_BEGIN",
          SourceColumn = "DATE_BEGIN",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_END",
          SourceColumn = "DATE_END",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PTURN_VAL",
          SourceColumn = "TURN_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        //Update Command
        adapter.UpdateCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "UPDATE VIZ_PRN.DG_TURNOVER_NZP SET " +
            "WHS = :PWHS, DATE_BEGIN = :PDATE_BEGIN, DATE_END = :PDATE_END, TURN_VAL = :PTURN_VAL " +
            "WHERE (WHS = :Original_WHS) AND (DATE_BEGIN = :Original_DATE_BEGIN) AND (DATE_END = :Original_DATE_END)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "PWHS",
          SourceColumn = "WHS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_BEGIN",
          SourceColumn = "DATE_BEGIN",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_END",
          SourceColumn = "DATE_END",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PTURN_VAL",
          SourceColumn = "TURN_VAL",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_WHS",
          SourceColumn = "WHS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_DATE_BEGIN",
          SourceColumn = "DATE_BEGIN",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_DATE_END",
          SourceColumn = "DATE_END",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        //DeleteCommand
        adapter.DeleteCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "DELETE VIZ_PRN.DG_TURNOVER_NZP WHERE (WHS = :Original_WHS) AND (DATE_BEGIN = :Original_DATE_BEGIN) AND (DATE_END = :Original_DATE_END)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_WHS",
          SourceColumn = "WHS",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.DeleteCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_DATE_BEGIN",
          SourceColumn = "DATE_BEGIN",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.DeleteCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "Original_DATE_END",
          SourceColumn = "DATE_END",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.DeleteCommand.Parameters.Add(param);

      }
      public int LoadData(string whs)
      {
        var lstPrmValue = new List<Object> { whs };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }
      public int SaveData()
      {
        return Odac.SaveChangedData(this, adapter);
      }




    }




  }
}
