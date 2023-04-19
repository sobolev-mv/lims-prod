using System;
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
  public sealed class QiExUo1RptParam : Smv.Xls.XlsInstanceParam
  {
    public string ListAnLot { get; set; }
    public QiExUo1RptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class QiExUo1 : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as QiExUo1RptParam);
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

    private Boolean RunRpt(QiExUo1RptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;

      try{
        DbVar.SetStringList(prm.ListAnLot, ",");

        const string sqlStmt0 = "VIZ_PRN.HIST_COILPROC.Prepare";
        Odac.ExecuteNonQuery(sqlStmt0, CommandType.StoredProcedure, false, null);

        const string sqlStmt1 = "SELECT * FROM VIZ_PRN.V_QIUO1_EX";
        odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);
        
        if (odr != null){
          int flds = odr.FieldCount;
          int row = 7;

          const int firstExcelColumn = 1;
          const int lastExcelColumn = 45;
          
          while (odr.Read()){
            
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);
            
            for (int i = 2; i < flds; i++)
              CurrentWrkSheet.Cells[row, i].Value = odr.GetValue(i);


            if (!odr.IsDBNull(1) && odr.IsDBNull(2))
            {
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Interior.Pattern = 1;//xlSolid
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Interior.ThemeColor = 5;
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Interior.TintAndShade = 0.599993896298105;

              CurrentWrkSheet.Cells[row, 1].Value = "Итого";
              CurrentWrkSheet.Cells[row, 2].Value = $"{odr.GetDouble(0):n0}";

              CurrentWrkSheet.Cells[row - odr.GetInt32(0), 1].Value = odr.GetValue(1);
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row - odr.GetInt32(0), 1], CurrentWrkSheet.Cells[row - 1, 1]].Merge();
            }

            if (odr.IsDBNull(1) && odr.IsDBNull(2))
            {
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Interior.Pattern = 1;//xlSolid
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Interior.ThemeColor = 4;
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Interior.TintAndShade = 0.399975585192419;
              CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Font.Bold = true;

              CurrentWrkSheet.Cells[row, 1].Value = "Всего";
              CurrentWrkSheet.Cells[row, 2].Value = $"{odr.GetDouble(0):n0}";
            }
            
            row++;
          }
          odr.Close();
          odr.Dispose();
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


