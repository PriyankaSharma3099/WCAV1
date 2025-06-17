using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WCA.Models
{
    public class Job
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string DriverNote { get; set; }
        public string TicketNo { get; set; }
        public string CancelReasonType { get; set; }
        public string Town { get; set; }
        public bool CancelReasonVisibility { get; set; }
        public bool WeightVisibility { get; set; }
        public bool SpotJob_Visibility { get; set; }
        public bool SpotJob_Visibility_Flag { get; set; }
        public int CancelReasonID { get; set; }
        public int Previous_CancelReasonID { get; set; }
        public string Previous_CancelReasonType { get; set; }
        public string Weight { get; set; }
        public string Customer { get; set; }
        public string CPID { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string CustomerOrderId { get; set; }
        public string InvoiceName { get; set; }
        public string Site { get; set; }
        public ObservableCollection<CustomerContainer> CustomerContainers { get; set; }
        public ObservableCollection<CancelReasonList> JobCancelReasonList
        {
            get;
            set;
        }
        public bool IsVisible { get; set; } = false;
        public bool OnStop { get; set; }
        public string PostCode { get; set; }

        public Job()
            {
            }
        public Job(string cpid,string customer, string site,string town,string postcode, string driver_note, ObservableCollection<CustomerContainer> customer_containers, ObservableCollection<CancelReasonList> jobcancelreasonlist,string cancelreasontype, int cancelreasonid,string weight,bool cancelreasonvisibility,string ticket_no,string address,string emailaddress,string customer_orderid,string invoice_name,bool spot_job_visibility)
        {
            CPID = cpid;
            Previous_CancelReasonType = cancelreasontype;
            Previous_CancelReasonID = cancelreasonid;
            Customer = customer;
            Site = site;
            Town = town;
            PostCode = postcode;
            DriverNote = driver_note;
            CustomerContainers = customer_containers;
            CancelReasonType = cancelreasontype;
            CancelReasonID = cancelreasonid;
            JobCancelReasonList = jobcancelreasonlist;
            Weight = weight;
            CancelReasonVisibility = cancelreasonvisibility;
            TicketNo = ticket_no;
            Address = address;
            EmailAddress = emailaddress;
            CustomerOrderId = customer_orderid;
            InvoiceName = invoice_name;
            WeightVisibility = false;
            SpotJob_Visibility = spot_job_visibility;
            SpotJob_Visibility_Flag = false;
        }
        public Job(string ticket_no, string customer, string site, string driver_note, bool on_stop, string post_code, ObservableCollection<CancelReasonList> jobcancelreasonlist, string cancelreasontype, int cancelreasonid, string weight,bool cancelreasonvisibility)
        {
            TicketNo = ticket_no;
            Customer = customer;
            Site = site;
            DriverNote = driver_note;
            OnStop = on_stop;
            PostCode = post_code;
           // Town = town;
            CancelReasonType = cancelreasontype;
            CancelReasonID = cancelreasonid;
            JobCancelReasonList = jobcancelreasonlist;
            Weight = weight;
            CancelReasonVisibility = cancelreasonvisibility;
        }
    }
}