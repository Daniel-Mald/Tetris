using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kirby_New_Adventure
{
    public class ControlNiveles
    {
        public int Nivel { get; set; } 
        public int PuntajeTotal { get; set; }
        public int PuntajeNivel { get; set; }
        public ControlNiveles()
        {
            Nivel = 1;
            PuntajeTotal = 0;
            PuntajeNivel = 0;
        }

        public void SiguienteNivel()
        {
             Nivel ++;
        }
        public void AumentarPuntaje()
        {
            PuntajeTotal += PuntajeNivel;
        }
        
    }
}
