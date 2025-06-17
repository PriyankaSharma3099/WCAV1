using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WCA.Models;
using WCA.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WCA.ViewModels
{
    public class JobsGroupViewModel : BaseViewModel
    {
        private JobsViewModel _oldJob;

        private ObservableCollection<JobsViewModel> items;
        public ObservableCollection<JobsViewModel> Items
        {
            get => items;
            set 
            {
                SetProperty(ref items, value); 
            }
            
        }

        public Command LoadJobsCommand { get; set; }
        public Command<JobsViewModel> RefreshItemsCommand { get; set; }
        public int WasteTypeSelectedItem { get; set; }

        public JobsGroupViewModel()
        {
            items = new ObservableCollection<JobsViewModel>();
            //  Items = new ObservableCollection<JobsViewModel>();
            LoadJobsCommand = new Command(() => ExecuteLoadItemsCommand());
            RefreshItemsCommand = new Command<JobsViewModel>((item) => ExecuteRefreshItemsCommand(item));
        }

        public bool isExpanded = false;
        
        private void ExecuteRefreshItemsCommand(JobsViewModel item)
        {
          //  item.CancelReasonVisibility = (item.CancelReasonVisibility == true)? item.Expanded == false ? true : false :false;
            if (_oldJob == item)
            {
                // click twice on the same item will hide it
                item.Expanded = !item.Expanded;
            }
            else
            {
                if (_oldJob != null)
                {
                    // hide previous selected item
                    _oldJob.Expanded = false;
                }
                // show selected item
                item.Expanded = true;
            }
           // item.CancelReasonVisibility = (item.CheckValue_Contaminated) == true ? false : true;
            _oldJob = item;
        }

        public async Task SaveCsvAsync(string data)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "JobContainers.csv");
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(data);
            }
        }

        public async Task<int> ReadCsvAsync()
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");

            if (backingFile == null || !File.Exists(backingFile))
            {
                return 0;
            }

            var count = 0;
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (int.TryParse(line, out var newcount))
                    {
                        count = newcount;
                    }
                }
            }
            return count;
        }

        public void ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                Items.Clear();

                // Variables
                int tempRows = 5;       // Rows in temporary array
                int tempRow = 0;        // Index in temporary array
                int siteCol = 17;       // Site ID column index
                int cols = 24;          // Columns in CSV, now 23 16/05/2022

                // Convert CSV to 2D string array
                string[] lines = Globals.CsvText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                int lineCount = lines.Length;
                cols = lines[0].Split('|').Length;
                string[,] data = new string[lineCount, cols];

                for (int i = 0; i < lineCount; i++) 
                {
                    Console.WriteLine(" Item read at ==> "+i.ToString());
                    var values = lines[i].Split('|');
                    for (int j = 0; j < cols; j++)
                    {
                        data[i, j] = values[j];
                    }
                }

                // Create temporary array to store data.
                string[,] tempData = new string[tempRows, cols];

                // Create list of jobs
                List<Job> joblist = new List<Job>();

                // Iterate through CSV lines
                for (int i = 0; i < lineCount; i++)
                {
                    // Ignore 1st line, just append data to temp array
                    if (i > 0)
                    {
                        // If there is a new customer (and site ID) or this is the last row.

                        /* if (data[i, siteCol] != data[i - 1, siteCol] || i == lineCount - 1)
                         {*/
                        if ((i + 1) < lineCount)
                        {
                            Console.WriteLine("selected vehicle type =>" + Globals.Vehicle_Type);
                            // for remove grouping accoring to site name
                            if ((data[i, siteCol] != data[i + 1, siteCol]) || Globals.Vehicle_Type == 2 || Globals.Vehicle_Type == 3)
                            {


                                if (data[i, 11] == "0" && data[i, 25] != "6")
                                {
                                    //Append last record to temp array
                                    for (int j = 0; j < cols; j++)
                                    {
                                        tempData[tempRow, j] = data[i, j];
                                    }
                                    tempRow++;
                                    // Create container list for previous customer
                                    ObservableCollection<CustomerContainer> containerlist = new ObservableCollection<CustomerContainer>();
                                    if (tempData[0, 3] != null)
                                    {
                                        AddDataToContainerList(tempRow, tempData, containerlist);
                                        AddNewJobToJobList(data, joblist, i, containerlist);
                                    }
                                    Array.Clear(tempData, 0, 95);
                                    tempRow = 0;
                                    //  i++;
                                }
                                else
                                {
                                  /*  if (data[i, 11] == "0")
                                    {
                                        //Append current record to temp array
                                        for (int j = 0; j < cols; j++)
                                        {
                                            tempData[tempRow, j] = data[i, j];
                                        }
                                        tempRow++;
                                    }*/
                                }

                                // else for remove grouping accoring to site name
                            }
                            else
                            {
                                if (i < lineCount - 1 && data[i, 11] == "0" && data[i, 25] != "6")
                                {
                                    //Append current record to temp array
                                    for (int j = 0; j < cols; j++)
                                    {
                                        tempData[tempRow, j] = data[i, j];
                                    }
                                    tempRow++;
                                }
                            }
                        }
                        else {
                            if (data[i, 11] == "0" && data[i, 25] != "6")
                            {
                                //Append last record to temp array
                                for (int j = 0; j < cols; j++)
                                {
                                    tempData[tempRow, j] = data[i, j];
                                }
                                tempRow++;
                                // Create container list for previous customer
                                ObservableCollection<CustomerContainer> containerlist = new ObservableCollection<CustomerContainer>();
                                if (tempData[0, 3] != null)
                                {
                                    AddDataToContainerList(tempRow, tempData, containerlist);
                                    AddNewJobToJobList(data, joblist, i, containerlist);
                                }
                                Array.Clear(tempData, 0, 95);
                                tempRow = 0;
                                
                            }
                        }
                       //}
                       
                    }
                          
                        }


                // -----------------------------------
                // read and set data for Email


                int cols_email = 1;          // Columns in CSV
                    string[] lines_email = Globals.csvText_email.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
                    int lineCount_email = lines_email.Length;
                    string[,] data_email = new string[lineCount_email, cols_email];

                Globals.EmailData = new List<Email_DataModel>();
               
                    for (int i = 0; i < lineCount_email; i++)
                    {
                        var values = lines_email[i].Split('|');
                        for (int j = 0; j < cols_email; j++)
                        {
                            data_email[i, j] = values[j];
                        }
                    }





                if (lineCount_email >= 6 && cols_email >= 1)
                {
                    Globals.EmailData.Add(new Email_DataModel()
                    {
                        Server =        data_email[0, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        Username =      data_email[1, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        Password =      data_email[2, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        Encryption =    data_email[3, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        Incomingport =  data_email[4, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        Outgoingport =  data_email[5, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        SenderEmail =   data_email[6, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        CopyEmail =     data_email[7, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],
                        Message =       data_email[8, 0].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1],

                    });
                }

                // Iterate through CSV lines
                //for (int i = 0; i < lineCount_email; i++)
                //{ 
                //    //if (i < lineCount_email)
                //    //{
                //        //Append current record to temp array
                //        for (int j = 0; j < cols_email; j++)
                //        {
                //        var a = data_email[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0];
                //        var b = data_email[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1];
                //            //VehicleLists.Add(new Vehicle_DataModel(data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0], data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1]));
                //        }
                //    //}
                //}

                //------------------------------------
                 
                // Add all jobs to job list
                 if (joblist != null && joblist.Count > 0)
                {
                    
                        foreach (var job in joblist)
                            Items.Add(new JobsViewModel(job));
                  
                }
                else { IsEmpty = true; }
              }
            catch (Exception ex)
            {
                IsBusy = false; 
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static void AddNewJobToJobList(string[,] data, List<Job> joblist, int i, ObservableCollection<CustomerContainer> containerlist)
        {
         ObservableCollection<CancelReasonList> CancelReasonList = new ObservableCollection<CancelReasonList>();


            if (Globals.csvText_cancel_reason != null)
            {
                // for add Select in list for remove selected cancel reason
                CancelReasonList.Add(new CancelReasonList(0, data[i , 21], data[i, 9],"Select", data[i, 4], data[i, 1]));
                int cols_l = 1;          // Columns in CSV
                string[] lines_l = Globals.csvText_cancel_reason.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
                int lineCount_l = lines_l.Length;
                string[,] data_l = new string[lineCount_l, cols_l];

                for (int i_l = 0; i_l < lineCount_l; i_l++)
                {
                    var values = lines_l[i_l].Split('|');
                    for (int j = 0; j < cols_l; j++)
                    {
                        data_l[i_l, j] = values[j];
                    }
                }

                // Iterate through CSV lines
                for (int i_l = 1; i_l < lineCount_l; i_l++)
                {
                    if (i_l < lineCount_l)
                    {
                        //Append current record to temp array
                        for (int j = 0; j < cols_l; j++)
                        {
                            if(Convert.ToInt32(data_l[i_l, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0])!=6) {
                                CancelReasonList.Add(new CancelReasonList(Convert.ToInt32(data_l[i_l, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0]), data[i, 21], data[i, 9], data_l[i_l, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1], data[i, 4], data[i, 1]));
                            }
                        }
                    }
                }
            }
            // add data to joblist
            joblist.Add(new Job()
            {
                CPID = data[i,1],
                Customer = data[i , 3],
                Site = data[i, 4],
                Town = data[i, 5],
                PostCode = data[i, 6], 
                DriverNote = data[i, 10],
                CustomerContainers = containerlist,
                JobCancelReasonList = CancelReasonList,
                CancelReasonType = Convert.ToInt32(data[i , 25]) == 0 || Convert.ToInt32(data[i, 25]) == 15 ? Convert.ToInt32(data[i, 25]) == 0? "Select": "Contaminated" : Convert.ToInt32(data[i, 25]) == 6 ?"On Stop": (CancelReasonList.Where(w => w.ID == Convert.ToInt32(data[i , 25])).Select(w => w.Type).FirstOrDefault()),
                CancelReasonID = Convert.ToInt32(data[i , 25]),
                Weight = containerlist[0].Weight_T.ToString(),
                CancelReasonVisibility = Convert.ToInt32(data[i, 25]) == 15 ? false : true,
                TicketNo= data[i , 27],
                Address=data[i,22],
                EmailAddress=data[i,19],
                CustomerOrderId=data[i,29],
                InvoiceName = data[i, 30],
                SpotJob_Visibility = (Globals.Vehicle_Type != 1 && Globals.Vehicle_Type != 11) ? data[i,31] == "1" ?true:false:false
            }) ;
        }

        private static void AddDataToContainerList(int tempRow, string[,] tempData, ObservableCollection<CustomerContainer> containerlist)
        {
            // Iterate though temp array adding data to container list etc.
            for (int k = 0; k < tempRow; k++)
            {

                ObservableCollection<ContainerQtyModel> tempContainer = new ObservableCollection<ContainerQtyModel>();
                ObservableCollection<WasteTypePickerModel> tempContainer_WasteTypeList = new ObservableCollection<WasteTypePickerModel>();
                int Quantity_read = (int.Parse(tempData[k, 7]));
                bool Charge_As_Contract_read = int.Parse(tempData[k, 15]) == 0 ? false : true;
                int Starting_Index = 1; //0

                if (Charge_As_Contract_read == true)
                {
                    Starting_Index = Quantity_read;
                }
                else
                {
                    Starting_Index = 1; //0
                }
                for (int index = Starting_Index; index <= 25; index++)
                {
                    tempContainer.Add(new ContainerQtyModel(index, tempData[k, 21], tempData[k, 9], tempData[k, 4], tempData[k, 8],tempData[k,1])); //21 CustomerId and 9 WasteType and 23 ExtraWasteID
                }

                // add data to extrawaste type list
                tempContainer_WasteTypeList.Add(new WasteTypePickerModel(0, tempData[k, 21], tempData[k, 9], "Default", tempData[k, 4], tempData[k, 8],tempData[k,1]));
                tempContainer_WasteTypeList.Add(new WasteTypePickerModel(1, tempData[k, 21], tempData[k, 9], "Extra Waste", tempData[k, 4], tempData[k, 8], tempData[k, 1]));
                tempContainer_WasteTypeList.Add(new WasteTypePickerModel(2, tempData[k, 21], tempData[k, 9], "Bin Refilled", tempData[k, 4], tempData[k, 8], tempData[k, 1]));

              
                containerlist.Add(new CustomerContainer
                {
                    RunNo = int.Parse(tempData[k, 0]),
                    Qty = Quantity_read,
                    ContainerName = tempData[k, 8],
                    WasteType = tempData[k, 9],
                    Completed = Convert.ToInt32(tempData[k,11])==0?false:true,
                    Weight_T =  tempData[k, 24]!=""?int.Parse(tempData[k, 24].ToString()):0,
                    ChargeAsContract = Charge_As_Contract_read,
                    ContainerListPicker = tempContainer,
                    ExtraWasteTypeList = tempContainer_WasteTypeList,
                    CustomerID = tempData[k, 21],
                    ExtraWasteId = Convert.ToInt32(tempData[k, 23]),
                    ExtraWasteType = Convert.ToInt32(tempData[k, 23]) == -1 ? "Select" : (tempContainer_WasteTypeList.Where(w => w.ID == Convert.ToInt32(tempData[k, 23])).Select(w => w.Type).FirstOrDefault()),
                    IsVisibilityWasteType = Convert.ToInt32(tempData[k, 23]) == -1 ? false :true,
                    SiteName= tempData[k, 4],
                    CPID=tempData[k,1]
                });
            }
        }

        //  Quantity picker change event
        public void PickerValueUpdated(Object pickerVal)
        {
            var CItems = (ContainerQtyModel)((Picker)pickerVal).SelectedItem;
            if (CItems != null)
            {
                string CustomerId = CItems.CustomerId;
                string CustomerWasteType = CItems.WasteType;
                int CustomerSelectedID = CItems.index;
                string SiteName = CItems.SiteName;
                string ContainerType = CItems.ContainerType;
                string CPID = CItems.CPID;
                if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
                {
                    string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    string temprow = "";
                    int item_at_index = 0;
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        var values = lines[i].Split('|');

                        if (values[21] == CustomerId && values[9] == CustomerWasteType && values[4]==SiteName && values[8]== ContainerType && values[1]==CPID)  
                        {
                            item_at_index = i;
                            for (int j = 0; j < values.Length; j++)
                            {
                                if (j == 0)
                                {
                                    temprow = values[j];
                                }
                                else
                                {
                                    temprow += "|" + (j != 7 ? j == 28 ? "1" : values[j] : CustomerSelectedID.ToString());
                                    if (j == values.Length - 1) {
                                        lines[item_at_index] = temprow;
                                    }
                                }
                            }


                        }

                    }

                    var index = 0;
                    foreach (var item in Items.Select((value, i) => new { i, value }))
                    {

                            if (item.value.Job.CustomerContainers.Count > 1)
                            {
                                foreach (var customer in item.value.Job.CustomerContainers)
                                {
                                    if (customer.CustomerID == CItems.CustomerId && customer.WasteType == CItems.WasteType && customer.SiteName== SiteName && customer.ContainerName==ContainerType && customer.CPID==CPID)
                                    {
                                    index = item.i;
                                    if (item.value.CancelReasonID == 0)
                                    {
                                        if (Globals.Vehicle_Type!=11) {
                                        if (Convert.ToInt32(CustomerSelectedID) > customer.Qty)
                                        {
                                            customer.IsVisibilityWasteType = true;
                                        }
                                        else
                                        {
                                            customer.IsVisibilityWasteType = false;
                                            customer.ExtraWasteId = -1;
                                            customer.ExtraWasteType = "Select";
                                            var valueAtRow = lines[item_at_index].Split('|');
                                            for (int j = 0; j < valueAtRow.Length; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    temprow = valueAtRow[j];
                                                }
                                                else
                                                {
                                                    temprow += "|" + (j != 23 ? valueAtRow[j] : "-1");
                                                    if (j == valueAtRow.Length - 1)
                                                    {
                                                        lines[item_at_index] = temprow;

                                                    }
                                                }
                                            }
                                        }

                                    }

                                        customer.Qty = Convert.ToInt32(CustomerSelectedID);
                                    }
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (item.value.Job.CustomerContainers[0].CustomerID == CItems.CustomerId &&
                                item.value.Job.CustomerContainers[0].WasteType == CItems.WasteType && item.value.Job.CustomerContainers[0].SiteName == SiteName && item.value.Job.CustomerContainers[0].ContainerName == ContainerType && item.value.Job.CustomerContainers[0].CPID==CPID)
                                {
                                index = item.i;
                                if (item.value.CancelReasonID == 0)
                                {
                                    if (Globals.Vehicle_Type != 11)
                                    {

                                        if (Convert.ToInt32(CustomerSelectedID) > item.value.Job.CustomerContainers[0].Qty)
                                        {
                                            item.value.Job.CustomerContainers[0].IsVisibilityWasteType = true;
                                        }
                                        else
                                        {
                                            item.value.Job.CustomerContainers[0].IsVisibilityWasteType = false;
                                            item.value.Job.CustomerContainers[0].ExtraWasteId = -1;
                                            item.value.Job.CustomerContainers[0].ExtraWasteType = "Select";
                                            var valueAtRow = lines[item_at_index].Split('|');
                                            for (int j = 0; j < valueAtRow.Length; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    temprow = valueAtRow[j];
                                                }
                                                else
                                                {
                                                    temprow += "|" + (j != 23 ? valueAtRow[j] : "-1");
                                                }
                                            }
                                            lines[item_at_index] = temprow;
                                        }
                                    }
                                    item.value.Job.CustomerContainers[0].Qty = Convert.ToInt32(CustomerSelectedID);
                                }
                                    break;
                                }
                            }
                        
                        if (index > 0) break;
                    }
                    var OldItemHold = _oldJob;
                    ExecuteRefreshItemsCommand(_oldJob);
                    if (Items[index].CancelReasonID == 0)
                    {
                       
                        File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                        File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName, lines);
                        Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName); // for read file from Internal Directory 
                        Globals.ContainerSelectedValue = CustomerSelectedID.ToString();
                    }
                    else 
                    {
                        Application.Current.MainPage.DisplayAlert("Alert", "Unable to update Quantity because job is processed for cancellation.", "OK");
                    }
                    ExecuteRefreshItemsCommand(OldItemHold);

                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");

                }
              
            }
        }

        //  WasteType picker change event
        public async void WasteTypePickerValueUpdated(Object pickerVal)
        {
            var WItems = (WasteTypePickerModel)((Picker)pickerVal).SelectedItem;
            if (WItems != null)
            {
                string CustomerId = WItems.CustomerId;
                string CustomerWasteType = WItems.WasteType;
                int SelectedType = WItems.ID;
                string SiteName = WItems.SiteName;
                string ContainerType = WItems.ContainerType;
                string CPID = WItems.CPID;
                if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
                {
                    string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    string temprow = "";
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        var values = lines[i].Split('|');

                        if (values[21] == CustomerId && values[9] == CustomerWasteType && values[4]== SiteName && values[8]== ContainerType && values[1]==CPID)
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
                                    temprow += "|" + (j != 23 ? (j == 28 ? "1" : values[j] ): SelectedType.ToString());

                                    if (j == values.Length - 1) {
                                        lines[item_at_index] = temprow;
                                    }
                                }
                            }
                          //  break;
                        }

                    }


                    var index = 0;
                    foreach (var item in Items.Select((value, i) => new { i, value }))
                    {
                        if (item.value.Job.CustomerContainers.Count > 1)
                        {
                            foreach (var customer in item.value.Job.CustomerContainers)
                            {
                                if (customer.CustomerID == WItems.CustomerId && customer.WasteType == WItems.WasteType && customer.SiteName== SiteName && customer.ContainerName==ContainerType && customer.CPID==CPID)
                                {
                                    index = item.i;
                                    if (item.value.CancelReasonID == 0)
                                    {
                                        customer.ExtraWasteId = WItems.ID;
                                        customer.ExtraWasteType = WItems.Type;
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (item.value.Job.CustomerContainers[0].CustomerID == WItems.CustomerId &&
                            item.value.Job.CustomerContainers[0].WasteType == WItems.WasteType && item.value.Job.CustomerContainers[0].SiteName== SiteName && item.value.Job.CustomerContainers[0].ContainerName == ContainerType && item.value.Job.CustomerContainers[0].CPID==CPID)
                            {
                                index = item.i;
                                if (item.value.CancelReasonID == 0)
                                {
                                    item.value.Job.CustomerContainers[0].ExtraWasteId = WItems.ID;
                                    item.value.Job.CustomerContainers[0].ExtraWasteType = WItems.Type;
                                }
                                break;
                            }
                        }
                        if (index > 0) break;
                    }
                    var OldItemHold = _oldJob;
                    ExecuteRefreshItemsCommand(_oldJob);
                    if (Items[index].CancelReasonID == 0)
                    {
                        File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                        File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName, lines);
                        Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName); // for read file from Internal Directory   
                    }
                    else {
                        Application.Current.MainPage.DisplayAlert("Alert", "Unable to update waste type because job is processed for cancellation.", "OK");
                    }
                    ExecuteRefreshItemsCommand(OldItemHold);

                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");
                } 
            }
        }


        // CancelReason Picker OnchangeEvent
        public void CancelReasonPickerValueUpdated(Object pickerVal) {
            var SelectedItemRec = (CancelReasonList)((Picker)pickerVal).SelectedItem;
            if (SelectedItemRec != null)
            {
                string CustomerId = SelectedItemRec.CustomerId;
                int SelectedReasonID = SelectedItemRec.ID;
                string SelectedReasonType = SelectedItemRec.Type;
                string SelectedSite = SelectedItemRec.SiteName;
                int countOfItems = _oldJob.Count();
                var OldItemHold = _oldJob;
                if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
                {
                    string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    string temprow = "";
                    for (int i = 0; i < lines.Count(); i++)
                    {

                        var values = lines[i].Split('|');

                        for (int indexOfSelectedItems = 0; indexOfSelectedItems < countOfItems; indexOfSelectedItems++)
                        {

                            if (values[21] == CustomerId && values[4] == SelectedSite && values[1]== OldItemHold[indexOfSelectedItems].CustomerContainer.CPID)  // && values[9]== SelectedItemRec.WasteType
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
                                        temprow += "|" + (j != 25 ? (j != 26 ? (j == 28? "1" : values[j] ): date_time) : SelectedReasonID.ToString()); // 28 complete column
                                        if (j == values.Length - 1) {
                                            lines[item_at_index] = temprow;

                                        }
                                    }
                                }
                               //  break;
                            }
                        }
                    }
                    foreach (var item in Items.Select((value, i) => new { i, value }))
                    {
                        /*   if (item.value.Job.CustomerContainers.Count > 1)
                           {
                               foreach (var customer in item.value.Job.CustomerContainers)
                               {
                                   if (customer.CustomerID == SelectedItemRec.CustomerId && customer.SiteName== SelectedItemRec.SiteName)
                                   {
                                       item.value.Job.CancelReasonID = SelectedItemRec.ID;
                                       item.value.Job.CancelReasonType = SelectedItemRec.Type;
                                       item.value.Job.CancelReasonVisibility = false;
                                   }
                               }
                           }
                           else
                           {*/
                        for (int indexOfSelectedItems = 0; indexOfSelectedItems < countOfItems; indexOfSelectedItems++)
                        {
                            if (item.value.Job.CustomerContainers[0].CustomerID == SelectedItemRec.CustomerId && item.value.Job.CustomerContainers[0].SiteName == SelectedItemRec.SiteName && item.value.Job.CustomerContainers[0].CPID== OldItemHold[indexOfSelectedItems].CustomerContainer.CPID)  // && item.value.Job.CustomerContainers[0].WasteType == SelectedItemRec.WasteType
                            {
                                item.value.Job.Previous_CancelReasonID= SelectedItemRec.ID;
                                item.value.Job.Previous_CancelReasonType= SelectedItemRec.Type;
                                item.value.Job.CancelReasonID = SelectedItemRec.ID;
                                item.value.Job.CancelReasonType = SelectedItemRec.Type;
                              //  item.value.Job.CancelReasonVisibility = false; // for set Isenable property of Cancel reason // comment in UI for now
                                break;
                            }
                            //
                        }
                        }
                    ExecuteRefreshItemsCommand(_oldJob);
                    File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName, lines);
                    Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName); // for read file from Internal Directory 
                    ExecuteRefreshItemsCommand(_oldJob);
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");

                }

            }
        }
        public async Task markJobAsDone_EventAsync( bool Flag_withoutEmail,bool Flag_spotjob) {
            // var a = Globals.Customer_WasteType_at_SelectedIndex; // current selected index's items

            //var a = _oldJob;

            try
            {
                if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
                {
                    string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        var values = lines[i].Split('|');

                        foreach (var SelectedJob_Item in Globals.Job_at_SelectedIndex.CustomerContainers)
                        {
                            if (values[21] == Globals.Customer_WasteType_at_SelectedIndex[0].CustomerID && values[4] == Globals.Customer_WasteType_at_SelectedIndex[0].SiteName && values[1] == SelectedJob_Item.CPID)  //  CustomerID check
                            { 
                                                                                                                                                                     //   && values[9] == Globals.Customer_WasteType_at_SelectedIndex[0].WasteType
                                    int item_at_index = i;
                                    string temprow = "";
                                    for (int j = 0; j < values.Length; j++)
                                    {
                                        if (j == 0)
                                        {
                                            temprow = values[j];
                                        }
                                        else
                                        {
                                            if (j != 11)
                                            {
                                                temprow += "|" + values[j];
                                                if (j == values.Length - 1)
                                                {
                                                    lines[item_at_index] = temprow;
                                                }
                                            }
                                            else
                                            {
                                            // Flag_spotjob &&
                                            if ( values[27] == Globals.Job_at_SelectedIndex.TicketNo)
                                            {

                                                temprow += "|" + '1';

                                            }
                                            else {
                                                temprow += "|" + values[j];
                                            }
                                        }
                                            // temprow += "|" + (j != ? values[j] :'1'); // completed status 11 default value 0

                                        }
                                    }
                                    //  lines[item_at_index] = temprow;
                                    //   break;

                                
                              
                        }
                        }
                    }

                    File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName, lines);
                    Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName); // for read file from Internal Directory
                    if (!Flag_withoutEmail) {
                        Application.Current.MainPage.Navigation.PopAsync();
                        Application.Current.MainPage.Navigation.PopAsync();
                        Application.Current.MainPage.DisplayAlert("Job Finished", "Mail sent and job marked as Finished.", "OK");

                    } else {
                       
                        Application.Current.MainPage.DisplayAlert("Job Finished", "Job Finished without sending Mail", "OK");

                    }
                    App.Current.MainPage.Navigation.PushAsync(new Jobs(new JobsGroupViewModel()));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");

                }
            }
            catch (Exception ex) {
                Application.Current.MainPage.DisplayAlert("Alert", "Try Again !", "OK");
            }

        }

        private int _weight;

        public int Weight
        {
            get { return _weight; }
            set
            {
                if (_weight != value)
                {
                    _weight = value;

                    // Update the weight in the selected CustomerContainer
                    UpdateCustomerContainerWeight(_weight);
                }
            }
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


                    // Set the weight for the selected container
                    Weight = (int)(_selectedCustomerContainer?.Weight_T);
                }
            }
        }

        private void UpdateCustomerContainerWeight(int newWeight)
        {
            if (_selectedCustomerContainer != null)
            {
                _selectedCustomerContainer.Weight_T = newWeight;
            }
        }

    }
}
