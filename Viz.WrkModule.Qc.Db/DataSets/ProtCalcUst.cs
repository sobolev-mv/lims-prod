using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ProtCalcUstDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ProtCalcUstDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("LocNum", typeof(string), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("GroupId", typeof(int), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("GroupName", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("ParamId", typeof(Int64), null, MappingType.Element)
        {
          AllowDBNull = false,
        };
        this.Columns.Add(col);

        col = new DataColumn("ParamName", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("IsExt", typeof(Int32), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("IsClcN", typeof(Int32), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("FactVal", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Agr", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("AnnealingLot", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["LocNum"], this.Columns["GroupId"], this.Columns["ParamId"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.V_QMF_STS_PROTCALC", tblName);
        dtm.ColumnMappings.Add("LOCNUM", "LocNum");
        dtm.ColumnMappings.Add("GROUP_ID", "GroupId");
        dtm.ColumnMappings.Add("GROUP_NAME", "GroupName");
        dtm.ColumnMappings.Add("PARAM_ID", "ParamId");
        dtm.ColumnMappings.Add("PARAM_NAME", "ParamName");
        dtm.ColumnMappings.Add("IS_EXT", "IsExt");
        dtm.ColumnMappings.Add("IS_CLCN", "IsClcN");
        dtm.ColumnMappings.Add("FACT_VAL", "FactVal");
        dtm.ColumnMappings.Add("AGR", "Agr");
        dtm.ColumnMappings.Add("ANNEALINGLOT", "AnnealingLot");

        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC",
          CommandType = CommandType.Text
        };

      }

      public int LoadData()
      {
        adapter.SelectCommand.Parameters.Clear();
        adapter.SelectCommand.CommandText = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC";
        return Odac.LoadDataTable(this, adapter, true, null);
      }

      public int LoadData(string agTyp)
      {
        adapter.SelectCommand.Parameters.Clear();
        adapter.SelectCommand.CommandText = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC WHERE AGTYP = :PAGTYP";

        var lstPrm = new List<Object>(){ agTyp };
        
        var prm = new OracleParameter
        {
          ParameterName = "PAGTYP",
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          Size = agTyp.Length,
          Value = agTyp
        };
        adapter.SelectCommand.Parameters.Add(prm);

        return Odac.LoadDataTable(this, adapter, true, lstPrm);
      }

      public int LoadData4WorkShop()
      {
        adapter.SelectCommand.Parameters.Clear();
        adapter.SelectCommand.CommandText = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC WHERE IS_PROCAGTYP = 'Y'";
        return Odac.LoadDataTable(this, adapter, true, null);
      }


    }


  }
}
