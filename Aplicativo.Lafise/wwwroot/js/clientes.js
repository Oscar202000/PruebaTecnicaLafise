
const apiFrontend = "/Clientes";
let clientesData = [];
let selectedId = null;

function showAlert(msg, type = "success") {
  const $a = $("#alertMessage");
  $a
    .removeClass("alert-success alert-danger alert-warning alert-info d-none")
    .addClass("alert-" + type)
    .text(msg)
    .addClass("show");
  setTimeout(() => $a.removeClass("show").addClass("d-none"), 3000);
}

function cargarClientes() {
  $.get(`${apiFrontend}/ObtenerClientes`)
    .done(data => {
      clientesData = data;
      renderRows(data);
    })
    .fail(() => showAlert("Error al cargar los clientes", "danger"));
}

function renderRows(list) {
  const rows = list.map(c => `
    <tr id="fila-${c.idClientes}">
      <td>${c.nombre}</td>
      <td>${c.identificacion}</td>
    </tr>
  `).join("");
  $("#tablaClientesBody").html(rows);
}

function applyFilter() {
  const nom   = $("#filtroNombre").val().toLowerCase();
  const ident = $("#filtroIdent").val().toLowerCase();
  renderRows(clientesData.filter(c =>
    c.nombre.toLowerCase().includes(nom) &&
    c.identificacion.toLowerCase().includes(ident)
  ));
}

function abrirEditar(id) {
  $.get(`${apiFrontend}/ObtenerClientesPorId?id=${id}`)
    .done(c => {
      $("#editarId").val(c.idClientes);
      $("#editarNombre").val(c.nombre);
      $("#editarIdentificacion").val(c.identificacion);
      $("#editarModal").modal("show");
    })
    .fail(() => showAlert("Error al obtener el cliente", "danger"));
}

// Limpia inputs de un modal
function limpiarForm(modalSel) {
  $(modalSel).find("input").val("");
}

// Eliminar cliente
function eliminar(id) {
    Swal.fire({
        title: '¿Seguro que deseas eliminar este cliente?',
        text: "¡Se eliminaran todas sus cuentas Activas!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        reverseButtons: true,
        customClass: {
            confirmButton: 'btn btn-danger',
            cancelButton: 'btn btn-secondary'
        },
        buttonsStyling: false
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${apiFrontend}/EliminarClientes?id=${id}`,
                type: "DELETE"
            })
                .done(() => {
                    $(`#fila-${id}`).remove();
                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: 'Cliente eliminado',
                        showConfirmButton: false,
                        timer: 2000
                    });
                })
                .fail(() => {
                    Swal.fire('Error', 'No se pudo eliminar el cliente', 'error');
                });
        }
    });
}

$(document).ready(function() {
  cargarClientes();
  $("#filtroNombre, #filtroIdent").on("input", applyFilter);

  $("#tablaClientesBody").on("click", "tr", function() {
    selectedId = parseInt(this.id.replace("fila-", ""));
    $("#tablaClientesBody tr").removeClass("table-active");
    $(this).addClass("table-active");
  });

  $("#btnAgregar").click(() => $("#crearModal").modal("show"));
  $("#btnModificar").click(() => {
    if (!selectedId) return showAlert("Selecciona un cliente primero", "warning");
    abrirEditar(selectedId);
  });
  $("#btnEliminar").click(() => {
    if (!selectedId) return showAlert("Selecciona un cliente primero", "warning");
    eliminar(selectedId);
  });

  $("#guardarNuevo").click(() => {
    const dto = {
      nombre:          $("#nuevoNombre").val(),
      identificacion:  $("#nuevaIdentificacion").val(),
      saldoDisponible: $("#nuevoSaldo").val()
    };
    $.ajax({
      url:         `${apiFrontend}/CrearClientesCuentas`,
      type:        "POST",
      contentType: "application/json",
      data:        JSON.stringify(dto)
    })
    .done(() => {
      $("#crearModal").modal("hide");
      limpiarForm("#crearModal");
      showAlert("Cliente creado exitosamente", "success");
      cargarClientes();
    })
    .fail(() => showAlert("Error al crear el cliente", "danger"));
  });

  $("#guardarEdicion").click(() => {
    const dto = {
      idClientes:     parseInt($("#editarId").val()),
      nombre:         $("#editarNombre").val(),
      identificacion: $("#editarIdentificacion").val()
    };
    $.ajax({
      url:         `${apiFrontend}/ActualizarClientes`,
      type:        "PUT",
      contentType: "application/json",
      data:        JSON.stringify(dto)
    })
    .done(() => {
      $("#editarModal").modal("hide");
      limpiarForm("#editarModal");
      showAlert("Cliente actualizado exitosamente", "success");
      cargarClientes();
    })
    .fail(() => showAlert("Error al actualizar el cliente", "danger"));
  });

  
  $('#crearModal').find('.close, .btn-secondary[data-dismiss="modal"]').on('click', function() {
    $('#crearModal').modal('hide');
    $('.modal-backdrop').remove();
  });
  $('#editarModal').find('.close, .btn-secondary[data-dismiss="modal"]').on('click', function() {
    $('#editarModal').modal('hide');
    $('.modal-backdrop').remove();
  });

});
