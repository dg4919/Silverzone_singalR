using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Configuration;
using SendGrid;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Constant;
using System.Linq;

namespace SilverzoneERP.Data
{
    // static class can only contain static methods
    internal static class ClassUtility
    {
        // by default private method()
        internal static string WriteImage(string Base64image)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Base64image))
                {
                    var imgPath = "";
                    if (Base64image.Contains(image_urlResolver.school_profilePic_main))
                    {
                        int startIndex = Base64image.IndexOf(image_urlResolver.school_profilePic_main);
                        imgPath = Base64image.Substring(startIndex, Base64image.Length - startIndex);
                        return imgPath;
                    }

                    if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/" + image_urlResolver.school_profilePic_main)))
                    {
                        System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + image_urlResolver.school_profilePic_main));
                    }

                    var imgData = Base64image.Split(',');
                    imgPath = image_urlResolver.school_profilePic_main + "IMG_" + Guid.NewGuid() + "." + (imgData[0].Split('/'))[1].Split(';')[0];
                    var mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + imgPath);
                    var imageBytes = Convert.FromBase64String(imgData[1]);
                    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        Image image = Image.FromStream(ms, true);
                        image.Save(mapPath);
                    }
                    return imgPath;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        // return project path where we want to save Image > at the end of string we metioned "Silverzone.Web" (project where we want to save image)
        static string image_Root = image_urlResolver.project_root;

        internal static bool DeleteImage(string ImagePath)
        {
            try
            {
                var mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + ImagePath);
                if (File.Exists(mapPath))
                {
                    File.Delete(mapPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static string ordinal(int num)
        {
            String[] suffix = { "th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th" };
            int m = num % 100;
            return num + suffix[(m > 10 && m < 20) ? 0 : (m % 10)];
        }

        internal static int get_smsCode()
        {
            // genrate 6 digit unique no.
            int _min = 100000;
            int _max = 999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        internal static bool send_message(string mobile, string msgText)
        {
            try
            {
                WebClient client = new WebClient();
                string baseurl = "http://www.fast.wbcsms.com/sendurlcomma.aspx?";

                baseurl += string.Format("user={0}&pwd={1}&senderid={2}&mobileno={3}&msgtext={4}&smstype=9",
                      ConfigurationManager.AppSettings["sms_userId"].ToString(),            // saved in web.config of Main project 
                      ConfigurationManager.AppSettings["sms_pwd"].ToString(),
                      ConfigurationManager.AppSettings["sms_senderId"].ToString(),
                      mobile, msgText);

                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // email is sending Async using Thread > so UI will not show progrssbar for long time untill method is not completed
        internal static bool sendMail(string EmailId, string subject, string body, string fromEmail)
        {
            string user = ConfigurationManager.AppSettings["userName"];
            string pasword = ConfigurationManager.AppSettings["Password"];

            try
            {
                SendGridMessage myMessage = new SendGridMessage();
                myMessage.AddTo(EmailId);
                myMessage.From = new MailAddress(fromEmail);
                myMessage.Subject = subject;
                myMessage.Html = body;

                // Create credentials, specifying your user name and password.
                var credentials = new NetworkCredential(user, pasword);

                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email, which returns an awaitable task.
                transportWeb.DeliverAsync(myMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // reqImg_Width & reqImg_Height r > optinal + default params
        public static List<string> upload_Images_toTemp(string tempPath, int reqImg_Width = 300, int reqImg_Height = 300)
        {
            var filesPath = new List<string>();

            var files = HttpContext.Current.Request.Files;
            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                // logging file Name
                TXTErrorLog(new Exception("Image File Log"), file.FileName);

                string FullfileName = genrate_uid() + ".jpg";
                filesPath.Add(image_urlResolver.image_baseUrl + tempPath + FullfileName);       // passing full > api Url + relative path

                string UploadFolderPath = image_Root + tempPath;

                if (!Directory.Exists(UploadFolderPath))
                    Directory.CreateDirectory(UploadFolderPath);

                GenerateThumbnails(Image.FromStream(file.InputStream), UploadFolderPath + FullfileName, reqImg_Width, reqImg_Height);
            }
            return filesPath;
        }


        internal static void GenerateThumbnails(Image image, string targetPath, int reqImg_Width, int reqImg_Height)
        {
            // static class / method can access static members > so we mark 'getScalFactor' as static
            float scaleFactor = getScalFactor(image.Width, image.Height, reqImg_Width, reqImg_Height);

            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);

            var thumbnailImg = new Bitmap(newWidth, newHeight);

            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            thumbGraph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);

            // if we got folder Access denied error after hosting project on IIS >
            // Right click on your main Project folder > Security > Add (IIS_IUSRS) & give full permissions
            thumbnailImg.Save(targetPath, ImageFormat.Jpeg);
        }

        private static float getScalFactor(int imgWidth, int imgHeight, int reqWidth, int reqHeight)
        {

            if ((imgWidth < reqWidth) && (imgHeight < reqHeight))
            {
                if ((float)imgWidth / (float)reqWidth < (float)imgHeight / (float)reqHeight)
                {
                    return ((float)reqHeight - (float)imgHeight) / (float)imgHeight;
                }
                else
                {
                    return ((float)reqWidth - (float)imgWidth) / (float)imgWidth;
                }
            }
            else
            {
                if ((float)imgWidth / (float)reqWidth > (float)imgHeight / (float)reqHeight)
                {
                    return (float)reqWidth / (float)imgWidth;
                }
                else
                {
                    return (float)reqHeight / (float)imgHeight;
                }
            }
        }

        internal static string genrate_uid()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return string.Format("{0:D8}", random);
        }

        private static IEnumerable<string> get_ImageNames(IEnumerable<string> images)
        {
            // image name contain full relative path like > '/Image/User/Profile/abc.jpg', So we just want image name
            return images.Select(x => x.Substring(x.LastIndexOf('/') + 1));
        }

        /// <summary>
        /// this method is Used to save Images in physical path
        /// </summary>
        /// <param name="imageName"> Contains only name of Image like abc.jpg </param>
        /// <param name="tempPath"> Source path from where file to be fetch/ temp Image path </param>
        /// <param name="finalPath"> Destination path from where file to be saved/ final Image path </param>
        /// <param name="is_save_Singlefile"> use if physical path contains a single file like for profile Pic </param>
        /// <param name="Singlefile_name"> Singlefile_name will contain name of file if 'is_save_Singlefile' will true </param>
        internal static bool save_Images_toPhysical(IEnumerable<string> imageName, string tempPath, string finalPath, bool is_save_Singlefile = false, string Singlefile_name = "") // default + optional parameters
        {
            try
            {
                imageName = get_ImageNames(imageName);

                //string Source = HostingEnvironment.MapPath("~/" + tempPath);         // get full path by relative path
                string Source = image_Root + tempPath;         // get full path by absolute(full) path
                string Destination = image_Root + finalPath;

                if (!Directory.Exists(Destination))         // check path is exist or not from where file to be saved
                    Directory.CreateDirectory(Destination);

                if (Directory.Exists(Source))        // check path is exist or not from where file to be fetch
                {
                    System.IO.DirectoryInfo myDirInfo = new DirectoryInfo(Source);

                    foreach (string image in imageName)         // parent Loop > loop through all filename
                    {
                        foreach (FileInfo file in myDirInfo.GetFiles())     // child Loop
                        {
                            if (file.Name.Equals(image))        // if image name from server is exist in temp path
                            {
                                System.IO.File.Copy
                                    (
                                    System.IO.Path.Combine(Source, file.Name),
                                    System.IO.Path.Combine(Destination, is_save_Singlefile ? Singlefile_name : file.Name),
                                    true        // override if file exists
                                    );

                                break;  // if condison match then exit from current loop & go to base/parent loop
                            }
                        }
                    }
                }
                Directory.Delete(Source, true);     // after save image to physial path remove it :)
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void TXTErrorLog(Exception ex, string Msg)
        {
            try
            {
                string strLogFilePath = image_Root + "logs.txt";

                if (!File.Exists(strLogFilePath))
                    File.Create(strLogFilePath).Close();

                using (StreamWriter w = File.AppendText(strLogFilePath))
                {
                    w.WriteLine("\r\nLog: ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error Message:" + ex;
                    w.WriteLine(err);
                    w.WriteLine("Message = " + Msg);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception exc)
            {
                ErrorLogsRepository obj = new ErrorLogsRepository(new SilverzoneERPContext());
                obj.logError(exc);
            }
            finally { }
        }
    }
}
