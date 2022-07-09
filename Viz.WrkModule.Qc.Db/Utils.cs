using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using Devart.Data.Oracle;
using Microsoft.Win32;
using Smv.Data.Oracle;
using Smv.Utils;

namespace Viz.WrkModule.Qc.Db
{
  public static class Utils
  {
    public static void ParamRpt()
    {
      var sfd = new SaveFileDialog
      {
        OverwritePrompt = false,
        AddExtension = true,
        DefaultExt = ".csv",
        Filter = "csv file (.csv)|*.csv"
      };

      if (sfd.ShowDialog().GetValueOrDefault() != true)
        return;

      if (File.Exists(sfd.FileName))
      {
        DxInfo.ShowDxBoxInfo("Файл", "Файл: " + sfd.FileName + " уже существует!", MessageBoxImage.Error);
        return;
      }

      OracleDataReader odr;

      odr = Odac.GetOracleReader("select * from viz_prn.v_qmf_rptparam", CommandType.Text, false, null, null);
      

      if (odr != null)
      {
        Etc.WriteToEndTxtFile(sfd.FileName, "", Encoding.GetEncoding("windows-1251"));
        Etc.WriteToEndTxtFile(sfd.FileName, "", Encoding.GetEncoding("windows-1251"));
        Etc.WriteToEndTxtFile(sfd.FileName, "ID Грп;Группа;ID Парам;Параметр;В Расчете;Толщина допуст.;Мин допуст. знач.;Макс допуст. знач.;Лог. допуст. знач;Толщина опт.;Мин опт. знач.;Макс опт. знач.;Лог. опт. знач", Encoding.GetEncoding("windows-1251"));

        while (odr.Read())
        {
          var strTmp = "";

          for (int i = 0; i < odr.FieldCount; i++)
            strTmp += odr.GetValue(i).ToString() + ";";

          Etc.WriteToEndTxtFile(sfd.FileName, strTmp, Encoding.GetEncoding("windows-1251"));
        }

      }

      odr?.Close();
      odr?.Dispose();

      DxInfo.ShowDxBoxInfo("Выгрузка", "Выгрузка завершена.", MessageBoxImage.Information);
    }

    public static double GetUst4LocNum(string typeSts, string locNum)
    {
      const string stmtSql = "select RATIO_LOCNUM from VIZ_PRN.V_QMF_STS where TYPE_CLC = :PTYPECLC and LOCNUM = :PLOCNUM";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PTYPECLC",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = typeSts.Length,
        Value = typeSts
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PLOCNUM",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = locNum.Length,
        Value = locNum
      };
      lstPrm.Add(prm);

      return Convert.ToDouble(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static void CalcParam4LocNum(string typeSts, string locNum)
    {
      const string stmtSql = "VIZ_PRN.QMF_CALC_CORE.CalcParam4LocNum";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "pi_LocNum",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = locNum.Length,
        Value = locNum
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "pi_TypeClc",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = typeSts.Length,
        Value = typeSts
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    public static void CalcParam4AgTypAgr(string typeSts, DateTime dateFrom, DateTime dateTo, string agTyp, string agr, int brig)
    {
      const string stmtSql = "VIZ_PRN.QMF_CALC_CORE.CalcParam4AgTypAgr";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "pi_TypeClc",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = typeSts.Length,
        Value = typeSts
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "pi_DateFrom",
        DbType = DbType.DateTime,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Date,
        Value = dateFrom
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "pi_DateTo",
        DbType = DbType.DateTime,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Date,
        Value = dateTo
      };
      lstPrm.Add(prm);

      if (!String.IsNullOrEmpty(agTyp))
      {
        prm = new OracleParameter
        {
          ParameterName = "pi_AgTyp",
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          Size = agTyp.Length,
          Value = agTyp
        };
        lstPrm.Add(prm);
      }

      if ((!String.IsNullOrEmpty(agTyp)) && (!String.IsNullOrEmpty(agr)))
      {
        prm = new OracleParameter
        {
          ParameterName = "pi_Agr",
          DbType = DbType.String,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.VarChar,
          Size = agr.Length,
          Value = agr
        };
        lstPrm.Add(prm);
      }

      if ((!String.IsNullOrEmpty(agTyp)) && (!String.IsNullOrEmpty(agr)) && (brig > 0))
      {
        prm = new OracleParameter
        {
          ParameterName = "pi_Brig",
          DbType = DbType.Int32,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.Integer,
          Value = brig
        };
        lstPrm.Add(prm);
      }

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    public static double GetUst4AgTypAgr(string typeSts)
    {
      const string stmtSql = "select RATIO_CLC from VIZ_PRN.V_QMF_STS where TYPE_CLC = :PTYPECLC";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PTYPECLC",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = typeSts.Length,
        Value = typeSts
      };
      lstPrm.Add(prm);

      return Convert.ToDouble(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static double GetSts999(string typeSts)
    {
      const string stmtSql = "select count(*) from VIZ_PRN.QMF_CLC where TYPE_CLC = :PTYPECLC";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PTYPECLC",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = typeSts.Length,
        Value = typeSts
      };
      lstPrm.Add(prm);

      return Convert.ToDouble(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static string GetNameTypeUst(int id)
    {
      const string stmtSql = "select NAME from VIZ_PRN.QMF_TYPE_UST where ID = :PID";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = id
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static string GetNameAgTyp(string agTyp)
    {
      const string stmtSql = "select NAME from VIZ_PRN.QMF_PARAM_GROUP where AGTYP = :PAGTYP";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PAGTYP",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = agTyp.Length,
        Value = agTyp
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static string GetNameAgregate(string agTyp, string agr)
    {
      const string stmtSql = "select NAME from VIZ_PRN.QMF_AGREGATE where AGTYP = :PAGTYP  AND AGR = :PAGR";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PAGTYP",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = agTyp.Length,
        Value = agTyp
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PAGR",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = agr.Length,
        Value = agr
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static string GetNameBrigade(int id)
    {
      const string stmtSql = "select NAME from VIZ_PRN.QMF_BRIGADE where ID = :PID";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = id
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static void CalcForecastQualityCoil(string locNum, int indicatorId)
    {
      Odac.ExecuteNonQuery("delete from VIZ_PRN.QMF_RESULT_LFCASTQ", CommandType.Text, false, null);

      const string stmtSql = "VIZ_PRN.QMF_CALC_FORECAST.CalcForecastQ4LocNum";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "pi_LocNum",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = locNum.Length,
        Value = locNum
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "pi_IndicateId",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = indicatorId
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    public static void CalcForecastQualityAnLot(string anLot, int indicatorId)
    {
      Odac.ExecuteNonQuery("delete from VIZ_PRN.QMF_RESULT_LFCASTQ", CommandType.Text, false, null);

      const string stmtSql = "VIZ_PRN.QMF_CALC_FORECAST.CalcForecastQ4AnLot";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "pi_AnLot",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = anLot.Length,
        Value = anLot
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "pi_IndicateId",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = indicatorId
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    public static void CalcForecastQualityListAnLot(string listAnLot, int indicatorId)
    {
      Odac.ExecuteNonQuery("delete from VIZ_PRN.QMF_RESULT_LFCASTQ", CommandType.Text, false, null);
      DbApp.Psi.DbVar.SetStringList(listAnLot, ",");

      const string stmtSql = "VIZ_PRN.QMF_CALC_FORECAST.CalcForecastQ4ListAnLot";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "pi_IndicateId",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = indicatorId
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    public static void CalcForecastQualityCoilsNzp(string erpLagerort, int indicatorId)
    {
      Odac.ExecuteNonQuery("delete from VIZ_PRN.QMF_RESULT_LFCASTQ", CommandType.Text, false, null);
      

      const string stmtSql = "VIZ_PRN.QMF_CALC_FORECAST.CalcForecastQ4Nzp";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "pi_ErpLagerort",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = erpLagerort.Length,
        Value = erpLagerort
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "pi_IndicateId",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = indicatorId
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    /*
    public static object GetResForecast()
    {
      const string stmtSql = "select CFCASTQ_ALL from VIZ_PRN.V_QMF_RESULT_LFCASTQ";
      return Odac.ExecuteScalar(stmtSql, CommandType.Text, false, null);
    }
    */

    public static string GetNameTypeForecast(int id)
    {
      const string stmtSql = "select NAME from VIZ_PRN.QMF_TYPE_FQ where ID = :PID";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = id
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }
    
    public static string GetNameTypeIndForecast(int id)
    {
      const string stmtSql = "select NAME from VIZ_PRN.V_QMF_TYPEIND_FQ where ID = :PID";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PID",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Integer,
        Value = id
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    public static string GetNameAgTypForecast(string agTypForecast)
    {
      const string stmtSql = "select NAME from VIZ_PRN.V_QMF_AGTYP_NZP where AGTYP = :PAGTYP";
      var lstPrm = new List<OracleParameter>();

      var prm = new OracleParameter
      {
        ParameterName = "PAGTYP",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Size = agTypForecast.Length,
        Value = agTypForecast
      };
      lstPrm.Add(prm);

      return Convert.ToString(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

  }
}
