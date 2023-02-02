using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.MapDefects.Db.DataSets
{
  public sealed partial class DsMapDef
  {
    public sealed class LstDefZonesDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public LstDefZonesDataTable() : base()
      {
        this.TableName = "LstDefZones";
        adapter = new OracleDataAdapter();

        var col = new DataColumn("ZoneFrom", typeof(decimal), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("ZoneTo", typeof(decimal), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("Cnt", typeof(int), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_LstDefZones",
          new[] { this.Columns["ZoneFrom"], this.Columns["ZoneTo"] }, true));
        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.T999", "LstDefZones");
        dtm.ColumnMappings.Add("ZONEFROM", "ZoneFrom");
        dtm.ColumnMappings.Add("ZONETO", "ZoneTo");
        dtm.ColumnMappings.Add("CNT", "Cnt");
        adapter.TableMappings.Add(dtm);
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "SELECT ZONEFROM, ZONETO, COUNT(*) CNT FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :ZDN) AND DEFECT_SIDE IN (:DS1, :DS2) GROUP BY ZONEFROM, ZONETO ORDER BY 1,2",
          CommandType = CommandType.Text
        };

        var param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "ZDN",
          SourceColumn = "ZDN",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.SelectCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int32,
          OracleDbType = OracleDbType.Integer,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "DS1",
          SourceColumn = "DEFECT_SIDE",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.SelectCommand.Parameters.Add(param);

        param = new OracleParameter
        {
          DbType = DbType.Int32,
          OracleDbType = OracleDbType.Integer,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "DS2",
          SourceColumn = "DEFECT_SIDE",
          SourceColumnNullMapping = false,
          SourceVersion = DataRowVersion.Current
        };
        adapter.SelectCommand.Parameters.Add(param);
      }

      public int LoadData(Int64 Zdn, int Side1, int Side2)
      {
        var lstPrmValue = new List<Object> { Zdn, Side1, Side2 };
        int rez = Odac.LoadDataTable(this, adapter, true, lstPrmValue);

        /*
          var lstPrm = new List<OracleParameter>();
          var prm = new OracleParameter()
          {
            ParameterName = "PZDN",
            DbType = DbType.Int64,
            OracleDbType = OracleDbType.Number,
            Direction = ParameterDirection.Input,
            Value = Zdn
          };
          lstPrm.Add(prm);
          Odac.ExecuteNonQuery("DELETE FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :PZDN)", CommandType.Text, false, lstPrm);
          */
        return rez;
      }
    }
  }
}