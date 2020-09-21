using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReceiptChecking
{
    class Program
    {
        public interface IChecking
        {
            void Reward(string number);
            void Winning(string item);
        }
        public class Checking : IChecking
        {
            private string _specialPrize;
            private string _grandPrize;
            private string _firstPrize1;
            private string _firstPrize2;
            private string _firstPrize3;
            private string _addSixPirze;
            public Checking(string SpecialPrize, string GrandPrize, string FirstPrize1, string FirstPrize2, string FirstPrize3, string AddSixPirze)
            {
                _specialPrize = SpecialPrize;
                _grandPrize = GrandPrize;
                _firstPrize1 = FirstPrize1;
                _firstPrize2 = FirstPrize2;
                _firstPrize3 = FirstPrize3;
                _addSixPirze = AddSixPirze;
            }
            public void Reward(string number)
            {
                //特別獎
                if (String.Compare(_specialPrize, number.Substring(0, 8)) == 0)
                {
                    Winning("SpecialPrize");
                }
                //特獎
                if (String.Compare(_grandPrize, number.Substring(0, 8)) == 0)
                {
                    Winning("GrandPrize");
                }
                //頭獎
                if (String.Compare(_firstPrize1, number.Substring(0, 8)) == 0 || String.Compare(_firstPrize2, number.Substring(0, 8)) == 0 || String.Compare(_firstPrize3, number.Substring(0, 8)) == 0)
                {
                    Winning("FirstPrize");
                }
                //二獎
                if (String.Compare(_firstPrize1, number.Substring(1, 8)) == 0 || String.Compare(_firstPrize2, number.Substring(1, 8)) == 0 || String.Compare(_firstPrize3, number.Substring(1, 8)) == 0)
                {
                    Winning("SecondPrize");
                }
                //三獎
                if (String.Compare(_firstPrize1, number.Substring(2, 8)) == 0 || String.Compare(_firstPrize2, number.Substring(2, 8)) == 0 || String.Compare(_firstPrize3, number.Substring(2, 8)) == 0)
                {
                    Winning("ThirdPrize");
                }
                //四獎
                if (String.Compare(_firstPrize1, number.Substring(3, 8)) == 0 || String.Compare(_firstPrize2, number.Substring(3, 8)) == 0 || String.Compare(_firstPrize3, number.Substring(3, 8)) == 0)
                {
                    Winning("FourthPrize");
                }
                //五獎
                if (String.Compare(_firstPrize1, number.Substring(4, 8)) == 0 || String.Compare(_firstPrize2, number.Substring(4, 8)) == 0 || String.Compare(_firstPrize3, number.Substring(4, 8)) == 0)
                {
                    Winning("FifthPrize");
                }
                //六獎
                if (String.Compare(_addSixPirze, number.Substring(5, 8)) == 0)
                {
                    Winning("SixthPrize");
                }
            }
            public void Winning(string item)
            {
                switch (item)
                {
                    case "SpecialPrize":
                        Console.WriteLine("您中了 特別獎! 獲得 獎金1,000萬元");
                        break;
                    case "GrandPrize":
                        Console.WriteLine("您中了 特獎! 獲得 獎金200萬元");
                        break;
                    case "FirstPrize":
                        Console.WriteLine("您中了 頭獎! 獲得 獎金20萬元");
                        break;
                    case "SecondPrize":
                        Console.WriteLine("您中了 二獎! 獲得 獎金4萬元");
                        break;
                    case "ThirdPrize":
                        Console.WriteLine("您中了 三獎! 獲得 獎金1萬元");
                        break;
                    case "FourthPrize":
                        Console.WriteLine("您中了 四獎! 獲得 獎金4千元");
                        break;
                    case "FifthPrize":
                        Console.WriteLine("您中了 五獎! 獲得 獎金1千元");
                        break;
                    case "SixthPrize":
                        Console.WriteLine("您中了 六獎! 獲得 獎金2百元");
                        break;
                    default:
                        Console.WriteLine("未中獎 請再接再厲!");
                        break;
                }
            }
        }
        static void Main(string[] args)
        {
            var url = "https://www.etax.nat.gov.tw/etw-main/web/ETW183W2_";
            string firstPrizeResult = "";
            string receiptNumber = "";
            string result = "";

            while (true)
            {
                Console.WriteLine("輸入欲兌獎月份(格式範例:10905)");
                string month = Console.ReadLine();
                result = ReadHtml(url + month + "/");
                if (result != "false")
                {
                    Console.WriteLine("輸入發票編號");
                    receiptNumber = Console.ReadLine();
                    bool numCheck = Regex.IsMatch(receiptNumber, @"^[0-9]{8}$");
                    if (numCheck)
                        break;
                }
                Console.WriteLine("發生錯誤請重試!\n");
            }

            //搜尋頭尾關鍵字(For頭獎)
            int first = result.IndexOf("<td headers=\"firstPrize\" class=\"number\"> ");
            int last = result.LastIndexOf("<p></p> </td>");

            //減去頭尾不要的字元或數字, 並將結果轉為string (For頭獎)
            string HTMLCut = result.Substring(first, last - first);
            firstPrizeResult = HTMLCut;

            //取得當月中獎號碼
            var prizeNumber = new ReceiptItem();
            prizeNumber.SpecialPrize = Regex.Match(result, @"(?<=<td headers=""specialPrize"" class=""number""> )((?:\d)+)(?= </td>)", RegexOptions.IgnoreCase).ToString();
            prizeNumber.GrandPrize = Regex.Match(result, @"(?<=<td headers=""grandPrize"" class=""number""> )((?:\d)+)(?= </td>)", RegexOptions.IgnoreCase).ToString();
            var FirstPrize = Regex.Matches(firstPrizeResult, @"(?<=<p>)((?:\d)+)(?=</p>)");
            prizeNumber.FirstPrize1 = FirstPrize[0].ToString();
            prizeNumber.FirstPrize2 = FirstPrize[1].ToString();
            prizeNumber.FirstPrize3 = FirstPrize[2].ToString();
            prizeNumber.AddSixPirze = Regex.Match(result, @"(?<=<td headers=""addSixPrize"" class=""number""> )((?:\d)+)(?= </td>)", RegexOptions.IgnoreCase).ToString();

            //將參數傳入並確認是否中獎
            IChecking checking = new Checking(prizeNumber.SpecialPrize, prizeNumber.GrandPrize, prizeNumber.FirstPrize1, prizeNumber.FirstPrize2, prizeNumber.FirstPrize2, prizeNumber.AddSixPirze);
            checking.Winning(receiptNumber);

            Console.ReadKey();

        }
        public static string ReadHtml(string url)
        {
            try
            {
                WebRequest myRequest = WebRequest.Create(url);

                //Method選擇GET
                myRequest.Method = "GET";

                //取得WebRequest的回覆
                WebResponse myResponse = myRequest.GetResponse();

                //Streamreader讀取回覆
                StreamReader sr = new StreamReader(myResponse.GetResponseStream());

                //將全文轉成string
                string result = sr.ReadToEnd();

                //關掉StreamReader
                sr.Close();

                //關掉WebResponse
                myResponse.Close();

                return result;
            }
            catch
            {
                return "false";
            }
        }        
    }

    class ReceiptItem
    {
        public string SpecialPrize { get; set; }
        public string GrandPrize { get; set; }
        public string FirstPrize1 { get; set; }
        public string FirstPrize2 { get; set; }
        public string FirstPrize3 { get; set; }
        public string AddSixPirze { get; set; }
    }
}
