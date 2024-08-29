using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appLivros
{
    public class Livros
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateOnly Data { get; set; }
        public string Genero { get; set; }
        public int Pg { get; set; }

        public override string ToString()
        {
            return $"Titulo {Titulo}, Autor {Autor}, Data {Data}, Genero {Genero}, Pg {Pg}";
        }

    }
}
