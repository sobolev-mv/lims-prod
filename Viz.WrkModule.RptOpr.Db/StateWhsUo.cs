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
  public sealed class StateWhsUoRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public StateWhsUoRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class StateWhsUo : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as StateWhsUoRptParam);
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

    private Boolean RunRpt(StateWhsUoRptParam prm, dynamic CurrentWrkSheet)
    {
      OracleDataReader odr = null;
      Boolean Result = false;

      try{
        CurrentWrkSheet.Cells[1, 6].Value = prm.DateBegin;//$"{prm.DateBegin:dd.MM.yyyy}";
        
        const string sqlStmt0 = "begin " + 
                                " VIZ_PRN.StockyardState_UO.preStockyardState(:zDtBegin);  "+
                                " VIZ_PRN.VAR_RPT.SetDate(:zDtBegin, ADD_MONTHS(:zDtBegin, 1),0); "+
                                "end;";

        var lstPrm = new List<OracleParameter>();
        var oraPrm = new OracleParameter
        {
          ParameterName = "zDtBegin",
          DbType = DbType.DateTime,
          Direction = ParameterDirection.Input,
          OracleDbType = OracleDbType.Date,
          Value = prm.DateBegin
        };
        lstPrm.Add(oraPrm);
        
        Odac.ExecuteNonQuery(sqlStmt0, CommandType.Text, false, lstPrm, true);
        
        const string sqlStmt1 = "SELECT * FROM VIZ_PRN.V_STOCKYARDSTATE_UO";
        odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);
       
        if (odr != null){
          int flds = odr.FieldCount;
          int row = 5;

          while (odr.Read()){

            for (int i = 0; i < flds; i++)
              CurrentWrkSheet.Cells[row, i + 3].Value = odr.GetValue(i);
            
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


