using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace oportunidade {
    public class FeedFileGetter : IFeedConnection
    {
        private string feedPath;
        private FileInfo feedFile;
        public FeedFileGetter(string _feedPath) {
            this.feedPath = _feedPath;
            this.feedFile = new FileInfo(this.feedPath);
            if(!this.feedFile.Exists) {
                throw new FileNotFoundException("Arquivo de feed não encontrado");
            }
        }

        public Task<XmlDocument> GetFeed()
        {
            Task<XmlDocument> xmlDoc = Task.Run(() => {
                FileStream fileReader = this.feedFile.OpenRead();
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.Load(fileReader);
                return _xmlDoc;
            });

            return xmlDoc;
        }
    }
}