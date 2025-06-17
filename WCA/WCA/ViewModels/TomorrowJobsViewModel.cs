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

namespace WCA.ViewModels
{
    public class TomorrowJobsViewModel : BaseViewModel
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
        public TomorrowJobsViewModel()
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
                string[] lines = Globals.CsvText_TomorrowJobs.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                int lineCount = lines.Length;
                cols = lines[0].Split('|').Length;
                string[,] data = new string[lineCount, cols];

                for (int i = 0; i < lineCount; i++)
                {
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

                            if (data[i, siteCol] != data[i + 1, siteCol])
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
                        else
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

                            }
                        }
                        //}

                    }

                }

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
                CancelReasonList.Add(new CancelReasonList(0, data[i, 21], data[i, 9], "Select", data[i, 4], data[i, 1]));
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
                            if (Convert.ToInt32(data_l[i_l, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0]) != 6)
                            {

                                CancelReasonList.Add(new CancelReasonList(Convert.ToInt32(data_l[i_l, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0]), data[i, 21], data[i, 9], data_l[i_l, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1], data[i, 4], data[i, 1]));
                            }
                        }
                    }
                }
            }
            // add data to joblist
            joblist.Add(new Job()
            {
                Customer = data[i, 3],
                Site = data[i, 4],
                Town = data[i, 5],
                PostCode = data[i, 6],
                DriverNote = data[i, 10],
                CustomerContainers = containerlist,
                JobCancelReasonList = CancelReasonList,
                CancelReasonType = Convert.ToInt32(data[i, 25]) == 0 || Convert.ToInt32(data[i, 25]) == 15 ? Convert.ToInt32(data[i, 25]) == 0 ? "Select" : "Contaminated" : Convert.ToInt32(data[i, 25]) == 6 ? "On Stop" : (CancelReasonList.Where(w => w.ID == Convert.ToInt32(data[i, 25])).Select(w => w.Type).FirstOrDefault()),
                CancelReasonID = Convert.ToInt32(data[i, 25]),
                Weight = containerlist[0].Weight_T.ToString(),
                CancelReasonVisibility = Convert.ToInt32(data[i, 25]) == 15 ? false : true,
                TicketNo = data[i, 27],
                Address = data[i, 22],
                EmailAddress = data[i, 19]
            });
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
                int Starting_Index = 0;

                if (Charge_As_Contract_read == true)
                {
                    Starting_Index = Quantity_read;
                }
                else
                {
                    Starting_Index = 0;
                }
                for (int index = Starting_Index; index <= 25; index++)
                {
                    tempContainer.Add(new ContainerQtyModel(index, tempData[k, 21], tempData[k, 9], tempData[k, 4], tempData[k, 8], tempData[k, 1])); //21 CustomerId and 9 WasteType and 23 ExtraWasteID
                }

                // add data to extrawaste type list
                tempContainer_WasteTypeList.Add(new WasteTypePickerModel(0, tempData[k, 21], tempData[k, 9], "Default", tempData[k, 4], tempData[k, 8], tempData[k, 1]));
                tempContainer_WasteTypeList.Add(new WasteTypePickerModel(1, tempData[k, 21], tempData[k, 9], "Extra Waste", tempData[k, 4], tempData[k, 8], tempData[k, 1]));
                tempContainer_WasteTypeList.Add(new WasteTypePickerModel(2, tempData[k, 21], tempData[k, 9], "Bin Refilled", tempData[k, 4], tempData[k, 8], tempData[k, 1]));


                containerlist.Add(new CustomerContainer
                {
                    RunNo = int.Parse(tempData[k, 0]),
                    Qty = Quantity_read,
                    ContainerName = tempData[k, 8],
                    WasteType = tempData[k, 9],
                    Completed = Convert.ToInt32(tempData[k, 11]) == 0 ? false : true,
                    Weight_T = tempData[k, 24] != "" ? int.Parse(tempData[k, 24].ToString()) : 0,
                    ChargeAsContract = Charge_As_Contract_read,
                    ContainerListPicker = tempContainer,
                    ExtraWasteTypeList = tempContainer_WasteTypeList,
                    CustomerID = tempData[k, 21],
                    ExtraWasteId = Convert.ToInt32(tempData[k, 23]),
                    ExtraWasteType = Convert.ToInt32(tempData[k, 23]) == -1 ? "Select" : (tempContainer_WasteTypeList.Where(w => w.ID == Convert.ToInt32(tempData[k, 23])).Select(w => w.Type).FirstOrDefault()),
                    IsVisibilityWasteType = Convert.ToInt32(tempData[k, 23]) == -1 ? false : true,
                    SiteName = tempData[k, 4],
                    CPID = tempData[k, 1]
                });
            }
        }

    }
}
