using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using Devart.Data.Oracle;
using Smv.Data.Oracle;
using Smv.Utils;
using Viz.DbApp.Psi;

namespace Viz.WrkModule.RptOpr.Db
{
  public static class DbUtils
  {
    public static double GetTotalRk(int idKey)
    {
      const string stmtSql = "select RKTOTAL from VIZ_PRN.DG_MONTRIMUOSHIFT where ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();
      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      return Convert.ToDouble(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static double GetLngRk(int idKey)
    {
      const string stmtSql = "select RKLNG from VIZ_PRN.DG_MONTRIMUOSHIFT where ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();
      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      return Convert.ToDouble(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static void SaveRkShiftUo(int idKey, DateTime dateBegin, DateTime dateEnd)
    {
      const string stmtSql = "UPDATE VIZ_PRN.DG_MONTRIMUOSHIFT SET RKTOTAL = :PRKTOTAL, RKLNG = :PRKLNG, DTUPDT = SYSDATE WHERE ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();

      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PRKTOTAL",
        DbType = DbType.Decimal,
        OracleDbType = OracleDbType.Number,
        Direction = ParameterDirection.Input,
        Value = dateBegin
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PRKLNG",
        DbType = DbType.Decimal,
        OracleDbType = OracleDbType.Number,
        Direction = ParameterDirection.Input,
        Value = dateEnd
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.Text, false, lstPrm, true);
    }

    public static DateTime GetDateBeginQuart(int idKey)
    {
      const string stmtSql = "select DTBEGIN from VIZ_PRN.DG_QUARTILE where ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();
      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      return Convert.ToDateTime(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static DateTime GetDateEndQuart(int idKey)
    {
      const string stmtSql = "select DTEND from VIZ_PRN.DG_QUARTILE where ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();
      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      return Convert.ToDateTime(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static void SaveDateQuart(int idKey, DateTime dateBegin, DateTime dateEnd)
    {
      const string stmtSql = "UPDATE VIZ_PRN.DG_QUARTILE SET DTBEGIN = TRUNC(:PDTBEGIN, 'MM'), DTEND = TRUNC(:PDTEND, 'MM'), DTUPDT = SYSDATE WHERE ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();

      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PDTBEGIN",
        DbType = DbType.DateTime,
        OracleDbType = OracleDbType.Date,
        Direction = ParameterDirection.Input,
        Value = dateBegin
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PDTEND",
        DbType = DbType.DateTime,
        OracleDbType = OracleDbType.Date,
        Direction = ParameterDirection.Input,
        Value = dateEnd
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.Text, false, lstPrm, true);
    }

    public static void SaveRk(int idKey, decimal rkTotal, decimal rkLng)
    {
      const string stmtSql = "UPDATE VIZ_PRN.DG_MONTRIMUOSHIFT SET RKTOTAL = :PRKTOTAL, RKLNG = :PRKLNG, DTUPDT = SYSDATE WHERE ID = :PID";
      List<OracleParameter> lstPrm = new List<OracleParameter>();

      OracleParameter prm = new OracleParameter
      {
        ParameterName = "PRKTOTAL",
        DbType = DbType.Decimal,
        OracleDbType = OracleDbType.Number,
        Direction = ParameterDirection.Input,
        Value = rkTotal
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PRKLNG",
        DbType = DbType.Decimal,
        OracleDbType = OracleDbType.Number,
        Direction = ParameterDirection.Input,
        Value = rkLng
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        OracleDbType = OracleDbType.Integer,
        Direction = ParameterDirection.Input,
        Value = idKey
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.Text, false, lstPrm, true);

    }

    public static string GetBrig()
    {
      const string stmtSql = "select VIZ_PRN.VAR_RPT.GETSTR1 from dual";
   
      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, null));
    }
    //VIZ_PRN.QUARTILE_TRIM_UO.preSmRpt_Shift (:vData, 'Н');

    public static void RunQuartileTrimUo(DateTime dateQartile, string shift)
    {
      const string stmtSql = "VIZ_PRN.QUARTILE_TRIM_UO.preSmRpt_Shift";
      //const string stmtSql = "VIZ_PRN.QUARTILE_TRIM_UO.preSmRpt_Shift(:PDATA, :PSHIFT)";
      List<OracleParameter> lstPrm = new List<OracleParameter>();

      OracleParameter prm = new OracleParameter
      {
        //ParameterName = "vData",
        DbType = DbType.DateTime,
        OracleDbType = OracleDbType.Date,
        Direction = ParameterDirection.Input,
        Value = dateQartile
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        //ParameterName = "vSmena",
        DbType = DbType.String,
        OracleDbType = OracleDbType.VarChar,
        Direction = ParameterDirection.Input,
        Size = shift.Length,
        Value = shift
      };
      lstPrm.Add(prm);
      

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }
    




  }
}
