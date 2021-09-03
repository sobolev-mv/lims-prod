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

namespace Viz.WrkModule.RptOtk.Db
{
  public sealed class QuantityWcRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }

    public QuantityWcRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {
      
    }
  }

  public sealed class QuantityWc : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      QuantityWcRptParam prm = (e.Argument as QuantityWcRptParam);
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

    private Boolean RunRpt(QuantityWcRptParam prm, dynamic CurrentWrkSheet)
    {
      List<OracleParameter> lstParam = new List<OracleParameter>();
      
      OracleDataReader odr = null;
      string SqlStmt = null;
      Boolean Result = false;

      try{
        
        var param = new OracleParameter
        {
          DbType = DbType.DateTime,
          OracleDbType = OracleDbType.Date,
          Direction = ParameterDirection.Input,
          ParameterName = "zDtBegin",
          Value = prm.DateBegin
        };
        lstParam.Add(param);
        
        Odac.ExecuteNonQuery("begin VIZ_PRN.QUARTILE_UO1.QRT_PERIOD_DT(:zDtBegin, 6); end;", CommandType.Text, false, lstParam, true);

        prm.DateEnd = new DateTime(prm.DateBegin.Year, prm.DateBegin.Month,DateTime.DaysInMonth(prm.DateBegin.Year, prm.DateBegin.Month));
        prm.DateBegin = new DateTime(prm.DateBegin.AddMonths(-5).Year, prm.DateBegin.AddMonths(-5).Month, 1);
        DbVar.SetRangeDate(prm.DateBegin, prm.DateEnd, 1);


        SqlStmt = "SELECT * FROM VIZ_PRN.V_TP_COUNT";
        odr = Odac.GetOracleReader(SqlStmt, System.Data.CommandType.Text, false, null, null);
        
        if (odr == null) return false;
        
        int flds = odr.FieldCount;
        int row = 5;

        while (odr.Read()){
          //CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row, 1], CurrentWrkSheet.Cells[row, 9]].Copy(CurrentWrkSheet.Range[CurrentWrkSheet.Cells[row + 1, 1], CurrentWrkSheet.Cells[row + 1, 9]]);

          for (int i = 0; i < flds; i++)
            CurrentWrkSheet.Cells[row, i + 1].Value = odr.GetValue(i);

          row++;
        }

        odr.Close();
        odr.Dispose();

        SqlStmt = "SELECT * FROM VIZ_PRN.V_TP_PERIOD";
        odr = Odac.GetOracleReader(SqlStmt, System.Data.CommandType.Text, false, null, null);

        if (odr == null) return false;

        flds = odr.FieldCount;
        row = 4;
        int colDate = 2;

        while (odr.Read()) {
          CurrentWrkSheet.Cells[row, colDate].Value = odr.GetValue(0);

          colDate++;
        }

        odr.Close();
        odr.Dispose();

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

