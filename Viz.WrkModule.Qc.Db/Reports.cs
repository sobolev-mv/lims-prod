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

namespace Viz.WrkModule.Qc.Db
{
  public static class Reports
  {
    public const string GnrUstSource = "\\Xlt\\Viz.WrkModule.Qc-GnrUst.xltx";
    public const string GnrUstDest = "Viz.WrkModule.Qc-GnrUst.xlsx";

    public static void GnrUst(DtoRptGnrUstParamInput dtoRpt)
    {
      const string stmtSql = "VIZ_PRN.QMF_CALC_CORE.GenRptGnrUst4WorkShop";

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
        ["ADGIN_SQL"] = dtoRpt.AdgInSql
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

      Odac.ExecuteNonQuery(stmtSql, CommandType.StoredProcedure, false, lstPrm);

      var src = Etc.StartPath + GnrUstSource;
      var dst = Etc.GetFullPathRptFile(GnrUstDest);

      var workBook = new Workbook();
      workBook.LoadDocument(src, DocumentFormat.Xltx);
      var workSheet = workBook.Worksheets.ActiveWorksheet = workBook.Worksheets[0];
      
      const string sqlStmt1 = "select NAMEGROUP, RATIO_GROUP from VIZ_PRN.V_QMF_RPTGRNUST_WORKSHOP";
      var odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);
      if (odr != null)
      {
        int flds = odr.FieldCount;
        int row = 4;

        while (odr.Read())
        {
          workSheet[row, 0].Value = odr.GetString(0);
          workSheet[row, 1].Value = odr.GetDouble(1);

          row++;
        }
        odr.Close();
        odr.Dispose();
      }

      workBook.SaveDocument(dst, DocumentFormat.Xlsx);
      workBook.Dispose();
      Etc.OpenRptFolderOnTargetFile(GnrUstDest);
    }

  }
}
