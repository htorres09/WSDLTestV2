using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesterSOAP.Clases
{
    class Alumno
    {
        private string matricula;
        private string primerApellido;
        private string segundoApellido;
        private string nombre;
        private string nomDependencia;
        private string cveDependencia;
        private string cveUnidad;

        public Alumno(string m, string ap, string am, string n, string cd, string cu, string d)
        {
            this.matricula = m;
            this.primerApellido = ap;
            this.segundoApellido = am;
            this.nombre = n;
            this.nomDependencia = d;
            this.cveDependencia = cd;
            this.cveUnidad = cu;
        }

    }
}
