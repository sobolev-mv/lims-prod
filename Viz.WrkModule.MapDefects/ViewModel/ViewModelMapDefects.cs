using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using DevExpress.Xpf.Core.HandleDecorator;
using Smv.MVVM.Commands;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Data.Linq.Helpers;
using System.Windows.Documents;
using System.Security.Policy;

namespace Viz.WrkModule.MapDefects
{

  internal class UiRef
  {
    public int Id { get; set; }
    public string Name { get; set; }
  } 


  internal sealed class ViewModelMapDefects : Smv.MVVM.ViewModels.ViewModelBase
  {

    #region Fields
    private readonly UserControl usrControl;
    private string findLocNumText = null;
    private ObservableCollection<UiRef> coilType;
    private int selectedCoilType = 1;

    private readonly Canvas cnv1;
    private readonly Canvas cnv2;
    private readonly Canvas cnv3;
    private readonly Db.DataSets.DsMapDef dsMapDef = new Db.DataSets.DsMapDef();
    private decimal scaleY;
    private decimal scaleX;
 

    #endregion

    #region Public Property
    public string FindLocNumText
    {
      get { return findLocNumText; }
      set
      {
        if (value == findLocNumText) return;
        findLocNumText = value;
        base.OnPropertyChanged("FindLocNumText");
      }
    }

    public ObservableCollection<UiRef> CoilType
    {
      get { return coilType; }
      set{
        if (value == coilType) return;
        coilType = value;
        base.OnPropertyChanged("CoilType");
      }
    }

    public Int32 SelectedCoilType
    {
      get { return selectedCoilType; }
      set
      {
        if (value == selectedCoilType) return;
        selectedCoilType = value;
        base.OnPropertyChanged("SelectedCoilType");
      }
    }

    public decimal ScaleY
    {
      get { return scaleY; }
      set{
        if (value == scaleY) return;
        scaleY = value;
        base.OnPropertyChanged("ScaleY");
      }
    }

    public decimal ScaleX
    {
      get { return scaleX; }
      set
      {
        if (value == scaleX) return;
        scaleX = value;
        base.OnPropertyChanged("ScaleX");
      }
    }



    #endregion

    #region Private Method
    private void SetScaleY()
    {
      this.cnv1.LayoutTransform = new ScaleTransform(Convert.ToDouble(this.scaleX / 100), Convert.ToDouble(this.scaleY / 100)); 
    }

    private string GetLabelDefect(DataView dvDef)
    {
      string rez;
      //Делаем скобки с категорией
      if (Convert.ToString(dvDef[0].Row["Cat"]) == "б/к")
        rez = "(" + Convert.ToString(dvDef[0].Row["Cat"]) + "/"; 
      else
        rez = "(" + Convert.ToString(dvDef[0].Row["Cat"]) + "к/";

      rez += Convert.ToString(dvDef[0].Row["FehlerTyp"]) + "/" + ((Convert.ToDecimal(dvDef[0].Row["ZoneTo"]) - Convert.ToDecimal(dvDef[0].Row["ZoneFrom"])) / 1000).ToString(CultureInfo.InvariantCulture) + ")";

      for (int i = 1; i <= dvDef.Count - 1; i++)
        rez += "(" + Convert.ToString(dvDef[i].Row["FehlerTyp"]) + "/" + Math.Round(Convert.ToDouble(dvDef[i].Row["YPOSVON"]),0).ToString(CultureInfo.InvariantCulture) + "-" +
               Math.Round(Convert.ToDouble(dvDef[i].Row["YPOSBIS"]),0).ToString(CultureInfo.InvariantCulture) + ")";
      /*
      foreach (DataRowView drv in dvDef){
        //MessageBox.Show(Convert.ToString(drv.Row["Cat"]));
      }
      */
      return rez;
    }

    private void PaintCoilRuleForward(Canvas cnv, double Kx, int WgtUnit, double xMin, double xMax, double yMin, double WgtCoil, int nRnd)
    {
      Label lbl = null;
      double xRuleUnit = Math.Round(WgtUnit * Kx, nRnd);
      int RulePartQnt = Convert.ToInt32(WgtCoil / WgtUnit);
     
      for (int i = 1; i < RulePartQnt; i++){
        cnv.Children.Add(new Line
                         {
                           X1 = xMax - xRuleUnit * i,
                           Y1 = yMin - 7,
                           X2 = xMax - xRuleUnit * i,
                           Y2 = yMin,
                           Stroke = Brushes.Black,
                           StrokeThickness = 1
                         }
                        );

        lbl = new Label
        {
          Content = ((i * WgtUnit * 0.001)).ToString("n1"),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 11,
        };

        Canvas.SetLeft(lbl, (xMax - xRuleUnit * i) + 2);
        Canvas.SetTop(lbl, yMin - 17);
        cnv.Children.Add(lbl);
      }

      lbl = new Label
      {
        Content = (WgtCoil / 1000).ToString("n3"),
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 11,
      };
      Canvas.SetLeft(lbl, xMin);
      Canvas.SetTop(lbl, yMin - 17);
      cnv.Children.Add(lbl);
    }

    private void PaintCoilRuleBackward(double Kx, int WgtUnit, double xMin, double xMax, double yMin, double WgtCoil, int nRnd)
    {
      Label lbl = null;
      double xRuleUnit = Math.Round(WgtUnit * Kx, nRnd);
      int RulePartQnt = Convert.ToInt32(WgtCoil / WgtUnit);

      for (int i = 1; i < RulePartQnt; i++){
        cnv2.Children.Add(new Line
        {
          X1 = xMin + xRuleUnit * i,
          Y1 = yMin - 7,
          X2 = xMin + xRuleUnit * i,
          Y2 = yMin,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        }
                        );

        lbl = new Label
        {
          Content = (i * WgtUnit * 0.001).ToString("n1"),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 11,
        };
        Canvas.SetLeft(lbl, (xMin + xRuleUnit * i) - 24);
        Canvas.SetTop(lbl, yMin - 18);
        cnv2.Children.Add(lbl);
      }
       
      
      lbl = new Label
      {
        Content = (WgtCoil / 1000).ToString("n3"),
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 11,
      };
      Canvas.SetLeft(lbl, xMax - 26);
      Canvas.SetTop(lbl, yMin - 18);
      cnv2.Children.Add(lbl);
    }

    private Brush GetBrush(int id)
    {
      switch (id)
      {
        case 1:
          return Brushes.Blue;
        case 2:
          return Brushes.Red;
        case 3:
          return Brushes.Green;
        case 4:
          return Brushes.Gold;
        case 5:
          return Brushes.Fuchsia;
        case 6:
          return Brushes.Firebrick;
        case 7:
          return Brushes.DarkOrange;
        default:
          return Brushes.Black;
      }
    }


    private VisualBrush GetHatchBrush(int Id, double Height, double Width)
    {
      double mVal = Math.Min(Height, Width); 

      var vb = new VisualBrush
      {
        TileMode = TileMode.Tile,
        Viewport = new Rect(0, 0, mVal, mVal),
        ViewportUnits = BrushMappingMode.Absolute,
        Viewbox = new Rect(0, 0, mVal, mVal),
        ViewboxUnits = BrushMappingMode.Absolute,
      };

      var cnvVb = new Canvas();

      if ((Id % 2) == 0)
        cnvVb.Children.Add(new Path()
                               {
                                 Stroke = GetBrush(Id),//Brushes.Black,
                                 Data = new LineGeometry(new Point(0, 0), new Point(mVal, mVal))
                               }
                          );
      else
        cnvVb.Children.Add(new Path()
        {
          Stroke = GetBrush(Id),//Brushes.Black,
          Data = new LineGeometry(new Point(0, mVal), new Point(mVal, 0))
        }
                          );

      vb.Visual = cnvVb;
      return vb;
    }

    private void DrawHeaderPageAll(Canvas cnv, string locNum, double xMin, decimal coilThick, double coilWidth, double coilWgt, decimal coilLen, string coilBrg, string coilCntr)
    {
      var hlbl = new Label
      {
        Content = "Лок № PSI: " + findLocNumText,
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, xMin);
      Canvas.SetTop(hlbl, 25);
      cnv.Children.Add(hlbl);


      hlbl = new Label
      {
        Content = "ПОВЕРХНОСТЬ РУЛОНА С АВО",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 16,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 250);
      Canvas.SetTop(hlbl, 21);
      cnv.Children.Add(hlbl);
      
      hlbl = new Label
      {
        Content = "Дата обработки и агрегат: " + Db.MapDefectsAction.GetStrannDateTimeCoil(locNum),
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 600);
      Canvas.SetTop(hlbl, 25);
      cnv.Children.Add(hlbl);

      hlbl = new Label
      {
        Content = "Ст.прт: " + Db.MapDefectsAction.GetMapDefInfo(findLocNumText) + "  " +
                  "Толщ: " + coilThick.ToString("n2") + "мм    " + "Ширина: " + coilWidth.ToString("n0") + "мм  " +
                  "Масса: " + (coilWgt / 1000).ToString("n3") + "т" + "  " +
                  "Длина: " + (coilLen / 1000).ToString("n3") + "м" + "  " +
                  "Бр №" + coilBrg + "  " + "Контролер ОТК: " + coilCntr,
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, xMin);
      Canvas.SetTop(hlbl, 43);
      cnv.Children.Add(hlbl);
    }

    private void DrawHeaderAdd(Canvas cnv)
    {
      var hlbl = new Label
      {
        Content = "Дата__________________________АПР №_________Бригада №________Контролер ОТК__________________________________" +
                  "      Ширина______________________мм  Масса_________________тн",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 20);
      Canvas.SetTop(hlbl, 100);
      cnv.Children.Add(hlbl);
    }

    private int DrawCoilSurf1(Canvas cnv, double xMin, double xMax, double yMin, double yMax, double kx, double ky, int roundDgt, double yForward, double coilWgt, List<double> lstSf)
    {
      var plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin));
      plCoil.Points.Add(new Point(xMax, yMin));
      plCoil.Points.Add(new Point(xMax, yMax));
      plCoil.Points.Add(new Point(xMin, yMax));
      plCoil.Points.Add(new Point(xMin, yMin));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv.Children.Add(plCoil);

      //рисуем весовую линейку первой стороны 
      this.PaintCoilRuleForward(cnv, kx, 500, xMin, xMax, yMin, coilWgt, roundDgt);

      //здесь начинается сама отрисовка дефектов первой стороны  
      int zIdx = 1;
      double oldX = xMax;

      foreach (DataRow rowZone in this.dsMapDef.LstDefZones.Rows)
      {

        double zoneFrom = Convert.ToDouble(rowZone["ZoneFrom"]);
        double zoneTo = Convert.ToDouble(rowZone["ZoneTo"]);
        this.dsMapDef.MapDef.DefaultView.RowFilter = "ZoneFrom=" +
                                                     zoneFrom.ToString(
                                                       System.Globalization.CultureInfo.InvariantCulture) +
                                                     " AND ZoneTo=" + zoneTo.ToString(System.Globalization.CultureInfo.InvariantCulture);


        var line = new Line
        {
          X1 = xMax - Math.Round(zoneTo * kx, roundDgt),
          Y1 = yMin,
          X2 = xMax - Math.Round(zoneTo * kx, roundDgt),
          Y2 = yMax + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv.Children.Add(line);
        lstSf.Add(line.X1);

        var lbl = new Label
        {
          Content = zIdx.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    GetLabelDefect(this.dsMapDef.MapDef.DefaultView),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };
        Canvas.SetLeft(lbl, xMax - Math.Round(zoneTo * kx, roundDgt) + 1);
        Canvas.SetTop(lbl, yMax + yForward * zIdx - 13);
        cnv.Children.Add(lbl);

        int idBrush = 1;
        foreach (DataRowView drv in this.dsMapDef.MapDef.DefaultView)
        {
          string strCat = Convert.ToString(drv.Row["Cat"]);
          string fehlerTyp = Convert.ToString(drv.Row["FehlerTyp"]);
          int rid = Convert.ToInt32(drv.Row["Rid"]);

          //double yPos1 = Convert.ToDouble(this.dsMapDef.MapDef.DefaultView[0].Row["YposvOn"]);
          //double yPos2 = Convert.ToDouble(this.dsMapDef.MapDef.DefaultView[0].Row["YposbIs"]);

          if ((fehlerTyp == "034") || (fehlerTyp == "038") || (strCat == "3") || (strCat == "б/к") || (strCat == "5") || (strCat == "4"))
          {
            //Здесь рисуем поперечный основной дефект
            double yPos1 = Convert.ToDouble(drv.Row["YposvOn"]);
            double yPos2 = Convert.ToDouble(drv.Row["YposbIs"]);

            var rect = new Rectangle()
            {
              Height = Math.Round((yPos2 - yPos1) * ky, roundDgt),
              Width = oldX - (xMax - Math.Round(zoneTo * kx, roundDgt)),
              Fill = GetHatchBrush(idBrush, Math.Round((yPos2 - yPos1) * ky, roundDgt), oldX - (xMax - Math.Round(zoneTo * kx, roundDgt))),
              Stroke = this.GetBrush(idBrush),
              StrokeThickness = 1
            };
            Canvas.SetLeft(rect, xMax - Math.Round(zoneTo * kx, roundDgt));
            Canvas.SetTop(rect, yMax - Math.Round(yPos1 * ky, roundDgt) - Math.Round((yPos2 - yPos1) * ky, roundDgt));
            cnv.Children.Add(rect);
          }
          idBrush++;
        }
        
        oldX = xMax - Math.Round(zoneTo * kx, roundDgt);
        zIdx++;
        
      }

      //Делаем подпись начала
      var hlbl = new Label
      {
        Content = "Начало",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin);
      cnv.Children.Add(hlbl);

      return zIdx;
    }

    private void PaintCoilRuleForwardXAxis(Canvas cnv, double Kx, int wgtUnit, double xMin, double xMax, double yMin, double wgtCoil, int roundDgt)
    {
      Label lbl = null;
      double xRuleUnit = Math.Round(wgtUnit * Kx, roundDgt);
      int rulePartQnt = Convert.ToInt32(wgtCoil / wgtUnit);

      for (int i = 1; i < rulePartQnt; i++)
      {
        cnv.Children.Add(new Line
          {
            X1 = xMax - xRuleUnit * i,
            Y1 = yMin - 7,
            X2 = xMax - xRuleUnit * i,
            Y2 = yMin,
            Stroke = Brushes.Black,
            StrokeThickness = 1
          }
        );

        lbl = new Label
        {
          Content = ((i * wgtUnit * 0.001)).ToString("n1"),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 11,
        };

        Canvas.SetLeft(lbl, (xMax - xRuleUnit * i) + 2);
        Canvas.SetTop(lbl, yMin + 2);
        cnv.Children.Add(lbl);
      }

      lbl = new Label
      {
        Content = (wgtCoil / 1000).ToString("n3"),
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 11,
      };
      Canvas.SetLeft(lbl, xMin);
      Canvas.SetTop(lbl, yMin + 2);
      cnv.Children.Add(lbl);
    }

    private void PaintCoilRuleYAxis(Canvas cnv, int yAxisUnit, double xMin, double xMax, double yMin, double yMax, int roundDgt)
    {
      var dashes = new DoubleCollection
      { 2, 2 };
      
      int rulePartQnt = Convert.ToInt32(120 / yAxisUnit);
      double yUnit = Math.Round((yMax - yMin) / rulePartQnt, roundDgt);
      
      for (int i = 1; i <= rulePartQnt; i++)
      {
        cnv.Children.Add(new Line
          {
            X1 = xMin,
            Y1 = yMax - yUnit * i,
            X2 = xMin + 7,
            Y2 = yMax - yUnit * i,
            Stroke = Brushes.Black,
            StrokeThickness = 1
          }
        );

        cnv.Children.Add(new Line
          {
            X1 = xMin + 8,
            Y1 = yMax - yUnit * i,
            X2 = xMax,
            Y2 = yMax - yUnit * i,
            Stroke = Brushes.LightGray,
            StrokeThickness = 1,
            StrokeDashArray = dashes
        }

        );

        var lbl = new Label
        {
          Content = ((i * yAxisUnit).ToString("d")),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 11,
        };

        Canvas.SetLeft(lbl, xMin + 2);
        Canvas.SetTop(lbl, yMax - yUnit * i);
        cnv.Children.Add(lbl);
      }

    }

    private void BuildMapDef()
    {
      if (Db.MapDefectsAction.IsMatLocked(findLocNumText))
      {
        Smv.Utils.DxInfo.ShowDxBoxInfo("Материал", "Показ дефектов невозможен!", MessageBoxImage.Error);
        return;
      }

      //Списки для запоминания координаты X дефекта каждой из поверхностей
      var lstSf1 = new List<double> { };
      var lstSf2 = new List<double> { };
      var lstSf3 = new List<double> { };

      var rm = new Random();
      Int64 zdn = rm.Next(10000000, 99999999);

      //Получаем локальный номер рельный в случае отмоток на котором будут дефекты
      string realLocId = Db.MapDefectsAction.GetRealLocNumStrann(findLocNumText);

      if (string.IsNullOrEmpty(realLocId))
        realLocId = findLocNumText;

      //Формируем данные в таблице VIZ_PRN.OTK_DEF
      Db.MapDefectsAction.CreateDefectsData(-zdn, findLocNumText, true);

      this.dsMapDef.MapDef.LoadData(-zdn, 1, 3);
      this.dsMapDef.LstDefZones.LoadData(-zdn, 1, 3);

      ScaleY = 100;
      ScaleX = 100;

      //получаем полную массу рулона в кг. 
      double coilWgt = Convert.ToDouble(Db.MapDefectsAction.GetCoilWgt(realLocId, "STRANN"));
      //получаем полную ширину рулона в мм. 
      double coilWidth = Convert.ToDouble(Db.MapDefectsAction.GetCoilWidth(realLocId, "STRANN"));

      cnv1.Children.Clear();
      cnv2.Children.Clear();
      cnv3.Children.Clear();
      cnv1.LayoutTransform = null;

      //Здесь проверяем широкий ли это монитор
      double screenWidth = cnv1.ActualWidth;

      if (cnv1.ActualWidth > 1280)
        screenWidth = 1280;

      const int roundDgt = 6; //кол-во знаков после зяпятой при округлении
      const double xMin = 20;
      const double yMin = 90;
      double xMax = Math.Round(screenWidth - screenWidth / 4, roundDgt);
      const double yMax = 190;
      const double yForward = 15; //высота на которую увеличивется растояние по y для описания дефектов
      double kx = Math.Round((xMax - xMin) / coilWgt, roundDgt); //масштабирование
      double ky = Math.Round((yMax - yMin) / coilWidth, roundDgt); //масштабирование

      decimal coilThick = Db.MapDefectsAction.GetTolsCoil(realLocId, "STRANN");
      decimal coilLen = Db.MapDefectsAction.GetLenCoil(realLocId, "STRANN");
      string coilBrg = Db.MapDefectsAction.GetBrigada(realLocId);
      string coilCntr = Db.MapDefectsAction.GetController(realLocId);

      //Здесь рисуем заголовок с необходимыми параметрами рулона на странице 1
      DrawHeaderPageAll(cnv1, realLocId, xMin, coilThick, coilWidth, coilWgt, coilLen, coilBrg, coilCntr);

      //Здесь рисуем заголовок с необходимыми параметрами рулона на странице 2
      DrawHeaderPageAll(cnv2, realLocId, xMin, coilThick, coilWidth, coilWgt, coilLen, coilBrg, coilCntr);
      DrawHeaderAdd(cnv2);

      //Здесь рисуем заголовок с необходимыми параметрами рулона на странице 3
      DrawHeaderPageAll(cnv3, realLocId, xMin, coilThick, coilWidth, coilWgt, coilLen, coilBrg, coilCntr);
      DrawHeaderAdd(cnv3);

      //рисуем первую сторону рулона
      int zIdx = DrawCoilSurf1(cnv1, xMin, xMax, yMin, yMax, kx, ky, roundDgt, yForward, coilWgt, lstSf1);
      int zIdx3 = DrawCoilSurf1(cnv3, xMin, xMax, 140, 140 + (yMax - yMin), kx, ky, roundDgt, yForward, coilWgt,
        lstSf3);

      /*
      var plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin));
      plCoil.Points.Add(new Point(xMax, yMin));
      plCoil.Points.Add(new Point(xMax, yMax));
      plCoil.Points.Add(new Point(xMin, yMax));
      plCoil.Points.Add(new Point(xMin, yMin));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv1.Children.Add(plCoil);

      //рисуем весовую линейку первой стороны 
      this.PaintCoilRuleForward(this.cnv1, kx, 500, xMin, xMax, yMin, coilWgt, roundDgt);

      //здесь начинается сама отрисовка дефектов первой стороны  
      int zIdx = 1;
      double oldX = xMax;

      foreach (DataRow rowZone in this.dsMapDef.LstDefZones.Rows)
      {

        double zoneFrom = Convert.ToDouble(rowZone["ZoneFrom"]);
        double zoneTo = Convert.ToDouble(rowZone["ZoneTo"]);
        this.dsMapDef.MapDef.DefaultView.RowFilter = "ZoneFrom=" +
                                                     zoneFrom.ToString(
                                                       System.Globalization.CultureInfo.InvariantCulture) +
                                                     " AND ZoneTo=" + zoneTo.ToString(System.Globalization.CultureInfo
                                                       .InvariantCulture);


        var line = new Line
        {
          X1 = xMax - Math.Round(zoneTo * kx, roundDgt),
          Y1 = yMin,
          X2 = xMax - Math.Round(zoneTo * kx, roundDgt),
          Y2 = yMax + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv1.Children.Add(line);
        lstSf1.Add(line.X1);

        var lbl = new Label
        {
          Content = zIdx.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    GetLabelDefect(this.dsMapDef.MapDef.DefaultView),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };
        Canvas.SetLeft(lbl, xMax - Math.Round(zoneTo * kx, roundDgt) + 1);
        Canvas.SetTop(lbl, yMax + yForward * zIdx - 13);
        cnv1.Children.Add(lbl);

        int idBrush = 1;
        foreach (DataRowView drv in this.dsMapDef.MapDef.DefaultView)
        {
          string strCat = Convert.ToString(drv.Row["Cat"]);
          string fehlerTyp = Convert.ToString(drv.Row["FehlerTyp"]);
          int rid = Convert.ToInt32(drv.Row["Rid"]);

          //double yPos1 = Convert.ToDouble(this.dsMapDef.MapDef.DefaultView[0].Row["YposvOn"]);
          //double yPos2 = Convert.ToDouble(this.dsMapDef.MapDef.DefaultView[0].Row["YposbIs"]);


          if ((fehlerTyp == "034") || (fehlerTyp == "038") || (strCat == "3") || (strCat == "б/к") || (strCat == "5") ||
              (strCat == "4"))
          {
            //Здесь рисуем поперечный основной дефект
            double yPos1 = Convert.ToDouble(drv.Row["YposvOn"]);
            double yPos2 = Convert.ToDouble(drv.Row["YposbIs"]);

            var rect = new Rectangle()
            {
              Height = Math.Round((yPos2 - yPos1) * ky, roundDgt),
              Width = oldX - (xMax - Math.Round(zoneTo * kx, roundDgt)),
              Fill = GetHatchBrush(idBrush, Math.Round((yPos2 - yPos1) * ky, roundDgt),
                oldX - (xMax - Math.Round(zoneTo * kx, roundDgt))),
              Stroke = this.GetBrush(idBrush),
              StrokeThickness = 1
            };
            Canvas.SetLeft(rect, xMax - Math.Round(zoneTo * kx, roundDgt));
            Canvas.SetTop(rect, yMax - Math.Round(yPos1 * ky, roundDgt) - Math.Round((yPos2 - yPos1) * ky, roundDgt));
            cnv1.Children.Add(rect);
          }

          idBrush++;
        }


        oldX = xMax - Math.Round(zoneTo * kx, roundDgt);
        zIdx++;

      }

      //Делаем подпись начала
      var hlbl = new Label
      {
        Content = "Начало",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin);
      cnv1.Children.Add(hlbl);
      */

      //Здесь начинаем рисовать вторую сторону рулона
      //zdn = rm.Next(10000000, 99999999);
      this.dsMapDef.MapDef.LoadData(-zdn, 2, 3);
      this.dsMapDef.LstDefZones.LoadData(-zdn, 2, 3);

      //Очищаем таблицу VIZ_PRN.OTK_DEF
      Db.MapDefectsAction.DeleteDefectsData(-zdn);

      //Определяем yMin для второй стороны рулона
      double yMin2 = yMax + yForward * zIdx + 10;
      double yMax2 = yMin2 + (yMax - yMin);

      DrawCoilSurf1(cnv1, xMin, xMax, yMin2, yMax2, kx, ky, roundDgt, yForward, coilWgt, lstSf2);

      /*
      var plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin2));
      plCoil.Points.Add(new Point(xMax, yMin2));
      plCoil.Points.Add(new Point(xMax, yMax2));
      plCoil.Points.Add(new Point(xMin, yMax2));
      plCoil.Points.Add(new Point(xMin, yMin2));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv1.Children.Add(plCoil);

      //рисуем весовую линейку второй стороны 
      this.PaintCoilRuleForward(this.cnv1, kx, 500, xMin, xMax, yMin2, coilWgt, roundDgt);

      double oldX = xMax;
      zIdx = 1; //сбрасываем

      foreach (DataRow rowZone in this.dsMapDef.LstDefZones.Rows)
      {

        double zoneFrom = Convert.ToDouble(rowZone["ZoneFrom"]);
        double zoneTo = Convert.ToDouble(rowZone["ZoneTo"]);
        this.dsMapDef.MapDef.DefaultView.RowFilter = "ZoneFrom=" +
                                                     zoneFrom.ToString(
                                                       System.Globalization.CultureInfo.InvariantCulture) +
                                                     " AND ZoneTo=" + zoneTo.ToString(System.Globalization.CultureInfo.InvariantCulture);

        var line = new Line
        {
          X1 = xMax - Math.Round(zoneTo * kx, roundDgt),
          Y1 = yMin2,
          X2 = xMax - Math.Round(zoneTo * kx, roundDgt),
          Y2 = yMax2 + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv1.Children.Add(line);
        lstSf2.Add(line.X1);

        var lbl = new Label
        {
          Content = zIdx.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    GetLabelDefect(this.dsMapDef.MapDef.DefaultView),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };

        Canvas.SetLeft(lbl, xMax - Math.Round(zoneTo * kx, roundDgt) + 1);
        Canvas.SetTop(lbl, yMax2 + yForward * zIdx - 13);
        cnv1.Children.Add(lbl);

        int idBrush = 1;
        foreach (DataRowView drv in this.dsMapDef.MapDef.DefaultView)
        {
          string strCat = Convert.ToString(drv.Row["Cat"]);
          string fehlerTyp = Convert.ToString(drv.Row["FehlerTyp"]);
          int rid = Convert.ToInt32(drv.Row["Rid"]);

          if ((fehlerTyp == "034") || (fehlerTyp == "038") || (strCat == "3") || (strCat == "б/к") || (strCat == "5") ||
              (strCat == "4"))
          {

            //Здесь рисуем поперечный основной дефект
            double yPos1 = Convert.ToDouble(drv.Row["YposvOn"]);
            double yPos2 = Convert.ToDouble(drv.Row["YposbIs"]);

            var rect = new Rectangle()
            {
              Height = Math.Round((yPos2 - yPos1) * ky, roundDgt),
              Width = oldX - (xMax - Math.Round(zoneTo * kx, roundDgt)),
              Fill = GetHatchBrush(idBrush, Math.Round((yPos2 - yPos1) * ky, roundDgt),
                oldX - (xMax - Math.Round(zoneTo * kx, roundDgt))),
              Stroke = this.GetBrush(idBrush),
              StrokeThickness = 1
            };
            Canvas.SetLeft(rect, xMax - Math.Round(zoneTo * kx, roundDgt));
            //Canvas.SetTop(rect, yMin2 + Math.Round(yPos1*ky, nrnd));
            Canvas.SetTop(rect, yMax2 - Math.Round(yPos1 * ky, roundDgt) - Math.Round((yPos2 - yPos1) * ky, roundDgt));
            cnv1.Children.Add(rect);
          }

          idBrush++;
        }

        oldX = xMax - Math.Round(zoneTo * kx, roundDgt);
        zIdx++;
      }

      //Делаем подпись начала
      var hlbl = new Label
      {
        Content = "Начало",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin2);
      cnv1.Children.Add(hlbl);
      */


      //Страница 2, Определяем yMin для раскроечного рулона

      double yMin3 = 140;
      double yMax3 = yMin3 + (yMax - yMin);

      var plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin3));
      plCoil.Points.Add(new Point(xMax, yMin3));
      plCoil.Points.Add(new Point(xMax, yMax3));
      plCoil.Points.Add(new Point(xMin, yMax3));
      plCoil.Points.Add(new Point(xMin, yMin3));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv2.Children.Add(plCoil);

      //рисуем весовую линейку для раскроечного рулона 
      this.PaintCoilRuleBackward(kx, 500, xMin, xMax, yMin3, coilWgt, roundDgt);

      //рисуем пунктиром дефектные зоны для раскроечного рулона
      foreach (double t in lstSf1)
      {
        var line = new Line
        {
          X1 = t,
          Y1 = yMin3,
          X2 = t,
          Y2 = yMax3,
          Stroke = Brushes.Black,
          StrokeThickness = 1,
          StrokeDashArray = DoubleCollection.Parse("5, 3")
        };
        cnv2.Children.Add(line);
      }

      foreach (double t in lstSf2)
      {
        var line = new Line
        {
          X1 = t,
          Y1 = yMin3,
          X2 = t,
          Y2 = yMax3,
          Stroke = Brushes.Black,
          StrokeThickness = 1,
          StrokeDashArray = DoubleCollection.Parse("5, 3")
        };
        cnv2.Children.Add(line);
      }

      //Делаем подпись конец
      var hlbl = new Label
      {
        Content = "Конец",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin3);
      cnv2.Children.Add(hlbl);

      /*********************************************************Для Сажина RR-1377***********************************************************************************/
      /***************************************************Отрисовка: "Порезанный рулон с АВО"************************************************************************/
      double yMin4 = yMax3 + 30;
      double yMax4 = yMin4 + (yMax - yMin);

      plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin4));
      plCoil.Points.Add(new Point(xMax, yMin4));
      plCoil.Points.Add(new Point(xMax, yMax4));
      plCoil.Points.Add(new Point(xMin, yMax4));
      plCoil.Points.Add(new Point(xMin, yMin4));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv2.Children.Add(plCoil);

      //рисуем весовую линейку второй стороны 
      this.PaintCoilRuleForward(cnv2, kx, 500, xMin, xMax, yMin4, coilWgt, roundDgt);

      //Делаем подпись "Начало"
      hlbl = new Label
      {
        Content = "Начало",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin4);
      cnv2.Children.Add(hlbl);

      Db.MapDefectsAction.CreateCutMatData(findLocNumText);
      this.dsMapDef.CutMat.LoadData();

      double oldX = xMax;
      zIdx = 1; //сбрасываем
      int idBrush2 = 1; //сбрасываем
      double xLen = xMax - xMin;

      foreach (DataRow rowCutMat in this.dsMapDef.CutMat.Rows)
      {
        double xStartAncWgt = Convert.ToDouble(rowCutMat["XstartAncWgt"]);
        double xEndAncWgt = Convert.ToDouble(rowCutMat["XendAncWgt"]);
        double yStartAnc = Convert.ToDouble(rowCutMat["YstartAnc"]);
        double yEndAnc = Convert.ToDouble(rowCutMat["YendAnc"]);
        double yStartChaild = Convert.ToDouble(rowCutMat["YstartChaild"]);
        double yEndChaild = Convert.ToDouble(rowCutMat["YendChaild"]);
        double xPart = Convert.ToDouble(rowCutMat["Xpart"]);


        string strInfo = "[" + Convert.ToString(rowCutMat["MatChild"]) + "]"
                         + ", Сорт: " + Convert.ToString(rowCutMat["Sort"])
                         + ", Категория: " + Convert.ToString(rowCutMat["Cat"])
                         + ", Дефект: " + Convert.ToString(rowCutMat["Def"])
                         + ", Масса: " + Math.Round((Convert.ToDouble(rowCutMat["Weight"]) / 1000), 3)
                           .ToString(CultureInfo.InvariantCulture) + "т"
                         + ", Ширина: " + (yEndChaild - yStartChaild).ToString(CultureInfo.InvariantCulture) + "мм"
                         + ", Статус: " + Convert.ToString(rowCutMat["Status"]);

        /* Первый способ
        var line = new Line
        {
          X1 = xMax - Math.Round(xEndAncWgt * kx, nrnd),
          Y1 = yMin4,
          X2 = xMax - Math.Round(xEndAncWgt * kx, nrnd),
          Y2 = yMax4 + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv2.Children.Add(line);
        
        var lbl = new Label
        {
          Content = zIdx.ToString(System.Globalization.CultureInfo.InvariantCulture) + ": " + strInfo,
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };

        Canvas.SetLeft(lbl, xMax - Math.Round(xEndAncWgt * kx, nrnd) + 1);
        Canvas.SetTop(lbl, yMax4 + yForward * zIdx - 13);
        cnv2.Children.Add(lbl);

        
        var rect = new Rectangle()
        {
          Height = Math.Round((yEndAnc - yStartAnc) * ky, nrnd),
          Width = Math.Round((xEndAncWgt - xStartAncWgt) * kx, nrnd),
          Fill = GetHatchBrush(idBrush2, Math.Round((yEndAnc - yStartAnc) * ky, nrnd), Math.Round((xEndAncWgt - xStartAncWgt) * kx, nrnd)),
          Stroke = this.GetBrush(idBrush2),
          StrokeThickness = 1
        };
        
        Canvas.SetLeft(rect, xMax - Math.Round(xEndAncWgt * kx, nrnd));
        Canvas.SetTop(rect, yMax4 - Math.Round(yStartAnc * ky, nrnd) - Math.Round((yEndAnc - yStartAnc) * ky, nrnd));
        cnv2.Children.Add(rect);
        */

        var line = new Line
        {
          X1 = Math.Round(oldX - xLen * xPart, roundDgt),
          Y1 = yMin4,
          X2 = Math.Round(oldX - xLen * xPart, roundDgt),
          Y2 = yMax4 + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv2.Children.Add(line);

        var lbl = new Label
        {
          Content = zIdx.ToString(System.Globalization.CultureInfo.InvariantCulture) + ": " + strInfo,
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };
        Canvas.SetLeft(lbl, Math.Round(oldX - xLen * xPart, roundDgt) + 1);
        Canvas.SetTop(lbl, yMax4 + yForward * zIdx - 13);
        cnv2.Children.Add(lbl);

        var rect = new Rectangle()
        {
          Height = Math.Round((yEndChaild - yStartChaild) * ky, roundDgt),
          Width = Math.Round(xLen * xPart, roundDgt),
          Fill = GetHatchBrush(idBrush2, Math.Round((yEndAnc - yStartAnc) * ky, roundDgt),
            Math.Round(xLen * xPart, roundDgt)),
          Stroke = this.GetBrush(idBrush2),
          StrokeThickness = 1
        };

        Canvas.SetLeft(rect, Math.Round(oldX - xLen * xPart, roundDgt));
        Canvas.SetTop(rect,
          yMax4 - Math.Round(yStartAnc * ky, roundDgt) - Math.Round((yEndChaild - yStartChaild) * ky, roundDgt));
        cnv2.Children.Add(rect);

        idBrush2++;
        oldX = Math.Round(oldX - xLen * xPart, roundDgt);
        zIdx++;
      }

      /**************************Строим график текстуры на Странице 3********************************************************/
      double yAxisMax = (140 + (yMax - yMin)) + zIdx3 * yForward + 10; //Конец оси Y
      double yAxisMin = yAxisMax + 150; //Начало (0) оси Y
      double kAxisY = Math.Round((yAxisMin - yAxisMax) / 120, roundDgt); //масштабирование

      plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yAxisMax));
      plCoil.Points.Add(new Point(xMin, yAxisMin));
      plCoil.Points.Add(new Point(xMax, yAxisMin));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv3.Children.Add(plCoil);

      PaintCoilRuleForwardXAxis(cnv3, kx, 500, xMin, xMax, yAxisMin, coilWgt, roundDgt);
      PaintCoilRuleYAxis(cnv3, 20, xMin, xMax, yAxisMax, yAxisMin, roundDgt);

      string agrAvo = Db.MapDefectsAction.GetStrannAgrCoil(realLocId);
      this.dsMapDef.Trend.LoadTextureData(Convert.ToDouble(coilLen), coilWidth, Convert.ToDouble(coilThick), realLocId, agrAvo);

      plCoil = new Polyline
      {
        Stroke = Brushes.Blue,
        StrokeThickness = 2
      };
     
      foreach (DataRow rowTrend in this.dsMapDef.Trend.Rows)
      {
        if (Convert.ToDouble(rowTrend[0]) > coilWgt)
          continue;

        plCoil.Points.Add(new Point(xMax - Math.Round(Convert.ToDouble(rowTrend[0]) * kx, roundDgt), yAxisMin - Math.Round(Convert.ToDouble(rowTrend[1]) * kAxisY, roundDgt)));
      }
      cnv3.Children.Add(plCoil);

    }

    private void BuildMapDefUo()
    {
      //Списки для запоминания координаты X дефекта каждой из поверхностей
      var lstSf1 = new List<double> { };
      var lstSf2 = new List<double> { };

      var rm = new Random();
      Int64 zdn = rm.Next(10000000, 99999999);
      //Int64 zdn = 1;

      Db.MapDefectsAction.CreateDefectsData(-zdn, findLocNumText, false); 
      this.dsMapDef.MapDef.LoadDataPack(-zdn, 1, 3);
      this.dsMapDef.LstDefZones.LoadData(-zdn, 1, 3);

      ScaleY = 100;
      ScaleX = 100;

      //получаем полную массу рулона в кг. 
      double coilWgt = Convert.ToDouble(Db.MapDefectsAction.GetCoilWgtUo(findLocNumText));
      //получаем полную ширину рулона в мм. 
      double coilWidth = Convert.ToDouble(Db.MapDefectsAction.GetCoilWidthUo(findLocNumText));
      //получаем % второго сорта от веса рулона. 
      decimal? k2Ssurf = Db.MapDefectsAction.GetK2sUo(findLocNumText);
      
      cnv1.Children.Clear();
      cnv2.Children.Clear();
      cnv1.LayoutTransform = null;

      //Здесь проверяем широкий ли это монитор
      double sreenWidth = cnv1.ActualWidth;
      if (cnv1.ActualWidth > 1280)
        sreenWidth = 1280;

      const int nrnd = 6;  //кол-во знаков после зяпятой при округлении
      const double xMin = 20;
      const double yMin = 90;
      double xMax = Math.Round(sreenWidth - sreenWidth / 4, nrnd);
      const double yMax = 190;
      const double yForward = 15; //высота на которую увеличивется растояние по y для описания дефектов
      double kx = Math.Round((xMax - xMin) / coilWgt, nrnd);//масштабирование
      double ky = Math.Round((yMax - yMin) / coilWidth, nrnd);//масштабирование

      //Здесь рисуем заголовок с необходимыми параметрами рулона на странице 1
      var hlbl = new Label
      {
        Content = "№ места: " + Db.MapDefectsAction.GetPlaceNumUo(findLocNumText) + "  К2с: " + Convert.ToInt32(k2Ssurf).ToString() + "%",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, xMin);
      Canvas.SetTop(hlbl, 25);
      cnv1.Children.Add(hlbl);
      
      hlbl = new Label
      {
        Content = "ПОВЕРХНОСТЬ СДАТОЧНОГО РУЛОНА УО",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 16,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 250);
      Canvas.SetTop(hlbl, 21);
      cnv1.Children.Add(hlbl);


      hlbl = new Label
      {
        Content = "Дата обработки и агрегат: " + Db.MapDefectsAction.GetDateTimeCoilUo(findLocNumText),
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 600);
      Canvas.SetTop(hlbl, 25);
      cnv1.Children.Add(hlbl);


      decimal tols = Db.MapDefectsAction.GetTolsCoilUo(findLocNumText);
      decimal lenCoil = Db.MapDefectsAction.GetLenCoilUo(findLocNumText);
      string brg = Db.MapDefectsAction.GetBrigadaUo(findLocNumText);
      string cntrl = Db.MapDefectsAction.GetControllerUo(findLocNumText);

      hlbl = new Label
      {
        Content = "Ст.прт: " + Db.MapDefectsAction.GetAnLot(findLocNumText) + "/" + findLocNumText + "  " +
                  "Толщ: " + tols.ToString("n2") + "мм    " + "Ширина: " + coilWidth.ToString("n0") + "мм  " +
                  "Масса: " + (coilWgt / 1000).ToString("n3") + "т" + "  " +
                  "Длина: " + (lenCoil / 1000).ToString("n3") + "м" + "  " +
                  "Бр №" + brg + "  " +
                  "Контролер ОТК: " + cntrl,
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, xMin);
      Canvas.SetTop(hlbl, 43);
      cnv1.Children.Add(hlbl);

      //Здесь рисуем заголовок с необходимыми параметрами рулона на странице 2
      hlbl = new Label
      {
        Content = "№ места: " + Db.MapDefectsAction.GetPlaceNumUo(findLocNumText) + "  К2с: " + Convert.ToInt32(k2Ssurf).ToString() + "%",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, xMin);
      Canvas.SetTop(hlbl, 25);
      cnv2.Children.Add(hlbl);

      hlbl = new Label
      {
        Content = "ПОВЕРХНОСТЬ СДАТОЧНОГО РУЛОНА УО",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 16,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 250);
      Canvas.SetTop(hlbl, 21);
      cnv2.Children.Add(hlbl);

      hlbl = new Label
      {
        Content = "Дата обработки и агрегат: " + Db.MapDefectsAction.GetDateTimeCoilUo(findLocNumText),
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 600);
      Canvas.SetTop(hlbl, 25);
      cnv2.Children.Add(hlbl);

      hlbl = new Label
      {
        Content = "Ст.прт: " + Db.MapDefectsAction.GetAnLot(findLocNumText) + "/" + findLocNumText + "  " +
                  "Толщ: " + tols.ToString("n2") + "мм    " + "Ширина: " + coilWidth.ToString("n0") + "мм  " +
                  "Масса: " + (coilWgt / 1000).ToString("n3") + "т" + "  " +
                  "Длина: " + (lenCoil / 1000).ToString("n3") + "м" + "  " +
                  "Бр №" + brg + "  " +
                  "Кнтр ОТК: " + cntrl,
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, xMin);
      Canvas.SetTop(hlbl, 43);
      cnv2.Children.Add(hlbl);

      //рисуем первую сторону рулона
      var plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin));
      plCoil.Points.Add(new Point(xMax, yMin));
      plCoil.Points.Add(new Point(xMax, yMax));
      plCoil.Points.Add(new Point(xMin, yMax));
      plCoil.Points.Add(new Point(xMin, yMin));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv1.Children.Add(plCoil);

      //рисуем весовую линейку первой стороны 
      this.PaintCoilRuleForward(this.cnv1, kx, 500, xMin, xMax, yMin, coilWgt, nrnd);

      //здесь начинается сама отрисовка дефектов первой стороны  
      int zIdx = 1;
      double oldX = xMax;

      foreach (DataRow rowZone in this.dsMapDef.LstDefZones.Rows)
      {

        double zoneFrom = Convert.ToDouble(rowZone["ZoneFrom"]);
        double zoneTo = Convert.ToDouble(rowZone["ZoneTo"]);
        this.dsMapDef.MapDef.DefaultView.RowFilter = "ZoneFrom=" + zoneFrom.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND ZoneTo=" + zoneTo.ToString(System.Globalization.CultureInfo.InvariantCulture);

        var line = new Line
        {
          X1 = xMax - Math.Round(zoneTo * kx, nrnd),
          Y1 = yMin,
          X2 = xMax - Math.Round(zoneTo * kx, nrnd),
          Y2 = yMax + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv1.Children.Add(line);
        lstSf1.Add(line.X1);

        var lbl = new Label
        {
          Content = zIdx.ToString(System.Globalization.CultureInfo.InvariantCulture) + GetLabelDefect(this.dsMapDef.MapDef.DefaultView),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };
        Canvas.SetLeft(lbl, xMax - Math.Round(zoneTo * kx, nrnd) + 1);
        Canvas.SetTop(lbl, yMax + yForward * zIdx - 13);
        cnv1.Children.Add(lbl);

        int idBrush = 1;
        foreach (DataRowView drv in this.dsMapDef.MapDef.DefaultView)
        {
          string strCat = Convert.ToString(drv.Row["Cat"]);
          //int rid = Convert.ToInt32(drv.Row["Rid"]);

          //double yPos1 = Convert.ToDouble(this.dsMapDef.MapDef.DefaultView[0].Row["YposvOn"]);
          //double yPos2 = Convert.ToDouble(this.dsMapDef.MapDef.DefaultView[0].Row["YposbIs"]);


          if ((strCat == "3") || (strCat == "б/к") || (strCat == "4")){
            //Здесь рисуем поперечный основной дефект
            double yPos1 = Convert.ToDouble(drv.Row["YposvOn"]);
            double yPos2 = Convert.ToDouble(drv.Row["YposbIs"]);

            var rect = new Rectangle()
            {
              Height = Math.Round((yPos2 - yPos1) * ky, nrnd),
              Width = oldX - (xMax - Math.Round(zoneTo * kx, nrnd)),
              Fill = GetHatchBrush(idBrush, Math.Round((yPos2 - yPos1) * ky, nrnd), oldX - (xMax - Math.Round(zoneTo * kx, nrnd))),
              Stroke = this.GetBrush(idBrush),
              StrokeThickness = 1
            };
            Canvas.SetLeft(rect, xMax - Math.Round(zoneTo * kx, nrnd));
            Canvas.SetTop(rect, yMax - Math.Round(yPos1 * ky, nrnd) - Math.Round((yPos2 - yPos1) * ky, nrnd));
            cnv1.Children.Add(rect);
          }

          idBrush++;
        }


        oldX = xMax - Math.Round(zoneTo * kx, nrnd);
        zIdx++;

      }

      //Делаем подпись начала
      hlbl = new Label
      {
        Content = "Начало",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin);
      cnv1.Children.Add(hlbl);

      //Здесь начинаем рисовать вторую сторону рулона
      this.dsMapDef.MapDef.LoadDataPack(-zdn, 2, 3);
      this.dsMapDef.LstDefZones.LoadData(-zdn, 2, 3);
      Db.MapDefectsAction.DeleteDefectsData(-zdn);

      //Определяем yMin для второй стороны рулона
      double yMin2 = yMax + yForward * zIdx + 10;
      double yMax2 = yMin2 + (yMax - yMin);

      plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin2));
      plCoil.Points.Add(new Point(xMax, yMin2));
      plCoil.Points.Add(new Point(xMax, yMax2));
      plCoil.Points.Add(new Point(xMin, yMax2));
      plCoil.Points.Add(new Point(xMin, yMin2));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv1.Children.Add(plCoil);

      //рисуем весовую линейку второй стороны 
      this.PaintCoilRuleForward(this.cnv1, kx, 500, xMin, xMax, yMin2, coilWgt, nrnd);

      oldX = xMax;
      zIdx = 1; //сбрасываем
      foreach (DataRow rowZone in this.dsMapDef.LstDefZones.Rows){

        double zoneFrom = Convert.ToDouble(rowZone["ZoneFrom"]);
        double zoneTo = Convert.ToDouble(rowZone["ZoneTo"]);
        this.dsMapDef.MapDef.DefaultView.RowFilter = "ZoneFrom=" + zoneFrom.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND ZoneTo=" + zoneTo.ToString(System.Globalization.CultureInfo.InvariantCulture);

        var line = new Line
        {
          X1 = xMax - Math.Round(zoneTo * kx, nrnd),
          Y1 = yMin2,
          X2 = xMax - Math.Round(zoneTo * kx, nrnd),
          Y2 = yMax2 + yForward * zIdx,
          Stroke = Brushes.Black,
          StrokeThickness = 1
        };
        cnv1.Children.Add(line);
        lstSf2.Add(line.X1);

        var lbl = new Label
        {
          Content = zIdx.ToString() + GetLabelDefect(this.dsMapDef.MapDef.DefaultView),
          Foreground = Brushes.Black,
          FontFamily = new FontFamily("Arial"),
          FontSize = 10,
          FontWeight = FontWeights.Bold
        };

        Canvas.SetLeft(lbl, xMax - Math.Round(zoneTo * kx, nrnd) + 1);
        Canvas.SetTop(lbl, yMax2 + yForward * zIdx - 13);
        cnv1.Children.Add(lbl);

        int idBrush = 1;
        foreach (DataRowView drv in this.dsMapDef.MapDef.DefaultView){
          string strCat = Convert.ToString(drv.Row["Cat"]);
          int rid = Convert.ToInt32(drv.Row["Rid"]);

          if ((strCat == "3") || (strCat == "б/к") || (strCat == "4")){

            //Здесь рисуем поперечный основной дефект
            double yPos1 = Convert.ToDouble(drv.Row["YposvOn"]);
            double yPos2 = Convert.ToDouble(drv.Row["YposbIs"]);

            var rect = new Rectangle()
            {
              Height = Math.Round((yPos2 - yPos1) * ky, nrnd),
              Width = oldX - (xMax - Math.Round(zoneTo * kx, nrnd)),
              Fill = GetHatchBrush(idBrush, Math.Round((yPos2 - yPos1) * ky, nrnd), oldX - (xMax - Math.Round(zoneTo * kx, nrnd))),
              Stroke = this.GetBrush(idBrush),
              StrokeThickness = 1
            };
            Canvas.SetLeft(rect, xMax - Math.Round(zoneTo * kx, nrnd));
            //Canvas.SetTop(rect, yMin2 + Math.Round(yPos1*ky, nrnd));
            Canvas.SetTop(rect, yMax2 - Math.Round(yPos1 * ky, nrnd) - Math.Round((yPos2 - yPos1) * ky, nrnd));
            cnv1.Children.Add(rect);
          }
          idBrush++;
        }
        oldX = xMax - Math.Round(zoneTo * kx, nrnd);
        zIdx++;
      }

      //Делаем подпись начала
      hlbl = new Label
      {
        Content = "Начало",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin2);
      cnv1.Children.Add(hlbl);


      //Далее рисуем заполняемый заголовок н странице 2
      //Определяем yMin для раскроечного рулона
      hlbl = new Label
      {
        Content = "Дата__________________________АПР №_________Бригада №________Контролер ОТК__________________________________" +
                  "      Ширина______________________мм  Масса_________________тн",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 13,
        FontWeight = FontWeights.Bold
      };
      Canvas.SetLeft(hlbl, 20);
      Canvas.SetTop(hlbl, 100);
      cnv2.Children.Add(hlbl);

      double yMin3 = 140;
      double yMax3 = yMin3 + (yMax - yMin);


      plCoil = new Polyline();
      plCoil.Points.Add(new Point(xMin, yMin3));
      plCoil.Points.Add(new Point(xMax, yMin3));
      plCoil.Points.Add(new Point(xMax, yMax3));
      plCoil.Points.Add(new Point(xMin, yMax3));
      plCoil.Points.Add(new Point(xMin, yMin3));
      plCoil.Stroke = Brushes.Black;
      plCoil.StrokeThickness = 2;
      cnv2.Children.Add(plCoil);

      //рисуем весовую линейку для раскроечного рулона 
      this.PaintCoilRuleBackward(kx, 500, xMin, xMax, yMin3, coilWgt, nrnd);

      //рисуем пунктиром дефектные зоны для раскроечного рулона
      foreach (double t in lstSf1)
      {
        var line = new Line
        {
          X1 = t,
          Y1 = yMin3,
          X2 = t,
          Y2 = yMax3,
          Stroke = Brushes.Black,
          StrokeThickness = 1,
          StrokeDashArray = DoubleCollection.Parse("5, 3")
        };
        cnv2.Children.Add(line);
      }

      foreach (double t in lstSf2)
      {
        var line = new Line
        {
          X1 = t,
          Y1 = yMin3,
          X2 = t,
          Y2 = yMax3,
          Stroke = Brushes.Black,
          StrokeThickness = 1,
          StrokeDashArray = DoubleCollection.Parse("5, 3")
        };
        cnv2.Children.Add(line);
      }

      //Делаем подпись конец
      hlbl = new Label
      {
        Content = "Конец",
        Foreground = Brushes.Black,
        FontFamily = new FontFamily("Arial"),
        FontSize = 10,
        FontWeight = FontWeights.Bold,
        RenderTransform = new RotateTransform(90),
      };
      Canvas.SetLeft(hlbl, xMax + 14);
      Canvas.SetTop(hlbl, yMin3);
      cnv2.Children.Add(hlbl);

    }



    #endregion

    #region Constructor
    internal ViewModelMapDefects(System.Windows.Controls.UserControl control)
    {
      this.usrControl = control;
      this.cnv1 = LogicalTreeHelper.FindLogicalNode(this.usrControl, "Cnvs1") as Canvas;
      this.cnv2 = LogicalTreeHelper.FindLogicalNode(this.usrControl, "Cnvs2") as Canvas;
      this.cnv3 = LogicalTreeHelper.FindLogicalNode(this.usrControl, "Cnvs3") as Canvas;
      scaleY = 100;
      scaleX = 100;

      coilType = new ObservableCollection<UiRef>()
      {
        new UiRef(){Id=1, Name="Рулон с АВО"},
        new UiRef(){Id=2, Name="Сдаточный рулон"}
      };
    }
    #endregion

    #region Commands
    private DelegateCommand<Object> buildMapDefectsCommand;
    private DelegateCommand<Object> printMapDefectsCommand;
    private DelegateCommand<Object> scaleYCommand;

    public ICommand BuildMapDefectsCommand
    {
      get {return buildMapDefectsCommand ?? (buildMapDefectsCommand = new DelegateCommand<Object>(ExecuteBuildMapDefects, CanExecuteBuildMapDefects));}
    }

    private void ExecuteBuildMapDefects(Object parameter)
    {
      //BuildMapDefects(3369.72, 1040);
      if (this.SelectedCoilType == 1)
        BuildMapDef();
      else
        BuildMapDefUo();
    }

    private bool CanExecuteBuildMapDefects(Object parameter)
    {
      return (!String.IsNullOrEmpty(this.findLocNumText));
    }


    public ICommand PrintMapDefectsCommand
    {
      get{return printMapDefectsCommand ?? (printMapDefectsCommand = new DelegateCommand<Object>(ExecutePrintMapDefects, CanExecutePrintMapDefects));}
    }

    private void ExecutePrintMapDefects(Object parameter)
    {
      var printDialog = new PrintDialog();

      if (printDialog.ShowDialog().GetValueOrDefault() == true){
        printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
        //printDialog.PrintQueue.GetPrintCapabilities().
        printDialog.PrintVisual(this.cnv1, "Print Defects Map1");
      }

      printDialog = new PrintDialog();
      if (printDialog.ShowDialog().GetValueOrDefault() != true) return;
      printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
      printDialog.PrintVisual(this.cnv2, "Print Defects Map2");
    }

    private bool CanExecutePrintMapDefects(Object parameter)
    {
      return (!String.IsNullOrEmpty(this.findLocNumText));
    }


    public ICommand ScaleYCommand
    {
      get { return scaleYCommand ?? (scaleYCommand = new DelegateCommand<Object>(ExecuteScaleY, CanExecuteScaleY)); }
    }

    private void ExecuteScaleY(Object parameter)
    {
      this.SetScaleY();
    }

    private bool CanExecuteScaleY(Object parameter)
    {
      return cnv1.Children.Count > 0;
    }




    #endregion

  }
}
