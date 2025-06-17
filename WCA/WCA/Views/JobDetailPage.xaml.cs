using WCA.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Linq;
using System;
using SignaturePad.Forms;

using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
/*using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;*/

namespace WCA.Views
{
    public partial class JobDetailPage : ContentPage
    {
        //  private Document document;
        #region Signature
        private Xamarin.Forms.Point[] points;
        System.Collections.Generic.List<String> SelectedImages = new System.Collections.Generic.List<String>();
        private bool flag_generate_ticket = true;
        private bool flag_sendMail_ticket = true;
        private void UpdateControls()
        {
            // btnSave.IsEnabled = !signatureView.IsBlank;
            btnSaveImage.IsEnabled = !signatureView.IsBlank;
        
            //  btnLoad.IsEnabled = points != null;
        }
  /*      private void SaveVectorClicked(object sender, EventArgs e)
        {
            points = signatureView.Points.ToArray();
            UpdateControls();
            DisplayAlert("Signature Pad", "Vector signature saved to memory.", "OK");
        }
        private void LoadVectorClicked(object sender, EventArgs e)
        {
            signatureView.Points = points;
        }*/
        private async void SaveImageClicked(object sender, EventArgs e)
        {
            bool saved;
            using (var bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png, Xamarin.Forms.Color.Black, Xamarin.Forms.Color.White, 1f))
            {
                saved = await App.SaveSignature(bitmap, Globals.storagePath + "/WCA/Signatures/" + Globals.TicketData[0].CustomerName + Globals.TicketData[0].TicketNo + Globals.TicketData[0].CPID +".png");
            }
            if (saved)
                await DisplayAlert("Signature Pad", "Signature saved.", "OK");
            else
                await DisplayAlert("Signature Pad", "There was an error saving the signature.", "OK");
        }
        private void SignatureChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        #endregion
        public JobDetailPage()
        {
            InitializeComponent();
            BindingContext = new JobDetailViewModel();
            UpdateControls();
        }
        JobDetailViewModel ViewModel = new JobDetailViewModel();
        public JobDetailPage(JobDetailViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
            UpdateControls();
            //string customer = ViewModel.Customer;
            //Stream bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
        }

        private void PDF_Clicked(object sender, EventArgs e)
        {
            if (flag_generate_ticket == true)
            {
                flag_generate_ticket = false;
                bool Generate_return = this.ViewModel.PDfTicket_Generate();
                if (Generate_return)
                {
                    string path = Globals.storagePath + "/WCA/Tickets/";
                    string pdfPath_Admin = System.IO.Path.Combine(path, "ISM_Copy_Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf"); // save ticket path
                    if (File.Exists(pdfPath_Admin))
                    {
                        btnSendTicket.IsVisible = true;

                    }
                }
                else
                {
                    btnSendTicket.IsVisible = false;
                }
                flag_generate_ticket = true;
            }
        }

        /*  public void CreateReport()
          {
              CreateDocument();
              SetStyles();
              AddHeader();
              AddContent();
              AddFooter();

              SaveShowPDF();
          }

          private void CreateDocument()
          {
              var TicketTemplateAndPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/WCA/", "TicketTemplate.pdf");
              document = new Document();
              //  PdfDocument TicketTemplateDoc = PdfReader.Open(TicketTemplateAndPath, PdfDocumentOpenMode.Import);
              document.Info.Title = "Product z";
              document.Info.Subject = "We present you the Product Catalog for this year.";
              document.Info.Author = "Luis Beltran";
              document.Info.Keywords = "Products";
          }

          private void SetStyles()
          {
              // Modifying default style
              MigraDocCore.DocumentObjectModel.Style style = document.Styles["Normal"];
              style.Font.Name = "OpenSans";
              style.Font.Color = Colors.Black;
              style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
              style.ParagraphFormat.PageBreakBefore = false;

              // Header style
              style = document.Styles[StyleNames.Header];
              style.Font.Name = "OpenSans";
              style.Font.Size = 18;
              style.Font.Color = Colors.Black;
              style.Font.Bold = true;
              style.Font.Underline = Underline.Single;
              style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

              // Footer style
              style = document.Styles[StyleNames.Footer];
              style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Right);

              // Modifying predefined style: HeadingN (where N goes from 1 to 9)
              style = document.Styles["Heading1"];
              style.Font.Name = "OpenSans"; // Can be changed (don't forget to add and register the Fonts!)
              style.Font.Size = 14;
              style.Font.Bold = true;
              style.Font.Italic = false;
              style.Font.Color = Colors.DarkBlue;
              style.ParagraphFormat.Shading.Color = Colors.SkyBlue;
              style.ParagraphFormat.Borders.Distance = "3pt";
              style.ParagraphFormat.Borders.Width = 2.5;
              style.ParagraphFormat.Borders.Color = Colors.CadetBlue;
              style.ParagraphFormat.SpaceAfter = "1cm";

              // Modifying predefined style: Heading2
              style = document.Styles["Heading2"];
              style.Font.Size = 12;
              style.Font.Bold = false;
              style.Font.Italic = true;
              style.Font.Color = Colors.DeepSkyBlue;
              style.ParagraphFormat.Shading.Color = Colors.White;
              style.ParagraphFormat.Borders.Width = 0;
              style.ParagraphFormat.SpaceAfter = 3;
              style.ParagraphFormat.SpaceBefore = 3;

              // Adding new style
              style = document.Styles.AddStyle("MyParagraphStyle", "Normal");
              style.Font.Size = 10;
              style.Font.Color = Colors.Blue;
              style.ParagraphFormat.SpaceAfter = 3;

              style = document.Styles.AddStyle("MyTableStyle", "Normal");
              style.Font.Size = 9;
              style.Font.Color = Colors.SlateBlue;
          }

          private void AddHeader()
          {
              var section = document.AddSection();

              var config = section.PageSetup;
              config.Orientation = Orientation.Portrait;
              config.TopMargin = "3cm";
              config.LeftMargin = 15;
              config.BottomMargin = "3cm";
              config.RightMargin = 15;
              config.PageFormat = PageFormat.A4;
              config.OddAndEvenPagesHeaderFooter = true;
              config.StartingNumber = 1;

              var oddHeader = section.Headers.Primary;

              var content = new Paragraph();
              content.AddText("\tProduct Catalog 2021 - Tech Solutions Inc\t");
              oddHeader.Add(content);
              oddHeader.AddTable();

              var headerForEvenPages = section.Headers.EvenPage;
              headerForEvenPages.AddParagraph("Product Catalog 2021");
              headerForEvenPages.AddTable();
          }

          void AddContent()
          {
              AddText1();
             // AddImage("comunidad.jpg");
              AddText2();
          }

          private void AddFooter()
          {
              var content = new Paragraph();
              content.AddText(" Page ");
              content.AddPageField();
              content.AddText(" of ");
              content.AddNumPagesField();

              var section = document.LastSection;
              section.Footers.Primary.Add(content);

              var contentForEvenPages = content.Clone();
              contentForEvenPages.AddTab();
              contentForEvenPages.AddText("\tDate: ");
              //contenidoPar.AddDateField("dddd, dd \"de\" MMMM \"de\" yyyy HH:mm:ss tt");
              contentForEvenPages.AddDateField("dddd, MMMM dd, yyyy HH:mm:ss tt");

              section.Footers.EvenPage.Add(contentForEvenPages);
          }
          private void AddText1()
          {
              var text = "At Tech Solutions Inc, it is our top priority to bring only products of the highest quality to our customers. Products always pass a strict quality control process before they are delivered to you. We put ourselves in the customer's shoes, and only want to offer products that will make our clients happy.";
              var section = document.LastSection;
              var mainParagraph = section.AddParagraph(text, "Heading1");
              mainParagraph.AddLineBreak();

              text = "All components of Tech Solutions Inc sample products have undergone strict laboratory tests for lead, nickel and cadmium content. A world-leading inspection, testing, and certification company has conducted these testsm and as you can see below, our products have passed with perfect note.";
              section.AddParagraph(text, "Heading2");
          }

          private void AddImage(string archivo)
          {
              var paragraph = document.LastSection.AddParagraph();
              paragraph.Format.Alignment = ParagraphAlignment.Center;

              var iimage = DependencyService.Get<IImage>();
              var path = iimage.Prefix + archivo;

              if (!iimage.Extension)
                  path = Path.GetFileNameWithoutExtension(path);

              MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource.ImageSourceImpl = iimage.Implementation;
              var logo = ImageSource.FromFile(path);
              var image = paragraph.AddImage((MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource.IImageSource)logo);

              image.LineFormat = new LineFormat() { Color = Colors.DarkGreen };
              image.LockAspectRatio = true;
              image.Width = "2.5cm";
          }

          private void AddText2()
          {
              var seccion = document.LastSection;

              var texto = "We recommend customers to purchase products only from reliable sources where products have been tested, and only when the lead, nickel and cadmium content have passed the laboratory tests. Your health is important.";
              var parrafo = seccion.AddParagraph(texto, "MyParagraphStyle");

              texto = "\nWearing products that are not tested, or have failed to meet regulatory standards may bring harm to your health and skin";
              parrafo = seccion.AddParagraph(texto, "MyParagraphStyle");
              parrafo.AddLineBreak();
          }

          private void SaveShowPDF()
          {
              var file = Xamarin.Forms.DependencyService.Get<IFile>();
              var appFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/WCA/";
              var TicketTemplate = "TicketTemplate.pdf";
              var TicketTemplateAndPath = Path.Combine(appFolder, TicketTemplate);
              //var fileName = $"{Guid.NewGuid()}.pdf";
              //var filePath = file.GetLocalPath(fileName);

              PdfDocumentRenderer printer = new PdfDocumentRenderer();
              printer.Document = document;
              printer.RenderDocument();
              printer.PdfDocument.Save(TicketTemplateAndPath);

              Launcher.OpenAsync(new OpenFileRequest
              {
                  File = new ReadOnlyFile(TicketTemplateAndPath)
              });
          }*/
        async Task imageSelectionAsync()
        {

            // https://www.telerik.com/blogs/create-file-picker-using-xamarin-essentials-telerik-ui-for-xamarin
            // working code


            bool imageFrom = await DisplayAlert("Attach Image", "Attach Images from:", "Gallery", "Camera");
            if (imageFrom)
            {

                try
                {


                    var seletedImage = await FilePicker.PickMultipleAsync(new PickOptions
                    {
                        FileTypes = FilePickerFileType.Images,
                        PickerTitle = "Select images"
                    });
                    var ListSelectedImages = seletedImage.ToList();
                   // SelectedImages = new System.Collections.Generic.List<String>();
                    foreach (var element in ListSelectedImages)
                    {
                        SelectedImages.Add(element.FullPath);
                    }

                }
                catch(Exception ex)
                {
                    DisplayAlert("Attach Images", "Unable to Attach Images,Try again !", "OK");

                }


            }
            else if(imageFrom==false) { 

                try
                {
                    var photo = await MediaPicker.CapturePhotoAsync();
                    //   String PhotoPath;
                    if (photo != null)
                    {

                        var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                        using (var stream = await photo.OpenReadAsync())
                        using (var newStream = File.OpenWrite(newFile))
                            await stream.CopyToAsync(newStream);
                        SelectedImages.Add(newFile);
                       // Console.WriteLine($"CapturePhotoAsync COMPLETED: {newFile}");
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Attach Images", "Unable to Attach Images,Try again !", "OK");

                }


            }



            /*  bool imageFrom=await DisplayAlert("Attach Image", "Do you want to attach images in Mail ?", "Yes","No");
              if (imageFrom)
              {
                  if (!CrossMedia.Current.IsPickPhotoSupported)
                  {
                      await DisplayAlert("Attach Image", "Your Device does not currently support this functionality", "OK");
                      return;
                  }
                  var mediaOptions = new PickMediaOptions()
                  {
                      PhotoSize = PhotoSize.Medium
                  };
                   var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                //  var selectedImageFile = await CrossMedia.Current.PickPhotosAsync(new PickMediaOptions { CompressionQuality = 80, SaveMetaData = true, }, new MultiPickerOptions { MaximumImagesCount = 3, });
                  if (selectedImageFile == null)
                  {
                      await DisplayAlert("Attach Image In Email", "Could not get the image, Please try again.", "OK");
                  }
              }*/

        }

        void btnSendTicket_Clicked(object sender, EventArgs e)
        {

            //  imageSelectionAsync();

            if (flag_sendMail_ticket == true)
            {
                flag_sendMail_ticket = false;
                // send Ticket Button click event
                try
                {
                    // generate ticket process start from

                    bool Generate_return = this.ViewModel.PDfTicket_Generate();
                    if (Generate_return)
                    {
                        string path_ticket = Globals.storagePath + "/WCA/Tickets/";
                        string pdfPath_Admin = System.IO.Path.Combine(path_ticket, "ISM_Copy_Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf"); // save ticket path
                        //if (File.Exists(pdfPath_Admin))
                        //{
                            //btnSendTicket.IsVisible = true;


                            // send mail process start from here
                            //ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
                            var current = Connectivity.NetworkAccess;

                            if (current == NetworkAccess.Internet)
                            {
                                if (File.Exists(Globals.storagePath + "/WCA/Tickets/" + "Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf") )
                                {
                                    var message = new MimeMessage();
                                    if (Globals.TicketData[0].CancelreasonId == 0 || Globals.TicketData[0].CancelreasonId == 15)
                                    {

                                        // send mail to customer 
                                        // var message = new MimeMessage();
                                        message.From.Add(InternetAddress.Parse(Globals.EmailData[0].SenderEmail));
                                        var customer_emails = Globals.TicketData[0].EmailAddress.Split(';'); // split multiple emails from string
                                        foreach (var selectedCustomerEmail in customer_emails)
                                        {
                                            message.To.Add(InternetAddress.Parse(selectedCustomerEmail)); // customer email in To section of email
                                        }
                                    message.Cc.Add(InternetAddress.Parse(Globals.EmailData[0].CopyEmail));// admin email                         
                                   //message.Cc.Add(InternetAddress.Parse("tickets@ismwaste.co.uk"));// admin email
                                    message.Subject = Globals.TicketData[0].Invocie_Name + " " + Globals.TicketData[0].TicketNo;
                                        //  System.Net.Mail.Attachment attachment;
                                        string path = Globals.storagePath + "/WCA/Tickets/" + "Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf";
                                        //  attachment = new System.Net.Mail.Attachment(path, System.Net.Mime.MediaTypeNames.Application.Pdf);
                                        var builder = new BodyBuilder();
                                    /* if (Globals.TicketData[0].CancelreasonId != 0)
                                     {
                                         builder.TextBody = "Job has been cancelled due to " + Globals.TicketData[0].Cancelreason;
                                     }*/


                                    String textBody_customer= "Dear Customer \nPlease find a attached copy of the ticket for today's service visit.";

                                    if (Globals.EmailData[0].Message != null)
                                    {
                                        textBody_customer += "\n" + Globals.EmailData[0].Message+ "\n";
                                    }
                                    textBody_customer += "\nRegards\nISM Waste & Recycling";

                                    builder.TextBody = textBody_customer;
                                        builder.Attachments.Add(path); // attach pdf
                                        message.Body = builder.ToMessageBody();

                                    }
                                    var message_admin = new MimeMessage();
                                    //if (Globals.Vehicle_Type == 1 )
                                    //{
                                    //    if (File.Exists(Globals.storagePath + "/WCA/Tickets/" + "ISM_Copy_Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf"))
                                    //{ 
                                        // send mail to admin

                                        message_admin.From.Add(InternetAddress.Parse(Globals.EmailData[0].SenderEmail));
                                //message_admin.To.Add(InternetAddress.Parse("tickets@ismwaste.co.uk"));// admin email
                                message_admin.To.Add(InternetAddress.Parse(Globals.EmailData[0].CopyEmail));// admin email
                                message_admin.Subject = "ISM Copy " + Globals.TicketData[0].Invocie_Name + " " + Globals.TicketData[0].TicketNo;
                                        //  System.Net.Mail.Attachment attachment;
                                        string path_admin_ticket = Globals.storagePath + "/WCA/Tickets/" + "ISM_Copy_Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf";
                                        //  attachment = new System.Net.Mail.Attachment(path, System.Net.Mime.MediaTypeNames.Application.Pdf);

                                        var builder_admin_email = new BodyBuilder();
                                        string job_body = "\nJob has Following Waste Type(s): " + Globals.TicketData[0].WasteTypeString + ".";

                                        if (Globals.TicketData[0].CancelreasonId != 0 && Globals.TicketData[0].CancelreasonId != 15)
                                        {
                                            job_body += "\n\nJob has been cancelled due to " + Globals.TicketData[0].Cancelreason;
                                        }
                                        if (Globals.TicketData[0].CancelreasonId == 15)
                                        {
                                            job_body += "\nContaminated Job.";
                                        }
                                        builder_admin_email.TextBody = job_body;
                                if (File.Exists(path_admin_ticket) && Globals.Vehicle_Type == 1)
                                {
                                    builder_admin_email.Attachments.Add(path_admin_ticket); // attach pdf
                                }
                                else if (!File.Exists(path_admin_ticket) && (Globals.Vehicle_Type == 2 || Globals.Vehicle_Type == 3))
                                {
                                    builder_admin_email.Attachments.Add(Globals.storagePath + "/WCA/Tickets/" + "Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf"); // attach pdf
                                }
                                //else
                                //{
                                //    DisplayAlert("Send Mail", "Please Try again !", "OK");
                                //}
                                foreach (var element in SelectedImages)
                                        {
                                            builder_admin_email.Attachments.Add(element); // attach images
                                        }
                                        message_admin.Body = builder_admin_email.ToMessageBody();
                                    //}
                                    //else
                                    //{
                                    //    DisplayAlert("Send Mail", "Please Try again !", "OK");
                                    //}
                                //}
                                //else
                                //{
                                //    DisplayAlert("Send Mail", "Please Try again !", "OK");
                                //}
                                using (var client = new SmtpClient())
                                    {
                                        client.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
                                        //client.CheckCertificateRevocation = false;
                                        client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                                    client.Connect(Globals.EmailData[0].Server, int.Parse(Globals.EmailData[0].Outgoingport), MailKit.Security.SecureSocketOptions.StartTls);
                                    //client.Connect(Globals.EmailData[0].Server, int.Parse(Globals.EmailData[0].Outgoingport), true);
                                    client.Authenticate(Globals.EmailData[0].Username, Globals.EmailData[0].Password);
                                        //if (Globals.Vehicle_Type == 1)
                                        //{
                                            client.Send(message_admin);
                                        //}
                                        if (Globals.TicketData[0].CancelreasonId == 0 || Globals.TicketData[0].CancelreasonId == 15)
                                        {
                                            client.Send(message);
                                        }
                                        client.Disconnect(true);
                                    }
                                    //  DisplayAlert("Send Mail", "Mail Sent Successfully !", "OK");


                                    //if (File.Exists(Globals.storagePath + "/WCA/Tickets/" + "Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf"))
                                    //{
                                      Application.Current.MainPage.Navigation.PopAsync();
                                    new JobsGroupViewModel().markJobAsDone_EventAsync(false,false);
                                    //}
                                    //else
                                    //{
                                    //    DisplayAlert("Finish Job", "Please Generate PDF Ticket!", "OK");
                                    //}



                                    //  btnFinishTicket.IsVisible = true;

                                }
                                else
                                {
                                    DisplayAlert("Send Mail", "Please Try again !", "OK");
                                    //DisplayAlert("Send Mail", "Please Generate PDF Ticket!", "OK");
                                }
                            }
                            else
                            {
                                DisplayAlert("Send Mail", "Internet Connection Required !", "OK");
                            }


                        //}
                    }



                  
                }
                catch (Exception ex)
                 {
                    btnFinishTicket.IsVisible = false;
                    DisplayAlert("Send Mail", "Mail Sending Failed,Try Again !", "OK");
                }
                flag_sendMail_ticket = true;
            }
            

        }

        private void btnFinish_Clicked(object sender, EventArgs e)
        {

            if (File.Exists(Globals.storagePath + "/WCA/Tickets/" + "Ticket_" + Globals.TicketData[0].CustomerName + "_" + Globals.TicketData[0].TicketNo + ".pdf"))
            {
                Application.Current.MainPage.Navigation.PopAsync();
                new JobsGroupViewModel().markJobAsDone_EventAsync(false,false);
            }
            else
            {
                DisplayAlert("Finish Job", "Please Generate PDF Ticket!", "OK");
            }

        }
        private void btn_Job_Completed(object sender, EventArgs e)
        {
            //bool Generate_return = this.ViewModel.PDfTicket_Generate();
            //if (Generate_return)
            //{
                new JobsGroupViewModel().markJobAsDone_EventAsync(true,false);
            //}
            //else {
            //    DisplayAlert("Finish Job", "Unable to Generate PDF Ticket.", "OK");
            //}
        }

        private void btnAttachImages_Clicked(object sender, EventArgs e)
        {
            imageSelectionAsync();

        }
    }

}
