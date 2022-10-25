using System;
using System.Data;
using System.Collections.Generic;
using Smv.Data.Oracle;
using Devart.Data.Oracle;

namespace Viz.WrkModule.RptOpr.Db.DataSets
{
  public sealed class DsRptOpr : DataSet
  {

    public PatternDataTable LstFinishApr { get; private set; }
    public PatternDataTable LstTrgtNextProc { get; private set; }
    public PatternDataTable LstTypeProd { get; private set; }
    public PatternDataTable LstThickness { get; private set; }
    public PatternDataTable LstSort { get; private set; }
    public StateWhsUoDataTable StateWhsUo { get; private set; }

    public DsRptOpr() : base()
    {
      this.DataSetName = "DsRptOpr";

      this.LstFinishApr = new PatternDataTable("LstFinishApr");
      this.Tables.Add(this.LstFinishApr);

      this.LstTrgtNextProc = new PatternDataTable("LstTrgtNextProc");
      this.Tables.Add(this.LstTrgtNextProc);

      this.LstTypeProd = new PatternDataTable("LstTypeProd");
      this.Tables.Add(this.LstTypeProd);

      this.LstThickness = new PatternDataTable("LstThickness");
      this.Tables.Add(this.LstThickness);

      this.LstSort = new PatternDataTable("LstSort");
      this.Tables.Add(this.LstSort);

      this.StateWhsUo = new StateWhsUoDataTable("StateWhsUo");
      this.Tables.Add(this.StateWhsUo);

    }

    public sealed class PatternDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public PatternDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Id", typeof(Int32), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("StrSql", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("StrDlg", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["Id"] }, true));
        this.Columns["Id"].Unique = true;

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.DG_QSTLANGL", tblName);
        dtm.ColumnMappings.Add("ID_ITEM", "Id");
        dtm.ColumnMappings.Add("STR_SQL", "StrSql");
        dtm.ColumnMappings.Add("STR_DLG", "StrDlg");
        adapter.TableMappings.Add(dtm);

        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT ID_ITEM, STR_SQL, STR_DLG FROM VIZ_PRN.DG_QSTLANGL WHERE ID_LIST = :IDLST ORDER BY 1",
          CommandType = CommandType.Text
        };

        var param = new OracleParameter
        {
          DbType = DbType.Int32,
          OracleDbType = OracleDbType.Integer,
          Direction = ParameterDirection.Input,
          ParameterName = "IDLST",
          SourceColumn = "ID_LIST",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.SelectCommand.Parameters.Add(param);
      }

      public int LoadData(int typeList)
      {
        var lstPrmValue = new List<Object> {typeList};
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }

    }

    public sealed class StateWhsUoDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public StateWhsUoDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("DateState", typeof(DateTime), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("Thickness", typeof(double), null, MappingType.Element) { AllowDBNull = false }; ;
        this.Columns.Add(col);

        col = new DataColumn("Sort1", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Cat3", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Np", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Sort23", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);
        
        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["DateState"], this.Columns["Thickness"] }, true));
        
        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.DG_STATEWHS", tblName);
        dtm.ColumnMappings.Add("DATE_STATE", "DateState");
        dtm.ColumnMappings.Add("THICKNESS", "Thickness");
        dtm.ColumnMappings.Add("SORT1", "Sort1");
        dtm.ColumnMappings.Add("CAT3", "Cat3");
        dtm.ColumnMappings.Add("NP", "Np");
        dtm.ColumnMappings.Add("SORT23", "Sort23");

        adapter.TableMappings.Add(dtm);

        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT DATE_STATE, THICKNESS, SORT1, CAT3, NP, SORT23 FROM VIZ_PRN.DG_STATEWHS WHERE DATE_STATE = :PDATE_STATE ORDER BY 2",
          CommandType = CommandType.Text
        };

        var param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_STATE",
          SourceColumn = "DATE_STATE",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.SelectCommand.Parameters.Add(param);

        //Insert Command
        adapter.InsertCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "INSERT INTO VIZ_PRN.DG_STATEWHS(DATE_STATE, THICKNESS, SORT1, CAT3, NP, SORT23) " +
            "VALUES(TRUNC(:PDATE_STATE,'MM'), :PTHICKNESS, :PSORT1, :PCAT3, :PNP, :PSORT23)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_STATE",
          SourceColumn = "DATE_STATE",
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
          ParameterName = "PSORT1",
          SourceColumn = "SORT1",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PCAT3",
          SourceColumn = "CAT3",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PNP",
          SourceColumn = "NP",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PSORT23",
          SourceColumn = "SORT23",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        //Update Command
        adapter.UpdateCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "UPDATE VIZ_PRN.DG_STATEWHS SET DATE_STATE = TRUNC(:PDATE_STATE,'MM'), THICKNESS = :PTHICKNESS, SORT1 = :PSORT1, CAT3 = :PCAT3, NP = :PNP, SORT23 = :PSORT23 " +
            "WHERE (DATE_STATE = TRUNC(:Original_DATE_STATE,'MM')) AND (THICKNESS = :Original_THICKNESS)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "PDATE_STATE",
          SourceColumn = "DATE_STATE",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);
        

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
          ParameterName = "PSORT1",
          SourceColumn = "SORT1",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PCAT3",
          SourceColumn = "CAT3",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PNP",
          SourceColumn = "NP",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Double,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          ParameterName = "PSORT23",
          SourceColumn = "SORT23",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_DATE_STATE",
          SourceColumn = "DATE_STATE",
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
            "DELETE VIZ_PRN.DG_STATEWHS WHERE (DATE_STATE = TRUNC(:Original_DATE_STATE,'MM')) AND (THICKNESS = :Original_THICKNESS)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_DATE_STATE",
          SourceColumn = "DATE_STATE",
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

      public int LoadData(DateTime dateState)
      {
        var lstPrmValue = new List<Object> { new DateTime(dateState.Year, dateState.Month, 1) };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }
      public int SaveData()
      {
        return Odac.SaveChangedData(this, adapter);
      }

    }





  }

}
