using Devart.Data.Oracle;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Viz.WrkModule.Qc.Db.Reports
{
  public static class Report
  {
    public static void CreateProtocol(Workbook workBook, OracleDataReader odrProtocol, int rowStart, int idxWorkSheet = 0)
    {
      //Здесь будем грузить протокол
      var workSheet = workBook.Worksheets.ActiveWorksheet = workBook.Worksheets[idxWorkSheet];

      if (odrProtocol != null)
      {
        int flds = odrProtocol.FieldCount;
        

        while (odrProtocol.Read())
        {
          var rangeFrom = workSheet.Range.FromLTRB(0, rowStart, 9, rowStart);
          var rangeTo = workSheet.Range.FromLTRB(0, rowStart + 1, 9, rowStart + 1);
          rangeTo.CopyFrom(rangeFrom, PasteSpecial.All);

          workSheet[rowStart, 0].Value = odrProtocol.GetString(0);
          workSheet[rowStart, 1].Value = odrProtocol.GetInt32(1);
          workSheet[rowStart, 2].Value = odrProtocol.GetString(2);
          workSheet[rowStart, 3].Value = odrProtocol.GetInt32(3);
          workSheet[rowStart, 4].Value = odrProtocol.GetString(4);
          workSheet[rowStart, 5].Value = odrProtocol.GetInt32(5);
          workSheet[rowStart, 6].Value = odrProtocol.GetInt32(6);
          workSheet[rowStart, 7].Value = odrProtocol.GetString(7);
          workSheet[rowStart, 8].Value = odrProtocol.GetString(8);
          workSheet[rowStart, 9].Value = odrProtocol.GetString(9);
          rowStart++;
        }

        odrProtocol.Close();
        odrProtocol.Dispose();
      }

    }





  }
}
