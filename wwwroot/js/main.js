$(document).ready(function () {
    
    obtenerEmpleados();
    obtenerCargos();
});

async function obtenerCargos() {
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

async function obtenerEmpleados() {
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
                                    <a class="btn btn-danger btn-sm">Eliminar</a>
                                </td>
                            </tr>`;
            });

            // Insertar las filas en el tbody de la tabla
            $('#tblEmpleados tbody').html(respuestaHtml);
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los empleados:', error);
            alert('Hubo un problema al cargar la lista de empleados.');
        }
    });
}

function fnEditarEmpleado(idEmpleado) {
    console.log(idEmpleado)
    //alert(idEmpleado)

    $.ajax({
        url: `/Home/GetEmpleadoById?idEmpleado=${idEmpleado}`,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                console.log(response.empleado)

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

function fnCrearEmpleado() {
    const tituloModal = document.getElementById('tituloModal');
    const btnGuardarEmpleado = document.getElementById('btnGuardarEmpleado');
    const btnEditarEmpleado = document.getElementById('btnEditarEmpleado');
    btnGuardarEmpleado.classList.remove('d-none');
    btnEditarEmpleado.classList.add('d-none');
    tituloModal.textContent = 'Nuevo Empleado';

    // Abrir el modal
    $('#mdlEmpleado').modal('show');

}

