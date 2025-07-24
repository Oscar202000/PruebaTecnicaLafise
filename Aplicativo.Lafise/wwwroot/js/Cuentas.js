const apiCuentas = "/Cuentas";
const apiClientes = "/Clientes";
let cuentasData = [];
let selectedCtaId = null;

// muestra toast (bootstrap)
function showAlert(msg, type = "success") {
    const $a = $("#alertMessage");
    $a
        .removeClass("alert-success alert-danger alert-warning alert-info d-none")
        .addClass("alert-" + type)
        .text(msg)
        .addClass("show");
    setTimeout(() => $a.removeClass("show").addClass("d-none"), 3000);
}

// carga lista de cuentas
function cargarCuentas() {
    $.get(`${apiCuentas}/ObtenerCuentas`)
        .done(data => {
            cuentasData = data;
            renderRows(data);
        })
        .fail(() => showAlert("Error al cargar cuentas", "danger"));
}

function renderRows(list) {
    const rows = list.map(c => {
        // ojo: 'fechaApertura' con minúscula inicial
        const fa = new Date(c.fechaApertura);
        const faFmt = fa.toLocaleDateString();

        return `
      <tr id="fila-${c.idCuentas}">
        <td>${c.idCuentas}</td>
        <td>${c.nombre}</td>
        <td>${c.identificacion}</td>
        <td>${faFmt}</td>        <!-- aquí -->
        <td>${c.saldoDisponible ?? ""}</td>
        <td>${c.estado}</td>
      </tr>`;
    }).join("");
    $("#tablaCuentasBody").html(rows);
}
// filtro en vivo
function applyFilter() {
    const nom = $("#filtroNombre").val().toLowerCase();
    const ident = $("#filtroIdent").val().toLowerCase();
    const filt = cuentasData.filter(c =>
        c.nombre.toLowerCase().includes(nom) &&
        c.identificacion.toLowerCase().includes(ident)
    );
    renderRows(filt);
}

// carga dropdown de clientes en modal "Crear"
function cargarClientesDropdown() {
    $.get(`${apiClientes}/ObtenerClientes`)
        .done(data => {
            const opts = data.map(c =>
                `<option value="${c.idClientes}">${c.identificacion} - ${c.nombre}</option>`
            ).join("");
            $("#selectCliente").html(opts);
        })
        .fail(() => showAlert("Error al cargar clientes", "danger"));
}

// abre modal de edición SIN ir al servidor
function abrirEditarCuenta(id) {
    const c = cuentasData.find(x => x.idCuentas === id);
    if (!c) return showAlert("Cuenta no encontrada", "warning");

    $("#editarCuentaId").val(c.idCuentas);
    $("#editarNombreCuenta").val(c.nombre);
    $("#editarIdentificacionCuenta").val(c.identificacion);
    $("#editarSaldo").val(c.saldoDisponible);
    $("#editarEstado").val(c.estado);

    $("#editarModalCuenta").modal("show");
}

// elimina por ajax
function eliminarCuenta(id) {
    Swal.fire({
        title: '¿Seguro que deseas eliminar esta Cuenta?',
        text: "¡Esto no es Reversible!",
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
                url: `${apiCuentas}/EliminarCuenta?id=${id}`,
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

// al DOM listo
$(document).ready(function () {
    cargarCuentas();
    cargarClientesDropdown();

    $("#filtroNombre, #filtroIdent").on("input", applyFilter);

    // fila click selecciona
    $("#tablaCuentasBody").on("click", "tr", function () {
        selectedCtaId = parseInt(this.id.replace("fila-", ""));
        $("#tablaCuentasBody tr").removeClass("table-active");
        $(this).addClass("table-active");
    });

    // botones globales
    $("#btnAgregarCuenta").click(() => $("#crearModalCuenta").modal("show"));
    $("#btnModificarCuenta").click(() => {
        if (!selectedCtaId) return showAlert("Selecciona una cuenta", "warning");
        abrirEditarCuenta(selectedCtaId);
    });
    $("#btnEliminarCuenta").click(() => {
        if (!selectedCtaId) return showAlert("Selecciona una cuenta", "warning");
        eliminarCuenta(selectedCtaId);
    });

    // guardar nueva
    $("#guardarNuevaCuenta").click(() => {
        const dto = {
            idClientes: parseInt($("#selectCliente").val()),
            saldoDisponible: parseFloat($("#nuevoSaldo").val())
        };
        $.ajax({
            url: `${apiCuentas}/CrearCuenta`,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(dto)
        })
            .done(() => {
                $("#crearModalCuenta").modal("hide");
                limpiarForm("#crearModalCuenta");
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Cuenta creada correctamente',
                    showConfirmButton: false,
                    timer: 2000
                });
                cargarCuentas();
            })
            .fail(() => showAlert("Error al crear cuenta", "danger"));
    });

    // guardar edición
    $("#guardarEdicionCuenta").click(() => {
        const dto = {
            idCuentas: parseInt($("#editarCuentaId").val()),
            estado: $("#editarEstado").val(),
            saldoDisponible: parseFloat($("#editarSaldo").val())
        };
        $.ajax({
            url: `${apiCuentas}/ActualizarCuenta`,
            type: "PUT",
            contentType: "application/json",
            data: JSON.stringify(dto)
        })
            .done(() => {
                $("#editarModalCuenta").modal("hide");
                limpiarForm("#editarModalCuenta");
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Cuenta modificada correctamente',
                    showConfirmButton: false,
                    timer: 2000
                });
                cargarCuentas();
            })
            .fail(() => showAlert("Error al actualizar cuenta", "danger"));
    });

    // limpio inputs del modal
    function limpiarForm(modalSel) {
        $(modalSel).find("input, select").val("");
    }

    // quitar backdrops residuales
    $('#crearModalCuenta, #editarModalCuenta').on('hidden.bs.modal', () => {
        $('.modal-backdrop').remove();
    });
    $('#crearModalCuenta').find('.close, .btn-secondary[data-dismiss="modal"]').on('click', function () {
        $('#crearModalCuenta').modal('hide');
        $('.modal-backdrop').remove();
    });
    $('#editarModalCuenta').find('.close, .btn-secondary[data-dismiss="modal"]').on('click', function () {
        $('#editarModalCuenta').modal('hide');
        $('.modal-backdrop').remove();
    });

});
