# Construcción de Aplicaciones Informáticas

Trabajo practico 5. Caso de uso “Solicitud de inscripción” en el Subsistema de Inscripción de alumnos a cursos regulares (en el
marco de un Sistema de Gestión Pedagógica de una unidad universitaria – Facultad).

## Documentación

Al seleccionar la opción 1 del menú principal (Iniciar el proceso de registración de la solicitud de inscripción a materias seleccionadas), se verifica que exista el archivo maestro de alumnos.

En caso de que no exista, se notifica la falta del mismo. En caso de que exista, se le solicita al alumno que ingrese su número de registro.

Una vez ingresado el registro, el sistema verifica que sea un número entero y que el mismo se encuentre dentro del rango de registros válidos. Si el registro ingresado no supera alguna de estas validaciones, se notifica por consola y se le pide al alumno que ingrese un registro válido.

Luego de ingresar un número de registro válido, se busca en el archivo maestro de alumnos al alumno asociado al número de registro ingresado. Si no se lo encuentra, se notifica por consola que el alumno no se encuentra dentro del archivo maestro.

En caso de encontrarlo, presenta por consola tanto el número de registro ingresado como el nombre y apellido del alumno.

Luego, se verifica que el alumno no se encuentre en estado “Libre”, es decir, que esté habilitado para la inscripción a materias. En caso de estar apto para la inscripción a materias, se verifica que exista el archivo maestro de materias.

En caso de que exista el archivo maestro de materias, se le pide al alumno que ingrese el código de la materia a la que desea inscribirse.

Una vez que el alumno lo indica, se valida que el código de materia ingresado sea un número entero y que se encuentre dentro del archivo maestro de materias. En caso de no superar alguna de estas validaciones, se le notifica al alumno por consola.

En caso de superar ambas validaciones, se muestra por consola el nombre de la materia y sus correlativas.

Una vez mostrado, se verifica que exista el archivo maestro de cursos. En caso de que no exista, se le notifica por consola al alumno.

En caso de que exista el archivo maestro de cursos, se le pide al alumno que ingrese el código del curso al que desea inscribirse.

Una vez que el alumno lo indica, se valida que el código de curso ingresado sea un número entero y que se encuentre dentro del archivo maestro de cursos. En caso de no superar alguna de estas validaciones, se le notifica al alumno por consola.

Una vez validado el curso, se le pregunta al alumno si desea inscribirse a otra materia.

En caso afirmativo, se le pide que ingrese el código de la materia a la que desea inscribirse y, a las ya mencionadas validaciones, se le suma la de que no se haya inscripto previamente a la misma.

En caso de superar todas las validaciones, se le solicita que ingrese el código del curso al que desea inscribirse, y vuelven a aplicarse las validaciones mencionadas sobre el código de curso.

Una vez superadas estas validaciones, se registra la nueva solicitud y se le vuelve a consultar al alumno si desea inscribirse a una nueva materia, siendo esta la última vez, dado que es la tercera.

En caso afirmativo, se repite el proceso de inscripción ya mencionado, validando tanto el código de materia como el de curso.

Una vez finalizada la inscripción a todas las materias, se muestra un mensaje que confirma la registración de la solicitud.

Si, en vez de seleccionarse la opción 1 del menú principal se selecciona la opción 2 (Consultar posibles materias y cursos a seleccionar), se muestra la oferta disponible de materias, con sus respectivos códigos de curso y profesores.
