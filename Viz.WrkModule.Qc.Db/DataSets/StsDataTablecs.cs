using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class StsDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public StsDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("TypeClc", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = true,
        };
        this.Columns.Add(col);

        col = new DataColumn("LocNum", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = true,
        };
        this.Columns.Add(col);

        col = new DataColumn("NameGroup", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = true,
        };
        this.Columns.Add(col);

        col = new DataColumn("RatioSts", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["TypeClc"], this.Columns["LocNum"], this.Columns["NameGroup"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.V_QMF_STS", tblName);
        dtm.ColumnMappings.Add("TYPE_CLC", "TypeClc");
        dtm.ColumnMappings.Add("LOCNUM", "LocNum");
        dtm.ColumnMappings.Add("NAMEGROUP", "NameGroup");
        dtm.ColumnMappings.Add("RATIO_STS", "RatioSts");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT TYPE_CLC, LOCNUM, NAMEGROUP, RATIO_STS FROM VIZ_PRN.V_QMF_STS WHERE TYPE_CLC = :PTYPE_CLC AND LOCNUM = :PLOCNUM",
          CommandType = CommandType.Text
        };

        var prm = new OracleParameter
        {
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          ParameterName = "PTYPE_CLC"
        };
        adapter.SelectCommand.Parameters.Add(prm);

        prm = new OracleParameter
        {
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          ParameterName = "PLOCNUM"
        };
        adapter.SelectCommand.Parameters.Add(prm);
      }
      
      public int LoadData(string typeClc, string locNUm)
      {
        var lstPrmValue = new List<Object> { typeClc, locNUm };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }
      

    }


  }
}

