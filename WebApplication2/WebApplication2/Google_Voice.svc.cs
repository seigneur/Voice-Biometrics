using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using System.Net;

// Author : Dias
// Date   : 26/9/2012
namespace WebApplication2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Google_Voice" in code, svc and config file together.
    public class Google_Voice : IGoogle_Voice
    {
        public void DoWork()
        {
        }
        public String Welcome(String name)
        {
            return String.Format("Welcome {0}", name);
        }

        private String Call_Google_Local(byte[] postData)
        {
            String data="";
            String Surl = "https://www.google.com/speech-api/v1/recognize?xjerr=1&client=chromium&lang=en-US";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Surl);

            httpWebRequest.ContentType = "audio/x-flac; rate=44100";

            httpWebRequest.Method = "POST";

            // start the synchronous operation  
            
                Stream newStream = httpWebRequest.GetRequestStream();
                newStream.Write(postData, 0, postData.Length);
                newStream.Close();
            

            WebResponse webResponse2 = httpWebRequest.GetResponse();

            Stream stream2 = webResponse2.GetResponseStream();
            StreamReader reader2 = new StreamReader(stream2);
            data = reader2.ReadToEnd();
            webResponse2.Close();
            httpWebRequest = null;
            webResponse2 = null;
            return data;
        }


        private int Converttosplitfiles(string SavedFileName,string flacfile)
        {
            int Number_of_split_files = 1;
            string[] path = flacfile.Split('.');
            string bigpath;
            int decimalLength = Number_of_split_files.ToString("D").Length + 2; ;
            // start the conversion process
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = SavedFileName + " " + flacfile + " silence 1 1 1% 1 .5 1% : newfile : restart";
            process.StartInfo.FileName = @"C:\Program Files (x86)\sox-14-4-0\sox.exe";
            process.Start();
            while (!process.HasExited)
            {

            }
            bigpath = path[0] + Number_of_split_files.ToString("D" + decimalLength.ToString()) + ".flac";

            while (File.Exists(bigpath))
            {
                decimalLength = Number_of_split_files.ToString("D").Length + 2;
                Number_of_split_files++;
                bigpath = path[0] + Number_of_split_files.ToString("D" + decimalLength.ToString()) + ".flac";
            }
            return Number_of_split_files-1;//one extra as we initialized it at 1.
        }

        public String[] Call_Google(string strFileName)
        {
            //save file starts
            //hard coded the path need to get the file through web service
            string folderPath = @"C:\VoiceAuth\";
            string Name = folderPath + strFileName;
            string savedFName = Name;
            string[] parts = strFileName.Split('.');
            string flac_file = folderPath + parts[0] + ".flac";
            int SplitFileCounter;
            Encoding encoding = Encoding.ASCII;
            string[] RecognizedWordByGoogle;
            int decimalLength;
            string BigFileName;
            Stream formDataStream ;
            byte[] postData = null;
            string jsonstring="";
            //read the file
            try
            {
                
                //Here first convert call the function which will split the given audio file into smaller flac files minus
                SplitFileCounter = Converttosplitfiles(savedFName, flac_file);
                //Conversion end
                RecognizedWordByGoogle = new String[SplitFileCounter];
                while (SplitFileCounter > 0)
                {
                    
                    decimalLength = SplitFileCounter.ToString("D").Length + 2;
                    BigFileName = folderPath + parts[0] + SplitFileCounter.ToString("D" + decimalLength.ToString()) + ".flac";
                    //below is the call to the google api here we need to check the number of files.
                    //System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                    //process1.StartInfo.Arguments = BigFileName;// parts[0] + SplitFileCounter.ToString("D" + decimalLength.ToString());
                    //process1.StartInfo.FileName = folderPath + "reco.bat";
                    //process1.StartInfo.RedirectStandardOutput = true;
                    //process1.StartInfo.UseShellExecute = false;
                    //process1.Start();


                    if (BigFileName != "")
                    {

                        FileStream fs = File.Open(BigFileName, FileMode.Open, FileAccess.Read);
                        long numBytes = new FileInfo(savedFName).Length;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] fileData = br.ReadBytes((int)numBytes);

                        //string header = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"file[]\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", BoundaryString, savedFName);
                        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(BigFileName);
                        formDataStream = new System.IO.MemoryStream();
                        formDataStream.Write(headerbytes, 0, headerbytes.Length);

                        formDataStream.Write(fileData, 0, fileData.Length);
                        fs.Close();
                        formDataStream.Position = 0;
                        postData = new byte[formDataStream.Length];
                        formDataStream.Read(postData, 0, postData.Length);
                        formDataStream.Close();
                    }
                    jsonstring = Call_Google_Local(postData);

                    //RecognizedWordByGoogle[SplitFileCounter - 1] = Call_Google_Local(postData);


                    // Read the returned JSON

                    // WebApplication2.AuthenticationInfo auth = new WebApplication2.AuthenticationInfo();

                    //bool file_exists = System.IO.File.Exists(folderPath + BigFileName + ".txt");
                    //string jsonstring = "";
                    //if (file_exists)
                    //{
                    //    using (StreamReader sr = new StreamReader(folderPath + BigFileName  + ".txt"))
                    //    {
                    //        String line;
                    //        // Read and display lines from the file until the end of
                    //        // the file is reached.
                    //        while ((line = sr.ReadLine()) != null)
                    //        {
                    //            jsonstring = line;
                    //        }
                      //  }
                        if (jsonstring != "")
                        {
                            WebApplication2.Reco obj = WebApplication2.JSONHelper.FromJSON(jsonstring);

                            // foreach (string data in obj.hypotheses)
                            if (obj.hypotheses != null && obj.hypotheses.Count > 0)
                            {
                                System.Collections.ArrayList hyp = obj.hypotheses;
                                System.Collections.Generic.Dictionary<string, object> al = (System.Collections.Generic.Dictionary<string, object>)hyp[0];

                                object actualWordSpoken = "";
                                al.TryGetValue("utterance", out actualWordSpoken);

                                object confidence = 0;
                                al.TryGetValue("confidence", out confidence);


                                RecognizedWordByGoogle[SplitFileCounter-1] = actualWordSpoken.ToString();
                                RecognizedWordByGoogle[SplitFileCounter - 1] = RecognizedWordByGoogle[SplitFileCounter - 1].Replace("  ", "");
                            }
                            else
                                RecognizedWordByGoogle[SplitFileCounter - 1] = "No Result";
                        }                      
                    //}
                    SplitFileCounter--;//decrement the counter for each file that is loaded.  --> Move to end
                    
                }
            }


            catch (Exception ex)
            {
                RecognizedWordByGoogle = new String[1];
                RecognizedWordByGoogle[0] = "Error:" + ex.Message;
                return RecognizedWordByGoogle;

            }
            return RecognizedWordByGoogle;
        }
    }
}
