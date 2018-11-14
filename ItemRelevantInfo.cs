using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace oportunidade {
    public class ItemRelevantInfo
    {
        private string tituloTopico;
        public string TituloTopico { 
            get { return this.tituloTopico; } 
        }

        private string[] rawTexts;
        private string[] bakedTexts;
        
        private KeyValuePair<string, int>[] topicWordsCount;
        public KeyValuePair<string, int>[] TopicWordsCount { 
            get { return this.topicWordsCount; } 
        }

        private int totalWords = 0;
        public int TotalWords { 
            get { return this.totalWords; } 
        }

        public ItemRelevantInfo(params string[] _relevantText) {
            this.rawTexts = _relevantText;

            this.bakedTexts = this.rawTexts
                .Select(raw => raw.ToLower())
                .Select(raw => " " + raw + " ")
                .Select(raw => this.RemoveUnwantedHtml(raw))
                .Select(raw => this.RemoveUnwantedScapesChars(raw))
                .Select(raw => this.RemoveUnwanted(raw))
                .ToArray();
            
            var uniBakedText = string.Join(" ", this.bakedTexts);

            var splitedWords = uniBakedText.Split(" ").Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();

            this.totalWords = splitedWords.Length;

            this.topicWordsCount = splitedWords.GroupBy(word => word).OrderByDescending(d => d.Count()).Select(item => new KeyValuePair<string, int>(item.Key, item.Count())).ToArray();
        }
        
        private string RemoveUnwanted(string _text) {
            var text = Regex.Replace(_text, @" o | os | a | as | um | uns | uma | umas | a | ao | aos | à | às | de | do | dos | da | das | dum | duns | duma | dumas | em | no | nos | na | nas | num | nuns | numa | numas | por | per | pelo | pelos | pela | pelas |[,]|[\.]|[?]|[!]/g", " " );
            return text;
        }
        private string RemoveUnwantedScapesChars(string _text) {
            var text = Regex.Replace(_text, @"[\n]|[\r]", " ");
            return text;
        }
        private string RemoveUnwantedHtml(string _text) {
            var text = Regex.Replace(_text, "<.*?>|&.*?;", string.Empty);
            return text;
        }

    }
}