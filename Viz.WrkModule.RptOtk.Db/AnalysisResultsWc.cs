using System;
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

namespace Viz.WrkModule.RptOtk.Db
{
  public sealed class AnalysisResultsWcRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }

    public AnalysisResultsWcRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {
      
    }
  }

  public sealed class AnalysisResultsWc : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      AnalysisResultsWcRptParam prm = (e.Argument as AnalysisResultsWcRptParam);
      dynamic wrkSheet = null;

      try
      {
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

        //вызывается в случае переключения целевой БД
        base.DoWorkXls(sender, e);
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

    private Boolean RunRpt(AnalysisResultsWcRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      string SqlStmt = null;
      Boolean Result = false;
      OdacErrorInfo oef = new OdacErrorInfo();
      DateTime? dtBegin = null;
      DateTime? dtEnd = null;

      try{

        SqlStmt = "SELECT * FROM VIZ_PRN.V_TP_ANALYSIS";
        DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 1);
        dtBegin = DbVar.GetDateBeginEnd(true, true);
        dtEnd = DbVar.GetDateBeginEnd(false, true);
        odr = Odac.GetOracleReader(SqlStmt, System.Data.CommandType.Text, false, null, null);

        if (odr == null) return false;

        CurrentWrkSheet.Cells[2, 1].Value = "за период " + $"{dtBegin:dd.MM.yyyy HH:mm:ss}" + " - " + string.Format("{0:dd.MM.yyyy HH:mm:ss}", dtEnd);

        int flds = odr.FieldCount;
        int row = 5;

        while (odr.Read()){
          //CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, 1], CurrentWrkSheet.Cells[row, 9]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, 1], CurrentWrkSheet.Cells[row + 1, 9]]);

          for (int i = 0; i < flds; i++)
            CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

          row++;
        }

        CurrentWrkSheet.Cells[1, 1].Select();
        Result = true;
      }
      catch (Exception){
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

