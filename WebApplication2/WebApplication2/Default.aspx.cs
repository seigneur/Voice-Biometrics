using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            ServiceReference1.Google_VoiceClient wcfclient = new ServiceReference1.Google_VoiceClient();
            //Label1.Text = 
            String[] mylist = new String[10]; 
                mylist = wcfclient.Call_Google(FileUpload1.FileName);
           //mylist = wcfclient.Call_Google_Local(FileUpload1.FileName);
            Console.WriteLine("before array");
            //Label1.Text = string.Join(".",mylist);
            foreach(string value in mylist)
            {
                RadioButtonList1.Items.Add(value);
            }
        }

    }
}
