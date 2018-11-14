using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace oportunidade {
    public class FeedReader {
        const string itemTagName = "item";
        const int itensLimit = 10;
        private IFeedConnection feedConnection;

        public FeedReader(IFeedConnection _feedConnection) {
            this.feedConnection = _feedConnection;
        }

        public async Task<IList<ItemRelevantInfo>> ReadRelavantInfo() {
            XmlDocument xmlDoc = await this.feedConnection.GetFeed();
            XmlNodeList rawItens = xmlDoc.GetElementsByTagName(itemTagName);

            IList<ItemRelevantInfo> bakedItens = new List<ItemRelevantInfo>();

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);  
            nsmgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");  

            int limit = rawItens.Count > 10 ? itensLimit : rawItens.Count;

            for(var i = 0; i < limit; i++) {
                XmlNode rawItem = rawItens[i];
                var descriptionNode = rawItem.SelectSingleNode("description");
                var titulo = rawItem.SelectSingleNode("title");                
                var contentNode = rawItem.SelectSingleNode("content:encoded", nsmgr);
                bakedItens.Add(new ItemRelevantInfo(titulo.InnerText, descriptionNode.InnerText, contentNode.InnerText));
            }

            return bakedItens;
        }
    }
}