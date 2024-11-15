document.addEventListener('DOMContentLoaded', function () {

    fnObtenerEmpleados();
    fnObtenerCargos();
});

async function fnObtenerEmpleados() {
    // Hacer la petición AJAX al cargar la página
    $.ajax({
        url: '/Home/GetEmpleados', // URL del método en el controlador
        type: 'GET',
        dataType: 'json',
        success: function (empleados) {
            let respuestaHtml = '';

            // Construir las filas de la tabla con los datos recibidos
            empleados.forEach(function (empleado) {
                console.log(empleado)
                respuestaHtml += `
                            <tr>
                                <td>${empleado.nombreCompleto}</td>
                                <td>${empleado.correo}</td>
                                <td>${empleado.telefono}</td>
                                <td>${empleado.cargoDescripcion}</td>
                                <td>
                                    <a class="btn btn-warning btn-sm" onclick="fnEditarEmpleado(${empleado.idEmpleado})">Editar</a>
                                    <a class="btn btn-danger btn-sm" onclick="fnEliminarEmpleado(${empleado.idEmpleado}, '${empleado.nombreCompleto}')">Eliminar</a>
                                </td>
                            </tr>`;
            });

            // Limpiar el data table
            $('#tblEmpleados tbody').html('');
            // Insertar las filas en el tbody de la tabla           
            $('#tblEmpleados tbody').html(respuestaHtml);
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los empleados:', error);
            alert('Hubo un problema al cargar la lista de empleados.');
        }
    });
}

async function fnObtenerCargos() {
    // Hacer la petición AJAX para obtener los cargos
    $.ajax({
        url: '/Home/GetCargos', // URL del método en el controlador
        type: 'GET',
        dataType: 'json',
        success: await function (cargos) {
            let opcionesHtml = '';

            // Construir las opciones del select con los datos recibidos
            cargos.forEach(function (cargo) {
                opcionesHtml += `<option value="${cargo.idCargo}">${cargo.descripcion}</option>`;
            });

            // Insertar las opciones en el select con id CargoSelect
            $('#cbCargos').append(opcionesHtml);
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los cargos:', error);
            alert('Hubo un problema al cargar la lista de cargos.');
        }
    });
}

function fnCrearEmpleado() {
    const tituloModal = document.getElementById('tituloModal');
    const btnGuardarEmpleado = document.getElementById('btnGuardarEmpleado');
    const btnEditarEmpleado = document.getElementById('btnEditarEmpleado');
    btnGuardarEmpleado.classList.remove('d-none');
    btnEditarEmpleado.classList.add('d-none');
    tituloModal.textContent = 'Nuevo Empleado';

    // Limpiar campos
    fnlimpiarCampos()

    // Abrir el modal
    $('#mdlEmpleado').modal('show');
}

function fnlimpiarCampos() {
    $('#idEmpleado').val('');
    $('#txtNombreCompleto').val('');
    $('#txtCorreo').val('');
    $('#txtTelefono').val('');
    $('#cbCargos').val('0');
}

function fnGuardarEmpleado(idEmpleado, nombreEmpleado) {
    // Obtener los datos del empleado desde el formulario
    const empleado = {
        NombreCompleto: $('#txtNombreCompleto').val(),
        Correo: $('#txtCorreo').val(),
        Telefono: $('#txtTelefono').val(),
        IdCargo: $('#cbCargos').val()
    };

    $.ajax({
        url: '/Home/GuardarEmpleado',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(empleado),
        success: function (response) {
            if (response.success) {
                let timerInterval;
                Swal.fire({
                    icon: "success",
                    title: "Empleado Guardado",
                    html: `<span style="font-size: 25px;">El Empleado ${nombreEmpleado} Se Guardo Correctamente</span>`,
                }).then((result) => { 
                    // Confirmar y Cerrar OK
                    if (result.isConfirmed) {
                        // Cerrar modal
                        $('#mdlEmpleado').modal('hide')
                        // Función traer y refrescar la lista de empleados en data table
                        fnObtenerEmpleados();
                    }
                });
                
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error al guardar el empleado:', error);
            alert('Hubo un problema al guardar el empleado.');
        }
    });
}

function fnEditarEmpleado(idEmpleado) {
    console.log(idEmpleado)
    // alert(idEmpleado)

    $.ajax({
        url: `/Home/GetEmpleadoById?idEmpleado=${idEmpleado}`,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                //console.log(response.empleado)

                const tituloModal = document.getElementById('tituloModal');
                const btnGuardarEmpleado = document.getElementById('btnGuardarEmpleado');
                const btnEditarEmpleado = document.getElementById('btnEditarEmpleado');
                btnGuardarEmpleado.classList.add('d-none');
                btnEditarEmpleado.classList.remove('d-none');
                tituloModal.textContent = 'Editar Empleado';

                // Llenar el formulario de edición con los datos del empleado
                $('#idEmpleado').val(response.empleado.idEmpleado);
                $('#txtNombreCompleto').val(response.empleado.nombreCompleto);
                $('#txtCorreo').val(response.empleado.correo);
                $('#txtTelefono').val(response.empleado.telefono);

                // Seleccionar el cargo del empleado en el dropdown
                $('#cbCargos').val(response.empleado.idCargo);

                // Abrir el modal de edición
                $('#mdlEmpleado').modal('show');
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los datos del empleado:', error);
            alert('Hubo un problema al obtener los datos del empleado.');
        }
    });
}

function fnActualizarEmpleado(idEmpleado, nombreEmpleado) {
    const empleadoData = {
        idEmpleado: $('#idEmpleado').val(),
        nombreCompleto: $('#txtNombreCompleto').val(),
        correo: $('#txtCorreo').val(),
        telefono: $('#txtTelefono').val(),
        idCargo: $('#cbCargos').val()
    };

    $.ajax({
        url: '/Home/ActualizarEmpleado',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(empleadoData),
        dataType: 'json',
        success: function (response) {
            if (response.success) {                             
                Swal.fire({
                    icon: "success",
                    title: "Empleado Actualizado",           
                    html: `<span style="font-size: 25px;">El Empleado ${nombreEmpleado} Se Actualizo Correctamente</span>`,
                }).then((result) => {                    
                    // Confirmar y Cerrar OK
                    if (result.isConfirmed) {
                        // Cerrar modal
                        $('#mdlEmpleado').modal('hide')
                        // Función traer y refrescar la lista de empleados en data table
                        fnObtenerEmpleados();     
                    }
                });
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar los datos del empleado:', error);
            alert('Hubo un problema al actualizar los datos del empleado.');
        }
    });
}

function fnEliminarEmpleado(idEmpleado, nombreEmpleado) {
    Swal.fire({
        title: "¿Deseas eliminar el empleado?",
        text: `${nombreEmpleado}`,
        icon: "question",
        showCancelButton: true,
        confirmButtonColor: "#C70039",
        cancelButtonColor: "#50",
        confirmButtonText: "Si, Eliminar",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Home/EliminarEmpleado', // Asegúrate de que esta URL coincida con tu ruta en el controlador
                type: 'POST',
                data: { idEmpleado: idEmpleado },
                success: function (response) {
                    if (response.success) {
                        let timerInterval;
                        Swal.fire({
                            title: "Empleado Eliminado",
                            html: `<span style="font-size: 25px;">El Empleado ${nombreEmpleado} Se Elimino Correctamente</span>`,                            
                        }).then((result) => {
                            /* Read more about handling dismissals below */
                            if (result.isConfirmed) {
                                // Cerrar modal
                                $('#mdlEmpleado').modal('hide')
                                // Función traer y refrescar la lista de empleados en data table
                                fnObtenerEmpleados();
                            }
                        });
                        
                    } else {
                        alert(response.message);
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error al eliminar el empleado:', error);
                    alert('Hubo un problema al eliminar el empleado.');
                }
            })
        }
    });
}