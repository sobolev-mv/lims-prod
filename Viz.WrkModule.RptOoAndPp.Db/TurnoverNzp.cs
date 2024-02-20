using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using Devart.Data.Oracle;
using Smv.Data.Oracle;
using Viz.DbApp.Psi;
using System.Windows.Input;
using System.Xml.Linq;

namespace Viz.WrkModule.RptOoAndPp.Db
{
  public sealed class TurnoverNzpRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public string Whs { get; set; }
    public TurnoverNzpRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    { }
  }

  public sealed class TurnoverNzp : Smv.Xls.XlsRpt
  {
    private readonly List<string> lstWhs = new List<string>() { "3134", "3135", "3136", "3137", "3138", "3139", "313H", "313A", "313B(АВО)", "313B(УО)" };

    private double GetTurnoverRatio(string whsTurn, DateTime? whsDate)
    {
      const string stmtSql = "select TURN_VAL from VIZ_PRN.DG_TURNOVER_NZP " +
                             "where WHS = :PWHS and :PTURNDATE between DATE_BEGIN + 1/24*8 and DATE_END + 1/24*7 + 1/24/60*59 + 1/24/60/60*59"; 

      var lstPrm = new List<OracleParameter>();
      var prm = new OracleParameter
      {
        ParameterName = "PWHS",
        DbType = DbType.String,
        OracleDbType = OracleDbType.VarChar,
        Direction = ParameterDirection.Input,
        Value = whsTurn
      };
      lstPrm.Add(prm);

      prm = new OracleParameter
      {
        ParameterName = "PTURNDATE",
        DbType = DbType.DateTime,
        OracleDbType = OracleDbType.Date,
        Direction = ParameterDirection.Input,
        Value = whsDate
      };
      lstPrm.Add(prm);

      return Convert.ToDouble(Odac.ExecuteScalar(stmtSql, CommandType.Text, false, lstPrm));
    }

    private void SetActiveWorkSheet(TurnoverNzpRptParam prm)
    {
      for (var i = 0; i < lstWhs.Count; i++)
      {
        if (lstWhs[i] != prm.Whs)
          prm.ExcelApp.ActiveWorkbook.WorkSheets[i + 1].Visible = false;
        else
          prm.ExcelApp.ActiveWorkbook.WorkSheets[i + 1].Select();
          //prm.ExcelApp.ActiveSheet.Visible = false;
      }
    }

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as TurnoverNzpRptParam);
      dynamic wrkSheet = null;

      try
      {
        //Выбираем нужный лист 
        prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select(); //выбираем лист
        wrkSheet = prm.ExcelApp.ActiveSheet;
        this.RunRpt(prm, wrkSheet);
        this.SaveResult(prm);
      }
      catch (Exception ex)
      {
        Debug.Assert(prm != null, "prm != null");
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка Excel", ex.Message, MessageBoxImage.Stop)));
      }
      finally
      {
        prm.WorkBook.Close();
        prm.ExcelApp.Quit();

        //Здесь код очистки      
        if (wrkSheet != null)
          Marshal.ReleaseComObject(wrkSheet);

        if (prm.ExcelApp != null)
          Marshal.ReleaseComObject(prm.ExcelApp);

        wrkSheet = null;
        prm.WorkBook = null;
        prm.ExcelApp = null;
        GC.Collect();
      }
    }

    private Boolean RunRpt(TurnoverNzpRptParam prm, dynamic currentWrkSheet)
    {
      DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 1);
      var dtBegin = DbVar.GetDateBeginEnd(true, true);

      var turnRatio = GetTurnoverRatio(prm.Whs, dtBegin);
      SetActiveWorkSheet(prm);

      const int firstExcelColumn = 1;
      int lastExcelColumn = 1;
      string viewName = null;

      currentWrkSheet = prm.ExcelApp.ActiveSheet;
      switch(prm.Whs)
      {
        case "3134":
          lastExcelColumn = 8;
          viewName = "VIZ_PRN.V_TRNVR_NZP3134";
          DbVar.SetString("3134");
          DbVar.SetNum(0,0);
          
          break;
        case "3135":
          lastExcelColumn = 10;
          viewName = "VIZ_PRN.V_TRNVR_NZP3135";
          DbVar.SetString("3135");
          DbVar.SetNum(0,0);
          break;
        case "3136":
          lastExcelColumn = 10;
          viewName = "VIZ_PRN.V_TRNVR_NZP3136";
          DbVar.SetString("3136");
          DbVar.SetNum(0,0);
          break;
        case "3137":
          lastExcelColumn = 11;
          viewName = "VIZ_PRN.V_TRNVR_NZP3137";
          DbVar.SetString("3137");
          DbVar.SetNum(0,0);
          break;
        case "3138":
          lastExcelColumn = 12;
          viewName = "VIZ_PRN.V_TRNVR_NZP3138";
          DbVar.SetString("3138");
          DbVar.SetNum(0,0);
          break;
        case "3139":
          lastExcelColumn = 14;
          viewName = "VIZ_PRN.V_TRNVR_NZP3139";
          DbVar.SetString("3139");
          DbVar.SetNum(0,0);
          break;
        case "313H":
          lastExcelColumn = 11;
          viewName = "VIZ_PRN.V_TRNVR_NZP313H";
          DbVar.SetString("313H");
          DbVar.SetNum(0,0);
          break;
        case "313A":
          lastExcelColumn = 15;
          viewName = "VIZ_PRN.V_TRNVR_NZP313A";
          DbVar.SetString("313A");
          DbVar.SetNum(0,0);
          break;
        case "313B(АВО)":
          lastExcelColumn = 16;
          viewName = "VIZ_PRN.V_TRNVR_NZP313BCLS";
          DbVar.SetString("313B");
          DbVar.SetNum(1,0);
          break;
        case "313B(УО)":
          lastExcelColumn = 16;
          viewName = "VIZ_PRN.V_TRNVR_NZP313BCLS";
          DbVar.SetString("313B");
          DbVar.SetNum(0,1);
          break;
      }
      
      var result = false;
      OracleDataReader odr = null;

      try
      {
        currentWrkSheet.Cells[1, 3].Value = $"{dtBegin:dd.MM.yyyy HH:mm:ss}";

        if (prm.Whs != "313H")
          currentWrkSheet.Cells[3, 4].Value = turnRatio;

        var stmtSql = "SELECT * FROM " + viewName;
        odr = Odac.GetOracleReader(stmtSql, CommandType.Text, false, null, null);

        if (odr != null)
        {
          var row = 6;
          while (odr.Read())
          {
            currentWrkSheet.Range[currentWrkSheet.Cells[row, firstExcelColumn], currentWrkSheet.Cells[row, lastExcelColumn]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[row + 1, firstExcelColumn], currentWrkSheet.Cells[row + 1, lastExcelColumn]]);

            for (int i = 0; i < odr.FieldCount; i++)
              currentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }
        }

        result = true;
      }
      catch (Exception e)
      {
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка", e.Message, MessageBoxImage.Stop)));
      }
      finally
      {
        if (odr != null)
        {
          odr.Close();
          odr.Dispose();
        }
      }

      return result;
    }


  }



}
