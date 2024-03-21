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
  public sealed class Pj4TsRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public int PageNumber { get; set; }
    public Pj4TsRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    { }
  }

  public sealed class Pj4Ts : Smv.Xls.XlsRpt
  {
    private void SetActiveWorkSheet(Pj4TsRptParam prm)
    {
      for (var i = 1; i < 9; i++)
      {
        if (i != prm.PageNumber)
          prm.ExcelApp.ActiveWorkbook.WorkSheets[i].Visible = false;
        else
          prm.ExcelApp.ActiveWorkbook.WorkSheets[i].Select();

      }
    }


    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as Pj4TsRptParam);
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

    private Boolean RunRpt(Pj4TsRptParam prm, dynamic currentWrkSheet)
    {
      DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 1);
      var dtBegin = DbVar.GetDateBeginEnd(true, true);
      var dtEnd = DbVar.GetDateBeginEnd(false, true);

      SetActiveWorkSheet(prm);

      const int firstExcelColumn = 1;
      int lastExcelColumn = 1;
      string viewName = null;

      currentWrkSheet = prm.ExcelApp.ActiveSheet;
      switch(prm.PageNumber)
      {
        case 1:
          lastExcelColumn = 10;
          viewName = "VIZ_PRN.V_PJ_1STROLL";
          break;
        case 2:
          lastExcelColumn = 21;
          viewName = "VIZ_PRN.V_PJ_1STCUT";
          break;
        case 3:
          lastExcelColumn = 28;
          viewName = "VIZ_PRN.V_PJ_DECARB";
          break;
        case 4:
          lastExcelColumn = 35;
          viewName = "VIZ_PRN.V_PJ_2NDROLL";
          break;
        case 5:
          lastExcelColumn = 43;
          viewName = "VIZ_PRN.V_PJ_2NDCUT";
          break;
        case 6:
          lastExcelColumn = 51;
          viewName = "VIZ_PRN.V_PJ_ISOGO";
          break;
        case 7:
          lastExcelColumn = 59;
          viewName = "VIZ_PRN.V_PJ_HTANNBF";
          break;
        case 8:
          lastExcelColumn = 68;
          viewName = "VIZ_PRN.V_PJ_STRANN";
          break;
      }
      
      var result = false;
      OracleDataReader odr = null;

      try{
       
        currentWrkSheet.Cells[3, 3].Value = $"{dtBegin:dd.MM.yyyy HH:mm:ss} - {dtEnd:dd.MM.yyyy HH:mm:ss}";
        
        var stmtSql = "SELECT * FROM " + viewName;
        odr = Odac.GetOracleReader(stmtSql, CommandType.Text, false, null, null);

        if (odr != null)
        {
          var row = 7;
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
