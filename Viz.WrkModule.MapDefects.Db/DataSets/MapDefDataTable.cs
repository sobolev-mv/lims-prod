using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.MapDefects.Db.DataSets
{
  public sealed partial class DsMapDef
  {
    public sealed class MapDefDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public MapDefDataTable() : base()
      {
        this.TableName = "MapDef";
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Rid", typeof(Int64), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("Zdn", typeof(Int64), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("CoilNo", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("FehlerTyp", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("WeightFrom", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("WeightTo", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("XposvOn", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("XposbIs", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("YposvOn", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("YposbIs", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("DefectSide", typeof(Int32), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("LfdNr", typeof(Int32), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Cat", typeof(String), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("AusPraeg", typeof(String), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("ZoneFrom", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("ZoneTo", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Ylen", typeof(decimal), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_MapDef", new[] { this.Columns["Rid"] }, true));
        this.Columns["Rid"].Unique = true;

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.OTK_DEF", "MapDef");
        dtm.ColumnMappings.Add("RID", "Rid");
        dtm.ColumnMappings.Add("ZDN", "Zdn");
        dtm.ColumnMappings.Add("COILNO", "CoilNo");
        dtm.ColumnMappings.Add("FEHLERTYP", "FehlerTyp");
        dtm.ColumnMappings.Add("WEIGHTFROM", "WeightFrom");
        dtm.ColumnMappings.Add("WEIGHTTO", "WeightTo");
        dtm.ColumnMappings.Add("XPOSVON", "XposvOn");
        dtm.ColumnMappings.Add("XPOSBIS", "XposbIs");
        dtm.ColumnMappings.Add("YPOSVON", "YposvOn");
        dtm.ColumnMappings.Add("YPOSBIS", "YposbIs");
        dtm.ColumnMappings.Add("DEFECT_SIDE", "DefectSide");
        dtm.ColumnMappings.Add("LFD_NR", "LfdNr");
        dtm.ColumnMappings.Add("CAT", "Cat");
        dtm.ColumnMappings.Add("AUSPRAEGUNG", "AusPraeg");
        dtm.ColumnMappings.Add("ZONEFROM", "ZoneFrom");
        dtm.ColumnMappings.Add("ZONETO", "ZoneTo");
        dtm.ColumnMappings.Add("YLEN", "Ylen");
        adapter.TableMappings.Add(dtm);
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          /*
                                    CommandText = "SELECT ROWNUM RID, ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
                                                  "FROM ( " +
                                                  "SELECT ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
                                                  "FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :PZDN) AND DEFECT_SIDE IN (:DS1, :DS2) " +
                                                  "UNION ALL " +
                                                  "SELECT ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, 3 AS DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
                                                  "FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :PZDN) AND (FEHLERTYP = 'WELDSEAM') " +
                                                  ") " +
                                                  "ORDER BY ZONEFROM, ZONETO, TO_NUMBER(AUSPRAEGUNG) DESC, YLEN DESC NULLS LAST",
                                    */
          CommandType = CommandType.Text
        };
      }

      public int LoadData(long Zdn, int Side1, int Side2)
      {
        adapter.SelectCommand.Parameters.Clear();

        adapter.SelectCommand.CommandText =
          "SELECT ROWNUM RID, ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
          "FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :PZDN) AND DEFECT_SIDE IN (:DS1, :DS2) " +
          "ORDER BY ZONEFROM, ZONETO, TO_NUMBER(AUSPRAEGUNG) DESC, YLEN DESC NULLS LAST";

        var param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "PZDN",
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

        var lstPrmValue = new List<Object> { Zdn, Side1, Side2 };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }

      public int LoadDataPack(long Zdn, int Side1, int Side2)
      {
        adapter.SelectCommand.Parameters.Clear();

        adapter.SelectCommand.CommandText =
          "SELECT ROWNUM RID, ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
          "FROM ( " +
          "SELECT ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
          "FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :PZDN) AND DEFECT_SIDE IN (:DS1, :DS2) " +
          "UNION ALL " +
          "SELECT ZDN, COILNO, FEHLERTYP, WEIGHTFROM, WEIGHTTO, XPOSVON, XPOSBIS, YPOSVON, YPOSBIS, 3 AS DEFECT_SIDE, LFD_NR, CAT, AUSPRAEGUNG, ZONEFROM, ZONETO, YLEN " +
          "FROM VIZ_PRN.OTK_DEF WHERE (ZDN = :PZDN) AND (FEHLERTYP = 'WELDSEAM') " +
          ") " +
          "ORDER BY ZONEFROM, ZONETO, TO_NUMBER(AUSPRAEGUNG) DESC, YLEN DESC NULLS LAST";

        var param = new OracleParameter
        {
          DbType = DbType.Int64,
          OracleDbType = OracleDbType.Number,
          Direction = ParameterDirection.Input,
          IsNullable = false,
          ParameterName = "PZDN",
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

        var lstPrmValue = new List<Object> { Zdn, Side1, Side2 };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }
    }
  }
}