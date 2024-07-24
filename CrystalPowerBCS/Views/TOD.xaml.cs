using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CrystalPowerBCS.DbFunctions.EventSinglePhaseFunctions;
using CrystalPowerBCS.DbFunctions.EventThreePhaseCTFunctions;
using CrystalPowerBCS.DbFunctions.EventThreePhaseFunctions;
using CrystalPowerBCS.Helpers;
using Infrastructure.DTOs.SinglePhaseEventDTOs;
using Infrastructure.DTOs.ThreePhaseEventCTDTOs;
using Infrastructure.DTOs.ThreePhaseEventDTOs;
using Infrastructure.Helpers;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.WPF;
using SkiaSharp;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.DTOs;
using CrystalPowerBCS.DbFunctions;

namespace CrystalPowerBCS.Views
{
    /// <summary>
    /// Interaction logic for TDO.xaml
    /// </summary>
    public partial class TOD : UserControl
    {
        public bool IsGraph;
        public bool IsFilterEnabled;
        public List<TODDto> TODDto;
        public TODCommand TODCommand;
        public int pageSize = 10;
        public string MeterType = Constants.SinglePhaseMeter;
        public string MeterNumber = "00000000";
        public ErrorHelper _errorHelper;
        public TOD()
        {
            InitializeComponent();
            TODGrid.Visibility = Visibility.Visible;
            _errorHelper = new ErrorHelper();
            TODDto = new List<TODDto>();
        }
        private void OnResize(object sender, TextChangedEventArgs e)
        {
            try
            {
                double height = Convert.ToDouble(CurrentHeight.Text.ToString());
                Grid_Parent.Height = height - 217;
                TODGrid.MaxHeight = height - 217;
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private async Task PopullateGrid()
        {
            TODCommand = new TODCommand();

            var gridData = await TODCommand.GetByMeterNo(MeterNumber);

            TODGrid.ItemsSource = gridData;
            TODDto = gridData;
        }

        private async Task BindMeterTypeAndNumber()
        {
            string[] meterData = MeterPhaseType.Text.Split("&&");
            MeterType = meterData[0].ToString();
            MeterNumber = meterData[1].ToString();
        }

        private void ChangeGridType(object sender, TextChangedEventArgs e)
        {
            this.Dispatcher.Invoke(async () =>
            {
                await BindMeterTypeAndNumber();
            });

            this.Dispatcher.Invoke(async () =>
            {
                await PopullateGrid();
            });
        }

        private void FilterByDate(object sender, RoutedEventArgs e)
        {

        }

        private void FilterByPageSize(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ExportData(object sender, SelectionChangedEventArgs e)
        {
            var exportType = ((System.Windows.Controls.ContentControl)cbExport.SelectedValue).Content as string;
            if (exportType == null || exportType == Constants.Export)
                return;

            if (exportType == Constants.ExportasExcel)
            {
                string FileName = Constants.TOD + Guid.NewGuid().ToString();
                DataTable dt = ExportHelper.ToDataTable(TODDto);
                bool res = ExportHelper.GenerateExcel(dt, FileName, Constants.TOD, MeterNumber);
            }
            else if (exportType == Constants.ExportasPdf)
            {
                string FileName = Constants.TOD + Guid.NewGuid().ToString();
                DataTable dt = ExportHelper.ToDataTable(TODDto);
                bool res = ExportHelper.GeneratePdf(dt, FileName, Constants.TOD, MeterNumber);
            }
        }
    }
}
