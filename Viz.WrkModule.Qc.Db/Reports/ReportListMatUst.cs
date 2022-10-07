using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using Devart.Data.Oracle;
using Microsoft.Win32;
using DevExpress.Spreadsheet;
using Smv.Data.Oracle;
using Smv.Utils;
using Viz.WrkModule.Qc.Db.Dto;
using System.Security.Cryptography;
using DevExpress.XtraSpreadsheet.Model;
using Worksheet = DevExpress.Spreadsheet.Worksheet;

namespace Viz.WrkModule.Qc.Db.Reports
{
  public static class ReportListMatUst
  {

    private const string GnrUstSource = "\\Xlt\\Viz.WrkModule.Qc-ListMatUst.xltx";
    private const string GnrUstDest = "Viz.WrkModule.Qc-ListMatUst.xlsx";

    public static void CreateListMatUst(DtoRptListMatUstParamInput dtoRpt)
    {




    }
  }
}
