Imports System.Data
Imports System.Data.OleDb
Public Class frmVenta
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Private dsVenta As DataSet
    Private dsDetalles As DataSet
    Private factura As Factura
    Private detalles As DataTable
    Private aux As String
    Public Sub New(provincia As String, aux As String, cliente As Cliente)
        InitializeComponent()
        lblProvincia.Content = provincia
        dsVenta = New DataSet()
        'dsDetalles = New DataSet()
        factura = New Factura()
        detalles = New DataTable()
        Me.aux = aux
        factura.P_cliente = cliente
        'Llenar_Datos()
        'detalles = dsVenta.Tables.Add("DETALLES")
        'detalles.Columns.Add("CANT.", Type.GetType("System.Byte"))
        'detalles.Columns.Add("PRODUCTO", Type.GetType("System.String"))
        'detalles.Columns.Add("P. UNIT.", Type.GetType("System.Double"))
        ''dsVenta.Tables.Add(detalles)
        'dtgDetalles.DataContext = dsVenta.Tables("DETALLES")

    End Sub

    Private Sub frmVenta1_Loaded(sender As Object, e As RoutedEventArgs) Handles frmVenta1.Loaded
        Llenar_Datos()
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            consulta = "SELECT * FROM PRODUCTO"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsVenta, "PRODUCTO")


            For Each prod As DataRow In dsVenta.Tables("PRODUCTO").Rows
                cbxProducto.Items.Add(prod(1))
            Next
            Dim sec As Integer = 1
            consulta = "SELECT * FROM FACTURA WHERE PROVINCIA = '" + lblProvincia.Content + "'"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Dim dsRegistros = New DataSet()
            Adapter.Fill(dsRegistros, "FACTURA")
            MessageBox.Show(dsRegistros.Tables("FACTURA").Rows.Count)
            sec = dsRegistros.Tables("FACTURA").Rows.Count + 1
            factura.P_numero = aux & "-001-00000" & sec
            MessageBox.Show(factura.P_numero)
            lblnumero.Content = factura.P_numero

        End Using

        Detalles_Factura()

    End Sub

    Public Sub Llenar_Datos()
        lblNombreCliente.Content = factura.P_cliente.P_nombre
        lblIdenCliente.Content = factura.P_cliente.P_identificacion
        lblDirCliente.Content = factura.P_cliente.P_direccion
        lblTelfCliente.Content = factura.P_cliente.P_telefono
        lblFechaFact.Content = DateString

    End Sub

    Private Sub Detalles_Factura()
        detalles = New DataTable("DETALLES")
        detalles.Columns.Add("CANT.", Type.GetType("System.String"))
        detalles.Columns.Add("PRODUCTO", Type.GetType("System.String"))
        detalles.Columns.Add("P. UNIT.", Type.GetType("System.Double"))
        detalles.Columns.Add("P. TOTAL", Type.GetType("System.Double"))
        dsVenta.Tables.Add(detalles)
        dtgDetalles.DataContext = dsVenta
    End Sub

    Private Sub cbxProducto_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbxProducto.SelectionChanged
        'MessageBox.Show("Selecciono: " + cbxProducto.SelectedItem)
        Dim dataProd As DataRow = dsVenta.Tables("PRODUCTO").Rows(cbxProducto.SelectedIndex)
        txtPrecio.Text = dataProd(3)
        txtStock.Text = dataProd(4)
        'For Each prod As DataRow In dsVenta.Tables("PRODUCTO").Rows
        '    cbxProducto.Items.Add(prod(1))
        'Next
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As RoutedEventArgs) Handles btnAgregar.Click
        Dim detalle As Detalle
        detalle = New Detalle()
        detalle.P_Cantidad = CByte(txtCantidad.Text)
        detalle.P_Producto = cbxProducto.SelectedItem
        detalle.P_PrecioUnit = CDbl(txtPrecio.Text)
        detalle.Calcular_Precio_Final()
        detalle.Calcular_Iva(14)
        Me.factura.P_detalles.Add(detalle)
        dsVenta.Tables("DETALLES").Rows.Add(CStr(detalle.P_Cantidad), detalle.P_Producto, detalle.P_PrecioUnit, detalle.P_PrecioFinal)


        Me.factura.Generar_Totales()
        txtSubtotal.Text = Me.factura.P_subTotal
        txtIva.Text = Me.factura.P_ivaTotal
        txtTotal.Text = Me.factura.P_total
    End Sub

End Class
