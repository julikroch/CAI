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
            int opcionSeleccionada;
            Console.WriteLine($"Bienvenido al sistema de la Facultad de Ciencias Economicas de la UBA.\n 1. Iniciar el proceso de registración de la solicitud de inscripción a materias seleccionadas\n 2. Consultar posibles materias y curso a seleccionar\n 3. Finalizar \n\n Ingrese su opcion");

            do {
                opcionSeleccionada = Convert.ToInt32(Console.ReadLine());
                switch (opcionSeleccionada) {
                    case 1:
                        solicitudInscripcion();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Seleccione una opcion correcta");
                        break;
                }
            } while (opcionSeleccionada >= 4);
        }

        public static void solicitudInscripcion() {
            string numeroRegistro = verificarRegistro();

            if (numeroRegistro == "Error") {
                Console.WriteLine(numeroRegistro);
                Console.ReadKey();
            }
            else
            {
                string materia1 = "";
                string solicitud = "";
                string materia2 = "";
                string solicitud2 = "";
                string materia3 = "";
                string solicitud3 = "";

                //solicitud = solicitud + ";" + VerificarCarrera();
                materia1 = verificarMateria();
                solicitud = numeroRegistro + ";" + materia1;
                solicitud = solicitud + ";" + VerificarCurso() + ";";

                Console.WriteLine("Queres inscribirte en otra materia? Ingresa S en caso afirmativo o presione cualquier tecla para finalizar");

                if (Console.ReadLine() == "s" || Console.ReadLine() == "S")
                {
                    materia2 = verificarMateria();
                    while (materia1 == materia2)
                    {
                        Console.WriteLine("Ya seleccionó esa materia, Elija otra materia a cursar");
                        materia2 = verificarMateria();
                    }
                    solicitud2 = numeroRegistro + ";" + materia2;
                    solicitud2 = solicitud2 + ";" + VerificarCurso() + ";";
                    Console.WriteLine("Queres inscribirte en otra materia? Ingresa S en caso afirmativo o presione cualquier teclar para finalizar");

                    if (Console.ReadLine() == "s")
                    {
                        materia3 = verificarMateria();
                        while (materia1 == materia3 || materia2 == materia3)
                        {
                            Console.WriteLine("Ya seleccionó esa materia, Elija otra materia a cursar");
                            materia3 = verificarMateria();
                        }
                        solicitud3 = numeroRegistro + ";" + materia3;
                        solicitud3 = solicitud3 + ";" + VerificarCurso() + ";";
                    }
                }

                string Path = "SolicitudInscripción " + numeroRegistro + ".csv";
                FileStream stream = null;
                stream = new FileStream(Path, FileMode.OpenOrCreate);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine("Numero de registro; Codigo de materia; Codigo de Curso;");
                    writer.WriteLine(solicitud);
                    if (solicitud2 != "")
                    {
                        writer.WriteLine(solicitud2);
                        if (solicitud3 != "")
                        {
                            writer.WriteLine(solicitud3);
                        }
                    }
                    Console.WriteLine("Su solicitud de insripción ha sido realizada con éxito. Aquí le dejamos su comprobante de Inscripción:");
                    Console.WriteLine(solicitud);
                    Console.WriteLine(solicitud2);
                    Console.WriteLine(solicitud3);
                }
            }
        }

        public static string verificarRegistro() {
            string Path = "alumnosRegulares.csv";
            FileInfo fileInfo = new FileInfo(Path);
            string mensaje = "Error";

            if (!fileInfo.Exists) {
                Console.WriteLine("Debe existir el maestro de alumnos.");
                return mensaje;
            }
            else {
                StreamReader sr = fileInfo.OpenText();
                int registro = 0;
                int registroMin = 000000;
                int registroMax = 999999;

                bool registroIngresado = false;
                bool registroValido = false;
                bool alumnoEncontrado = false;

                do {
                    Console.WriteLine("Ingrese su número de registro");

                    registroValido = int.TryParse(Console.ReadLine(), out registro);
                    if (!registroValido) {
                        Console.WriteLine("Ingrese un número entero");
                    }
                    else {
                        if (registro > registroMin & registro < registroMax) {
                            registroIngresado = true;
                        }
                        else Console.WriteLine($"Número de registro incorrecto. Por favor ingrese un numero de registro entre {registroMin} y {registroMax}");
                    }
                } while (!registroIngresado && !registroValido);

                do {
                    string p = sr.ReadLine();
                    string[] arr = p.Split(';');

                    if (registro == Convert.ToInt32(arr[0]))
                    {
                        alumnoEncontrado = true;
                        Console.WriteLine($"Alumno: {arr[1]} {arr[2]}");
                        if (arr[3] == "Libre")
                        {
                            Console.WriteLine("El alumno no se encuentra en regularidad para cursar");
                        }
                        else
                        {
                            mensaje = arr[0];
                        }
                    }
                } while (!sr.EndOfStream);
                if (!alumnoEncontrado)
                {
                    Console.WriteLine("El alumno no figura en el maestro de alumnos");
                }
            }
            return mensaje;
        }

        public static string verificarMateria() {
            string Path = "materiasxAlumno.csv";
            FileInfo FI = new FileInfo(Path);

            if (!FI.Exists) {
                Console.WriteLine("No existe el archivo de materias por alumno");
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
                        Console.WriteLine("Ingrese un número entero");
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
                        Console.WriteLine("Materia: " + vector[1] + 
                                           "\nCorrelativas: " + vector[2]);
                        return vector[0];
                    }
                }
                return "error";
            }
        }

        public static string VerificarCurso() {
            string Path = "cursosxAlumno.csv";
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
                        Console.WriteLine("Titular: " + vector[1]);
                        return vector[0];
                    }
                }
                return "error";
            }
        }
    }
}