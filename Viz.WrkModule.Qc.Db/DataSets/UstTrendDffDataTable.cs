using System;
using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using Smv.Data.Oracle;

namespace Viz.WrkModule.Qc.Db.DataSets
{
  public sealed partial class DsQc
  {
    public class UstTrendDffDataTable : UstTrendQualityDataTable
    {
      public UstTrendDffDataTable(string tblName) : base(tblName)
      {
        //Select Command
        adapter.SelectCommand.CommandText = "SELECT TYPE_CLC, LOCNUM, NAMEGROUP, RATIO_STS FROM VIZ_PRN.V_QMF_UST_DFF WHERE TYPE_CLC = :PTYPE_CLC AND LOCNUM = :PLOCNUM";
      }

      public override int LoadData(string typeClc, string locNUm)
      {
        //вызывается что б отключить фильтрацию по группам
        DbApp.Psi.DbVar.SetNum(0,0);

        var lstPrmValue = new List<Object> { typeClc, locNUm };
        return Odac.LoadDataTable(this, adapter, true, lstPrmValue);
      }
    }


  }
}

