using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace oportunidade {
    public class FeedWebGetter: IFeedConnection {
        private string feedUrl;
        private HttpClient httpClient;
        public FeedWebGetter(string _feedUrl) {
            this.feedUrl = _feedUrl;
            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.feedUrl);
        }

        public Task<XmlDocument> GetFeed() {
            var responseStream = httpClient.GetStreamAsync("");

            var xmlDoc = responseStream.ContinueWith((_feedResult) => {
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.Load(_feedResult.Result);
                return _xmlDoc;
            });

            return xmlDoc;
        }
    }
}