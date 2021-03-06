﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace trabajoPractico5
{
    class Program
    {
        static void Main(string[] args)
        {
            exhibirMenu(); // Exhibicion del menu de opciones al comienzo de la aplicacion
        }

        public static void exhibirMenu()
        {
            int opcionSeleccionada;
            Console.WriteLine($"Bienvenido al sistema de la Facultad de Ciencias Economicas de la UBA.\n 1. Iniciar el proceso de registración de la solicitud de inscripción a materias seleccionadas\n 2. Consultar posibles materias y curso a seleccionar\n 3. Finalizar \n\n Ingrese su opcion");

            do
            {
                opcionSeleccionada = Convert.ToInt32(Console.ReadLine());
                switch (opcionSeleccionada)
                {
                    case 1:
                        solicitudInscripcion(); // Llamado al metodo de solicitud de inscripcion a materias
                        break;
                    case 2:
                        verOferta(); // Mostrar oferta calificada
                        Console.WriteLine("Ingrese la letra V para volver al menu \n");
                        if (Console.ReadLine().ToLower() == "v")
                        {
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

        public static void solicitudInscripcion()
        {

            string numeroRegistro = verificarRegistro(out string[] materiasAprobadas); // Metodo para verificar que el registro pertenezca al maestro de alumnos
            const string cuatrimestre = "2do 2020";

            if (numeroRegistro == "Error")
            {
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

                List<string> solicitudTotal = new List<string>(); // Array de materias y cursos para cada registro

                materia1 = verificarMateria(materiasAprobadas);
                while (materia1 == "Error")
                {
                    materia1 = verificarMateria(materiasAprobadas);
                }

                curso1 = verificarCurso(materia1);
                while (curso1 == "Error")
                {
                    curso1 = verificarCurso(materia1);
                }

                solicitudTotal.Add($"{cuatrimestre};{numeroRegistro};{materia1};{curso1}"); // Se agrega al array la inscripcion de la primer materia y curso

                Console.WriteLine("¿Desea incribirse a más materias? En caso afirmativo, ingrese la tecla 'S', sino, presione cualquier tecla para finalizar\n");

                if (Console.ReadLine().ToLower() == "s")
                {

                    materia2 = verificarMateria(materiasAprobadas);
                    while (materia1 == materia2)
                    { // Validacion para evitar que se seleccione una materia que ya se haya seleccionado anteriormente
                        Console.WriteLine("Ya ha solicitado la inscripcion a la materia ingresada. Por favor, seleccione un nuevo numero de materia\n");
                        materia2 = verificarMateria(materiasAprobadas);
                    }

                    while (materia2 == "Error")
                    {
                        materia2 = verificarMateria(materiasAprobadas);
                    }

                    curso2 = verificarCurso(materia2);
                    while (curso2 == "Error")
                    {
                        curso2 = verificarCurso(materia2);
                    }

                    solicitudTotal.Add($"{cuatrimestre};{numeroRegistro};{materia2};{curso2}"); // Se agrega al array la inscripcion de la segunda materia y curso

                    Console.WriteLine("¿Desea incribirse a más materias? En caso afirmativo, ingrse 'S', sino, presione cualquier tecla para finalizar\n");

                    if (Console.ReadLine().ToLower() == "s")
                    {
                        materia3 = verificarMateria(materiasAprobadas);
                        while (materia1 == materia3 || materia2 == materia3)
                        { // Validacion para evitar que se seleccione una materia que ya se haya seleccionado anteriormente
                            Console.WriteLine("Ya ha solicitado la inscripcion a la materia ingresada. Por favor, seleccione un nuevo numero de materia\n");
                            materia3 = verificarMateria(materiasAprobadas);
                        }

                        while (materia3 == "Error")
                        {
                            materia3 = verificarMateria(materiasAprobadas);
                        }

                        curso3 = verificarCurso(materia3);
                        while (curso3 == "Error")
                        {
                            curso3 = verificarCurso(materia3);
                        }
                        solicitudTotal.Add($"{cuatrimestre};{numeroRegistro};{materia3};{curso3}"); // Se agrega al array la inscripcion de la tercer materia y curso
                    }
                }

                // Generacion del archivo CSV con el numero de registro, materias y cursos seleccionados por el alumno
                string Path = "solicitudInscripción-" + numeroRegistro + ".csv";
                FileStream stream = null;
                stream = new FileStream(Path, FileMode.OpenOrCreate);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine("Cuatrimestre; Numero de registro; Codigo de materia; Codigo de Curso;");
                    foreach (var item in solicitudTotal)
                    {
                        writer.WriteLine(item);
                    }

                    Console.WriteLine("Su solicitud fue registrada exitosamente. Se ha generado un archivo con el detalle de la misma. Presione cualquier tecla para finalizar la inscripción\n");
                    Console.ReadKey();
                }

                //Archivo de inscripciones generales
                string pathInscripcion = "inscripcionesCuatrimestre.csv";
                FileStream streamInscripcion = null;
                streamInscripcion = new FileStream(pathInscripcion, FileMode.Append);

                using (StreamWriter writer = new StreamWriter(streamInscripcion, Encoding.UTF8))
                {
                    foreach (var item in solicitudTotal)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
        }

        public static string verificarRegistro(out string[] materiasAprobadas)
        {
            string Path = "alumnos.csv";
            FileInfo fileInfo = new FileInfo(Path);
            StreamReader sr = fileInfo.OpenText();

            string mensaje = "Error";

            string pathInscripcion = "inscripcionesCuatrimestre.csv";
            FileInfo fileInfoInscripcion = new FileInfo(pathInscripcion);
            using (StreamReader srInscripcion = fileInfoInscripcion.OpenText())
            {
                int registro = 0;
                int registroMin = 000000;
                int registroMax = 999999;

                bool registroValido = false;
                bool alumnoEncontrado = false;

                bool alumnoInscripto = false;
                int registroAlumno = 0;
                int registroInscripcion = 0;

                materiasAprobadas = null;

                do
                {
                    Console.WriteLine("Ingrese un número de registro");
                    registroValido = int.TryParse(Console.ReadLine(), out registro);

                    if (!registroValido)
                    {
                        Console.WriteLine("El numero ingresado no es entero. Por favor, revise su ingreso");
                    }
                    else if (registro < registroMin || registro > registroMax)
                    {
                        Console.WriteLine($"Número de registro incorrecto. Por favor ingrese un numero de registro entre {registroMin} y {registroMax}");
                        registroValido = false;
                    }
                } while (!registroValido);

                do
                {
                    string p = sr.ReadLine();
                    string[] arr = p.Split(';');

                    registroAlumno = int.Parse(arr[0]);

                    if (registro == registroAlumno)
                    { // Verifico si el registro ingresado corresponde con el de un alumno en la lista de alumnos
                        alumnoEncontrado = true;
                        do
                        {
                            string pInscripcion = srInscripcion.ReadLine();
                            string[] arrInscripcion = pInscripcion.Split(';');
                            registroInscripcion = int.Parse(arrInscripcion[1]);

                            if (registro == registroInscripcion)
                            { // Verifico que el alumno no haya realizado una inscripicion previa
                                Console.WriteLine("El registro ingresado ya ha realizado una inscripcion previamente \n");
                                materiasAprobadas = null;
                                return mensaje;
                            }
                        } while (!srInscripcion.EndOfStream);

                        Console.WriteLine($"Alumno: {arr[1]} {arr[2]} \n");

                        if (arr[3] == "Libre")
                        {
                            Console.WriteLine("El registro ingresado no se encuentra en regularidad para cursar, por favor revise su numero de registro \n");
                        }
                        else
                        {
                            materiasAprobadas = arr[4].Split(',');
                            mensaje = arr[0];
                        }
                    }
                } while (!sr.EndOfStream);
                if (!alumnoEncontrado)
                {
                    Console.WriteLine("No hay un alumno registrado con ese número de registro");
                }
                return mensaje;
            }
        }

        public static string verificarMateria(string[] materiasAprobadas)
        {
            string Path = "materias.csv";
            string mensaje = "";
            bool materiaEncontrada = false;
            FileInfo FI = new FileInfo(Path);

            StreamReader sr = FI.OpenText();
            int materia = 0;
            bool codigoMateria = false;

            string[] materiasCorrelativas;

            while (!codigoMateria)
            {
                Console.WriteLine("Ingrese código de materia");
                codigoMateria = int.TryParse(Console.ReadLine(), out materia);
                if (!codigoMateria)
                {
                    Console.WriteLine("El numero de materia ingresado no es valido. Por favor, revise su ingreso");
                }
            }

            while (!sr.EndOfStream)
            {
                string p = sr.ReadLine();
                string[] vector = p.Split(';');

                if (materia == Convert.ToInt32(vector[0]))
                { // Recorro el array de materias para verificar que exista la materia ingresada
                    Console.WriteLine($"Materia: {vector[1]} \nCorrelativas: {vector[2]} \n");
                    materiasCorrelativas = vector[3].Split(',');

                    foreach (var item in materiasCorrelativas)
                    {
                        if (Array.IndexOf(materiasAprobadas, item) == -1 && item != "")
                        {
                            Console.WriteLine("No tiene las correlativas aprobadas para cursar esta materia");
                            return "Error";
                        }
                    }

                    materiaEncontrada = true;
                    return vector[0];
                }
            }

            if (!materiaEncontrada)
            {
                Console.WriteLine("No se ha encontrado materia registrada con dicho codigo. Por favor ingrese otro numero de materia \n");
                mensaje = "Error";
            }
            return mensaje;
        }

        public static string verificarCurso(string materia)
        {
            string Path = "cursosXMateria.csv";
            string mensaje = "";
            bool cursoEncontrado = false;

            FileInfo FI = new FileInfo(Path);

            StreamReader sr = FI.OpenText();
            int curso = 0;
            bool codigoCurso = false;
            while (!codigoCurso)
            {
                Console.WriteLine("Ingrese código de curso");
                codigoCurso = int.TryParse(Console.ReadLine(), out curso);
                if (codigoCurso == false)
                {
                    Console.WriteLine("El codigo de curso ingresado no corresponde a un numero entero");
                }
            }

            while (!sr.EndOfStream)
            {
                string p = sr.ReadLine();
                string[] vector = p.Split(';');

                if (materia == vector[1])
                {
                    if (curso == Convert.ToInt32(vector[0]))
                    { // Recorro el array de los cursos para verificar que se haya encontrado el curso ingresado
                        Console.WriteLine("Titular: " + vector[3]);
                        cursoEncontrado = true;
                        return vector[0];
                    }
                }
            }

            if (!cursoEncontrado)
            {
                Console.WriteLine("El numero de curso ingresado no está disponible para la materia a la que se desea inscribir. Por favor ingrese otro numero de curso \n");
                mensaje = "Error";
            }
            return mensaje;
        }

        public static string verOferta()
        { // Metodo para mostrar la oferta calificada del cuatrimestre entrante
            string Path = "cursosXMateria.csv";
            FileInfo FI = new FileInfo(Path);
            string mensaje = "";

            StreamReader sr = FI.OpenText();
            Console.WriteLine("\nA continuacion se muestra la oferta calificada para el cuatrimestre entrante: \n");

            while (!sr.EndOfStream)
            {
                string p = sr.ReadLine();
                string[] vector = p.Split(';');
                Console.WriteLine($"Materia: {vector[1]} - {vector[2]} \nCurso: {vector[0]} - {vector[3]}\n\n");
                mensaje += vector;
            }
            return mensaje;
        }
    }
}