using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WCA.Models;
using Xamarin.Forms;
using SignaturePad.Forms;
using System.Collections.Generic;
using System.Windows.Input;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;
using Xamarin.Essentials;

namespace WCA.ViewModels
{
    //[QueryProperty(nameof(JobId), nameof(JobId))]
    public class JobDetailViewModel : BaseViewModel
    {
        private string jobId;
        private string customer;
        private string site;
        private string weight;
        private bool signaturepadvisibility;
        private bool spot_job_visibility;
        private bool signaturefoundvisibility;
        private bool isntsmode;
        private bool isimageattachmode;
        private string customer_wastetype;
        private int containertotal_qty;
     //   private int containertotalweight;
        private string container_type_detailpage;
    //    private string container_qty_detailpage;

		//==========================================
		#region Signature
		private IEnumerable<Point> currentSignature;
        private Point[] savedSignature;
		public JobDetailViewModel(Func<SignatureImageFormat, ImageConstructionSettings, Task<Stream>> getImageDelegate)
		{
			GetImageStreamAsync = getImageDelegate;
			/*SaveVectorCommand = new Command(OnSaveVector);
			LoadVectorCommand = new Command(OnLoadVector);*/
			SaveImageCommand = new Command(OnSaveImage);
		}
        public JobDetailViewModel()
        {
        }
        public IEnumerable<Point> CurrentSignature
		{
			get => currentSignature;
			set
			{
				currentSignature = value;
				OnPropertyChanged();
			}
		}
		public Point[] SavedSignature
		{
			get => savedSignature;
			set
			{
				savedSignature = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasSavedSignature));
			}
		}
		public bool HasSavedSignature => SavedSignature?.Length > 0;
	/*	public ICommand SaveVectorCommand { get; }
		public ICommand LoadVectorCommand { get; }*/
		public ICommand SaveImageCommand { get; }
		private Func<SignatureImageFormat, ImageConstructionSettings, Task<Stream>> GetImageStreamAsync { get; }
	/*	private void OnSaveVector()
		{
			SavedSignature = CurrentSignature.ToArray();
			DisplayAlert("Vector signature saved to memory.");
		}
		private void OnLoadVector()
		{
			CurrentSignature = SavedSignature;
		}*/
		private async void OnSaveImage()
		{
			var settings = new ImageConstructionSettings
			{
				StrokeColor = System.Drawing.Color.Black,
				BackgroundColor = System.Drawing.Color.White,
				StrokeWidth = 1f
			};
			using (var bitmap = await GetImageStreamAsync(SignatureImageFormat.Png, settings))
			{
				var saved = await App.SaveSignature(bitmap, "signature.png");
				if (saved)
					DisplayAlert("Signature saved.");
				else
					DisplayAlert("There was an error saving the signature.");
			}
		}
		private void DisplayAlert(string message)
		{
			Application.Current.MainPage.DisplayAlert("Signature Pad", message, "OK");
		}

		public bool PDfTicket_Generate() {

            /*   var appFolder_cache = FileSystem.CacheDirectory;
               string SignatureAndPath = System.IO.Path.Combine(appFolder_cache, "signature.png");
               if (File.Exists(SignatureAndPath))
               {
                   Console.WriteLine("signature exist");
               }*/

            // Generate PDF using HTML
            //Console.WriteLine("Waste Types ===> "+Globals.TicketData[0].WasteTypeString);
          
            try
            {
                string CPID = Globals.TicketData[0].CPID;
                string CustomerName = Globals.TicketData[0].CustomerName;
                string InvoiceName = Globals.TicketData[0].Invocie_Name;
                string TicketNo = Globals.TicketData[0].TicketNo;
                string CompleteAddress = Globals.TicketData[0].CompleteAddress;
                string TotalWeight = " ";
                string CustomerOrderId = Globals.TicketData[0].CustomerOrderId;
                //Console.WriteLine("Customer Order Id ===============> "+CustomerOrderId);
                bool isCheck_Sign_Avl = false;
                string path = Globals.storagePath + "/WCA";
                string SignaturesPath = path + "/Signatures/" + CustomerName + TicketNo +CPID+ ".png";  // signature read path

                // check for signature exist or not
                if (File.Exists(SignaturesPath))
                {
                    isCheck_Sign_Avl = true;
                }
                else
                {
                    isCheck_Sign_Avl = false;
                }
                string TicketsPath = path + "/Tickets/"; // directory where we save ticket
                // generate PDF ticket for customer without weight
                string pdfPath_Customer = System.IO.Path.Combine(TicketsPath, "Ticket_" + CustomerName + "_" + TicketNo + ".pdf"); // save ticket path
                if (File.Exists(pdfPath_Customer))
                {
                    File.Delete(pdfPath_Customer);
                }
                System.IO.FileStream fs = new FileStream(pdfPath_Customer, FileMode.Create);
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                HTMLWorker worker = new HTMLWorker(document);
                document.Open();
                StringBuilder html = new StringBuilder();

                if (Globals.Vehicle_Type == 2 || Globals.Vehicle_Type == 3)
                {

                    // Ticket of Customer for Vehicle Type 2 and 3

                    html.Append("< !DOCTYPE html >\r\n < html lang =\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=., initial-scale=1.0\">\r\n</head>\r\n\r\n<body>\r\n    <div>\r\n        <div>\r\n            <p style=\"font-size: 12px;color:black;text-align:right; \">\r\n  <span style=\"word-break:break-all\">\r\n      <span style=\"font-size:1px;color:#00FFFFFF;\"><span style=\"font-size:1px;color:#00FFFFFF;\"></span></span>\r\n                    <!-- this contact Used -->\r\n                      " + TicketNo + "\r\n                    <!-- end -->\r\n                    <span style=\"font-size:5px;color:white\">\r\n                   ....... .  <span style=\"font-size:5px;color:#008dc9\">........</span>\r\n<span style=\"font-size:11px;color:white\">00</span>\r\n                        </span>\r\n                </span>\r\n            </p>\r\n            <!-- this contact Used -->\r\n            <p style=\"font-size:0.5px;color:white;\">space tag using</P>\r\n            <div style=\"text-align: center; width: 10%;margin: auto;color:#fff;font-weight: 900;\">\r\n                <h5 style=\"margin: 0;font-size:10px;\">IRWELL STREET METAL CO. LTD.</h5>\r\n                <p style=\" margin-bottom: 0;font-size:10px;\">KENYON STREET, RAMSBOTTOM, BURY, LANCS. BL0 0AB.</p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">Tel: <span>(01706) 824344 &amp; 823001</span> Fax:\r\n                    <span>(01706) 828517</span>\r\n                </p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">email:<span> mailto:info@ismwaste.co.uk </span> web:\r\n                    <span>https://www.ismwaste.co.uk </span>\r\n                </p>\r\n            </div>\r\n            <!-- end -->\r\n            <div style=\"clear: both;\"></div>\r\n            <div style=\"text-align: center;\">\r\n                <img src=\"" + Globals.storagePath + "/WCA/images/top_bg.png\" alt=\"\">\r\n            </div>\r\n        </div>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + InvoiceName + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ..........................................................................................................................................................\r\n        </p>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + CompleteAddress + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ...................................................................................................................... Date:- " + DateTime.Now.ToString("dd-MM-yyyy") + "\r\n        </p>\r\n        <p style=\"font-size:0.2px;color:white;\">space tag using</P>\r\n\r\n    </div>\r\n    <!-- this contact Used -->\r\n    <div style=\"text-align: center;\">\r\n        <div style=\"position: absolute;text-align: center;\">\r\n            <table border=\"1\" style=\"width:100%;font-size:10px;\">\r\n                <tr>\r\n                    <td colspan=\"1\"> Container </td>\r\n                    <td colspan=\"4\"> Waste Type </td>\r\n                 </tr>\r\n                <tr>\r\n                    <td colspan=\"1\"> 25 OPEN </td>\r\n                    <td colspan=\"4\"> " + Globals.TicketData[0].OPEN_25_WasteType.ToString() + " </td>\r\n                </tr>\r\n                <tr>\r\n                    <td colspan=\"1\"> COMP35 </td>\r\n                    <td colspan=\"4\"> " + Globals.TicketData[0].COMP35_WasteType.ToString() + "  </td>\r\n                   </tr>\r\n                <tr>\r\n                    <td colspan=\"1\"> ENCL35 </td>\r\n                    <td colspan=\"4\"> " + Globals.TicketData[0].ENCL35_WasteType.ToString() + "  </td>\r\n                   </tr>\r\n                <tr>\r\n                    <td colspan=\"1\"> 40 OPEN </td>\r\n                    <td colspan=\"4\"> " + Globals.TicketData[0].OPEN_40_WasteType.ToString() + "  </td>\r\n        </tr>\r\n\r\n     </table> <table border=\"1\" style=\"width:100%;font-size:10px;\">\r\n            <tr style=\"height: 10pt\">\r\n                    <td style=\"width:50%;\"> DRIVER </td>\r\n                    <td style=\"width:50%;\">\r\n  DISPOSAL SITE                   </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td  style=\"height: 50%, widthg:50%\"> " + Globals.DriverSelectedValue + " </td>\r\n                    <td style=\"width:50%;\">\r\n    Transfer Station    </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td style=\"width:50%;\"> VEH. REG. No. </td>\r\n                    <td style=\"width:50%;\">\r\n                      CUSTOMER ORDER NO.                   </td>\r\n                </tr>\r\n                  <tr style=\"height: 15pt\">\r\n                    <td style=\"height: 50%,width:50%\"> " + Globals.VehicleSelectedValue + " </td>\r\n                    <td style=\"width:50%;\">\r\n  " + CustomerOrderId + "\r\n                    </td>\r\n                </tr>\r\n                <tr style=\"height: 10pt\">\r\n                   </table>\r\n        </div>\r\n        <!-- bg img -->\r\n        <img src=\"" + Globals.storagePath + "/WCA/images/center_img.png\" style=\"width:1200px\" alt=\"\">\r\n    </div>\r\n    <!-- end -->\r\n    <div>\r\n        <div><p style =\"font-size: 10px;text-align:center;font-weight:bold;\">ANY DISCREPANCY MUST BE NOTIFIED TO US IMMEDIATELY</p><p style=\"font-size: 10px;font-weight:700;\">CONDITIONS OF HIRE      <span style=\"font-size: 9px; color:white;\">..........................</span> ALL SERVICES ARE SUBJECT TO OUR TERMS AND CONDITIONS </p><span style=\"font-size: 8px;line-height:12;\">All our services are supplied subject to our Terms and Conditions, available on request. Skips dropped on public highways/drives/private land are the hirer's own risk.</span>   <p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">RESPONSIBILITY.</span> Hirer accepts all responsibility in respect of all claims for damage to  person or property while siting,collecting or on site. <span style=\"font-size: 9px;font-weight:bold;\">OVERLOADING.</span> No container must be loaded dangerously or in excess of G.V.W.Fire and any damages/loss of equipment will be charged for, plus loss of use. Void journey(s) Chargeable.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">HIRE PERIOD.</span> Skips placed on any LocalAuthority Council Highway will be subject to a minimum hire period of not less than one month (31 days) and the company or person in possession of any skip will become the  owner  for the purpose of the Highways Act 1980 section 139 & 140 and Builders Skip Marking Regulations 1984.</p><span style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">SKIP RENTALS.</span> Each skip hired will be subject to rental charge of £0.50 per day or Euro equivalent (when legal  tender) after the first seven days hire.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">LIGHTS / CONES / SKIP MARKINGS.</span> In accordance with Highways Act 1980,  Section 139 & 140 and the Builders Skips Marking Regulations 1984, along with any amendments and any other statutory requirements. The Hirer  is responsible for ensuring adequate skip markings, obtaining and  placing of lamps, cones, lighting of lamps and maintenance of same and also obtaining and renewing and  Highways permission/skip permits from the Local Authority  Council Highways Department.</span> <p style=\"font-size: 8px;font-weight:bold;line-height:12\"><i>I have read and accept the additional Conditions of Hire as printed above.</i></p>     </div>\r\n        <table>\r\n            <tr>\r\n                <td colspan=\"3\" style=\"text-align:center;\">\r\n                                      ");
                    if (isCheck_Sign_Avl)
                        html.Append("<img src=\"" + SignaturesPath + "\" width=\"100px\" height=\"40%\"  alt=\"\">\r\n   ");
                    else
                        html.Append("<div style=\"width:200px; heigth:80%;\"> <p style=\"font-size:18px;\">NTS</p><p style=\"color:white;font-size:4.8px;\">sign</p></div>");
                    html.Append("         </td>\r\n                <td colspan=\"4\" style=\"text-align:center;\">\r\n\r\n");
                    html.Append(" </td>\r\n            </tr>\r\n        </table>\r\n <p style=\"font-size: 9px;\">Signed ..................................................................................................  Signed by Hirer /Customer  </p>       <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">\r\n            Registered Waste Carrier CB/DU96892</p>\r\n        <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">Licenced\r\n            Special Waste\r\n            Transfer Station RD/LIC/255/82</p>\r\n    </div>\r\n</body>\r\n\r\n</html>\r\n");
                }
                else if (Globals.Vehicle_Type == 1)
                {
                    // Ticket of Cusotmer for Vehicle Type 1

                    html.Append("< !DOCTYPE html >\r\n < html lang =\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=., initial-scale=1.0\">\r\n</head>\r\n\r\n<body>\r\n    <div>\r\n        <div>\r\n            <p style=\"font-size: 12px;color:black;text-align:right; \">\r\n  <span style=\"word-break:break-all\">\r\n      <span style=\"font-size:1px;color:#00FFFFFF;\"><span style=\"font-size:1px;color:#00FFFFFF;\"></span></span>\r\n                    <!-- this contact Used -->\r\n                      " + TicketNo + "\r\n                    <!-- end -->\r\n                    <span style=\"font-size:5px;color:white\">\r\n                   ....... .  <span style=\"font-size:5px;color:#008dc9\">........</span>\r\n<span style=\"font-size:11px;color:white\">00</span>\r\n                        </span>\r\n                </span>\r\n            </p>\r\n            <!-- this contact Used -->\r\n            <p style=\"font-size:0.5px;color:white;\">space tag using</P>\r\n            <div style=\"text-align: center; width: 10%;margin: auto;color:#fff;font-weight: 900;\">\r\n                <h5 style=\"margin: 0;font-size:10px;\">IRWELL STREET METAL CO. LTD.</h5>\r\n                <p style=\" margin-bottom: 0;font-size:10px;\">KENYON STREET, RAMSBOTTOM, BURY, LANCS. BL0 0AB.</p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">Tel: <span>(01706) 824344 &amp; 823001</span> Fax:\r\n                    <span>(01706) 828517</span>\r\n                </p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">email:<span> mailto:info@ismwaste.co.uk </span> web:\r\n                    <span>https://www.ismwaste.co.uk </span>\r\n                </p>\r\n            </div>\r\n            <!-- end -->\r\n            <div style=\"clear: both;\"></div>\r\n            <div style=\"text-align: center;\">\r\n                <img src=\"" + Globals.storagePath + "/WCA/images/top_bg.png\" alt=\"\">\r\n            </div>\r\n        </div>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + InvoiceName + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ..........................................................................................................................................................\r\n        </p>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + CompleteAddress + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ...................................................................................................................... Date:- " + DateTime.Now.ToString("dd-MM-yyyy") + "\r\n        </p>\r\n        <p style=\"font-size:0.2px;color:white;\">space tag using</P>\r\n\r\n    </div>\r\n    <!-- this contact Used -->\r\n    <div style=\"text-align: center;\">\r\n        <div style=\"position: absolute;text-align: center;\">\r\n            <table border=\"1\" style=\"width:100%;font-size:10px; background:red; \">\r\n                <tr>\r\n                    <td> Container </td>\r\n                    <td> General </td>\r\n                    <td> DMR </td>\r\n                    <td> Cardboard </td>\r\n                    <td> Container </td>\r\n                    <td> General </td>\r\n                    <td> DMR </td>\r\n                    <td> Cardboard </td>\r\n                    <td> Glass </td>\r\n  <td> Food </td>\r\n                 </tr>\r\n                <tr>\r\n                    <td> 08 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL08_General + " </td>\r\n                    <td> " + Globals.TicketData[0].REL08_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL08_Cardboard + "  </td>\r\n                    <td> WB 1100 </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_General + " </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_DMR + " </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_Cardboard + " </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_Glass + " </td>\r\n  <td> " + Globals.TicketData[0].WB1100_Food + " </td>\r\n                 </tr>\r\n                <tr>\r\n                    <td> 12 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL12_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL12_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL12_Cardboard + "  </td>\r\n                    <td> WB 660 </td>\r\n                    <td> " + Globals.TicketData[0].WB660_General + " </td>\r\n                    <td> " + Globals.TicketData[0].WB660_DMR + " </td>\r\n                    <td> " + Globals.TicketData[0].WB660_Cardboard + " </td>\r\n                    <td> " + Globals.TicketData[0].WB660_Glass + " </td>\r\n        <td> " + Globals.TicketData[0].WB660_Food + " </td>\r\n           </tr>\r\n                <tr>\r\n                    <td> 14 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL14_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL14_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL14_Cardboard + "  </td>\r\n                    <td> WB 240 </td>\r\n                    <td> " + Globals.TicketData[0].WB240_General + " </td>\r\n                    <td> " + Globals.TicketData[0].WB240_DMR + " </td>\r\n                    <td> " + Globals.TicketData[0].WB240_Cardboard + " </td>\r\n                    <td> " + Globals.TicketData[0].WB240_Glass + " </td>\r\n           <td> " + Globals.TicketData[0].WB240_Food + " </td>\r\n        </tr>\r\n                <tr>\r\n                    <td> 16 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL16_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL16_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL16_Cardboard + "  </td>\r\n                    <td style=\"font-size:8px;\"> Extra Waste </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_General == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_DMR == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_Cardboard == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_Glass == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n    <td> " + ((Globals.TicketData[0].ExtraWaste_Food == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n               </tr>\r\n\r\n                <tr style=\"height: 10pt\">\r\n                    <td colspan=\"4\"> DRIVER </td>\r\n                    <td colspan=\"6\" rowspan=\"6\" b>\r\n  DISPOSAL SITE                   </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"4\" style=\"height: 50%\"> " + Globals.DriverSelectedValue + " </td>\r\n                    <td colspan=\"6\" rowspan=\"6\">\r\n  Transfer Station  </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"4\"> VEH. REG. No. </td>\r\n                    <td colspan=\"6\" rowspan=\"6\">\r\n                      CUSTOMER ORDER NO.                   </td>\r\n                </tr>\r\n                  <tr style=\"height: 15pt\">\r\n                    <td colspan=\"4\" style=\"height: 50%\"> " + Globals.VehicleSelectedValue + " </td>\r\n                    <td colspan=\"6\" rowspan=\"6\" style=\"height: 10pt\">\r\n  " + CustomerOrderId + "  \r\n                  </td>\r\n                </tr>\r\n                <tr style=\"height: 10pt\">\r\n                   </table>\r\n        </div>\r\n        <!-- bg img -->\r\n        <img src=\"" + Globals.storagePath + "/WCA/images/center_img.png\" style=\"width:1200px\" alt=\"\">\r\n    </div>\r\n    <!-- end -->\r\n    <div>\r\n        <div><p style =\"font-size: 10px;text-align:center;font-weight:bold;\">ANY DISCREPANCY MUST BE NOTIFIED TO US IMMEDIATELY</p><p style=\"font-size: 10px;font-weight:700;\">CONDITIONS OF HIRE      <span style=\"font-size: 9px; color:white;\">..........................</span> ALL SERVICES ARE SUBJECT TO OUR TERMS AND CONDITIONS </p><span style=\"font-size: 8px;line-height:12;\">All our services are supplied subject to our Terms and Conditions, available on request. Skips dropped on public highways/drives/private land are the hirer's own risk.</span>   <p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">RESPONSIBILITY.</span> Hirer accepts all responsibility in respect of all claims for damage to  person or property while siting,collecting or on site. <span style=\"font-size: 9px;font-weight:bold;\">OVERLOADING.</span> No container must be loaded dangerously or in excess of G.V.W.Fire and any damages/loss of equipment will be charged for, plus loss of use. Void journey(s) Chargeable.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">HIRE PERIOD.</span> Skips placed on any LocalAuthority Council Highway will be subject to a minimum hire period of not less than one month (31 days) and the company or person in possession of any skip will become the  owner  for the purpose of the Highways Act 1980 section 139 & 140 and Builders Skip Marking Regulations 1984.</p><span style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">SKIP RENTALS.</span> Each skip hired will be subject to rental charge of £0.50/day or Euro equivalent (when legal  tender) after the first seven days hire.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">LIGHTS / CONES / SKIP MARKINGS.</span> In accordance with Highways Act 1980,  Section 139 & 140 and the Builders Skips Marking Regulations 1984, along with any amendments and any other statutory requirements. The Hirer  is responsible for ensuring adequate skip markings, obtaining and  placing of lamps, cones, lighting of lamps and maintenance of same and also obtaining and renewing and  Highways permission/skip permits from the Local Authority  Council Highways Department.</span> <p style=\"font-size: 8px;font-weight:bold;line-height:12\"><i>I have read and accept the additional Conditions of Hire as printed above.</i></p>     </div>\r\n        <table>\r\n            <tr>\r\n                <td colspan=\"3\" style=\"text-align:center;\">\r\n                                      ");
                    if (isCheck_Sign_Avl)
                        html.Append("<img src=\"" + SignaturesPath + "\" width=\"100px\" height=\"40%\"  alt=\"\">\r\n   ");
                    else
                        html.Append("<div style=\"width:200px; heigth:80%;\"> <p style=\"font-size:18px;\">NTS</p><p style=\"color:white;font-size:4.8px;\">sign</p></div>");
                    html.Append("         </td>\r\n                <td colspan=\"4\" style=\"text-align:center;\">\r\n\r\n");
                    html.Append(" </td>\r\n            </tr>\r\n        </table>\r\n <p style=\"font-size: 9px;\">Signed ..................................................................................................  Signed by Hirer /Customer  </p>       <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">\r\n            Registered Waste Carrier CB/DU96892</p>\r\n        <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">Licenced\r\n            Special Waste\r\n            Transfer Station RD/LIC/255/82</p>\r\n    </div>\r\n</body>\r\n\r\n</html>\r\n");
                } 
                else if (Globals.Vehicle_Type == 11) {
                    // PDF Ticket for vehicle type 11

                    html.Append("< !DOCTYPE html >\r\n < html lang =\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=., initial-scale=1.0\">\r\n</head>\r\n\r\n<body>\r\n    <div>\r\n        <div>\r\n            <p style=\"font-size: 12px;color:black;text-align:right; \">\r\n  <span style=\"word-break:break-all\">\r\n      <span style=\"font-size:1px;color:#00FFFFFF;\"><span style=\"font-size:1px;color:#00FFFFFF;\"></span></span>\r\n                    <!-- this contact Used -->\r\n                      " + TicketNo + "\r\n                    <!-- end -->\r\n                    <span style=\"font-size:5px;color:white\">\r\n                   ....... .  <span style=\"font-size:5px;color:#008dc9\">........</span>\r\n<span style=\"font-size:11px;color:white\">00</span>\r\n                        </span>\r\n                </span>\r\n            </p>\r\n            <!-- this contact Used -->\r\n            <p style=\"font-size:0.5px;color:white;\">space tag using</P>\r\n            <div style=\"text-align: center; width: 10%;margin: auto;color:#fff;font-weight: 900;\">\r\n                <h5 style=\"margin: 0;font-size:10px;\">IRWELL STREET METAL CO. LTD.</h5>\r\n                <p style=\" margin-bottom: 0;font-size:10px;\">KENYON STREET, RAMSBOTTOM, BURY, LANCS. BL0 0AB.</p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">Tel: <span>(01706) 824344 &amp; 823001</span> Fax:\r\n                    <span>(01706) 828517</span>\r\n                </p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">email:<span> mailto:info@ismwaste.co.uk </span> web:\r\n                    <span>https://www.ismwaste.co.uk </span>\r\n                </p>\r\n            </div>\r\n            <!-- end -->\r\n            <div style=\"clear: both;\"></div>\r\n            <div style=\"text-align: center;\">\r\n                <img src=\"" + Globals.storagePath + "/WCA/images/top_bg.png\" alt=\"\">\r\n            </div>\r\n        </div>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + InvoiceName + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ..........................................................................................................................................................\r\n        </p>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + CompleteAddress + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ...................................................................................................................... Date:- " + DateTime.Now.ToString("dd-MM-yyyy") + "\r\n        </p>\r\n        <p style=\"font-size:0.2px;color:white;\">space tag using</P>\r\n\r\n    </div>\r\n    <!-- this contact Used -->\r\n    <div style=\"text-align: center;\">\r\n        <div style=\"position: absolute;text-align: center;\">\r\n            <table border=\"1\" style=\"width:100%;font-size:10px; background:red; \">\r\n                <tr>\r\n                    <td> Container </td>\r\n                    <td> General </td>\r\n                    <td> DMR </td>\r\n                    <td> Cardboard </td>\r\n                    <td> Glass </td>\r\n                   </tr>\r\n                <tr>\r\n                    <td> 06 FEL </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_General + " </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_Cardboard + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_Glass + " </td>\r\n         </tr>\r\n                <tr>\r\n                    <td> 08 FEL </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_Cardboard + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_Glass + "  </td>\r\n              </tr>\r\n                <tr>\r\n                    <td> 12 FEL </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_Cardboard + "  </td>\r\n                    <td>  " + Globals.TicketData[0].FEL12_Glass + "  </td>\r\n          </tr>\r\n         <tr style=\"height: 10pt\">\r\n                    <td colspan=\"2\"> DRIVER </td>\r\n                    <td colspan=\"3\" rowspan=\"6\" b>\r\n  DISPOSAL SITE                   </td>\r\n                </tr>\r\n                <tr style=\"height: 10pt\">\r\n                    <td colspan=\"2\"> " + Globals.DriverSelectedValue + " </td>\r\n                    <td colspan=\"3\" rowspan=\"6\">\r\n     Transfer Station       </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"2\"> VEH. REG. No. </td>\r\n                    <td colspan=\"3\" rowspan=\"6\">\r\n                      CUSTOMER ORDER NO.                   </td>\r\n                </tr>\r\n                  <tr style=\"height: 15pt\">\r\n                    <td colspan=\"2\" style=\"height: 50%\"> " + Globals.VehicleSelectedValue + " </td>\r\n                    <td colspan=\"3\" rowspan=\"6\" style=\"height: 10pt\">\r\n  " + CustomerOrderId + " \r\n                  </td>\r\n                </tr>\r\n                <tr style=\"height: 10pt\">\r\n                   </table>\r\n        </div>\r\n        <!-- bg img -->\r\n        <img src=\"" + Globals.storagePath + "/WCA/images/center_img.png\" style=\"width:1200px\" alt=\"\">\r\n    </div>\r\n    <!-- end -->\r\n    <div>\r\n        <div><p style =\"font-size: 10px;text-align:center;font-weight:bold;\">ANY DISCREPANCY MUST BE NOTIFIED TO US IMMEDIATELY</p><p style=\"font-size: 10px;font-weight:700;\">CONDITIONS OF HIRE      <span style=\"font-size: 9px; color:white;\">..........................</span> ALL SERVICES ARE SUBJECT TO OUR TERMS AND CONDITIONS </p><span style=\"font-size: 8px;line-height:12;\">All our services are supplied subject to our Terms and Conditions, available on request. Skips dropped on public highways/drives/private land are the hirer's own risk.</span>   <p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">RESPONSIBILITY.</span> Hirer accepts all responsibility in respect of all claims for damage to  person or property while siting,collecting or on site. <span style=\"font-size: 9px;font-weight:bold;\">OVERLOADING.</span> No container must be loaded dangerously or in excess of G.V.W.Fire and any damages/loss of equipment will be charged for, plus loss of use. Void journey(s) Chargeable.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">HIRE PERIOD.</span> Skips placed on any LocalAuthority Council Highway will be subject to a minimum hire period of not less than one month (31 days) and the company or person in possession of any skip will become the  owner  for the purpose of the Highways Act 1980 section 139 & 140 and Builders Skip Marking Regulations 1984.</p><span style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">SKIP RENTALS.</span> Each skip hired will be subject to rental charge of £0.50 per day or Euro equivalent (when legal  tender) after the first seven days hire.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">LIGHTS / CONES / SKIP MARKINGS.</span> In accordance with Highways Act 1980,  Section 139 & 140 and the Builders Skips Marking Regulations 1984, along with any amendments and any other statutory requirements. The Hirer  is responsible for ensuring adequate skip markings, obtaining and  placing of lamps, cones, lighting of lamps and maintenance of same and also obtaining and renewing and  Highways permission/skip permits from the Local Authority  Council Highways Department.</span> <p style=\"font-size: 8px;font-weight:bold;line-height:12\"><i>I have read and accept the additional Conditions of Hire as printed above.</i></p>     </div>\r\n        <table>\r\n            <tr>\r\n                <td colspan=\"3\" style=\"text-align:center;\">\r\n                                      ");
                    if (isCheck_Sign_Avl)
                        html.Append("<img src=\"" + SignaturesPath + "\" width=\"100px\" height=\"40%\"  alt=\"\">\r\n   ");
                    else
                        html.Append("<div style=\"width:200px; heigth:80%;\"> <p style=\"font-size:18px;\">NTS</p><p style=\"color:white;font-size:4.8px;\">sign</p></div>");
                    html.Append("         </td>\r\n                <td colspan=\"4\" style=\"text-align:center;\">\r\n\r\n");
                    html.Append(" </td>\r\n            </tr>\r\n        </table>\r\n <p style=\"font-size: 9px;\">Signed ..................................................................................................  Signed by Hirer /Customer  </p>       <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">\r\n            Registered Waste Carrier CB/DU96892</p>\r\n        <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">Licenced\r\n            Special Waste\r\n            Transfer Station RD/LIC/255/82</p>\r\n    </div>\r\n</body>\r\n\r\n</html>\r\n");
                }

                // ------------------------------------------------------------
                TextReader reader = new StringReader(html.ToString());
                worker.StartDocument();
                worker.Parse(reader);
                worker.EndDocument();
                worker.Close();
                document.Close();
                writer.Close();
                fs.Close();

                if (Globals.Vehicle_Type == 1)
                 {
                    // generate ticket for Admin with weight

                    TotalWeight = Globals.TicketData[0].TotalWeight == "" ? "-" : Globals.TicketData[0].TotalWeight;

                    string pdfPath_Admin = System.IO.Path.Combine(TicketsPath, "ISM_Copy_Ticket_" + CustomerName + "_" + TicketNo + ".pdf"); // save ticket path
                    if (File.Exists(pdfPath_Admin))
                    {
                        File.Delete(pdfPath_Admin);
                    }
                    System.IO.FileStream fileSystem = new FileStream(pdfPath_Admin, FileMode.Create);
                    Document document_admin = new Document(PageSize.A4);
                    PdfWriter writer_admin = PdfWriter.GetInstance(document_admin, fileSystem);
                    HTMLWorker worker_admin = new HTMLWorker(document_admin);
                    document_admin.Open();
                    StringBuilder html_admin_doc = new StringBuilder();
                    html_admin_doc.Append("< !DOCTYPE html >\r\n < html lang =\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=., initial-scale=1.0\">\r\n</head>\r\n\r\n<body>\r\n    <div>\r\n        <div>\r\n            <p style=\"font-size: 12px;color:black;text-align:right; \">\r\n  <span style=\"word-break:break-all\">\r\n      <span style=\"font-size:1px;color:#00FFFFFF;\"><span style=\"font-size:1px;color:#00FFFFFF;\"></span></span>\r\n                    <!-- this contact Used -->\r\n                      " + TicketNo + "\r\n                    <!-- end -->\r\n                    <span style=\"font-size:5px;color:white\">\r\n                   ....... .  <span style=\"font-size:5px;color:#008dc9\">........</span>\r\n<span style=\"font-size:11px;color:white\">00</span>\r\n                        </span>\r\n                </span>\r\n            </p>\r\n            <!-- this contact Used -->\r\n            <p style=\"font-size:0.5px;color:white;\">space tag using</P>\r\n            <div style=\"text-align: center; width: 10%;margin: auto;color:#fff;font-weight: 900;\">\r\n                <h5 style=\"margin: 0;font-size:10px;\">IRWELL STREET METAL CO. LTD.</h5>\r\n                <p style=\" margin-bottom: 0;font-size:10px;\">KENYON STREET, RAMSBOTTOM, BURY, LANCS. BL0 0AB.</p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">Tel: <span>(01706) 824344 &amp; 823001</span> Fax:\r\n                    <span>(01706) 828517</span>\r\n                </p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">email:<span> mailto:info@ismwaste.co.uk </span> web:\r\n                    <span>https://www.ismwaste.co.uk </span>\r\n                </p>\r\n            </div>\r\n            <!-- end -->\r\n            <div style=\"clear: both;\"></div>\r\n            <div style=\"text-align: center;\">\r\n                <img src=\"" + Globals.storagePath + "/WCA/images/top_bg.png\" alt=\"\">\r\n            </div>\r\n        </div>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + InvoiceName + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ..........................................................................................................................................................\r\n        </p>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + CompleteAddress + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ...................................................................................................................... Date:- " + DateTime.Now.ToString("dd-MM-yyyy") + "\r\n        </p>\r\n        <p style=\"font-size:0.2px;color:white;\">space tag using</P>\r\n\r\n    </div>\r\n    <!-- this contact Used -->\r\n    <div style=\"text-align: center;\">\r\n        <div style=\"position: absolute;text-align: center;\">\r\n            <table border=\"1\" style=\"width:100%;font-size:10px;\">\r\n                <tr>\r\n                    <td> Container </td>\r\n                    <td> General </td>\r\n                    <td> DMR </td>\r\n                    <td> Cardboard </td>\r\n                    <td> Container </td>\r\n                    <td> General </td>\r\n                    <td> DMR </td>\r\n                    <td> Cardboard </td>\r\n                    <td> Glass </td>\r\n                 <td> Food </td>\r\n </tr>\r\n                <tr>\r\n                    <td> 08 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL08_General + " </td>\r\n                    <td> " + Globals.TicketData[0].REL08_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL08_Cardboard + "  </td>\r\n                    <td> WB 1100 </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_General + " </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_DMR + " </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_Cardboard + " </td>\r\n                    <td> " + Globals.TicketData[0].WB1100_Glass + " </td>\r\n                <td> " + Globals.TicketData[0].WB1100_Food + " </td>\r\n </tr>\r\n                <tr>\r\n                    <td> 12 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL12_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL12_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL12_Cardboard + "  </td>\r\n                    <td> WB 660 </td>\r\n                    <td> " + Globals.TicketData[0].WB660_General + " </td>\r\n                    <td> " + Globals.TicketData[0].WB660_DMR + " </td>\r\n                    <td> " + Globals.TicketData[0].WB660_Cardboard + " </td>\r\n                    <td> " + Globals.TicketData[0].WB660_Glass + " </td>\r\n               <td> " + Globals.TicketData[0].WB660_Food + " </td>\r\n  </tr>\r\n                <tr>\r\n                    <td> 14 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL14_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL14_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL14_Cardboard + "  </td>\r\n                    <td> WB 240 </td>\r\n                    <td> " + Globals.TicketData[0].WB240_General + " </td>\r\n                    <td> " + Globals.TicketData[0].WB240_DMR + " </td>\r\n                    <td> " + Globals.TicketData[0].WB240_Cardboard + " </td>\r\n                    <td> " + Globals.TicketData[0].WB240_Glass + " </td>\r\n                <td> " + Globals.TicketData[0].WB240_Food + " </td>\r\n </tr>\r\n                <tr>\r\n                    <td> 16 REL </td>\r\n                    <td> " + Globals.TicketData[0].REL16_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL16_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].REL16_Cardboard + "  </td>\r\n                    <td style=\"font-size:8px;\"> Extra Waste </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_General == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_DMR == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_Cardboard == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n                    <td> " + ((Globals.TicketData[0].ExtraWaste_Glass == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n       <td> " + ((Globals.TicketData[0].ExtraWaste_Food == true) ? "<img src=\"" + Globals.storagePath + "/WCA/images/tick_icon.png" + "\" width=\"10%\" alt=\"\">\r\n   " : " ") + " </td>\r\n         </tr>\r\n\r\n                <tr style=\"height: 10pt\">\r\n                    <td colspan=\"4\"> DRIVER </td>\r\n                    <td colspan=\"6\" rowspan=\"6\" b>\r\n  DISPOSAL SITE                   </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"4\" style=\"height: 50%\"> " + Globals.DriverSelectedValue + " </td>\r\n                    <td colspan=\"6\" rowspan=\"6\">\r\n     Transfer Station       </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"4\"> VEH. REG. No. </td>\r\n                    <td colspan=\"6\" rowspan=\"6\">\r\n                      TOTAL WEIGHT                   </td>\r\n                </tr>\r\n                  <tr style=\"height: 15pt\">\r\n                    <td colspan=\"4\" style=\"height: 50%\"> " + Globals.VehicleSelectedValue + " </td>\r\n                    <td colspan=\"6\" rowspan=\"6\">\r\n " + TotalWeight + " \r\n                    </td>\r\n                </tr>\r\n                <tr style=\"height: 10pt\">\r\n                   </table>\r\n        </div>\r\n        <!-- bg img -->\r\n        <img src=\"" + Globals.storagePath + "/WCA/images/center_img.png\" style=\"width:1200px\" alt=\"\">\r\n    </div>\r\n    <!-- end -->\r\n    <div>\r\n        <div><p style =\"font-size: 10px;text-align:center;font-weight:bold;\">ANY DISCREPANCY MUST BE NOTIFIED TO US IMMEDIATELY</p><p style=\"font-size: 10px;font-weight:700;\">CONDITIONS OF HIRE      <span style=\"font-size: 9px; color:white;\">..........................</span> ALL SERVICES ARE SUBJECT TO OUR TERMS AND CONDITIONS </p><span style=\"font-size: 8px;line-height:12;\">All our services are supplied subject to our Terms  and Conditions, available on request. Skips dropped on public highways/drives/private land are the hirer's own risk.</span>   <p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">RESPONSIBILITY.</span> Hirer accepts all responsibility in respect of all claims for damage to  person or property while siting,collecting or on site. <span style=\"font-size: 9px;font-weight:bold;\">OVERLOADING.</span> No container must be loaded dangerously or in excess of G.V.W.Fire and any damages/loss of equipment will be charged for, plus loss of use. Void journey(s) Chargeable.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">HIRE PERIOD.</span> Skips placed on any LocalAuthority Council Highway will be subject to a minimum hire period of not less than one month (31 days) and the company or person in possession of any skip will become the  owner  for the purpose of the Highways Act 1980 section 139 & 140 and Builders Skip Marking Regulations 1984.</p><span style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">SKIP RENTALS.</span> Each skip hired will be subject to rental charge of £0.50 per day or Euro equivalent (when legal  tender) after the first seven days hire.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">LIGHTS / CONES / SKIP MARKINGS.</span> In accordance with Highways Act 1980,  Section 139 & 140 and the Builders Skips Marking Regulations 1984, along with any amendments and any other statutory requirements. The Hirer  is responsible for ensuring adequate skip markings, obtaining and  placing of lamps, cones, lighting of lamps and maintenance of same and also obtaining and renewing and  Highways permission/skip permits from the Local Authority  Council Highways Department.</span> <p style=\"font-size: 8px;font-weight:bold;line-height:12\"><i>I have read and accept the additional Conditions of Hire as printed above.</i></p>     </div>\r\n        <table>\r\n            <tr>\r\n                <td colspan=\"3\" style=\"text-align:center;\">\r\n                                      ");
                    if (isCheck_Sign_Avl)
                        html_admin_doc.Append("<img src=\"" + SignaturesPath + "\" width=\"100px\" height=\"40%\"  alt=\"\">\r\n   ");
                    else
                        html_admin_doc.Append("<div style=\"width:200px; heigth:80%;\"> <p style=\"font-size:18px;\">NTS</p><p style=\"color:white;font-size:4.8px;\">sign</p></div>");
                    html_admin_doc.Append("         </td>\r\n                <td colspan=\"4\" style=\"text-align:center;\">\r\n\r\n");
                    html_admin_doc.Append(" </td>\r\n            </tr>\r\n        </table>\r\n <p style=\"font-size: 9px;\">Signed ..................................................................................................  Signed by Hirer /Customer  </p>       <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">\r\n            Registered Waste Carrier CB/DU96892</p>\r\n        <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">Licenced\r\n            Special Waste\r\n            Transfer Station RD/LIC/255/82</p>\r\n    </div>\r\n</body>\r\n\r\n</html>\r\n");
                    TextReader reader_pdf_admin = new StringReader(html_admin_doc.ToString());
                    worker_admin.StartDocument();
                    worker_admin.Parse(reader_pdf_admin);
                    worker_admin.EndDocument();
                    worker_admin.Close();
                    document_admin.Close();
                    writer_admin.Close();
                    fileSystem.Close();
                }
                else if (Globals.Vehicle_Type == 11) 
                {
                    // generate ticket for Admin with weight

                    TotalWeight = Globals.TicketData[0].TotalWeight == "" ? "-" : Globals.TicketData[0].TotalWeight;

                    string pdfPath_Admin = System.IO.Path.Combine(TicketsPath, "ISM_Copy_Ticket_" + CustomerName + "_" + TicketNo + ".pdf"); // save ticket path
                    if (File.Exists(pdfPath_Admin))
                    {
                        File.Delete(pdfPath_Admin);
                    }
                    System.IO.FileStream fileSystem = new FileStream(pdfPath_Admin, FileMode.Create);
                    Document document_admin = new Document(PageSize.A4);
                    PdfWriter writer_admin = PdfWriter.GetInstance(document_admin, fileSystem);
                    HTMLWorker worker_admin = new HTMLWorker(document_admin);
                    document_admin.Open();
                    StringBuilder html_admin_doc = new StringBuilder();
                    html_admin_doc.Append("< !DOCTYPE html >\r\n < html lang =\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=., initial-scale=1.0\">\r\n</head>\r\n\r\n<body>\r\n    <div>\r\n        <div>\r\n            <p style=\"font-size: 12px;color:black;text-align:right; \">\r\n  <span style=\"word-break:break-all\">\r\n      <span style=\"font-size:1px;color:#00FFFFFF;\"><span style=\"font-size:1px;color:#00FFFFFF;\"></span></span>\r\n                    <!-- this contact Used -->\r\n                      " + TicketNo + "\r\n                    <!-- end -->\r\n                    <span style=\"font-size:5px;color:white\">\r\n                   ....... .  <span style=\"font-size:5px;color:#008dc9\">........</span>\r\n<span style=\"font-size:11px;color:white\">00</span>\r\n                        </span>\r\n                </span>\r\n            </p>\r\n            <!-- this contact Used -->\r\n            <p style=\"font-size:0.5px;color:white;\">space tag using</P>\r\n            <div style=\"text-align: center; width: 10%;margin: auto;color:#fff;font-weight: 900;\">\r\n                <h5 style=\"margin: 0;font-size:10px;\">IRWELL STREET METAL CO. LTD.</h5>\r\n                <p style=\" margin-bottom: 0;font-size:10px;\">KENYON STREET, RAMSBOTTOM, BURY, LANCS. BL0 0AB.</p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">Tel: <span>(01706) 824344 &amp; 823001</span> Fax:\r\n                    <span>(01706) 828517</span>\r\n                </p>\r\n                <p style=\"margin-bottom: 0;font-size:10px;\">email:<span> mailto:info@ismwaste.co.uk </span> web:\r\n                    <span>https://www.ismwaste.co.uk </span>\r\n                </p>\r\n            </div>\r\n            <!-- end -->\r\n            <div style=\"clear: both;\"></div>\r\n            <div style=\"text-align: center;\">\r\n                <img src=\"" + Globals.storagePath + "/WCA/images/top_bg.png\" alt=\"\">\r\n            </div>\r\n        </div>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + InvoiceName + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ..........................................................................................................................................................\r\n        </p>\r\n        <!-- this contact Used -->\r\n        <p style=\"font-size:12px;\">" + CompleteAddress + "</P>\r\n        <!-- end -->\r\n        <p style=\"line-height:3.5;\">\r\n            ...................................................................................................................... Date:- " + DateTime.Now.ToString("dd-MM-yyyy") + "\r\n        </p>\r\n        <p style=\"font-size:0.2px;color:white;\">space tag using</P>\r\n\r\n    </div>\r\n    <!-- this contact Used -->\r\n    <div style=\"text-align: center;\">\r\n        <div style=\"position: absolute;text-align: center;\">\r\n            <table border=\"1\" style=\"width:100%;font-size:10px;\">\r\n                <tr>\r\n                    <td> Container </td>\r\n                    <td> General </td>\r\n                    <td> DMR </td>\r\n                    <td> Cardboard </td>\r\n                    <td> Glass </td>\r\n     </tr>\r\n                <tr>\r\n                    <td> 06 FEL </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_General + " </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_Cardboard + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL06_Glass + " </td>\r\n        </tr>\r\n                <tr>\r\n                    <td> 08 FEL </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_Cardboard + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL08_Glass + " </td>\r\n     </tr>\r\n                <tr>\r\n                    <td> 12 FEL </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_General + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_DMR + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_Cardboard + "  </td>\r\n                    <td> " + Globals.TicketData[0].FEL12_Glass + " </td>\r\n             </tr>\r\n             <tr style=\"height: 10pt\">\r\n                    <td colspan=\"2\"> DRIVER </td>\r\n                    <td colspan=\"3\" rowspan=\"6\" b>\r\n  DISPOSAL SITE                   </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"2\" style=\"height: 50%\"> " + Globals.DriverSelectedValue + " </td>\r\n                    <td colspan=\"3\" rowspan=\"6\">\r\n    Transfer Station      </td>\r\n                </tr>\r\n                <tr style=\"height: 15pt\">\r\n                    <td colspan=\"2\"> VEH. REG. No. </td>\r\n                    <td colspan=\"3\" rowspan=\"6\">\r\n                      TOTAL WEIGHT                   </td>\r\n                </tr>\r\n                  <tr style=\"height: 15pt\">\r\n                    <td colspan=\"2\" style=\"height: 50%\"> " + Globals.VehicleSelectedValue + " </td>\r\n                    <td colspan=\"3\" rowspan=\"6\">\r\n " + TotalWeight + "  \r\n                    </td>\r\n                </tr>\r\n                <tr style=\"height: 10pt\">\r\n                   </table>\r\n        </div>\r\n        <!-- bg img -->\r\n        <img src=\"" + Globals.storagePath + "/WCA/images/center_img.png\" style=\"width:1200px\" alt=\"\">\r\n    </div>\r\n    <!-- end -->\r\n    <div>\r\n        <div><p style =\"font-size: 10px;text-align:center;font-weight:bold;\">ANY DISCREPANCY MUST BE NOTIFIED TO US IMMEDIATELY</p><p style=\"font-size: 10px;font-weight:700;\">CONDITIONS OF HIRE      <span style=\"font-size: 9px; color:white;\">..........................</span> ALL SERVICES ARE SUBJECT TO OUR TERMS AND CONDITIONS </p><span style=\"font-size: 8px;line-height:12;\">All our services are supplied subject to our Terms  and Conditions, available on request. Skips dropped on public highways/drives/private land are the hirer's own risk.</span>   <p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">RESPONSIBILITY.</span> Hirer accepts all responsibility in respect of all claims for damage to  person or property while siting,collecting or on site. <span style=\"font-size: 9px;font-weight:bold;\">OVERLOADING.</span> No container must be loaded dangerously or in excess of G.V.W.Fire and any damages/loss of equipment will be charged for, plus loss of use. Void journey(s) Chargeable.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">HIRE PERIOD.</span> Skips placed on any LocalAuthority Council Highway will be subject to a minimum hire period of not less than one month (31 days) and the company or person in possession of any skip will become the  owner  for the purpose of the Highways Act 1980 section 139 & 140 and Builders Skip Marking Regulations 1984.</p><span style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">SKIP RENTALS.</span> Each skip hired will be subject to rental charge of £0.50 per day or Euro equivalent (when legal  tender) after the first seven days hire.</p><p style=\"font-size: 8px;line-height:12;\"><span style=\"font-size: 9px;font-weight:bold;\">LIGHTS / CONES / SKIP MARKINGS.</span> In accordance with Highways Act 1980,  Section 139 & 140 and the Builders Skips Marking Regulations 1984, along with any amendments and any other statutory requirements. The Hirer  is responsible for ensuring adequate skip markings, obtaining and  placing of lamps, cones, lighting of lamps and maintenance of same and also obtaining and renewing and  Highways permission/skip permits from the Local Authority  Council Highways Department.</span> <p style=\"font-size: 8px;font-weight:bold;line-height:12\"><i>I have read and accept the additional Conditions of Hire as printed above.</i></p>     </div>\r\n        <table>\r\n            <tr>\r\n                <td colspan=\"3\" style=\"text-align:center;\">\r\n                                      ");
                    if (isCheck_Sign_Avl)
                        html_admin_doc.Append("<img src=\"" + SignaturesPath + "\" width=\"100px\" height=\"40%\"  alt=\"\">\r\n   ");
                    else
                        html_admin_doc.Append("<div style=\"width:200px; heigth:80%;\"> <p style=\"font-size:18px;\">NTS</p><p style=\"color:white;font-size:4.8px;\">sign</p></div>");
                    html_admin_doc.Append("         </td>\r\n                <td colspan=\"4\" style=\"text-align:center;\">\r\n\r\n");
                    html_admin_doc.Append(" </td>\r\n            </tr>\r\n        </table>\r\n <p style=\"font-size: 9px;\">Signed ..................................................................................................  Signed by Hirer /Customer  </p>       <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">\r\n            Registered Waste Carrier CB/DU96892</p>\r\n        <p class=\"s16\" style=\"font-size: 8px;margin-top: 0px;\">Licenced\r\n            Special Waste\r\n            Transfer Station RD/LIC/255/82</p>\r\n    </div>\r\n</body>\r\n\r\n</html>\r\n");
                    TextReader reader_pdf_admin = new StringReader(html_admin_doc.ToString());
                    worker_admin.StartDocument();
                    worker_admin.Parse(reader_pdf_admin);
                    worker_admin.EndDocument();
                    worker_admin.Close();
                    document_admin.Close();
                    writer_admin.Close();
                    fileSystem.Close();

                }
                SendMail_Visibility = true;
              /*  Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(pdfPath_Customer)
                });*/

                // save files to SD Card



                // Remove Signature if exist
                if (File.Exists(SignaturesPath))
                {
                    File.Delete(SignaturesPath);
                }
                //Application.Current.MainPage.DisplayAlert("PDF Ticket", "Ticket has been Generated Successfully !", "OK");
                return true;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Send Ticket", "Please Try Again !", "OK");
                return false;
            }
        }
        #endregion
        //==========================================
        public string Id { get; set; }
        public string Customer { get => customer; set => SetProperty(ref customer, value); }
        public bool SpotJob_visibility { get => spot_job_visibility; set => SetProperty(ref spot_job_visibility, value); }
		public string Site { get => site; set => SetProperty(ref site, value); }
		public string Weight { get => weight; set => SetProperty(ref weight, value); }
		public bool SendMail_Visibility { get; set; }
		public bool FinishButton_Visibility { get; set; }
		public bool SignaturePadVisibility { get => signaturepadvisibility; set => SetProperty(ref signaturepadvisibility, value); }
        public bool SignatureFoundVisibility { get => signaturefoundvisibility; set => SetProperty(ref signaturefoundvisibility, value); }
        public bool IsNTSMode { get => isntsmode; set => SetProperty(ref isntsmode, value); }
        public bool IsImageAttachMode { get => isimageattachmode; set => SetProperty(ref isimageattachmode, value); }
       //	public string Container_Qty_DetailPage { get => container_qty_detailpage; set => SetProperty(ref container_qty_detailpage, value); }
        public string Customer_WasteType { get => customer_wastetype; set => SetProperty(ref customer_wastetype, value); }
		public int ContainerTotalQty { get => containertotal_qty; set => SetProperty(ref containertotal_qty, value); }
		public string Container_Type_DetailPage { get => container_type_detailpage; set => SetProperty(ref container_type_detailpage, value); }
		public string JobId { get { return jobId; } set { jobId = value; LoadJobId(value); } }
        public async void LoadJobId(string jobId)
        {
            try
            {
                var job = await DataStore.GetJobAsync(jobId);
                Id = job.Id;
                Customer = job.Customer;
				Site = job.Site;
			}
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Job");
            }
        }
    }
}
