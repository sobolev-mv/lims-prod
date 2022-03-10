using System;
using System.Collections.Generic;
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

namespace Viz.WrkModule.RptMagLab.Db
{
  public sealed class QcHrcRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public string TechStepInspLot { get; set; }
    //public string TechStepPrjJornal { get; set; }

    public QcHrcRptParam(string sourceXlsFile, string destXlsFile)
           : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class QcHrc : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      QcHrcRptParam prm = (e.Argument as QcHrcRptParam);
      dynamic wrkSheet = null;

      try{
        //Выбираем нужный лист 
        prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select(); //выбираем лист
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
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка Excel", ex.Message, MessageBoxImage.Stop)));
      }
      finally{
        prm.ExcelApp.Quit();

        //Здесь код очистки      
        if (wrkSheet != null)
          Marshal.ReleaseComObject(wrkSheet);
        
        Marshal.ReleaseComObject(prm.ExcelApp);
        wrkSheet = null;
        prm.WorkBook = null;
        prm.ExcelApp = null;
        GC.Collect();
      }
    }

    private Boolean RunRpt(QcHrcRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;
      DateTime? dtBegin = null;
      DateTime? dtEnd = null;

      try{
        
        DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 1);
        dtBegin = DbVar.GetDateBeginEnd(true, true);
        dtEnd = DbVar.GetDateBeginEnd(false, true);


        CurrentWrkSheet.Cells[1, 2].Value = $"за период с {dtBegin:dd.MM.yyyy HH:mm:ss} по {dtEnd:dd.MM.yyyy HH:mm:ss}"; 

        //Заполняем Лист1
        const string sqlStmt1 = "SELECT * FROM VIZ_PRN.V_QCHRC_L1T1";
        odr = Odac.GetOracleReader(sqlStmt1, System.Data.CommandType.Text, false, null, null);

        if (odr != null) {

          int flds = odr.FieldCount;
          int row = 7;

          while (odr.Read()) {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }

        const string sqlStmt2 = "SELECT * FROM VIZ_PRN.V_QCHRC_L1T2";
        odr = Odac.GetOracleReader(sqlStmt2, System.Data.CommandType.Text, false, null, null);

        if (odr != null) {

          int flds = odr.FieldCount;
          int row = 14;

          while (odr.Read()) {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }

        const string sqlStmt3 = "SELECT * FROM VIZ_PRN.V_QCHRC_L1T3";
        odr = Odac.GetOracleReader(sqlStmt3, System.Data.CommandType.Text, false, null, null);

        if (odr != null) {

          int flds = odr.FieldCount;
          int row = 21;

          while (odr.Read()) {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }

        //Заполняем Лист2
        prm.ExcelApp.ActiveWorkbook.WorkSheets[2].Select();
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;

        CurrentWrkSheet.Cells[1, 7].Value = $"за период с {dtBegin:dd.MM.yyyy HH:mm:ss} по {dtEnd:dd.MM.yyyy HH:mm:ss}";

        const string sqlStmt4 = "SELECT * FROM VIZ_PRN.V_QCHRC_L2T1";
        odr = Odac.GetOracleReader(sqlStmt4, System.Data.CommandType.Text, false, null, null);

        if (odr != null) {

          int flds = odr.FieldCount;
          int row = 8;

          while (odr.Read()) {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }

        const string sqlStmt5 = "SELECT * FROM VIZ_PRN.V_QCHRC_L2T2";
        odr = Odac.GetOracleReader(sqlStmt5, System.Data.CommandType.Text, false, null, null);

        if (odr != null) {

          int flds = odr.FieldCount;
          int row = 16;

          while (odr.Read()) {
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, 1], CurrentWrkSheet.Cells[row, 12]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, 1], CurrentWrkSheet.Cells[row + 1, 12]]);

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }

        //Заполняем Лист3
        int rowOffset = 0;

        prm.ExcelApp.ActiveWorkbook.WorkSheets[3].Select();
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;

        CurrentWrkSheet.Cells[1, 8].Value = $"за период с {dtBegin:dd.MM.yyyy HH:mm:ss} по {dtEnd:dd.MM.yyyy HH:mm:ss}";

        const string sqlStmt6 = "SELECT * FROM VIZ_PRN.V_QCHRC_L3T1";
        odr = Odac.GetOracleReader(sqlStmt6, System.Data.CommandType.Text, false, null, null);

        if (odr != null){

          int flds = odr.FieldCount;
          int row = 9;

          while (odr.Read()) {

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();
        }

        const string sqlStmt7 = "SELECT * FROM VIZ_PRN.V_QCHRC_L3T2";
        odr = Odac.GetOracleReader(sqlStmt7, System.Data.CommandType.Text, false, null, null);

        if (odr != null)
        {

          int flds = odr.FieldCount;
          int inRow1 = 20;
          int inRowInsert1 = 22;

          while (odr.Read()){
            
            if (inRow1 == inRowInsert1) {
              CurrentWrkSheet.Rows[inRow1].Insert();
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[inRow1 - 1, 1], CurrentWrkSheet.Cells[inRow1 - 1, 4]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[inRow1, 1], CurrentWrkSheet.Cells[inRow1, 4]]);
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[inRow1, 1], CurrentWrkSheet.Cells[inRow1, 4]].ClearContents();
              inRowInsert1++;
              rowOffset++;
            }

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[inRow1, i + 1].Value = odr.GetValue(i);

            inRow1++;
          }
          odr.Close();
          odr.Dispose();
        }

        const string sqlStmt8 = "SELECT * FROM VIZ_PRN.V_QCHRC_L3T3";
        odr = Odac.GetOracleReader(sqlStmt8, System.Data.CommandType.Text, false, null, null);

        if (odr != null) {

          int flds = odr.FieldCount;
          int row = 29 + rowOffset;

          while (odr.Read()) {
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, 1], CurrentWrkSheet.Cells[row, 13]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, 1], CurrentWrkSheet.Cells[row + 1, 13]]);

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }
          odr.Close();
          odr.Dispose();
        }

        prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select();
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;
        CurrentWrkSheet.Cells[2, 1].Select();
        Result = true;
      }
      catch (Exception ex){
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка", ex.Message, MessageBoxImage.Stop)));
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
