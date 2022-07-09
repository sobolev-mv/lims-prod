using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class ThicknessDataTable : DataTable
    {
      protected readonly OracleDataAdapter adapter;

      public ThicknessDataTable(string tblName) : base()
      {
        this.TableName = tblName;
        adapter = new OracleDataAdapter();

        var col = new DataColumn("Thickness", typeof(double), null, MappingType.Element)
        {
          AllowDBNull = false,
          AutoIncrement = false,
          AutoIncrementStep = -1
        };
        this.Columns.Add(col);

        col = new DataColumn("TextDispaly", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        this.Constraints.Add(new UniqueConstraint("Pk_" + tblName, new[] { this.Columns["Thickness"] }, true));

        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.QMF_THICKNESS_CHR", tblName);
        dtm.ColumnMappings.Add("THICKNESS", "Thickness");
        dtm.ColumnMappings.Add("TEXT_DISPLAY", "TextDispaly");
        adapter.TableMappings.Add(dtm);

        //Select Command
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText = "SELECT THICKNESS, TEXT_DISPLAY FROM VIZ_PRN.QMF_THICKNESS_CHR ORDER BY THICKNESS",
          CommandType = CommandType.Text
        };
      }

      public int LoadData()
      {
        return Odac.LoadDataTable(this, adapter, true, null);
      }
    }
  }
}