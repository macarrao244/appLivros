using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appLivros
{
    public class Livros
    {
        public string Nome { get; set; }
        public string Curso { get; set; }
        public DateOnly DataMatricula { get; set; }
        public int Idade { get; set; }

        public override string ToString()
        {
            return $"Nome {Nome}, Curso {Curso}, DataMatricula {DataMatricula}, Idade {Idade}";
        }

    }
}
