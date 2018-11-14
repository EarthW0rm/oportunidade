using System;
using System.Collections.Generic;
using System.Linq;

namespace oportunidade
{
    class Program
    {
        const string feed_url = @"https://www.minutoseguros.com.br/blog/feed/";
        const string feed_path = @"D:\FONTE\GitHub\feed-palavras\feed.xml";
        static void Main(string[] args)
        {
            IFeedConnection webGetter = new FeedWebGetter(feed_url);
            // Caso necessario, foi aplicado inversao de controle. Varias fontes de Feed podem ser adicionadas.
            // IFeedConnection fileGetter = new FeedFileGetter(feed_path);

            FeedReader reader = new FeedReader(webGetter);
            reader.ReadRelavantInfo().ContinueWith((data) => {
                IList<ItemRelevantInfo> listTopics = data.Result;

                List<KeyValuePair<string, int>> unionTopics = new List<KeyValuePair<string, int>>();

                foreach(ItemRelevantInfo topic in listTopics) {
                    Console.WriteLine("Tópico: {0}\n\tTotal de Palavras: {1}", topic.TituloTopico, topic.TotalWords);
                    unionTopics.AddRange(topic.TopicWordsCount);
                }

                Console.WriteLine("\nAbaixo a lista das 10 palavras mais presentes.");

                var summaryWords = unionTopics.GroupBy(topicWords => topicWords.Key)
                    .Select(item => new KeyValuePair<string, int>(item.Key, item.Sum(d => d.Value)))
                    .OrderByDescending(d => d.Value)
                    .Take(10)
                    .Select(item => {
                        Console.WriteLine("\tPalavra: {0}, Total de aparições:{1}", item.Key, item.Value);
                        return item;
                    })
                    .ToArray();


            }).Wait();

            Console.WriteLine("Análise finalizada, pressione enter para finalizar...");
            Console.Read();
        }
    }
}
