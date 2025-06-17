using WCA.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using WCA.Views;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WCA.ViewModels
{
    public class JobsViewModel : ObservableRangeCollection<CustomerContainerViewModel>, INotifyPropertyChanged
    {
        private ObservableRangeCollection<CustomerContainerViewModel> jobCustomerContainers = new ObservableRangeCollection<CustomerContainerViewModel>();
        public Command TicketCommand { get; }
        public Command NTSCommand { get; }
        public Command JobCompleteCommand { get; }

        public List<CustomerContainerViewModel> CustomerContainerViewModels = new List<CustomerContainerViewModel>();

        public JobsViewModel(Job job, bool expanded = false)
        {
            this.Job = job;
            this._expanded = expanded;

            foreach (CustomerContainer customercontainer in job.CustomerContainers)
            {
                jobCustomerContainers.Add(new CustomerContainerViewModel(customercontainer));
            }
            if (expanded)
                this.AddRange(jobCustomerContainers);
            TicketCommand = new Command(OnTicketClicked);
            NTSCommand = new Command(OnNTSClicked);
            JobCompleteCommand = new Command(JobCompleted_Clicked);

        }
        private void JobCompleted_Clicked(object obj)
        {
            Globals.Customer_WasteType_at_SelectedIndex = Customer_WasteType;
            Globals.Job_at_SelectedIndex = Job;
            new JobsGroupViewModel().markJobAsDone_EventAsync(true, true);
        }
        private void OnNTSClicked(object obj)
        {
            // NTS Button Click action here
            JobDetailViewModeDataBindingAsync(obj, false);
        }
        private void OnTicketClicked(object obj)
        {
            JobDetailViewModeDataBindingAsync(obj, true);
        }


        private async Task JobDetailViewModeDataBindingAsync(object obj, bool issignature)
        {
            // Navigate to the Job detail page, passing the JobDetailViewModel as a query parameter.
            JobDetailViewModel jobDetailViewModel = new JobDetailViewModel();
            bool WasteTypeFillCheck = true;
            // bool WeightFillCheck = true;
            var Weight =0;
            var a = Job;
            jobDetailViewModel.Customer = Customer;
            jobDetailViewModel.Site = Site;
            jobDetailViewModel.SendMail_Visibility = false;
            jobDetailViewModel.FinishButton_Visibility = false;
            jobDetailViewModel.SpotJob_visibility = Job.SpotJob_Visibility;
            //if (Globals.Vehicle_Type == 1) {
                jobDetailViewModel.IsImageAttachMode = true;
            //}
            //else {
            //    jobDetailViewModel.IsImageAttachMode = false;
            //}



            for (int i = 0; i < Job.CustomerContainers.Count; i++) 
            {
                if (Job.CustomerContainers[i].Weight_T != null && Job.CustomerContainers[i].Weight_T != 0) {
                    Weight+=int.Parse(Job.CustomerContainers[i].Weight_T.ToString());
                }
            }


            string path_sign = Globals.storagePath + "/WCA";
            string SignaturesPath = path_sign + "/Signatures/" + Customer + TicketNo + CPID+".png";  // signature read path

            // check for signature exist or not
            if (File.Exists(SignaturesPath))
            {
                if (!issignature)
                {
                    File.Delete(SignaturesPath);
                    jobDetailViewModel.SignatureFoundVisibility = false;
                }
                else {
                    jobDetailViewModel.SignatureFoundVisibility = true;
                }
            }
            else {
                jobDetailViewModel.SignatureFoundVisibility = false;
            }
            string path = Globals.storagePath + "/WCA/Tickets/";
            string pdfPath_Admin = System.IO.Path.Combine(path, "ISM_Copy_Ticket_" +Customer + "_" + TicketNo + ".pdf"); // save ticket path
            string pdfPath_Customer = System.IO.Path.Combine(path, "Ticket_" + Customer + "_" + TicketNo + ".pdf"); // save ticket path
            if (File.Exists(pdfPath_Admin))
            {
                File.Delete(pdfPath_Admin);
            }
            if (File.Exists(pdfPath_Customer))
            {
                File.Delete(pdfPath_Customer);
            }
            if (CancelReasonID == 0 || CancelReasonID==15)
            {
                jobDetailViewModel.Weight = Weight.ToString();
            }
            if (issignature)
            {
                jobDetailViewModel.SignaturePadVisibility = true;
                jobDetailViewModel.IsNTSMode = false;
            }
            else
            {
                jobDetailViewModel.SignaturePadVisibility = false;
                jobDetailViewModel.IsNTSMode = true;
            }
            //    jobDetailViewModel.Container_Qty_DetailPage = Globals.ContainerSelectedValue;
            // for update weight in tradewaste file
            if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
            {
                string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                string temprow = "";
                //bool checkWeight_Updated = true;
                bool General_wasteType = false, DMR_wasteType = false, Cardboard_wasteType = false, Glass_wasteType = false, Food_wasteType=false;
                string WB1100_General = "",
                    WB660_General = "",
                    WB240_General = "",
                    WB1100_DMR = "",
                    WB660_DMR = "",
                    WB240_DMR = "",
                    WB1100_Cardboard = "",
                    WB660_Cardboard = "", 
                    WB240_Cardboard = "",
                    WB1100_Glass = "",
                    WB660_Glass = "",
                    WB240_Glass = "",
                    WB1100_Food = "",
                    WB660_Food = "",
                    WB240_Food = "";
                string REL08_General = "",
                    REL12_General = "",
                    REL14_General = "",
                    REL16_General = "",
                    REL08_DMR = "",
                    REL12_DMR = "",
                    REL14_DMR = "",
                    REL16_DMR = "",
                    REL08_Cardboard = "",
                    REL12_Cardboard = "",
                    REL14_Cardboard = "",
                    REL16_Cardboard = "";

                // Variable for VehicleType 2 & 3
                string OPEN_25_WasteType = "",
                       COMP35_WasteType = "",
                       ENCL35_WasteType = "",
                       OPEN_40_WasteType = "";

                // for vehicle Type 11
                string FEL06_General = "",
                    FEL08_General = "",
                    FEL12_General = "",
                    FEL06_DMR = "",
                    FEL08_DMR = "",
                    FEL12_DMR = "",
                    FEL06_Cardboard = "",
                    FEL08_Cardboard = "",
                    FEL12_Cardboard = "",
                    FEL06_Glass = "",
                    FEL08_Glass = "",
                    FEL12_Glass = "";


                foreach (var element in Customer_WasteType)
                {
                    if (element.IsVisibilityWasteType == true && element.ExtraWasteId == -1)
                    {
                        WasteTypeFillCheck = false;
                        break;
                    }
                    // check for weight must be select
                    /*  if (Weight == 0)
                      {
                          WeightFillCheck = false;
                          break;
                      }*/
                    if (jobDetailViewModel.Customer_WasteType == null)
                    {
                        jobDetailViewModel.Customer_WasteType = element.WasteType;
                        jobDetailViewModel.Container_Type_DetailPage = element.ContainerName; 
                        jobDetailViewModel.ContainerTotalQty = element.Qty;
                    }
                    else
                    {
                        jobDetailViewModel.Customer_WasteType += "," + element.WasteType;
                        jobDetailViewModel.Container_Type_DetailPage += "," + element.ContainerName;
                        jobDetailViewModel.ContainerTotalQty += element.Qty;
                    }
                    // extra waste value check
                    if (element.WasteType == "General" && element.ExtraWasteId != -1)
                    {
                        General_wasteType = true;
                    }
                    else if (element.WasteType == "DMR" && element.ExtraWasteId != -1)
                    {
                        DMR_wasteType = true;
                    }
                    else if (element.WasteType == "Cardboard" && element.ExtraWasteId != -1)
                    {
                        Cardboard_wasteType = true;
                    }
                    else if (element.WasteType == "Glass" && element.ExtraWasteId != -1)
                    {
                        Glass_wasteType = true;
                    }
                    else if (element.WasteType == "Food" && element.ExtraWasteId != -1)
                    {
                        Food_wasteType = true;
                    }

                    // add check for get values in case of VehicleType 2
                    if (element.ContainerName == "25 OPEN")
                    {
                        OPEN_25_WasteType += " "+element.Qty.ToString() + " X " + element.WasteType;
                    }

                    if (element.ContainerName == "35 COMP")
                    {
                        COMP35_WasteType += " " + element.Qty.ToString() + " X " + element.WasteType;
                    }

                    if (element.ContainerName == "35 ENCL")
                    {
                        ENCL35_WasteType += " " + element.Qty.ToString() + " X " + element.WasteType;
                    }

                    if (element.ContainerName == "40 OPEN")
                    {
                        OPEN_40_WasteType += " " + element.Qty.ToString() + " X " + element.WasteType;
                    }






                    // Extra Waste Value Check
                    if (element.ContainerName == "WB1100")
                    {
                        if (element.WasteType == "General")
                        {
                            WB1100_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            WB1100_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            WB1100_Cardboard = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Glass")
                        {
                            WB1100_Glass = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Food")
                        {
                            WB1100_Food = element.Qty.ToString();
                        }
                    }
                    if (element.ContainerName == "WB660")
                    {
                        if (element.WasteType == "General")
                        {
                            WB660_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            WB660_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            WB660_Cardboard = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Glass")
                        {
                            WB660_Glass = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Food")
                        {
                            WB660_Food = element.Qty.ToString();
                        }
                    }
                    if (element.ContainerName == "WB240")
                    {
                        if (element.WasteType == "General")
                        {
                            WB240_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            WB240_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            WB240_Cardboard = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Glass")
                        {
                            WB240_Glass = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Food")
                        {
                            WB240_Food = element.Qty.ToString();
                        }
                    }

                    // Quantity check for all REL wastetypes

                    if (element.ContainerName == "08REL")
                    {
                        if (element.WasteType == "General")
                        {
                            REL08_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            REL08_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            REL08_Cardboard = element.Qty.ToString();
                        }
                    }
                    if (element.ContainerName == "12REL")
                    {
                        if (element.WasteType == "General")
                        {
                            REL12_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            REL12_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            REL12_Cardboard = element.Qty.ToString();
                        }
                    }
                    if (element.ContainerName == "14REL")
                    {
                        if (element.WasteType == "General")
                        {
                            REL14_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            REL14_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            REL14_Cardboard = element.Qty.ToString();
                        }
                    }
                    if (element.ContainerName == "16REL")
                    {
                        if (element.WasteType == "General")
                        {
                            REL16_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            REL16_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            REL16_Cardboard = element.Qty.ToString();
                        }
                    }


                    // For Vehicle Type 11 FEL

                    if (element.ContainerName == "06FEL")
                    {
                        if (element.WasteType == "General")
                        {
                            FEL06_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            FEL06_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            FEL06_Cardboard = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Glass")
                        {
                            FEL06_Glass = element.Qty.ToString();
                        }
                    }

                    if (element.ContainerName == "08FEL")
                    {
                        if (element.WasteType == "General")
                        {
                            FEL08_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            FEL08_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            FEL08_Cardboard = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Glass")
                        {
                            FEL08_Glass = element.Qty.ToString();
                        }
                    }

                    if (element.ContainerName == "12FEL")
                    {
                        if (element.WasteType == "General")
                        {
                            FEL12_General = element.Qty.ToString();
                        }
                        else if (element.WasteType == "DMR")
                        {
                            FEL12_DMR = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Cardboard")
                        {
                            FEL12_Cardboard = element.Qty.ToString();
                        }
                        else if (element.WasteType == "Glass")
                        {
                            FEL12_Glass = element.Qty.ToString();
                        }
                    }



                    // write weight in tradewastefile
                    //if (Globals.Vehicle_Type == 1 || Globals.Vehicle_Type == 11)
                    //{
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            var values = lines[i].Split('|');

                           /* foreach (var item_customer_container in Job.CustomerContainers)
                            {*/
                                if (values[21] == element.CustomerID && values[4] == element.SiteName && values[1] == Job.CustomerContainers[0].CPID && values[9] == element.WasteType)  // "5490" CustomerID and "Glass" is Waste_Type
                        { 
                            //  && values[1]== Job.CustomerContainers[0].CPID   removed as we need to update weight at all waste types of same job
                            int item_at_index = i;

                                    /*  if (CancelReasonID == 0)
                                      {*/

                                    for (int j = 0; j < values.Length; j++)
                                    {
                                        if (j == 0)
                                        {
                                            temprow = values[j];
                                        }
                                        else
                                        {
                                            string weight_local = "";
                                            if (CancelReasonID == 0 && Job.CustomerContainers[0].Weight_T.ToString() == "0")
                                            {
                                                weight_local = "";
                                            }
                                            else if (CancelReasonID == 0 || CancelReasonID == 15)
                                            {
                                                //weight_local = Weight.ToString();
                                                weight_local = Job.CustomerContainers[0].Weight_T.ToString();
                                            }
                                            else
                                            {
                                                weight_local = "0";
                                            }
                                            string date_time = DateTime.Now.ToString("HH:mm");
                                            temprow += "|" + (j != 24 ? (j != 26 ?(j == 28 ? "1" : values[j]) : date_time) : weight_local);  
                                        if (j==values.Length-1) {
                                            lines[item_at_index] = temprow;
                                            //checkWeight_Updated = false;

                                        }

                                    }
                                    }
                                    // }
                                    //  break;
                                }
                            // }
                        }
                    //}
                }

                if (WasteTypeFillCheck)
                {
                    Globals.TicketData = new List<TicketDataModel>();
                    Globals.TicketData.Add(new TicketDataModel()
                    {
                        CPID = CPID,
                        TicketNo = TicketNo,
                        CustomerName = Customer,
                        EmailAddress=EmailAddress,
                        CancelreasonId=CancelReasonID,
                        Cancelreason=CancelReasonType,
                        WasteTypeString= jobDetailViewModel.Customer_WasteType, //Container_Type_DetailPage
                        CompleteAddress =  Address + ", " + Town + ", " + PostCode, //Site + ", " + Address + ", " + Town + ", " + PostCode,
                        TotalWeight =CancelReasonID==0 || CancelReasonID==15? Weight.ToString():"0",
                        CustomerOrderId=Customer_Order_Id,
                        Invocie_Name= Invoice_Name,
                        ExtraWaste_Cardboard = Cardboard_wasteType,
                        ExtraWaste_DMR = DMR_wasteType,
                        ExtraWaste_General = General_wasteType,
                        ExtraWaste_Glass = Glass_wasteType,
                        ExtraWaste_Food = Food_wasteType,
                        WB1100_General = WB1100_General,
                        WB1100_Cardboard = WB1100_Cardboard,
                        WB1100_DMR = WB1100_DMR,
                        WB1100_Glass = WB1100_Glass,
                        WB1100_Food = WB1100_Food,
                        WB660_General = WB660_General,
                        WB660_DMR = WB660_DMR,
                        WB660_Cardboard = WB660_Cardboard,
                        WB660_Glass = WB660_Glass,
                        WB660_Food = WB660_Food,
                        WB240_General = WB240_General,
                        WB240_Food = WB240_Food,
                        WB240_DMR = WB240_DMR,
                        WB240_Cardboard = WB240_Cardboard,
                        WB240_Glass = WB240_Glass,
                        REL08_General = REL08_General,
                        REL08_DMR = REL08_DMR,
                        REL08_Cardboard = REL08_Cardboard,
                        REL12_General = REL12_General,
                        REL12_DMR = REL12_DMR,
                        REL12_Cardboard = REL12_Cardboard,
                        REL14_General = REL14_General,
                        REL14_DMR = REL14_DMR,
                        REL14_Cardboard = REL14_Cardboard,
                        REL16_General = REL16_General,
                        REL16_DMR = REL16_DMR,
                        REL16_Cardboard = REL16_Cardboard,
                        OPEN_25_WasteType = OPEN_25_WasteType,
                        COMP35_WasteType= COMP35_WasteType,
                        ENCL35_WasteType = ENCL35_WasteType,
                        OPEN_40_WasteType = OPEN_40_WasteType,
                        FEL06_General = FEL06_General,
                        FEL08_General = FEL08_General,
                        FEL12_General = FEL12_General,
                        FEL06_DMR = FEL06_DMR,
                        FEL08_DMR = FEL08_DMR,
                        FEL12_DMR = FEL12_DMR,
                        FEL06_Cardboard = FEL06_Cardboard,
                        FEL08_Cardboard = FEL08_Cardboard,
                        FEL12_Cardboard = FEL12_Cardboard,
                        FEL06_Glass = FEL06_Glass,
                        FEL08_Glass = FEL08_Glass,
                        FEL12_Glass = FEL12_Glass
                    });


                     if (Globals.TicketData.Count == 1)
                    {
                      /*  if (CancelReasonID == 0)
                        {*/
                            File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                            File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName, lines);
                            Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName); // for read file from Internal Directory 
                      /*  }
                        else*/
                        if(Weight.ToString() !="" && (CancelReasonID!=0 && CancelReasonID!=15)){
                            Application.Current.MainPage.DisplayAlert("Alert", "Unable to update TotalWeight because job is processed for cancellation.", "OK");
                        }
                        Globals.Customer_WasteType_at_SelectedIndex = Customer_WasteType;
                        Globals.Job_at_SelectedIndex = Job;
                        await Application.Current.MainPage.Navigation.PushAsync(new JobDetailPage(jobDetailViewModel)); // pass ViewModel object to next page
                    }
                    else {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Unable to Genrate Ticket,Try Again !", "OK");
                    }
                }
                else if (!WasteTypeFillCheck)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Please Select Extra Waste Type !", "OK");
                }
                else
                {

                }
                /*   else if (!WeightFillCheck)
                   {
                       await Application.Current.MainPage.DisplayAlert("Alert", "Please Enter Total Weight !", "OK");
                   }*/

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");

            }
        }

        public JobsViewModel()
        {
        }

        private bool _expanded;
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                    if (_expanded)
                    {
                        this.AddRange(jobCustomerContainers);
                    }
                    else
                    {
                        this.Clear();
                    }
                    if ((Globals.Vehicle_Type == 1 || Globals.Vehicle_Type == 11) && _expanded)
                    {
                        weightUpdateWasteType(true);
                        //WeightVisibility = true;
                    }
                    else if ((Globals.Vehicle_Type == 1 || Globals.Vehicle_Type == 11) && !_expanded)
                    {
                        weightUpdateWasteType(false);
                        //WeightVisibility = false;

                    }

                    if (Job.SpotJob_Visibility && _expanded)
                    {
                        SpotJobvisibility = true;
                    }
                    else
                    {
                        SpotJobvisibility = false;
                    }

                }
            }
        }

        public void weightUpdateWasteType(bool visibility) {
            for (int i = 0; i < Job.CustomerContainers.Count; i++) {
                Job.CustomerContainers[i].WeightVisibility =  visibility;
            }
        }

        public string StateIcon
        {
            get
            {
                if (Expanded)
                {
                    return "arrow_a.png";
                }
                else
                { return "arrow_b.png"; }
            }
        }
        public string CPID { get { return Job.CPID; } }
        public string Customer { get { return Job.Customer; } }
        public string Site { get { return Job.Site; } }
        public string Town { get { return Job.Town; } }
        public string PostCode { get { return Job.PostCode; } }
        public string Address { get { return Job.Address; } }
        public string EmailAddress { get { return Job.EmailAddress; } }
        public string Customer_Order_Id { get { return Job.CustomerOrderId; } }
        public string Invoice_Name { get { return Job.InvoiceName; } }
        public string TicketNo { get { return Job.TicketNo; } }
        public bool SpotJobvisibility { get {
                //if (SpotJobvisibility && _expanded)
                //{
                //    return true;
                //}
                //else
                //{
                    return Job.SpotJob_Visibility_Flag;
                //}
            }
            set
            {
                Job.SpotJob_Visibility_Flag = value;
                //WeightVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpotJobvisibility"));
            }
        }
        //public bool WeightVisibility
        //{
        //    get
        //    {
        //        return Job.WeightVisibility;
        //        //if (Globals.Vehicle_Type == 2 || Globals.Vehicle_Type == 3)
        //        //{
        //        //    //if (_expanded)
        //        //    //{
        //        //        return false;
        //        //    //}
        //        //    //else
        //        //    //{
        //        //    //    return true;
        //        //    //}
        //        //}
        //        //else
        //        //{
        //        //    if (_expanded)
        //        //    {
        //        //        return true;
        //        //    }
        //        //    else
        //        //    {
        //        //        return false;
        //        //    }
        //        //}
        //        //return ((Globals.Vehicle_Type == 2 || Globals.Vehicle_Type == 3 || !Expanded) ? false : true);
        //    }
        //    set
        //    {
        //        Job.WeightVisibility = value;
        //        //WeightVisibility = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("WeightVisibility"));
        //    }

        //}
        public ObservableCollection<CustomerContainer> Customer_WasteType { get { return Job.CustomerContainers; } }
        public Job Job { get; set; }
        public string DriverNote { get { return Job.DriverNote; } }
        public string CancelReasonType { get { return Job.CancelReasonType; }set {
                Job.CancelReasonType = value;
            } }
        public bool CancelReasonVisibility { get { return (Job.CancelReasonVisibility==true? true : false);
            }
            set
            {
                Job.CancelReasonVisibility = value;
            }
        }
        public bool CheckValue_Contaminated { get { return !(Job.CancelReasonVisibility); }
            set
            {
                if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
                {
                    string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    string temprow = "";
                    for (int i = 0; i < lines.Count(); i++)
                    {

                        var values = lines[i].Split('|');

                        for (int indexOfSelectedItems = 0; indexOfSelectedItems < Customer_WasteType.Count; indexOfSelectedItems++)
                        {

                            if ( values[1] == Customer_WasteType[indexOfSelectedItems].CPID)  // && values[9]== SelectedItemRec.WasteType
                            {
                                int item_at_index = i;
                                for (int j = 0; j < values.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        temprow = values[j];
                                    }
                                    else
                                    {
                                        string date_time = DateTime.Now.ToString("HH:mm");
                                        temprow += "|" + (j != 25 ? (j != 26 ?(j == 28 ? "1" : values[j] ): date_time) : (value ==true ? "15": Job.Previous_CancelReasonID.ToString()));
                                        if (j == values.Length - 1)
                                        {
                                            lines[item_at_index] = temprow;

                                        }
                                    }
                                }
                                //  break;
                            }
                        }
                    }
                    if (value)
                    {
                        CancelReasonID = 15;
                        Job.CancelReasonID = 15;
                        Job.CancelReasonType = "Contaminated";
                        CancelReasonType = "Contaminated";
                    }
                    else {
                        Job.CancelReasonID = Job.Previous_CancelReasonID;
                       CancelReasonID = Job.Previous_CancelReasonID;
                        CancelReasonType = Job.Previous_CancelReasonType;
                        Job.CancelReasonType = Job.Previous_CancelReasonType;
                    }
                    JobCancelReasonList = Job.JobCancelReasonList;
                    File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName, lines);
                    Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName); // for read file from Internal Directory 
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");
                }
                //CancelReasonVisibility = value;
                Job.CancelReasonVisibility = !value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancelReasonVisibility"));
                OnPropertyChanged(new PropertyChangedEventArgs("CancelReasonID"));
                OnPropertyChanged(new PropertyChangedEventArgs("CancelReasonType"));
                OnPropertyChanged(new PropertyChangedEventArgs("JobCancelReasonList"));



            }
        }
        public int CancelReasonID { get { return Job.CancelReasonID; } set {
                Job.CancelReasonID = value;
            } }
        //public string Weight
        //{
        //    get { return Job.Weight; }
        //    set
        //    {

        //            Job.Weight = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Weight"));

        //    }
        //}
        //private int _weight;

        //public int Weight
        //{
        //    get { return _weight; }
        //    set
        //    {
        //        if (_weight != value)
        //        {
        //            _weight = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Weight"));

        //            // Update the weight in the selected CustomerContainer
        //            UpdateCustomerContainerWeight(_weight);
        //        }
        //    }
        //}
        public ObservableCollection<CancelReasonList> JobCancelReasonList
        {
            get { return Job.JobCancelReasonList; }
            set { Job.JobCancelReasonList = value; }

        }

        private CustomerContainer _selectedCustomerContainer;

        public CustomerContainer SelectedCustomerContainer
        {
            get { return _selectedCustomerContainer; }
            set
            {
                if (_selectedCustomerContainer != value)
                {
                    _selectedCustomerContainer = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerContainer"));


                    // Set the weight for the selected container
                    //Weight = (int)(_selectedCustomerContainer?.Weight);
                }
            }
        }

        //private void UpdateCustomerContainerWeight(int newWeight)
        //{
        //    if (_selectedCustomerContainer != null)
        //    {
        //        _selectedCustomerContainer.Weight = newWeight;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Customer_WasteType"));
        //    }
        //}


    }

}