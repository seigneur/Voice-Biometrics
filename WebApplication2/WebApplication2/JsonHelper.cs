using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebApplication2
{
    public static class JSONHelper
    {
        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJSON(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }
        public static Reco FromJSON(string obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Reco obj1 = serializer.Deserialize<Reco>(obj);
            return obj1;
        }

    }

    public class Reco
    {
        public string status { get; set; }
        public string id { get; set; }

        public System.Collections.ArrayList hypotheses { get; set; }
    }

    public class Analys
    {
        public string utterance { get; set; }
    }


    public class AuthenticationInfo
    {
        public string UserName { get; set; }
        public string RecognizedWordByGoogle { get; set; }
        public decimal Confidence { get; set; }
    }

}

