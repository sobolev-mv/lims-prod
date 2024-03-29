﻿using System;
using System.Collections.Generic;
using System.Data;
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

namespace Viz.WrkModule.RptOpr.Db
{
  public sealed class CrossTrimUoRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public CrossTrimUoRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class CrossTrimUo : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as CrossTrimUoRptParam);
      dynamic wrkSheet = null;

      try{
        //Выбираем нужный лист 
        prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select(); //выбираем лист
        wrkSheet = prm.ExcelApp.ActiveSheet;
        this.RunRpt(prm, wrkSheet);
        //Здесь визуализация Экселя
        //prm.ExcelApp.ScreenUpdating = true;
        //prm.ExcelApp.Visible = true;
        this.SaveResult(prm);
      }
      catch (Exception ex){
        Debug.Assert(prm != null, "prm != null");
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка", ex.Message, MessageBoxImage.Stop)));
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

    private Boolean RunRpt(CrossTrimUoRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;

      try{
        DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 1);
        var dtBegin = DbVar.GetDateBeginEnd(true, true);
        var dtEnd = DbVar.GetDateBeginEnd(false, true);

        //CurrentWrkSheet.Range["H1", "L1"].ClearContents();
        //CurrentWrkSheet.Range["H1:L1"].ClearContents();
        CurrentWrkSheet.Cells[1, 4].Value = $"с {dtBegin:dd.MM.yyyy HH:mm:ss} по {dtEnd:dd.MM.yyyy HH:mm:ss}";
        
        const string sqlStmt0 = "VIZ_PRN.CrossTrim_UO.preSchrott_TRIM_UO";
        var res = Odac.ExecuteNonQuery(sqlStmt0, CommandType.StoredProcedure, false, null);

        if (!res)
          throw new DataException();

        const string sqlStmt1 = "SELECT * FROM VIZ_PRN.UO_SCRAB_VTRIM";
        odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);
       
        if (odr != null){
          int flds = odr.FieldCount;
          
          int row = 6;

          const int firstExcelColumn = 1;
          const int lastExcelColumn = 13;

          while (odr.Read()){

            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;

          }
        }

        odr.Close();
        odr.Dispose();
        
        
        prm.ExcelApp.ActiveWorkbook.WorkSheets[2].Select();
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;
        CurrentWrkSheet.Cells[2, 5].Value = $"с {dtBegin:dd.MM.yyyy HH:mm:ss} по {dtEnd:dd.MM.yyyy HH:mm:ss}";
        
        var sum = Odac.ExecuteScalar("SELECT VES FROM VIZ_PRN.UO_SCRAB_TRIM_VES", CommandType.Text,false,null);
        CurrentWrkSheet.Cells[3, 5].Value = sum;

        
        odr = Odac.GetOracleReader("SELECT * FROM VIZ_PRN.UO_SCRAB_VTRIM_DEF", CommandType.Text, false, null, null);
        
        if (odr != null)
        {
          var row = 6;
          //var isertRow = 7;

          int flds = odr.FieldCount;

          const int firstExcelColumn = 2;
          const int lastExcelColumn = 6;
          

          
          while (odr.Read())
          {
            //CurrentWrkSheet.Rows[row].Insert();

            //CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);

            CurrentWrkSheet.Rows[row + 1].Insert();
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);
            //CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].ClearContents();


            for (int i = 0; i < odr.FieldCount; i++)
              CurrentWrkSheet.Cells[row, i + 2].Value = odr.GetValue(i);
            
            row++;
            //isertRow++;
          }
          odr.Close();
          odr.Dispose();
        }

        prm.ExcelApp.ActiveWorkbook.WorkSheets[3].Select();
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;

        CurrentWrkSheet.Cells[2, 5].Value = $"с {dtBegin:dd.MM.yyyy HH:mm:ss} по {dtEnd:dd.MM.yyyy HH:mm:ss}";
        CurrentWrkSheet.Cells[3, 5].Value = sum;

        odr = Odac.GetOracleReader("SELECT * FROM VIZ_PRN.UO_SCRAB_VTRIM_MAT", CommandType.Text, false, null, null);

        if (odr != null)
        {
          var row = 7;
          //var isertRow = 7;

          int flds = odr.FieldCount;

          const int firstExcelColumn = 2;
          const int lastExcelColumn = 5;

          while (odr.Read())
          {
            //CurrentWrkSheet.Rows[row].Insert();

            //CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);

            CurrentWrkSheet.Rows[row + 1].Insert();
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);
            //CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].ClearContents();


            for (int i = 0; i < odr.FieldCount; i++)
              CurrentWrkSheet.Cells[row, i + 2].Value = odr.GetValue(i);

            row++;
            //isertRow++;
          }
          odr.Close();
          odr.Dispose();
        }
        

        prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select();
        CurrentWrkSheet = prm.ExcelApp.ActiveSheet;
        CurrentWrkSheet.Cells[1, 1].Select();
        
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


