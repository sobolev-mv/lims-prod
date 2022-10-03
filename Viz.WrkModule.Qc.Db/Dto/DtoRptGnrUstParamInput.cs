using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace Viz.WrkModule.Qc.Db.Dto
{
  public class DtoRptGnrUstParamInput
  {
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public double ?FinalThickness { get; set; }
    public string FinalThicknessSql => Convert.ToString(this.FinalThicknessItem.Row["TextSql"]);
    public DataRowView FinalThicknessItem { get; set; }
    public Boolean IsKesiAvg { get; set; }
    public Int32 KesiAvgMin { get; set; }
    public Int32 KesiAvgMax { get; set; }
    public Boolean IsKesiWorst { get; set; }
    public Int32 KesiWorstMin { get; set; }
    public Int32 KesiWorstMax { get; set; }
    public Boolean IsP1750 { get; set; }
    public double P1750Min { get; set; }
    public double P1750Max { get; set; }
    public Boolean IsDefectTolowCat { get; set; }
    public string DefectTolowCat { get; set; }
    public Boolean IsDefectTo2Sort { get; set; }
    public string DefectTo2Sort { get; set; }
    public Boolean IsAdgIn { get; set; }
    public string AdgInSql { get; set; }
    public DataTable DtThickness { get; set; }
    public string AgTyp { get; set; }

  }
}
