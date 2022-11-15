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
  public sealed class EliminateDefAvoRptParam : Smv.Xls.XlsInstanceParam
  {
    public string LocNum { get; set; }
    public EliminateDefAvoRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class EliminateDefAvo : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as EliminateDefAvoRptParam);
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

    private Boolean RunRpt(EliminateDefAvoRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;

      try{
        const string sqlStmt = "VIZ_PRN.OTK_AVO_DEF_USTR.PREAVO_DEF_USTR";
        var lstOraPrm = new List<OracleParameter>()
        {
          new OracleParameter()
          {
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            OracleDbType = OracleDbType.VarChar,
            Size = prm.LocNum.Length,
            Value = prm.LocNum
          }
        };
        Odac.ExecuteNonQuery(sqlStmt, CommandType.StoredProcedure, false, lstOraPrm);
        
        const string sqlStmt1 = "SELECT * FROM VIZ_PRN.V_AVO_RUL";
        odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);
       
        if (odr != null){

          if (odr.Read())
          {
            CurrentWrkSheet.Cells[2, 2].Value = odr.GetValue(0);
            CurrentWrkSheet.Cells[3, 2].Value = odr.GetValue(1);
            CurrentWrkSheet.Cells[4, 2].Value = odr.GetValue(2);
            CurrentWrkSheet.Cells[5, 2].Value = odr.GetValue(3);
            CurrentWrkSheet.Cells[2, 6].Value = odr.GetValue(4);
            CurrentWrkSheet.Cells[2, 8].Value = odr.GetValue(5);
            CurrentWrkSheet.Cells[3, 6].Value = odr.GetValue(6);
            CurrentWrkSheet.Cells[3, 8].Value = odr.GetValue(7);
          }
          odr.Close();
          odr.Dispose();
        }

        const string sqlStmt2 = "SELECT * FROM VIZ_PRN.V_DEFECTS_AVO_USTR";
        odr = Odac.GetOracleReader(sqlStmt2, CommandType.Text, false, null, null);

        if (odr != null)
        {
          int flds = odr.FieldCount;
          int row = 9;

          const int firstExcelColumn = 1;
          const int lastExcelColumn = 9;

          while (odr.Read())
          {
            CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, firstExcelColumn], CurrentWrkSheet.Cells[row, lastExcelColumn]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, firstExcelColumn], CurrentWrkSheet.Cells[row + 1, lastExcelColumn]]);

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

            row++;
          }
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


