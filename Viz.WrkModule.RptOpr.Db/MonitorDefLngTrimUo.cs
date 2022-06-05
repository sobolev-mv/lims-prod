using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using Devart.Data.Oracle;
using Smv.Data.Oracle;
using Viz.DbApp.Psi;

namespace Viz.WrkModule.RptOpr.Db
{
  public sealed class MonitorDefLngTrimUoRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public decimal RkTotal { get; set; }
    public decimal RkPlan { get; set; }
    public DateTime DateMon { get; set; }
    public string Shift { get; set; }

    public MonitorDefLngTrimUoRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class MonitorDefLngTrimUo : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as MonitorDefLngTrimUoRptParam);
      dynamic wrkSheet = null;

      try{
        //Выбираем нужный лист 
        /*
        if (prm.TypeUm == "R")
          prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select();
        else
          prm.ExcelApp.ActiveWorkbook.WorkSheets[2].Select();
        */

        wrkSheet = prm.ExcelApp.ActiveSheet;
        this.RunRpt(prm, wrkSheet);
        //Здесь формирование самого отчета
        //wrkSheet.Range("A1").Value = prm.ExcelApp.Version;
        //wrkSheet.Range("A2").Value = "asdadsdgsfgsfsg";

        //Здесь визуализация Экселя
        //prm.ExcelApp.ScreenUpdating = true;
        //prm.ExcelApp.Visible = true;
        this.SaveResult(prm);
      }
      catch (Exception ex){
        Debug.Assert(prm != null, "prm != null");
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка Excel", ex.Message, MessageBoxImage.Stop)));
      }
      finally{
        prm.ExcelApp.Quit();

        //Здесь код очистки      
        if (wrkSheet != null)
          Marshal.ReleaseComObject(wrkSheet);

        //Marshal.ReleaseComObject(prm.WorkBook);
        Marshal.ReleaseComObject(prm.ExcelApp);
        wrkSheet = null;
        prm.WorkBook = null;
        prm.ExcelApp = null;
        GC.Collect();
      }
    }

    private void FillMonthHeader(dynamic currentWrkSheet)
    {
      const string sqlStmt = "SELECT DATA FROM VIZ_PRN.TMP_DAY ORDER BY NPP";
      OracleDataReader odr = null;
      odr = Odac.GetOracleReader(sqlStmt, CommandType.Text, false, null, null);

      if (odr != null){
        int col = 5;
        const int row = 5;

        while (odr.Read())
        {
          //MessageBox.Show(odr.GetValue(0).ToString());
          currentWrkSheet.Cells[row, col].Value = odr.GetValue(0);
          col++;
        }

        odr.Close();
        odr.Dispose();
      }
    }

    private Boolean RunRpt(MonitorDefLngTrimUoRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;

      DateTime? dtBegin = null;
      DateTime? dtEnd = null;
      string sqlStmt = null;
      
      try {
        //Корректируем конечную дату
        prm.DateEnd = prm.DateEnd.AddMonths(1).AddDays(-1);
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;
        


        DbUtils.RunQuartileTrimUo(prm.DateMon, prm.Shift);
        DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 0);

        dtBegin = DbVar.GetDateBeginEnd(true, false);
        dtEnd = DbVar.GetDateBeginEnd(false, false);


        CurrentWrkSheet.Cells[3, 3].Value = $"с {dtBegin:dd.MM.yyyy} по {dtEnd:dd.MM.yyyy}";
        CurrentWrkSheet.Cells[4, 8].Value = $"{prm.DateMon:dd.MM.yyyy}";
        CurrentWrkSheet.Cells[4, 9].Value = DbUtils.GetBrig();
        CurrentWrkSheet.Cells[4, 10].Value = prm.Shift;

        CurrentWrkSheet.Cells[9, 10].Value = prm.RkTotal;
        CurrentWrkSheet.Cells[10, 10].Value = prm.RkPlan;

        odr = Odac.GetOracleReader("select * from viz_prn.V_QUARTILE_SMRPT", CommandType.Text, false, null, null);
        if (odr != null)
        {

          int flds = odr.FieldCount;
          int row = 6;
          int col = 4;

          while (odr.Read())
          {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, col].Value = odr.GetValue(1);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }
        

        odr = Odac.GetOracleReader("select * from viz_prn.V_QUARTILE_SMRPT_CROSSCUT", CommandType.Text, false, null, null);
        if (odr != null){

          int flds = odr.FieldCount;
          int row = 15;

          while (odr.Read()){

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }
        
          odr.Close();
          odr.Dispose();
        }

        odr = Odac.GetOracleReader("select * from viz_prn.V_QUARTILE_SMRPT_EDGE", CommandType.Text, false, null, null);
        if (odr != null)
        {

          int flds = odr.FieldCount;
          int row = 15;

          while (odr.Read())
          {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 6].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }



        Result = true;
      }
      catch (Exception ex){
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка Excel", ex.Message, MessageBoxImage.Stop)));
        Result = false;
      }
      finally{
        if (odr != null){
          odr.Close();
          odr.Dispose();
        }
      }

      return Result;
    }


  }






}

