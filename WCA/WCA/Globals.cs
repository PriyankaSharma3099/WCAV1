using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WCA.Models;

namespace WCA
{
    static class Globals
    {
        private static string _csv;
        private static string _csv_tomorrowjobs;
        private static string _csv_areas;
        private static string _csv_driver;
        private static string _csv_vehicleTypes;
        private static string _csv_vehicle;
        private static string _csv_wasteconfig;
        private static string _csv_cancel_resson_list;
        private static string _csv_email;
        public static string ContainerSelectedValue { get; set; }
        public static int DriverSelectedIndex { get; set; }
        public static string DriverSelectedValue { get; set; }
        public static int VehicleSelectedIndex { get; set; }
        public static string VehicleSelectedValue { get; set; }
        public static int Vehicle_TypesSelectedIndex { get; set; }
        public static int Vehicle_Type { get; set; }
        public static string Vehicle_TypesSelectedValue { get; set; }
        public static int AreaSelectedIndex { get; set; }
        public static string AreaSelectedValue { get; set; }
        public static string storagePath { get; set; }
        public static string TradeWaste_FileName { get; set; }
        public static string TradeWaste_FileName_withoutext { get; set; }
        public static ObservableCollection<CustomerContainer> Customer_WasteType_at_SelectedIndex { get; set; }
        public static Job Job_at_SelectedIndex { get; set; }
        public static List<TicketDataModel> TicketData { get; set; }
        public static List<Email_DataModel> EmailData { get; set; }
        public static string CsvText_TomorrowJobs
        {
            set { _csv_tomorrowjobs = value; }
            get { return _csv_tomorrowjobs; }
        }
        public static string CsvText
        {
            set { _csv = value; }
            get { return _csv; }
        }
        public static string CsvText_areas
        {
            set { _csv_areas = value; }
            get { return _csv_areas; }
        }
        public static string CsvText_driver
        {
            set { _csv_driver = value; }
            get { return _csv_driver; }
        }
        public static string CsvText_VehicleTypes
        {
            set { _csv_vehicleTypes = value; }
            get { return _csv_vehicleTypes; }
        }
        public static string CsvText_vehicle
        {
            set { _csv_vehicle = value; }
            get { return _csv_vehicle; }
        }
        public static string csvText_wasteconfig
        {
            set { _csv_wasteconfig = value; }
            get { return _csv_wasteconfig; }
        }
        public static string csvText_cancel_reason
        {
            set { _csv_cancel_resson_list = value; }
            get { return _csv_cancel_resson_list; }
        }
        public static string csvText_email
        {
            set { _csv_email = value; }
            get { return _csv_email; }
        }
    }

}
