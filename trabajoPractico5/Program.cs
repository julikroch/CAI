using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace trabajoPractico5
{
    class Program
    {
        static void Main(string[] args) {
            exhibirMenu(); // Exhibicion del menu de opciones al comienzo de la aplicacion 
        }

        public static void exhibirMenu(){
            int opcionSeleccionada;
            Console.WriteLine($"Bienvenido al sistema de la Facultad de Ciencias Economicas de la UBA.\n 1. Iniciar el proceso de registración de la solicitud de inscripción a materias seleccionadas\n 2. Consultar posibles materias y curso a seleccionar\n 3. Finalizar \n\n Ingrese su opcion");

            do {
                opcionSeleccionada = Convert.ToInt32(Console.ReadLine());
                switch (opcionSeleccionada) {
                    case 1:
                        solicitudInscripcion(); // Llamado al metodo de solicitud de inscripcion a materias
                        break;
                    case 2:
                        verOferta(); // Mostrar oferta calificada
                        Console.WriteLine("Ingrese la letra V para volver al menu \n");
                        if(Console.ReadLine().ToLower() == "v") {
                            exhibirMenu();
                        }
                        break;
                    case 3:
                        Console.WriteLine("Ingrese cualquier tecla para finalizar\n"); // Cierro la aplicacion
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Seleccione una opcion correcta\n");
                        break;
                }
            } while (opcionSeleccionada >= 4);
        }

        public static void solicitudInscripcion() {

            string numeroRegistro = verificarRegistro(); // Metodo para verificar que el registro pertenezca al maestro de alumnos

            if (numeroRegistro == "Error") {
                exhibirMenu();
            }
            else {
                string materia1 = "";
                string curso1 = "";
                string materia2 = "";
                string curso2 = "";
                string materia3 = "";
                string curso3 = "";

                List<string> solicitudTotal = new List<string>(); // Array de materias y cursos para cada registro

                materia1 = verificarMateria();
                while (materia1 == "Error") {
                    Console.WriteLine("La materia ingresada no es correcta\n");
                    materia1 = verificarMateria();
                }

                curso1 = verificarCurso();
                while (curso1 == "Error") {
                    Console.WriteLine("El curso ingresado no es correcto\n");
                    curso1 = verificarCurso();
                }

                solicitudTotal.Add($"{numeroRegistro};{materia1};{curso1}"); // Se agrega al array la inscripcion de la primer materia y curso

                Console.WriteLine("¿Desea incribirse a más materias? En caso afirmativo, ingrese la tecla 'S', sino, presione cualquier tecla para finalizar\n");

                if (Console.ReadLine().ToLower() == "s") {

                    materia2 = verificarMateria();
                    while (materia1 == materia2) { // Validacion para evitar que se seleccione una materia que ya se haya seleccionado anteriormente
                        Console.WriteLine("Ya ha solicitado la inscripcion a la materia ingresada. Por favor, seleccione un nuevo numero de materia\n");
                        materia2 = verificarMateria();
                    }

                    while (materia2 == "Error") {
                        Console.WriteLine("La materia ingresada no es correcta\n");
                        materia2 = verificarMateria();
                    }

                    curso2 = verificarCurso();
                    while (curso2 == "Error") {
                        Console.WriteLine("El curso ingresado no es correcto\n");
                        curso2 = verificarCurso();
                    }

                    solicitudTotal.Add($"{numeroRegistro};{materia2};{curso2}"); // Se agrega al array la inscripcion de la segunda materia y curso

                    Console.WriteLine("¿Desea incribirse a más materias? En caso afirmativo, ingrse 'S', sino, presione cualquier tecla para finalizar\n");

                    if (Console.ReadLine().ToLower() == "s") {

                        materia3 = verificarMateria();
                        while (materia1 == materia3 || materia2 == materia3) { // Validacion para evitar que se seleccione una materia que ya se haya seleccionado anteriormente
                            Console.WriteLine("Ya ha solicitado la inscripcion a la materia ingresada. Por favor, seleccione un nuevo numero de materia\n"); 
                            materia3 = verificarMateria();
                        }                       

                        while (materia3 == "Error") {
                            Console.WriteLine("La materia ingresada no es correcta\n"); 
                            materia3 = verificarMateria();
                        }

                        curso3 = verificarCurso();
                        while (curso3 == "Error") {
                            Console.WriteLine("El curso ingresado no es correcto\n");
                            curso3 = verificarCurso();
                        }

                        solicitudTotal.Add($"{numeroRegistro};{materia3};{curso3}"); // Se agrega al array la inscripcion de la tercer materia y curso
                    }
                }

                // Generacion del archivo CSV con el numero de registro, materias y cursos seleccionados por el alumno
                string Path = "solicitudInscripción-" + numeroRegistro + ".csv";
                FileStream stream = null;
                stream = new FileStream(Path, FileMode.OpenOrCreate);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8)) {
                    writer.WriteLine("Numero de registro; Codigo de materia; Codigo de Curso;");
                    foreach (var item in solicitudTotal) {
                        writer.WriteLine(item);
                    }
                  
                    Console.WriteLine("Su solicitud fue registrada exitosamente. Se ha generado un archivo con el detalle de la misma. Presione cualquier tecla para finalizar la inscripción\n");
                    Console.ReadKey();
                }
            }
        }

        public static string verificarRegistro() {
            string Path = "alumnosRegulares.csv";
            FileInfo fileInfo = new FileInfo(Path);
            string mensaje = "Error";

            if (!fileInfo.Exists) { // Verificacion de la existencia del archivo 
                Console.WriteLine("Debe existir el archivo maestro de alumnos.");
                return mensaje;
            }
            else {
                StreamReader sr = fileInfo.OpenText();
                int registro = 0;
                int registroMin = 000000;
                int registroMax = 999999;

                bool registroValido = false;
                bool alumnoEncontrado = false;

                do {
                    Console.WriteLine("Ingrese un número de registro");

                    registroValido = int.TryParse(Console.ReadLine(), out registro);
                    if (!registroValido) {
                        Console.WriteLine("El numero ingresado no es entero. Por favor, revise su ingreso");
                    }
                    else if (registro < registroMin || registro > registroMax) {
                        Console.WriteLine($"Número de registro incorrecto. Por favor ingrese un numero de registro entre {registroMin} y {registroMax}");
                        registroValido = false;
                    }
                } while (!registroValido);

                do {
                    string p = sr.ReadLine();
                    string[] arr = p.Split(';');

                    if (registro == Convert.ToInt32(arr[0])) { // Recorro el array para encontrar al alumno ingresado y valido que su condicion no sea 'Libre'
                        alumnoEncontrado = true;
                        Console.WriteLine($"Alumno: {arr[1]} {arr[2]} \n");
                        if (arr[3] == "Libre") {
                            Console.WriteLine("El registro ingresado no se encuentra en regularidad para cursar, por favor revise su numero de registro \n");
                        }
                        else {
                            mensaje = arr[0];
                        }
                    }
                } while (!sr.EndOfStream);
                if (!alumnoEncontrado) {
                    Console.WriteLine("El registro ingresado no figura en el maestro de alumnos");
                }
            }
            return mensaje;
        }

        public static string verificarMateria() { 
            string Path = "materiasxAlumno.csv";
            string mensaje = "";
            bool materiaEncontrada = false;
            FileInfo FI = new FileInfo(Path);

            if (!FI.Exists) { // Verificacion de la existencia del archivo 
                Console.WriteLine("No existe el archivo de materias por alumno");
                return "error";
            }
            else {
                StreamReader sr = FI.OpenText();
                int materia = 0;
                bool codigoMateria = false;
              
                while (!codigoMateria) {
                    Console.WriteLine("Ingrese código de materia");
                    codigoMateria = int.TryParse(Console.ReadLine(), out materia);
                    if (codigoMateria == false) {
                        Console.WriteLine("El numero de materia no es valido. Por favor, revise su ingreso");
                    }
                }

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (materia == Convert.ToInt32(vector[0])) { // Recorro el array de materias para verificar que exista la materia ingresada
                        Console.WriteLine($"Materia: {vector[1]} \nCorrelativas: {vector[2]} \n");
                        materiaEncontrada = true;
                        return vector[0];
                    }
                }

                if (materiaEncontrada == false) {
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
                Console.WriteLine("No existe el archivo de cursos asociados a las materias");
                return "error";
            }
            else {
                StreamReader sr = FI.OpenText();
                int curso = 0;
                bool codigoCurso = false;
                while (!codigoCurso) {
                    Console.WriteLine("Ingrese código de curso");
                    codigoCurso = int.TryParse(Console.ReadLine(), out curso);
                    if (codigoCurso == false) {
                        Console.WriteLine("El codigo ingresado no corresponde a un numero entero");
                    }
                }

                while (!sr.EndOfStream) {
                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    if (curso == Convert.ToInt32(vector[0])) { // Recorro el array de los cursos para verificar que se haya encontrado el curso ingresado
                        Console.WriteLine("Titular: " + vector[1]);
                        cursoEncontrado = true;
                        return vector[0];
                    }
                }
                if (cursoEncontrado == false) {
                    Console.WriteLine("El numero de curso ingresado no se encuentra presente en el archivo. Por favor ingrese otro numero de curso \n");
                    mensaje = "Error";
                }
                return mensaje;
            }
        }

        public static string verOferta() { // Metodo para mostrar la oferta calificada del cuatrimestre entrante
            string Path = "ofertaCalificada.csv";
            FileInfo FI = new FileInfo(Path);
            string mensaje = "";

            if (!FI.Exists) {
                Console.WriteLine("No existe el archivo de oferta calificada");
                mensaje = "error";
            }
            else {
                StreamReader sr = FI.OpenText();
                Console.WriteLine("\nA continuacion se muestra la oferta calificada para el cuatrimestre entrante: \n");

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