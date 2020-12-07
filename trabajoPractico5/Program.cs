using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace trabajoPractico5
{
    class Program
    {
        static void Main(string[] args) {
            exhibirMenu();
        }

        public static void exhibirMenu(){
            int opcionSeleccionada;
            Console.WriteLine($"Bienvenido al sistema de la Facultad de Ciencias Economicas de la UBA.\n 1. Iniciar el proceso de registración de la solicitud de inscripción a materias seleccionadas\n 2. Consultar posibles materias y curso a seleccionar\n 3. Finalizar \n\n Ingrese su opcion");

            do
            {
                opcionSeleccionada = Convert.ToInt32(Console.ReadLine());
                switch (opcionSeleccionada)
                {
                    case 1:
                        solicitudInscripcion();
                        break;
                    case 2:
                        verOferta();
                        Console.WriteLine("Ingrese la letra V para volver al menu \n");
                        if(Console.ReadLine().ToLower() == "v") {
                            exhibirMenu();
                        }
                        break;
                    case 3:
                        Console.WriteLine("Ingrese cualquier caracter para finalizar\n");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Seleccione una opcion correcta\n");
                        break;
                }
            } while (opcionSeleccionada >= 4);
        }

        public static void solicitudInscripcion() {
            string numeroRegistro = verificarRegistro();

            if (numeroRegistro == "Error") {
                exhibirMenu();
            }
            else
            {
                string materia1 = "";
                string curso1 = "";
                string materia2 = "";
                string curso2 = "";
                string materia3 = "";
                string curso3 = "";

                List<string> solicitudTotal = new List<string>();

                materia1 = verificarMateria();
                while (materia1 == "Error")
                {
                    Console.WriteLine("La materia ingresada no es correcta\n");
                    materia1 = verificarMateria();
                }

                curso1 = verificarCurso();

                while (curso1 == "Error")
                {
                    Console.WriteLine("El curso ingresado no es correcto\n");
                    curso1 = verificarCurso();
                }

                solicitudTotal.Add($"{numeroRegistro};{materia1};{curso1}");

                Console.WriteLine("¿Desea incribirse a más materias? En caso afirmativo, ingrse 'S', sino, presione cualquier tecla para finalizar\n");

                if (Console.ReadLine().ToLower() == "s")
                {
                    materia2 = verificarMateria();

                    while (materia1 == materia2)
                    {
                        Console.WriteLine("Ya está inscripto a la materia seleccionada\n");
                        materia2 = verificarMateria();
                    }

                    while (materia2 == "Error")
                    {
                        Console.WriteLine("La materia ingresada no es correcta\n");
                        materia2 = verificarMateria();
                    }

                    curso2 = verificarCurso();

                    while (curso2 == "Error")
                    {
                        Console.WriteLine("El curso ingresado no es correcto\n");
                        curso2 = verificarCurso();
                    }

                    solicitudTotal.Add($"{numeroRegistro};{materia2};{curso2}");

                    Console.WriteLine("¿Desea incribirse a más materias? En caso afirmativo, ingrse 'S', sino, presione cualquier tecla para finalizar\n");

                    if (Console.ReadLine().ToLower() == "s")
                    {
                        materia3 = verificarMateria();
                        while (materia1 == materia3 || materia2 == materia3)
                        {
                            Console.WriteLine("Ya está inscripto a la materia seleccionada\n");
                            materia3 = verificarMateria();
                        }                       

                        while (materia3 == "Error")
                        {
                            Console.WriteLine("La materia ingresada no es correcta\n");
                            materia3 = verificarMateria();
                        }

                        curso3 = verificarCurso();

                        while (curso3 == "Error")
                        {
                            Console.WriteLine("El curso ingresado no es correcto\n");
                            curso3 = verificarCurso();
                        }

                        solicitudTotal.Add($"{numeroRegistro};{materia3};{curso3}");
                    }
                }

                string Path = "SolicitudInscripción " + numeroRegistro + ".csv";
                FileStream stream = null;
                stream = new FileStream(Path, FileMode.OpenOrCreate);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine("Numero de registro; Codigo de materia; Codigo de Curso;");
                    foreach (var item in solicitudTotal)
                    {
                        writer.WriteLine(item);
                    }
                  
                    Console.WriteLine("Su solicitud fue registrada exitosamente. Se ha generado un archivo con el detalle de la misma.\n");
                    Console.ReadKey();
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
                        Console.WriteLine($"Alumno: {arr[1]} {arr[2]} \n");
                        if (arr[3] == "Libre")
                        {
                            Console.WriteLine("El alumno no se encuentra en regularidad para cursar, por favor revise su numero de registro \n");
                        }
                        else
                        {
                            mensaje = arr[0];
                        }
                    }
                } while (!sr.EndOfStream);
                if (!alumnoEncontrado) {
                    Console.WriteLine("El alumno no figura en el maestro de alumnos");
                }
            }
            return mensaje;
        }

        public static string verificarMateria() {
            string Path = "materiasxAlumno.csv";
            string mensaje = "";
            bool materiaEncontrada = false;
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
                    materiaOk = true;
                }

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (materia == Convert.ToInt32(vector[0])) {
                        Console.WriteLine($"Materia: {vector[1]} \nCorrelativas: {vector[2]} \n");
                        materiaEncontrada = true;
                        return vector[0];
                    }
                }

                if (materiaEncontrada == false)
                {
                    Console.WriteLine("El numero de materia ingresado no se encuentra presente en el archivo. Por favor ingrese otro numero de materia \n");
                    mensaje = "Error";
                }
            }
            return mensaje;
        }

        public static string verificarCurso() {
            string Path = "cursosxAlumno.csv";
            string mensaje = "";
            bool cursoEncontrado = false;
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
                    cursoOk = true;
                }

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (curso == Convert.ToInt32(vector[0])) {
                        Console.WriteLine("Titular: " + vector[1]);
                        cursoEncontrado = true;
                        return vector[0];
                    }
                }
                if (cursoEncontrado == false)
                {
                    Console.WriteLine("El numero de curso ingresado no se encuentra presente en el archivo. Por favor ingrese otro numero de curso \n");
                    mensaje = "Error";
                }
                return mensaje;
            }
        }

        /****************************************

            Opcion 2 del menu
         
         ****************************************/

        public static string verOferta() {
            string Path = "ofertaCalificada.csv";
            FileInfo FI = new FileInfo(Path);
            string mensaje = "";

            if (!FI.Exists) {
                Console.WriteLine("No existe el archivo de oferta calificada");
                mensaje = "error";
            }
            else {
                StreamReader sr = FI.OpenText();
                Console.WriteLine("\nA continuacion se muestra la oferta calificada para el cuatrimestre entrante \n");

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');
                        Console.WriteLine($"Materia: {vector[1]} \nCurso: {vector[2]} \n\n");
                    mensaje += vector;
                }
            }
            return mensaje;
        }
    }
}