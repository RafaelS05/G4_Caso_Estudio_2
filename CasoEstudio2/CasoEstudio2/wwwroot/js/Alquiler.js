document.getElementById('ddlCasas').addEventListener('change', function () {
    const selected = this.options[this.selectedIndex];
    const precio = selected.getAttribute('data-precio');
    document.getElementById('txtPrecio').value = precio
        ? parseFloat(precio).toLocaleString('es-CR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        })
        : '';
});