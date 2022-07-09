using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ParamGroupDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ParamGroupDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Id", typeof(int), null, MappingType.Element)
        {
          AllowDBNull = false,
          AutoIncrement = true,
          AutoIncrementStep = -1
        };
        col.Caption = "ID";
        this.Columns.Add(col);

        col = new DataColumn("Name", typeof(string), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["Id"] }, true));
        this.Columns["Id"].Unique = true;

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_PARAM_GROUP", tblName);
        dtm.ColumnMappings.Add("ID", "Id");
        dtm.ColumnMappings.Add("NAME", "Name");

        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT ID, NAME FROM VIZ_PRN.QMF_PARAM_GROUP ORDER BY ID",
          CommandType = CommandType.Text
        };

        //Insert Command
        adapter.InsertCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "INSERT INTO VIZ_PRN.QMF_PARAM_GROUP(ID, NAME) " +
            "VALUES(:PID, :PNAME)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        var param = new OracleParameter
        {
          DbType = DbType.Int32,
          OracleDbType = OracleDbType.Integer,
          Direction = ParameterDirection.Input,
          ParameterName = "PID",
          SourceColumn = "ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "PNAME",
          SourceColumn = "NAME",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.InsertCommand.Parameters.Add(param);

        //Update Command
        adapter.UpdateCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "UPDATE VIZ_PRN.QMF_PARAM_GROUP SET NAME = :PNAME " +
            "WHERE (ID = :Original_ID)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.String,
          OracleDbType = OracleDbType.VarChar,
          Direction = ParameterDirection.Input,
          ParameterName = "PNAME",
          SourceColumn = "NAME",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.UpdateCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int32,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_ID",
          SourceColumn = "ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.UpdateCommand.Parameters.Add(param);

        //DeleteCommand
        adapter.DeleteCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "DELETE VIZ_PRN.QMF_PARAM_GROUP WHERE (ID = :Original_ID)",
          CommandType = CommandType.Text,
          PassParametersByName = true,
          UpdatedRowSource = UpdateRowSource.None
        };

        param = new OracleParameter
        {
          DbType = DbType.Int32,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "Original_ID",
          SourceColumn = "ID",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Original
        };
        adapter.DeleteCommand.Parameters.Add(param);
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