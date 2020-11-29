using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace trabajoPractico5
{
    class Program
    {
        static void Main(string[] args)
        {
            string numeroRegistro = verificarRegistro();

            string materia1 = "";
            string solicitud = "";
            string materia2 = "";
            string solicitud2 = "";
            string materia3 = "";
            string solicitud3 = "";

            //solicitud = solicitud + ";" + VerificarCarrera();
            materia1 = VerificarMateria();
            solicitud = numeroRegistro + ";" + materia1;
            solicitud = solicitud + ";" + VerificarCurso() + ";";

            Console.WriteLine("Queres inscribirte en otra materia? Ingresa S en caso afirmativo o presione cualquier tecla para finalizar");

            if (Console.ReadLine() == "s" || Console.ReadLine() == "S") {
                materia2 = VerificarMateria();
                while (materia1 == materia2) {
                    Console.WriteLine("Ya seleccionó esa materia, Elija otra materia a cursar");
                    materia2 = VerificarMateria();
                }
                solicitud2 = numeroRegistro + ";" + materia2;
                solicitud2 = solicitud2 + ";" + VerificarCurso() + ";";
                Console.WriteLine("Queres inscribirte en otra materia? Ingresa S en caso afirmativo o presione cualquier teclar para finalizar");

                if (Console.ReadLine() == "s") {
                    materia3 = VerificarMateria();
                    while (materia1 == materia3 || materia2 == materia3) {
                        Console.WriteLine("Ya seleccionó esa materia, Elija otra materia a cursar");
                        materia3 = VerificarMateria();
                    }
                    solicitud3 = numeroRegistro + ";" + materia3;
                    solicitud3 = solicitud3 + ";" + VerificarCurso() + ";";
                }
            }

            string Path = "SolicitudInscripción " + numeroRegistro + ".csv";
            FileStream stream = null;
            stream = new FileStream(Path, FileMode.OpenOrCreate);

            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8)) {
                writer.WriteLine(solicitud);
                if (solicitud2 != "") {
                    writer.WriteLine(solicitud2);
                    if (solicitud3 != "") {
                        writer.WriteLine(solicitud3);
                    }
                }
                Console.WriteLine("Su solicitud de insripción ha sido realizada con éxito. Aquí le dejamos su comprobante de Inscripción:");
                Console.WriteLine(solicitud);
                Console.WriteLine(solicitud2);
                Console.WriteLine(solicitud3);
            }
        }

        public static string verificarRegistro() {
            string Path = "alumnosRegulares.csv";
            FileInfo fileInfo = new FileInfo(Path);

            if (!fileInfo.Exists) {
                Console.WriteLine("El archivo de alumnos regulares no se encuentra dentro de la aplicacion.");
                return "error";
            }
            else {
                StreamReader sr = fileInfo.OpenText();
                int registro = 0;
                bool registroOk = false;
                bool correcto = false;
                while (!registroOk) {
                    Console.WriteLine("Ingrese número de registro");
                    correcto = int.TryParse(Console.ReadLine(), out registro);
                    if (correcto == false) {
                        Console.WriteLine("Ingrese un Número");
                    }
                    else {
                        if (registro > 888000 & registro < 999999) {
                            registroOk = true;
                        }
                        else Console.WriteLine("Número de registro incorrecto. Vuelva a intentar");
                    }
                }
                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (registro == Convert.ToInt32(vector[0])) {
                        Console.WriteLine("Bienvenido " + vector[1]);
                        return vector[0];
                    }
                }
                return "error";
            }
        }

        public static string VerificarMateria() {
            string Path = "MateriasxAlumno.csv";
            FileInfo FI = new FileInfo(Path);

            if (!FI.Exists) {
                Console.WriteLine("No existe el archivo");
                return "error";
            }
            else {
                StreamReader sr = FI.OpenText();
                int materia = 0;
                bool materiaOk = false;
                bool correcto = false;
                while (!materiaOk) {
                    Console.WriteLine("Ingrese código de materia");
                    correcto = int.TryParse(Console.ReadLine(), out materia);
                    if (correcto == false) {
                        Console.WriteLine("Ingrese un Número");
                    }
                    else {
                        if (materia > 0 & materia < 6) {
                            materiaOk = true;
                        }
                        else Console.WriteLine("Codigo de materia incorrecto. Vuelva a intentar");
                    }
                }

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (materia == Convert.ToInt32(vector[0])) {
                        Console.WriteLine("Usted selecciono la materia " + vector[1]);
                        return vector[0];
                    }
                }
                return "error";
            }
        }

        public static string VerificarCurso() {
            string Path = "CursosxAlumno.csv";
            FileInfo FI = new FileInfo(Path);

            if (!FI.Exists) {
                Console.WriteLine("No existe el archivo");
                return "error";
            }
            else {
                StreamReader sr = FI.OpenText();
                int curso = 0;
                bool cursoOk = false;
                bool correcto = false;
                while (!cursoOk) {
                    Console.WriteLine("Ingrese código de curso");
                    correcto = int.TryParse(Console.ReadLine(), out curso);
                    if (correcto == false) {
                        Console.WriteLine("Ingrese un Número");
                    }
                    else {
                        if (curso > 0 & curso < 4) {
                            cursoOk = true;
                        }
                        else Console.WriteLine("Codigo de curso incorrecto. Vuelva a intentar");
                    }
                }

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (curso == Convert.ToInt32(vector[0])) {
                        Console.WriteLine("Usted selecciono curso " + vector[1]);
                        return vector[0];
                    }
                }
                return "error";
            }
        }
    }
}