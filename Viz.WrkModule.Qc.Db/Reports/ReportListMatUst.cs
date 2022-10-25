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
using Smv.SpreadSheet;

namespace Viz.WrkModule.Qc.Db.Reports
{
  public static class ReportListMatUst
  {

    private const string GnrUstSource = "\\Xlt\\Viz.WrkModule.Qc-ListMatUst.xltx";
    private const string GnrUstDest = "Viz.WrkModule.Qc-ListMatUst.xlsx";

    public static void CreateListMatUst(DtoRptListMatUstParamInput dtoRpt)
    {
      var workBook = DxExSpreadSheet.CreateAndLoadWorkBook(GnrUstSource);

      Odac.ExecuteNonQuery("delete from VIZ_PRN.QMF_CLC", CommandType.Text, false, null);
      DbApp.Psi.DbVar.SetStringList(dtoRpt.ListMatStringDelim, ",");

      var lstPrm = new List<OracleParameter>();
      var prm = new OracleParameter
      {
        ParameterName = "pi_UnitType",
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Value = dtoRpt.UnitType,
        Size = dtoRpt.UnitType.Length

      };
      lstPrm.Add(prm);
      Odac.ExecuteNonQuery("VIZ_PRN.QMF_CALC_CORE.GenRptListMatUst", CommandType.StoredProcedure, false, lstPrm);

      const string stmtSqlProt = "SELECT LOCNUM, GROUP_ID, GROUP_NAME, PARAM_ID, PARAM_NAME, IS_EXT, IS_CLCN, FACT_VAL, AGR, ANNEALINGLOT FROM VIZ_PRN.V_QMF_STS_PROTCALC";
      var odr = Odac.GetOracleReader(stmtSqlProt, CommandType.Text, false, null, null);
      Report.CreateProtocol(workBook, odr, 3);



      DxExSpreadSheet.SaveWorkBook(workBook, GnrUstDest);

    }
  }
}
