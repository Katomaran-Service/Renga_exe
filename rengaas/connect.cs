using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Security.AccessControl;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Data;
using System.Net.Http;

namespace rengaas
{
    
    
    public class connect
    {
        public static  HttpClient client = new HttpClient();
       
        public class Item1
        {
            public string num { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public string po { get; set; }
            public string customer_name { get; set; }
            public string sales_man { get; set; }
            public string color { get; set; }
            public string stat { get; set; }
            public string product_code { get; set; }
            public string product_name { get; set; }
            public string pquantity { get; set; }
            public string poquantity { get; set; }
            public string price { get; set; }
        }
        public static List<Item1> _items1 = new List<Item1>();
        public static List<Item1> Items1
        {
            get { return _items1; }
            set { _items1 = value; }
        }
        public static string username,details,auth_token;
        static string _ipString = string.Empty;
        public static string login_url, product_url,customer_url,software_get_url,android_get_url,as_add_url,as_delete_url ,android_edit_url,order_get_url, shop_get_url , admin_view_url, android_no_url;
        public static string ipconnectString
        {
            get { return _ipString; }

            set { _ipString = value; }
        }
        public static void configure()
        {
            try
            {


                ipconnectString = "http://192.168.43.84:3000/";
                login_url = ipconnectString + "login";
                product_url = ipconnectString + "api/v1/products";
                customer_url = ipconnectString + "api/v1/customers";
                software_get_url = ipconnectString + "api/v1/users/admin";
                android_get_url = ipconnectString + "api/v1/users/user";
                android_no_url = ipconnectString + "approval_no";
                as_add_url = ipconnectString + "api/v1/users";
                as_delete_url = ipconnectString + "api/v1/users";
                order_get_url = ipconnectString + "api/v1/orders";
                shop_get_url = ipconnectString + "shop_details";
                admin_view_url = ipconnectString + "view";
                //ipconnectString = "http://192.168.0.105:3000/";

                //MessageBox.Show(ipconnectString);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "config error");
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    configure();
                    
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool valid_email(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
       
        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean isvalidph(string phone)
        {
            if (Regex.Match(phone, @"^([0]|\+6[0-9]{1})([0-9]{9})$").Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
