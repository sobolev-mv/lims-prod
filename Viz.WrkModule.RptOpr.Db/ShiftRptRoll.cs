using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using Devart.Data.Oracle;
using Smv.Data.Oracle;
using Viz.DbApp.Psi;

namespace Viz.WrkModule.RptOpr.Db
{
  internal enum TypeRoll
  {
    Rm1300,
    OtherRoll,
    NotExistsRoll
  }

  /*Расположение листов АПР в файле*/
  internal enum SheetsOfRoll
  {
    Rm1300  =  1,
    Rm12001 =  2,
    Rm12002 =  3,
    Rrm     =  4
  }

  public sealed class ShiftRptRollRptParam : Smv.Xls.XlsInstanceParam
  {
    public DateTime DateBegin { get; set; }
    public string Roll { get; set; }
    public string RollLabel { get; set; }
    public string TeamRoll { get; set; }
    public string ShiftMasterRoll { get; set; }
    public string TopWorkerRoll { get; set; }
    public string ShiftTypeRoll { get; set; }
   
    public string[] RuAnlage = new string[] { "Все", "СТАН1300", "СТАН12001", "СТАН12002", "РПС" };

    public ShiftRptRollRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class ShiftRptRoll : Smv.Xls.XlsRpt
  {
    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as ShiftRptRollRptParam);
      dynamic wrkSheet = null;

      try{
        //Выбираем нужный лист 
        prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select(); //выбираем лист
        wrkSheet = prm.ExcelApp.ActiveSheet;
        this.RunRpt(prm, wrkSheet);
        this.SaveResult(prm);

        //вызывается в случае перключения целевой БД
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

        if (wrkSheet != null)
          Marshal.ReleaseComObject(prm.ExcelApp);

        wrkSheet = null;
        prm.WorkBook = null;
        prm.ExcelApp = null;
        GC.Collect();
      }
    }

    private TypeRoll GetTypeRoll(string psiNameApr)
    {
      if ((psiNameApr != null) && string.Equals(psiNameApr.ToUpper(CultureInfo.InvariantCulture), "СТАН1300"))
        return TypeRoll.Rm1300;
      if ((psiNameApr != null) && string.Equals(psiNameApr.ToUpper(CultureInfo.InvariantCulture), "СТАН12001"))
        return TypeRoll.OtherRoll;
      if ((psiNameApr != null) && string.Equals(psiNameApr.ToUpper(CultureInfo.InvariantCulture), "СТАН12002"))
        return TypeRoll.OtherRoll;
      if ((psiNameApr != null) && string.Equals(psiNameApr.ToUpper(CultureInfo.InvariantCulture), "РПС"))
        return TypeRoll.OtherRoll;
      else
        return TypeRoll.NotExistsRoll;
    }

    private int GetOneVisibleSheet(ShiftRptRollRptParam prm)
    {
      if (prm.Roll.IndexOf(",", StringComparison.Ordinal) > -1)
        return 0;
      
      int idxActiveSheet = 1;

      foreach (int idxSheet in Enum.GetValues(typeof(SheetsOfRoll)))
      {

        var name = prm.Roll; //Enum.GetName(typeof(SheetsOfRoll), idxSheet);
        Boolean isVisible = (name != null) && (string.Equals(name.ToUpper(CultureInfo.InvariantCulture), prm.RuAnlage[idxSheet].ToUpper(CultureInfo.InvariantCulture), StringComparison.Ordinal));

        if (!isVisible){
          prm.ExcelApp.ActiveWorkbook.WorkSheets[idxSheet].Select();
          prm.ExcelApp.ActiveSheet.Visible = false;
        }
        else
          idxActiveSheet = idxSheet;
      }

      prm.ExcelApp.ActiveWorkbook.WorkSheets[idxActiveSheet].Select();
      return idxActiveSheet;
    }

    private void Roll1300(ShiftRptRollRptParam prm, DateTime? dtBegin, int idxSheet)
    {
      OracleDataReader odr = null;
      
      prm.ExcelApp.ActiveWorkbook.WorkSheets[idxSheet].Select();
      dynamic currentWrkSheet = prm.ExcelApp.ActiveSheet;

      currentWrkSheet.Cells[2, 7].Value = $"{dtBegin:dd.MM.yyyy}";
      currentWrkSheet.Cells[2, 9].Value = prm.TeamRoll;
      currentWrkSheet.Cells[2, 10].Value = prm.ShiftTypeRoll;
      currentWrkSheet.Cells[2, 11].Value = prm.RuAnlage[idxSheet]; 
      currentWrkSheet.Cells[5, 9].Value = prm.ShiftMasterRoll;
      currentWrkSheet.Cells[7, 9].Value = prm.TopWorkerRoll;
      

      int qntInsert = 0;

      const string sqlStmt121 = "SELECT * FROM VIZ_PRN.SM_RAPORT_PU_1300";
      odr = Odac.GetOracleReader(sqlStmt121, CommandType.Text, false, null, null);

      if (odr != null){

        int inRow1 = 13;
        int inRowInsert1 = 15;

        while (odr.Read()){

          if (inRow1 == inRowInsert1){
            currentWrkSheet.Rows[inRow1].Insert();
            currentWrkSheet.Range[currentWrkSheet.Cells[inRow1 - 1, 1], currentWrkSheet.Cells[inRow1 - 1, 13]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[inRow1, 1], currentWrkSheet.Cells[inRow1, 13]]);
            currentWrkSheet.Range[currentWrkSheet.Cells[inRow1, 1], currentWrkSheet.Cells[inRow1, 13]].ClearContents();
            inRowInsert1++;
            qntInsert++;
          }

          currentWrkSheet.Cells[inRow1, 1].Value = odr.GetValue(0);
          currentWrkSheet.Cells[inRow1, 2].Value = odr.GetValue(1);
          currentWrkSheet.Cells[inRow1, 3].Value = odr.GetValue(2);
          currentWrkSheet.Cells[inRow1, 4].Value = odr.GetValue(3);
          currentWrkSheet.Cells[inRow1, 5].Value = odr.GetValue(4);
          currentWrkSheet.Cells[inRow1, 6].Value = odr.GetValue(5);
          currentWrkSheet.Cells[inRow1, 7].Value = odr.GetValue(6);
          currentWrkSheet.Cells[inRow1, 8].Value = odr.GetValue(7);
          currentWrkSheet.Cells[inRow1, 9].Value = odr.GetValue(8);
          currentWrkSheet.Cells[inRow1, 10].Value = odr.GetValue(9);
          currentWrkSheet.Cells[inRow1, 11].Value = odr.GetValue(10);

          inRow1++;
        }
        odr.Close();
        odr.Dispose();
      }

      const string sqlStmt2 = "SELECT * FROM VIZ_PRN.SM_RAPORT_PROSTOI_ALL";
      odr = Odac.GetOracleReader(sqlStmt2, CommandType.Text, false, null, null);

      int qntInsert2 = 0;
    

      if (odr != null){

        int inRowFuter2 = 23 + qntInsert;
        int inRowInsertFuter2 = 25 + qntInsert;

        while (odr.Read()){

          if (inRowFuter2 == inRowInsertFuter2){
            currentWrkSheet.Rows[inRowFuter2].Insert();
            currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter2 + 1, 1], currentWrkSheet.Cells[inRowFuter2 + 1, 17]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter2, 1], currentWrkSheet.Cells[inRowFuter2, 17]]);
            inRowInsertFuter2++;
            qntInsert2++;
          }

          currentWrkSheet.Cells[inRowFuter2, 1].Value = odr.GetValue(0);
          currentWrkSheet.Cells[inRowFuter2, 3].Value = odr.GetValue(1);
          currentWrkSheet.Cells[inRowFuter2, 4].Value = odr.GetValue(2);
          currentWrkSheet.Cells[inRowFuter2, 6].Value = odr.GetValue(3);
          inRowFuter2++;
        }
        odr.Close();
        odr.Dispose();
      }

      DbVar.SetRangeDate(prm.DateBegin, prm.DateBegin, 0);
      DbVar.SetString(prm.RuAnlage[idxSheet], prm.TeamRoll);

      const string sqlStmt21 = "SELECT * FROM VIZ_PRN.SM_RAPORT_FIO";
      odr = Odac.GetOracleReader(sqlStmt21, CommandType.Text, false, null, null);

      if (odr != null){
        int inRowFuter3 = 23 + qntInsert;
        int inRowInsertFuter3 = 25 + qntInsert + qntInsert2;

        while (odr.Read()){

          if (inRowFuter3 == inRowInsertFuter3) {
            currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter3, 11], currentWrkSheet.Cells[inRowFuter3, 17]].Insert(-4121, 0);
            currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter3 + 1, 11], currentWrkSheet.Cells[inRowFuter3 + 1, 17]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter3, 11], currentWrkSheet.Cells[inRowFuter3, 17]]);
            inRowInsertFuter3++;
          }

          currentWrkSheet.Cells[inRowFuter3, 11].Value = odr.GetValue(0);
          currentWrkSheet.Cells[inRowFuter3, 12].Value = odr.GetValue(1);
          inRowFuter3++;
        }
        odr.Close();
        odr.Dispose();
      }
    }

    private void RollOther(ShiftRptRollRptParam prm, DateTime? dtBegin, int idxSheet)
    {
      OracleDataReader odr = null;
      prm.ExcelApp.ActiveWorkbook.WorkSheets[idxSheet].Select();
      dynamic currentWrkSheet = prm.ExcelApp.ActiveSheet;

      currentWrkSheet.Cells[2, 10].Value = $"{dtBegin:dd.MM.yyyy}";
      currentWrkSheet.Cells[2, 12].Value = prm.TeamRoll;
      currentWrkSheet.Cells[2, 13].Value = prm.ShiftTypeRoll;
      currentWrkSheet.Cells[2, 15].Value = prm.RuAnlage[idxSheet];
      currentWrkSheet.Cells[4, 12].Value = prm.ShiftMasterRoll;
      currentWrkSheet.Cells[6, 12].Value = prm.TopWorkerRoll;

      int qntInsert = 0;
      
      var sqlStmt1 = "SELECT * FROM VIZ_PRN.SM_RAPORT_PU_1200";
      odr = Odac.GetOracleReader(sqlStmt1, CommandType.Text, false, null, null);

      if (odr != null){

        int inRow1 = 12;
        int inRowInsert1 = 14;


        while (odr.Read()){

          if (inRow1 == inRowInsert1){
            currentWrkSheet.Rows[inRow1].Insert();
            currentWrkSheet.Range[currentWrkSheet.Cells[inRow1 - 1, 1], currentWrkSheet.Cells[inRow1 - 1, 24]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[inRow1, 1], currentWrkSheet.Cells[inRow1, 24]]);
            currentWrkSheet.Range[currentWrkSheet.Cells[inRow1, 1], currentWrkSheet.Cells[inRow1, 23]].ClearContents();
            inRowInsert1++;
            qntInsert++;
          }

          currentWrkSheet.Cells[inRow1, 1].Value = odr.GetValue(0);
          currentWrkSheet.Cells[inRow1, 2].Value = odr.GetValue(1);
          currentWrkSheet.Cells[inRow1, 3].Value = odr.GetValue(2);
          currentWrkSheet.Cells[inRow1, 4].Value = odr.GetValue(3);
          currentWrkSheet.Cells[inRow1, 5].Value = odr.GetValue(4);
          currentWrkSheet.Cells[inRow1, 6].Value = odr.GetValue(5);
          currentWrkSheet.Cells[inRow1, 7].Value = odr.GetValue(6);
          currentWrkSheet.Cells[inRow1, 8].Value = odr.GetValue(7);
          currentWrkSheet.Cells[inRow1, 9].Value = odr.GetValue(8);
          currentWrkSheet.Cells[inRow1, 10].Value = odr.GetValue(9);
          currentWrkSheet.Cells[inRow1, 11].Value = odr.GetValue(10);
          currentWrkSheet.Cells[inRow1, 12].Value = odr.GetValue(11);
          currentWrkSheet.Cells[inRow1, 13].Value = odr.GetValue(12);
          currentWrkSheet.Cells[inRow1, 14].Value = odr.GetValue(13);
          currentWrkSheet.Cells[inRow1, 17].Value = odr.GetValue(14);
          currentWrkSheet.Cells[inRow1, 18].Value = odr.GetValue(15);

          inRow1++;
        }

        odr.Close();
        odr.Dispose();
      }

      const string sqlStmt2 = "SELECT * FROM VIZ_PRN.SM_RAPORT_PROSTOI_ALL";
      odr = Odac.GetOracleReader(sqlStmt2, CommandType.Text, false, null, null);

      int qntInsert2 = 0;


      if (odr != null)
      {

        int inRowFuter2 = 22 + qntInsert;
        int inRowInsertFuter2 = 24 + qntInsert;

        while (odr.Read())
        {

          if (inRowFuter2 == inRowInsertFuter2)
          {
            currentWrkSheet.Rows[inRowFuter2].Insert();
            currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter2 + 1, 1], currentWrkSheet.Cells[inRowFuter2 + 1, 20]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter2, 1], currentWrkSheet.Cells[inRowFuter2, 20]]);
            inRowInsertFuter2++;
            qntInsert2++;
          }

          currentWrkSheet.Cells[inRowFuter2, 1].Value = odr.GetValue(0);
          currentWrkSheet.Cells[inRowFuter2, 3].Value = odr.GetValue(1);
          currentWrkSheet.Cells[inRowFuter2, 4].Value = odr.GetValue(2);
          currentWrkSheet.Cells[inRowFuter2, 6].Value = odr.GetValue(3);
          inRowFuter2++;
        }
        odr.Close();
        odr.Dispose();
      }

      DbVar.SetRangeDate(prm.DateBegin, prm.DateBegin, 0);
      DbVar.SetString(prm.RuAnlage[idxSheet], prm.TeamRoll);

      const string sqlStmt21 = "SELECT * FROM VIZ_PRN.SM_RAPORT_FIO";
      odr = Odac.GetOracleReader(sqlStmt21, CommandType.Text, false, null, null);

      if (odr != null)
      {
        int inRowFuter3 = 22 + qntInsert;
        int inRowInsertFuter3 = 24 + qntInsert + qntInsert2;

        while (odr.Read())
        {

          if (inRowFuter3 == inRowInsertFuter3)
          {
            currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter3, 11], currentWrkSheet.Cells[inRowFuter3, 20]].Insert(-4121, 0);
            currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter3 + 1, 11], currentWrkSheet.Cells[inRowFuter3 + 1, 20]].Copy(currentWrkSheet.Range[currentWrkSheet.Cells[inRowFuter3, 11], currentWrkSheet.Cells[inRowFuter3, 20]]);
            inRowInsertFuter3++;
          }

          currentWrkSheet.Cells[inRowFuter3, 11].Value = odr.GetValue(0);
          currentWrkSheet.Cells[inRowFuter3, 12].Value = odr.GetValue(1);
          inRowFuter3++;
        }
        odr.Close();
        odr.Dispose();
      }
      
      //здесь вытаскиваем производительность
      const string sqlStmtProizv = "SELECT * FROM VIZ_PRN.SM_RAPORT_PROD_PU";
      odr = Odac.GetOracleReader(sqlStmtProizv, CommandType.Text, false, null, null);

      if (odr != null){
        if (odr.Read())
        {
          currentWrkSheet.Cells[30 + qntInsert + qntInsert2, 3].Value = odr.GetValue(0);
          currentWrkSheet.Cells[30 + qntInsert + qntInsert2, 4].Value = odr.GetValue(1);
          currentWrkSheet.Cells[30 + qntInsert + qntInsert2, 5].Value = odr.GetValue(2);
          currentWrkSheet.Cells[30 + qntInsert + qntInsert2, 6].Value = odr.GetValue(3);
          currentWrkSheet.Cells[30 + qntInsert + qntInsert2, 7].Value = odr.GetValue(4);
          currentWrkSheet.Cells[30 + qntInsert + qntInsert2, 8].Value = odr.GetValue(5);
        }

        odr.Close();
        odr.Dispose();
      }
      
      const string sqlStmtProstoi = "SELECT * FROM VIZ_PRN.SM_RAPORT_PROSTOIMIN_PU";
      odr = Odac.GetOracleReader(sqlStmtProstoi, CommandType.Text, false, null, null);

      int qntInsertAll = qntInsert;

      if (odr != null)
      {
        if (odr.Read())
          currentWrkSheet.Cells[36 + qntInsert + qntInsert2, 3].Value = odr.GetValue(0);

        odr.Close();
        odr.Dispose();
      }
      
      //currentWrkSheet.PageSetup.PrintArea = "$A$1:$U$" + (50 + qntInsertAll).ToString();
  }

  private Boolean RunRpt(ShiftRptRollRptParam prm, dynamic currentWrkSheet)
  {
      OracleDataReader odr = null;
      Boolean result = false;
      
      try{
        var idxActiveSheet = GetOneVisibleSheet(prm);
        
        const string sqlStmt = "VIZ_PRN.SMEN_RAPORT_PU.preSM_Raport_ALL";
        var lstOraPrm = new List<OracleParameter>()
        {
          new OracleParameter()
          {
            DbType = DbType.DateTime,
            Direction = ParameterDirection.Input,
            OracleDbType = OracleDbType.Date,
            Value = prm.DateBegin
          },

          new OracleParameter()
          {
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            OracleDbType = OracleDbType.VarChar,
            Size = string.IsNullOrEmpty(prm.RollLabel) ? 0 : prm.RuAnlage[idxActiveSheet].Length,
            Value = prm.RuAnlage[idxActiveSheet]
          },
           
          new OracleParameter()
          {
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            OracleDbType = OracleDbType.VarChar,
            Size = string.IsNullOrEmpty(prm.TeamRoll) ? 0 : prm.TeamRoll.Length,
            Value = prm.TeamRoll
          },

          new OracleParameter()
          {
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            OracleDbType = OracleDbType.VarChar,
            Size = string.IsNullOrEmpty(prm.Roll) ? 0 : prm.Roll.Length,
            Value = prm.Roll
          },

          new OracleParameter()
          {
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            OracleDbType = OracleDbType.VarChar,
            Size = string.IsNullOrEmpty(prm.ShiftTypeRoll) ? 0 : prm.ShiftTypeRoll.Length,
            Value = prm.ShiftTypeRoll
          },
        };

        Odac.ExecuteNonQuery(sqlStmt, CommandType.StoredProcedure, false, lstOraPrm);
        DbVar.SetRangeDate(prm.DateBegin, prm.DateBegin, 0);
        var dtBegin = DbVar.GetDateBeginEnd(true, false);

        if (idxActiveSheet == 0){
          //здесь идет заполнение всех листов (для всех АПР)

          foreach (int idxSheet in Enum.GetValues(typeof(SheetsOfRoll))){

            var name = Enum.GetName(typeof(SheetsOfRoll), idxSheet);

            if (String.IsNullOrEmpty(name))
              continue;

            //Выставляем переменные среды
            DbVar.SetString(prm.RuAnlage[idxSheet]);
            
            if (GetTypeRoll(prm.RuAnlage[idxSheet]) == TypeRoll.Rm1300)
              Roll1300(prm, dtBegin, idxSheet);
            else if (GetTypeRoll(prm.RuAnlage[idxSheet]) == TypeRoll.OtherRoll)
              RollOther(prm, dtBegin, idxSheet);
            else
              prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart) (() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка", "Шаблона под выбранный агрегат не существует!", MessageBoxImage.Stop)));
          }

          prm.ExcelApp.ActiveWorkbook.WorkSheets[1].Select();
        }
        else
        {
          DbVar.SetString(prm.Roll);

          if (GetTypeRoll(prm.Roll) == TypeRoll.Rm1300)
            Roll1300(prm, dtBegin, idxActiveSheet);
          else if (GetTypeRoll(prm.Roll) == TypeRoll.OtherRoll)
            RollOther(prm, dtBegin, idxActiveSheet);
          else
            prm.Disp.Invoke(DispatcherPriority.Normal,
              (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка", "Шаблона под выбранный агрегат не существует!", MessageBoxImage.Stop)));
        }
        
        result = true;
      }
      
      catch (Exception e){
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка", e.Message, MessageBoxImage.Stop)));
        result = false;
      }
      finally{
        if (odr != null){
          odr.Close();
          odr.Dispose();
        }
      }

      return result;
    }


  }






}
