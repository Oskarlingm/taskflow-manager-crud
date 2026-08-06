# Historias de usuario - TaskFlow Manager

Estas historias deben registrarse individualmente en Jira o Azure DevOps.

## HU-01 - Iniciar sesion

**Como** administrador, **quiero** iniciar sesion con mis credenciales **para** acceder de forma controlada al gestor de tareas.

### Criterios de aceptacion
- Con credenciales validas se muestra el panel de tareas.
- Las credenciales incorrectas producen un mensaje comprensible.
- Correo y contrasena son obligatorios.

### Criterios de rechazo
- Se rechazan credenciales incorrectas o campos vacios.
- El panel no se muestra si la autenticacion falla.

## HU-02 - Crear una tarea

**Como** administrador, **quiero** registrar una tarea **para** mantener organizado el trabajo pendiente.

### Criterios de aceptacion
- La tarea valida aparece en el listado al guardarse.
- El titulo acepta de 3 a 100 caracteres y la descripcion hasta 500.

### Criterios de rechazo
- Se rechaza el titulo vacio, menor de 3 o mayor de 100 caracteres.
- No se crea una tarea cuando falla la validacion.

## HU-03 - Consultar las tareas

**Como** administrador, **quiero** consultar las tareas registradas **para** conocer su titulo, descripcion y estado.

### Criterios de aceptacion
- El panel muestra las tareas guardadas y su estado.
- El contador coincide con los registros presentados.

### Criterios de rechazo
- Una tarea inexistente no aparece en el listado.
- No se muestran registros falsos si la base esta vacia.

## HU-04 - Actualizar una tarea

**Como** administrador, **quiero** modificar una tarea **para** corregir sus datos o cambiar su estado.

### Criterios de aceptacion
- El formulario carga los datos actuales.
- Se pueden actualizar titulo, descripcion y estado.
- El listado refleja los nuevos valores.

### Criterios de rechazo
- Se rechaza un titulo menor de 3 o mayor de 100 caracteres.
- Los datos anteriores permanecen si la validacion falla.

## HU-05 - Eliminar una tarea

**Como** administrador, **quiero** eliminar una tarea **para** retirar registros que ya no necesito.

### Criterios de aceptacion
- Se solicita confirmacion antes de eliminar.
- Al confirmar la tarea desaparece; al cancelar permanece.

### Criterios de rechazo
- No se elimina sin confirmacion expresa.
- Cancelar no altera el registro.

## Matriz de cobertura

| Historia | Camino feliz | Prueba negativa | Prueba de limites |
|---|---|---|---|
| HU-01 Login | Credenciales correctas | Credenciales incorrectas | Campos vacios |
| HU-02 Crear | Tarea valida | Titulo menor de 3 | Titulo de 3 caracteres |
| HU-03 Consultar | Tarea visible | Tarea inexistente | Titulo de 100 caracteres |
| HU-04 Actualizar | Modificacion valida | Nuevo titulo invalido | Nuevo titulo de 3 caracteres |
| HU-05 Eliminar | Confirmar eliminacion | Cancelar eliminacion | Eliminar ultima tarea creada |
