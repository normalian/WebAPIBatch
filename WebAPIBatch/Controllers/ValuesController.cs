using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using RazorEngine;
using RazorEngine.Templating;
using WebAPIBatch.Models;

namespace WebAPIBatch.Controllers
{
    public class ValuesController : ApiController
    {
        // POST api/values
        public async Task Post([FromBody]WebAPIArgs args)
        {
            // （1）Yahoo!ニュースの結果を絞り込むために利用するキーワード
            var keyword = ConfigurationManager.AppSettings["YahooAPI:Keyword"];

            // （2）非同期形式でYahoo!ニュースWeb APIを呼び、キーワードで絞った検索結果を取得
            var results = await GetNewsAsync(keyword);

            // （3）メールを送信
            SendMails(args, keyword, results);
        }

        //Yahoo!ニュースWeb APIで利用する値を記載
        const string YAHOO_API_STRING = @"http://news.yahooapis.jp/NewsWebService/V2/topics?appid={0}&pickupcategory={1}";

        private async Task<IEnumerable<YahooNewsResult>> GetNewsAsync(string keyword)
        {
            // （a）Yahoo!ニュースの情報を取得
            var res = await new HttpClient().GetAsync(string.Format(YAHOO_API_STRING,
                ConfigurationManager.AppSettings["YahooAPI:Appid"],
                ConfigurationManager.AppSettings["YahooAPI:Pickupcategory"]));

            // （b）XMLデータを利用し、キーワードで該当するニュースを抽出
            return (await res.Content.ReadAsAsync<YahooNewsResultSet>())
                .Where(_ => _.Title.Contains(keyword) || _.Overview.Contains(keyword));
        }

        private void SendMails(WebAPIArgs args, string keyword, IEnumerable<YahooNewsResult> results)
        {
            using (var sc = new SmtpClient())
            {
                //SMTPサーバーを指定
                sc.Host = ConfigurationManager.AppSettings["Mail:SMTPServerAddress"];
                sc.Credentials =
                    new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["Mail:SMTPServerUsername"],
                        ConfigurationManager.AppSettings["Mail:SMTPServerPassword"]);

                //送付先のメールアドレスにニュースを送付する
                foreach (var toAddress in args.ToMailAddresses)
                {
                    //送付用 MailMessage の作成
                    using (var msg = CreateMailMessage(args, keyword, results, toAddress))
                    {
                        sc.Send(msg);
                    }
                }
            }
        }

        private MailMessage CreateMailMessage(WebAPIArgs args, string keyword, IEnumerable<YahooNewsResult> results, string toAddress)
        {
            var msg = new MailMessage();

            //送信者の設定
            msg.From = new MailAddress(args.FromMailAddress, args.FromMailName);

            //宛先の設定
            msg.To.Add(new MailAddress(toAddress));

            //件名の設定
            msg.Subject = keyword + "に関する記事です";

            //本文の設定(RazorEngine テンプレートエンジンを利用)
            msg.Body = results.Any() ? Razor.Parse(keyword + @"に関する記事のURLは以下になります
@foreach( var result in Model ){
@:・ @result.SmartphoneUrl
}", results) : keyword + @"に関する記事は見つかりませんでした";
            return msg;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}