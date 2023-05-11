using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Models.Anamnesis
{
    public class Anamnesis
    {
        private List<string> _sympthomes { get; set; }  
        private List<string> _alergens { get; set; }

        public Anamnesis(List<string> sympthomes, List<string> alergens)
        {
            _sympthomes = sympthomes;
            _alergens = alergens;
        }   
        public Anamnesis()
        {
            _sympthomes = new List<string>();
            _alergens = new List<string>();
        }

        public bool ContainsSympthome(string sympthome)
        {
            return _sympthomes.Contains(sympthome);
        }

        public bool ContainsAlergen(string alergen)
        {
            return _alergens.Contains(alergen);
        }
    }
}
