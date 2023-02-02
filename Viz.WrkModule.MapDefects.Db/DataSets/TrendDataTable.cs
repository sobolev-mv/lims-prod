using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.MapDefects.Db.DataSets
{
  public sealed partial class DsMapDef
  {
    public sealed class TrendDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public TrendDataTable() : base()
      {
        this.TableName = "Trend";
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Xarg", typeof(double), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("Value", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_Trend", new[] { this.Columns["Xarg"] }, true));
        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.CUTMATG_AVO", "Trend");
        dtm.ColumnMappings.Add("XARG", "Xarg");
        dtm.ColumnMappings.Add("VALUE", "Value");
        adapter.TableMappings.Add(dtm);

        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandType = CommandType.Text
        };
      }

      public int LoadTextureData(double coilLength, double coilWidth, double coilThick, string matLocId,
        string agrStrann)
      {
        adapter.SelectCommand.Parameters.Clear();

        adapter.SelectCommand.CommandText =
          "select(:PLENGTH - d.LENGTH) * :PWIDTH * :PTHICK * 7.65 / 1000000 as XARG,  d.VALUE " +
          "from VIZ.PAM_MV_DATA d " +
          "where D.MV_ID = VIZ_PRN.SPEP_UTL.GetTrendVal((select me_id from viz.mat where BEZEICHNUNG = :PLOC), :PAGR, 'AVO_TEXTURA', 'MVID') " +
          "order by 1 ";

        var prm = new OracleParameter
        {
          ParameterName = "PLENGTH",
          DbType = DbType.Double,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.Number,
          Value = coilLength
        };
        adapter.SelectCommand.Parameters.Add(prm);

        prm = new OracleParameter
        {
          ParameterName = "PWIDTH",
          DbType = DbType.Double,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.Number,
          Value = coilWidth
        };
        adapter.SelectCommand.Parameters.Add(prm);

        prm = new OracleParameter
        {
          ParameterName = "PTHICK",
          DbType = DbType.Double,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.Number,
          Value = coilThick
        };
        adapter.SelectCommand.Parameters.Add(prm);

        prm = new OracleParameter
        {
          ParameterName = "PLOC",
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          Size = matLocId.Length,
          Value = matLocId
        };
        adapter.SelectCommand.Parameters.Add(prm);

        prm = new OracleParameter
        {
          ParameterName = "PAGR",
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          Size = agrStrann.Length,
          Value = agrStrann
        };
        adapter.SelectCommand.Parameters.Add(prm);

        int rez = Odac.LoadDataTable(this, adapter, true, null);
        return rez;
      }
    }

  }
}