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
  public sealed class HistCoilProcRptParam : Smv.Xls.XlsInstanceParam
  {
    public string ListAnLot { get; set; }
    public Boolean IsRemarkInclude { get; set; }

    public HistCoilProcRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class HistCoilProc : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as HistCoilProcRptParam);
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

    private Boolean RunRpt(HistCoilProcRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;

      try{
        //CurrentWrkSheet.Cells[1, 12].Value = $"{DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        
        if (prm.IsRemarkInclude)
          DbVar.SetString("Y");

        DbVar.SetStringList(prm.ListAnLot, ",");

        const string sqlStmt0 = "VIZ_PRN.HIST_COILPROC.Prepare";
        Odac.ExecuteNonQuery(sqlStmt0, CommandType.StoredProcedure, false, null);

        const string sqlStmt1 = "SELECT * FROM VIZ_PRN.V_HIST_COILPROC";
        odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);

        var lstUnionCell = new List<int>();

        if (odr != null){
          int flds = odr.FieldCount;
          int row = 6;

          double heightRow = CurrentWrkSheet.Rows(row).RowHeight;
          
          const int firstExcelColumn = 1;
          const int lastExcelColumn = 34;

          int rnkAnLotOld = 0;
  
          while (odr.Read()){

            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);

            if (rnkAnLotOld != odr.GetInt32(0))
            {
              lstUnionCell.Add(odr.GetInt32(flds - 1));
              rnkAnLotOld = odr.GetInt32(0);

              CurrentWrkSheet.Cells[row, 1].Value = odr.GetValue(0);
              CurrentWrkSheet.Cells[row, 2].Value = odr.GetValue(1);
            }
            
            for (int i = 2; i < flds - 1; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }

          odr.Close();
          odr.Dispose();

          int rowStart = 6;
          foreach (var item in lstUnionCell)
          {
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[rowStart, 1], CurrentWrkSheet.Cells[rowStart + item - 1, 1]].Merge();
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[rowStart, 2], CurrentWrkSheet.Cells[rowStart + item - 1, 2]].Merge();
            rowStart += item;
          }

          /*
          for (int i = rowStart; i < row; i++)
            CurrentWrkSheet.Cells[i, 6].WrapText = CurrentWrkSheet.Cells[i, 9].WrapText = CurrentWrkSheet.Cells[i, 12].WrapText =
            CurrentWrkSheet.Cells[i, 15].WrapText = CurrentWrkSheet.Cells[i, 18].WrapText = CurrentWrkSheet.Cells[i, 21].WrapText =
            CurrentWrkSheet.Cells[i, 25].WrapText = CurrentWrkSheet.Cells[i, 28].WrapText = CurrentWrkSheet.Cells[i, 31].WrapText = false;
          */
          
          CurrentWrkSheet.Columns(7).Hidden = true;
          CurrentWrkSheet.Columns(10).Hidden = true;
          CurrentWrkSheet.Columns(13).Hidden = true;
          CurrentWrkSheet.Columns(16).Hidden = true;
          CurrentWrkSheet.Columns(19).Hidden = true;
          CurrentWrkSheet.Columns(22).Hidden = true;
          CurrentWrkSheet.Columns(26).Hidden = true;
          CurrentWrkSheet.Columns(29).Hidden = true;
          CurrentWrkSheet.Columns(32).Hidden = true;
          
        }
        
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


