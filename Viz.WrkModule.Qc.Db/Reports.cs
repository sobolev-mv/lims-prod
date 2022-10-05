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
using DevExpress.Spreadsheet;
using Smv.Data.Oracle;
using Smv.Utils;
using Viz.WrkModule.Qc.Db.Dto;
using System.Security.Cryptography;
using DevExpress.XtraSpreadsheet.Model;
using Worksheet = DevExpress.Spreadsheet.Worksheet;

namespace Viz.WrkModule.Qc.Db
{
  public static class ReportsGeneralUst
  {
    public const string GnrUstSource = "\\Xlt\\Viz.WrkModule.Qc-GnrUst.xltx";
    public const string GnrUstDest = "Viz.WrkModule.Qc-GnrUst.xlsx";

    private static void CreateProtocol(Workbook workBook, OracleDataReader odrProtocol)
    {
      //Здесь будем грузить протокол
      var workSheet = workBook.Worksheets.ActiveWorksheet = workBook.Worksheets[0];

      if (odrProtocol != null)
      {
        int flds = odrProtocol.FieldCount;
        int row = 3;

        while (odrProtocol.Read())
        {
          var rangeFrom = workSheet.Range.FromLTRB(0, row, 9, row);
          var rangeTo = workSheet.Range.FromLTRB(0, row + 1, 9, row + 1);
          rangeTo.CopyFrom(rangeFrom, PasteSpecial.All);

          workSheet[row, 0].Value = odrProtocol.GetString(0);
          workSheet[row, 1].Value = odrProtocol.GetInt32(1);
          workSheet[row, 2].Value = odrProtocol.GetString(2);
          workSheet[row, 3].Value = odrProtocol.GetInt32(3);
          workSheet[row, 4].Value = odrProtocol.GetString(4);
          workSheet[row, 5].Value = odrProtocol.GetInt32(5);
          workSheet[row, 6].Value = odrProtocol.GetInt32(6);
          workSheet[row, 7].Value = odrProtocol.GetString(7);
          workSheet[row, 8].Value = odrProtocol.GetString(8);
          workSheet[row, 9].Value = odrProtocol.GetString(9);
          row++;
        }

        odrProtocol.Close();
        odrProtocol.Dispose();
      }

    }

    private static void CreateDtoObjOnDb(DtoRptGnrUstParamInput dtoRpt)
    {
      Odac.ExecuteNonQuery("delete from VIZ_PRN.QMF_CLC", CommandType.Text, false, null);

      Odac.DbConnection.Open();
      var objDtoRpt = new OracleObject("VIZ_PRN.T_DTORPTGNRUSTPARAMINPUT", Odac.DbConnection)
      {
        ["DATE_FROM"] = dtoRpt.DateFrom,
        ["DATE_TO"] = dtoRpt.DateTo,
        ["FINAL_THICKNESS_SQL"] = dtoRpt.FinalThicknessSql,
        ["IS_KESIAVG"] = dtoRpt.IsKesiAvg ? 1 : 0,
        ["KESIAVG_MIN"] = dtoRpt.KesiAvgMin,
        ["KESIAVG_MAX"] = dtoRpt.KesiAvgMax,
        ["IS_KESIWORST"] = dtoRpt.IsKesiWorst ? 1 : 0,
        ["KESIWORST_MIN"] = dtoRpt.KesiWorstMin,
        ["KESIWORST_MAX"] = dtoRpt.KesiWorstMax,
        ["IS_P1750"] = dtoRpt.IsP1750 ? 1 : 0,
        ["P1750_MIN"] = dtoRpt.P1750Min,
        ["P1750_MAX"] = dtoRpt.P1750Max,
        ["IS_DEFECT_TOLOWCAT"] = dtoRpt.IsDefectTolowCat ? 1 : 0,
        ["DEFECT_TOLOWCAT"] = dtoRpt.DefectTolowCat,
        ["IS_DEFECT_TO2SORT"] = dtoRpt.IsDefectTo2Sort ? 1 : 0,
        ["DEFECT_TO2SORT"] = dtoRpt.DefectTo2Sort,
        ["IS_ADGIN"] = dtoRpt.IsAdgIn ? 1 : 0,
        ["ADGIN_SQL"] = dtoRpt.AdgInSql,
        ["AGTYP"] = dtoRpt.AgTyp,
        ["AGR"] = dtoRpt.Agr
      };
      Odac.DbConnection.Close();

      var lstPrm = new List<OracleParameter>();
      var prm = new OracleParameter
      {
        ParameterName = "pi_RptParam",
        DbType = DbType.Object,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.Object,
        Value = objDtoRpt
      };
      lstPrm.Add(prm);

      const string stmtSql = "VIZ_PRN.QMF_CALC_CORE.GenRptGnrUst4WorkShop";
      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);
    }

    private static Workbook CreateAndLoadWorkBook()
    {
      var src = Etc.StartPath + GnrUstSource;
      var workBook = new Workbook();
      workBook.LoadDocument(src, DocumentFormat.Xltx);

      return workBook;
    }

    private static void SaveWorkBook(Workbook workBook)
    {
      var dst = Etc.GetFullPathRptFile(GnrUstDest);

      workBook.SaveDocument(dst, DocumentFormat.Xlsx);
      workBook.Dispose();
      Etc.OpenRptFolderOnTargetFile(GnrUstDest);
    }

    public static void CreateCreteria4AllReports(DtoRptGnrUstParamInput dtoRpt, Worksheet workSheet)
    {
      workSheet[4, 1].Value = dtoRpt.DateFrom;
      workSheet[4, 2].Value = dtoRpt.DateTo;
      workSheet[5, 1].Value = dtoRpt.FinalThicknessSql;
      workSheet[6, 1].Value = dtoRpt.IsKesiAvg ? dtoRpt.KesiAvgMin : (int?)null;
      workSheet[6, 2].Value = dtoRpt.IsKesiAvg ? dtoRpt.KesiAvgMax : (int?)null;
      workSheet[7, 1].Value = dtoRpt.IsKesiWorst ? dtoRpt.KesiWorstMin : (int?)null;
      workSheet[7, 2].Value = dtoRpt.IsKesiWorst ? dtoRpt.KesiWorstMax : (int?)null;
      workSheet[8, 1].Value = dtoRpt.IsP1750 ? dtoRpt.P1750Min : (double?)null;
      workSheet[8, 2].Value = dtoRpt.IsP1750 ? dtoRpt.P1750Max : (double?)null;
      workSheet[9, 1].Value = dtoRpt.IsDefectTolowCat ? dtoRpt.DefectTolowCat : string.Empty;
      workSheet[10, 1].Value = dtoRpt.IsDefectTo2Sort ? dtoRpt.DefectTo2Sort : string.Empty;
      workSheet[11, 1].Value = dtoRpt.IsAdgIn ? dtoRpt.AdgInSql : string.Empty;
    }

    public static void CreateGeneralUstWs(DtoRptGnrUstParamInput dtoRpt)
    {
      CreateDtoObjOnDb(dtoRpt);

      var workBook = CreateAndLoadWorkBook();
      workBook.Worksheets[2].Visible = false;
      
      const string stmtSqlProt = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC";
      var odr = Odac.GetOracleReader(stmtSqlProt, CommandType.Text, false, null, null);
      CreateProtocol(workBook, odr);

      var workSheet = workBook.Worksheets.ActiveWorksheet = workBook.Worksheets[1];

      CreateCreteria4AllReports(dtoRpt, workSheet);

      const string sqlStmtDff = "select NAMEGROUP, RATIO_GROUP from VIZ_PRN.V_QMF_RPTGRNDFF_WS";
      odr = Odac.GetOracleReader(sqlStmtDff, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 16;

        while (odr.Read())
        {
          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 2].Value = odr.GetDouble(1);
          row++;
        }

        odr.Close();
        odr.Dispose();
      }

      const string sqlStmtUst = "select NAMEGROUP, RATIO_GROUP from VIZ_PRN.V_QMF_RPTGRNUST_WS";
      odr = Odac.GetOracleReader(sqlStmtUst, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 16;

        while (odr.Read())
        {
          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 1].Value = odr.GetDouble(1);
          row++;
        }

        odr.Close();
        odr.Dispose();
      }

      var valUstAll = Convert.ToDouble(Odac.ExecuteScalar("select RATIO_ALL from VIZ_PRN.V_QMF_RPTGRNUST_WS", CommandType.Text, false, null));
      var valDffAll = Convert.ToDouble(Odac.ExecuteScalar("select RATIO_ALL from VIZ_PRN.V_QMF_RPTGRNDFF_WS", CommandType.Text, false, null));
      var charTitle = $"ЦХП, УСТ -  {valUstAll:n2}; КНД - {valDffAll:n2}";
      workSheet.Charts[0].Title.SetValue(charTitle);

      SaveWorkBook(workBook);
    }

    public static void CreateGeneralUstAgTyp(DtoRptGnrUstParamInput dtoRpt)
    {
      CreateDtoObjOnDb(dtoRpt);

      var workBook = CreateAndLoadWorkBook();
      workBook.Worksheets[1].Visible = workBook.Worksheets[3].Visible = false;

      const string stmtSqlProt = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC";
      var odr = Odac.GetOracleReader(stmtSqlProt, CommandType.Text, false, null, null);
      CreateProtocol(workBook, odr);
      
      var workSheet = workBook.Worksheets.ActiveWorksheet = workBook.Worksheets[2];
      CreateCreteria4AllReports(dtoRpt, workSheet);
      workSheet[12, 1].Value = Utils.GetNameAgTyp(dtoRpt.AgTyp);

      const string sqlStmtDff = "select AGR, RATIO_AGR from VIZ_PRN.V_QMF_RPTGRNDFF_AGTYP";
      DbApp.Psi.DbVar.SetNum(Db.Utils.GetGroupIdAgTyp(dtoRpt.AgTyp));
      odr = Odac.GetOracleReader(sqlStmtDff, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 17;

        while (odr.Read())
        {
          var rangeFrom = workSheet.Range.FromLTRB(0, row, 2, row);
          var rangeTo = workSheet.Range.FromLTRB(0, row + 1, 2, row + 1);
          rangeTo.CopyFrom(rangeFrom, PasteSpecial.All);

          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 2].Value = odr.GetDouble(1);
          row++;
        }

        odr.Close();
        odr.Dispose();
      }

      const string sqlStmtUst = "select AGR, RATIO_AGR from VIZ_PRN.V_QMF_RPTGRNUST_AGTYP";
      odr = Odac.GetOracleReader(sqlStmtUst, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 17;

        while (odr.Read())
        {
          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 1].Value = odr.GetDouble(1);
          row++;
        }

        odr.Close();
        odr.Dispose();
      }

      var valUstAll = Convert.ToDouble(Odac.ExecuteScalar("select RATIO_ALL from VIZ_PRN.V_QMF_RPTGRNUST_AGTYP", CommandType.Text, false, null));
      var valDffAll = Convert.ToDouble(Odac.ExecuteScalar("select RATIO_ALL from VIZ_PRN.V_QMF_RPTGRNDFF_AGTYP", CommandType.Text, false, null));
      var charTitle = $"{Utils.GetNameAgTyp(dtoRpt.AgTyp)}, УСТ -  {valUstAll:n2}; КНД - {valDffAll:n2}";
      workSheet.Charts[0].Title.SetValue(charTitle);
      
      SaveWorkBook(workBook);
    }

    public static void CreateGeneralUstAgregate(DtoRptGnrUstParamInput dtoRpt)
    {
      CreateDtoObjOnDb(dtoRpt);
      //Odac.ExecuteNonQuery("INSERT INTO VIZ_PRN.QMF_CLC_DEBUG SELECT * FROM VIZ_PRN.QMF_CLC", CommandType.Text, false, null);

      var workBook = CreateAndLoadWorkBook();
      workBook.Worksheets[1].Visible = workBook.Worksheets[2].Visible = false;

      const string stmtSqlProt = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC";
      var odr = Odac.GetOracleReader(stmtSqlProt, CommandType.Text, false, null, null);
      CreateProtocol(workBook, odr);
      
      var workSheet = workBook.Worksheets.ActiveWorksheet = workBook.Worksheets[3];
      CreateCreteria4AllReports(dtoRpt, workSheet);
      workSheet[12, 1].Value = dtoRpt.Agr;

      const string sqlStmtDff = "select BRIG, RATIO_BRIG from VIZ_PRN.V_QMF_RPTGRNDFF_AGR";
      odr = Odac.GetOracleReader(sqlStmtDff, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 17;

        while (odr.Read())
        {
          var rangeFrom = workSheet.Range.FromLTRB(0, row, 2, row);
          var rangeTo = workSheet.Range.FromLTRB(0, row + 1, 2, row + 1);
          rangeTo.CopyFrom(rangeFrom, PasteSpecial.All);

          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 2].Value = odr.GetDouble(1);
          row++;
        }

        odr.Close();
        odr.Dispose();
      }

      const string sqlStmtUst = "select BRIG, RATIO_BRIG from VIZ_PRN.V_QMF_RPTGRNUST_AGR";
      odr = Odac.GetOracleReader(sqlStmtUst, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 17;

        while (odr.Read())
        {
          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 1].Value = odr.GetDouble(1);
          row++;
        }

        odr.Close();
        odr.Dispose();
      }
      
      SaveWorkBook(workBook);
    }


  }
}