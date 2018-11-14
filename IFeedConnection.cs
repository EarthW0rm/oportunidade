using System.Threading.Tasks;
using System.Xml;

namespace oportunidade {
    public interface IFeedConnection {
        Task<XmlDocument> GetFeed();
    }
}