using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.MapDefects.Db.DataSets
{
  public sealed partial class DsMapDef
  {
    public sealed class CutMatDataTable : DataTable
    {
      private readonly OracleDataAdapter adapter;

      public CutMatDataTable() : base()
      {
        this.TableName = "CutMat";
        adapter = new OracleDataAdapter();

        var col = new DataColumn("MatChild", typeof(string), null, MappingType.Element) { AllowDBNull = false };
        this.Columns.Add(col);

        col = new DataColumn("XstartAncWgt", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("XendAncWgt", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("YstartAnc", typeof(int), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("YendAnc", typeof(int), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("WeightAnc", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Weight", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Sort", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Cat", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Def", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Status", typeof(string), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("Xpart", typeof(double), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("YstartChaild", typeof(int), null, MappingType.Element);
        this.Columns.Add(col);

        col = new DataColumn("YendChaild", typeof(int), null, MappingType.Element);
        this.Columns.Add(col);


        this.Constraints.Add(new UniqueConstraint("Pk_CutMat", new[] { this.Columns["MatChild"] }, true));
        adapter.TableMappings.Clear();
        var dtm = new System.Data.Common.DataTableMapping("VIZ_PRN.CUTMATG_AVO", "CutMat");
        dtm.ColumnMappings.Add("MAT_CHILD", "MatChild");
        dtm.ColumnMappings.Add("XSTARTANC_WGT", "XstartAncWgt");
        dtm.ColumnMappings.Add("XENDANC_WGT", "XendAncWgt");
        dtm.ColumnMappings.Add("YSTARTANC", "YstartAnc");
        dtm.ColumnMappings.Add("YENDANC", "YendAnc");
        dtm.ColumnMappings.Add("WEIGHTANC", "WeightAnc");
        dtm.ColumnMappings.Add("WEIGHT", "Weight");
        dtm.ColumnMappings.Add("SORT", "Sort");
        dtm.ColumnMappings.Add("CAT", "Cat");
        dtm.ColumnMappings.Add("DEF", "Def");
        dtm.ColumnMappings.Add("STATUS", "Status");
        dtm.ColumnMappings.Add("YSTARTCHILD", "YstartChaild");
        dtm.ColumnMappings.Add("YENDCHILD", "YendChaild");
        adapter.TableMappings.Add(dtm);
        adapter.SelectCommand = new OracleCommand
        {
          Connection = Odac.DbConnection,
          CommandText =
            "select MAT_CHILD, XSTARTANC_WGT, XENDANC_WGT, YSTARTANC, YENDANC, WEIGHTANC, WEIGHT, SORT, CAT, DEF, STATUS, XPART, YSTARTCHILD, YENDCHILD " +
            "from VIZ_PRN.CUTMATG_AVO",
          CommandType = CommandType.Text
        };
      }

      public int LoadData()
      {
        int rez = Odac.LoadDataTable(this, adapter, true, null);
        return rez;
      }
    }
  }
}