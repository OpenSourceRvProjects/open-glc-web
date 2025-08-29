using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.InternalModels
{
    public class FrasesMotivacionale
    {
        public string frase { get; set; }
        public string autor { get; set; }
    }

    public class PhraseModel
    {
        public List<FrasesMotivacionale> frases_motivacionales { get; set; }
    }

}
