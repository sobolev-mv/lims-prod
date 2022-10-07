using DevExpress.Spreadsheet;
using Smv.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smv.SpreadSheet
{
  public static class DxExSpreadSheet
  {
    public static Workbook CreateAndLoadWorkBook(string fileName)
    {
      var src = Etc.StartPath + fileName;
      var workBook = new Workbook();
      workBook.LoadDocument(src, DocumentFormat.Xltx);

      return workBook;
    }

    public static void SaveWorkBook(Workbook workBook, string fileName)
    {
      var dst = Etc.GetFullPathRptFile(fileName);

      workBook.SaveDocument(dst, DocumentFormat.Xlsx);
      workBook.Dispose();
      Etc.OpenRptFolderOnTargetFile(fileName);
    }

  }
}
