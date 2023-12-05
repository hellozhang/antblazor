using AntDesign.Pro.Template;
using AntDesign.Pro.Template.Models;

using Flurl.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace AntDesign.Services
{
    public class AliFundExt
    {
        private AliFundExt() { }
        public static AliFundExt instance = new AliFundExt();
        public async Task<AliAuth> GetAlIAuthAsync()
        {
            IFlurlResponse deafultRes = await "http://www.fund123.cn/fund".GetAsync();
            string cooike = string.Empty;
            foreach (var ite in deafultRes.Cookies)
            {
                cooike += $"{ite.Name}={ite.Value};";
            }
            string content = await deafultRes.GetStringAsync();
            MatchCollection matchs = Regex.Matches(content, "<script>(.*?)</script>");
            string[] valArray = matchs.FirstOrDefault().Value.Split(new string[] { "<script>", "</script>", "window.context = ", ";" }, StringSplitOptions.RemoveEmptyEntries);
            JObject js = JObject.Parse(valArray.FirstOrDefault());
            string csrf = js["csrf"].ToString();
            return new AliAuth { Csrf = csrf, Cooike = cooike };
        }
        public async Task<List<Item>> GetListX(string productId)
        {
            List<Item> lst = new List<Item>();
            DateTime startDate = DateTime.Now.AddDays(-3);
            DateTime endDate = DateTime.Now;  //本周周5

            //DateTime startDate = DateTime.Now.AddDays(1 - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))).AddDays(-3);
            //DateTime endDate = DateTime.Now.AddDays(1 - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))).AddDays(4);  //本周周5
            DateTime weekStart = DateTime.Now;
            AliAuth authToken = await AliFundExt.instance.GetAlIAuthAsync();
            var res = await "http://www.fund123.cn/api/fund/queryFundHistoryNetValueList"
            .WithHeader("Host", "www.fund123.cn")
            .WithHeader("Cookie", authToken.Cooike)
            .SetQueryParams(new { _csrf = authToken.Csrf })
            .PostJsonAsync(new
            {
                pageNum = 1,
                startDate = startDate.ToString("yyyyMMdd"),
                endDate = endDate.ToString("yyyyMMdd"),
                pageSize = 100,
                productId = productId
            }).ReceiveString();
            JObject objRes = JObject.Parse(res);
            JArray jArray = JArray.Parse(objRes["list"].ToString());
            foreach (var item in jArray)
            {
                //           {{
                // "key": 1,
                // "netValueDate": "2023-11-20",
                // "netValue": "1.9290",
                // "totalNetValue": "2.5090",
                // "dayOfGrowth": "0.0063

                lst.Add(new Item
                {
                    netValueDate = item["netValueDate"].ToDate(),
                    netValue = item["netValue"].ToDecimal(),
                    dayOfGrowth = Math.Round(item["dayOfGrowth"].ToDecimal()*100,2)
                });
            }
            
            return lst;
            //double percent = lst.Count > 0 ? (lst.LastOrDefault().netValue - lst.FirstOrDefault().netValue) / lst.FirstOrDefault().netValue : 0;
            //return new JObject(
            //    new JProperty("fundName", "fundName"),
            //    new JProperty("netValue", "1")
            //    );
        }


        public class Item
        {
            public DateTime netValueDate { get; set; }
            public decimal netValue { get; set; }
            public decimal dayOfGrowth { get; set; }
            public decimal totalNetValue { get; set; }

        }
    }
}
