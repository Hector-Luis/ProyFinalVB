Imports System.Data
Imports System.Data.OleDb
Public Class frmVenta
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Private dsVenta As DataSet
    Private dsDetalles As DataSet
    Private factura As Factura
    Private detalles As DataTable

    Public Sub New(factura As Factura)
        InitializeComponent()
        Me.factura = factura
        lblProvincia.Content = factura.P_Provincia
        lblIva.Content = "IVA " & factura.P_baseIva & ":"
        Me.factura.P_formaPago = cbxTipoPago.SelectedItem
        dsVenta = New DataSet()
        detalles = New DataTable()

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

            consulta = "SELECT * FROM TIPO_PAGO"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsVenta, "TIPO_PAGO")
            For Each tipo As DataRow In dsVenta.Tables("TIPO_PAGO").Rows
                cbxTipoPago.Items.Add(tipo(1))
            Next

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
        detalles.Columns.Add("P. UNIT.", Type.GetType("System.String"))
        detalles.Columns.Add("P. TOTAL", Type.GetType("System.String"))
        dsVenta.Tables.Add(detalles)
        dtgDetalles.DataContext = dsVenta
    End Sub

    Private Sub cbxProducto_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbxProducto.SelectionChanged
        Dim dataProd As DataRow = dsVenta.Tables("PRODUCTO").Rows(cbxProducto.SelectedIndex)
        txtPrecio.Text = dataProd(3)
        txtStock.Text = dataProd(4)
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As RoutedEventArgs) Handles btnAgregar.Click
        If CInt(txtCantidad.Text) > CInt(txtStock.Text) Then
            MessageBox.Show("EXCEDE EL STOCK")
        Else
            Dim produc As Producto = New Producto()
            Using dbConexion As New OleDbConnection(strConexion)
                Dim consulta As String
                Dim Adapter As New OleDbDataAdapter
                consulta = "SELECT PRODUCTO.IDPRODUCTO, PRODUCTO.NOMBRE, CATEGORIA.PAGO_IVA FROM PRODUCTO, CATEGORIA WHERE (PRODUCTO.NOMBRE = '" + cbxProducto.SelectedItem + "' AND PRODUCTO.CATEGORIA = CATEGORIA.IDCATEGORIA)"
                Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
                Adapter.Fill(dsVenta, "PROD")
                For Each prod As DataRow In dsVenta.Tables("PROD").Rows
                    produc.P_idProducto = prod(0)
                    produc.P_nombre = prod(1)
                    If prod(2) = "SI" Then
                        produc.P_pagoIva = True
                    Else
                        produc.P_pagoIva = False
                    End If
                Next

            End Using
            Dim detalle As Detalle
            detalle = New Detalle()
            detalle.P_Cantidad = CByte(txtCantidad.Text)
            detalle.P_Producto = produc
            detalle.P_PrecioUnit = CDbl(txtPrecio.Text)
            detalle.Calcular_Precio_Final()
            detalle.Calcular_Iva(factura.P_baseIva)

            Me.factura.P_detalles.Add(detalle)
            dsVenta.Tables("DETALLES").Rows.Add(detalle.P_Cantidad, detalle.P_Producto.P_nombre, detalle.P_PrecioUnit, detalle.P_PrecioFinal)


            Me.factura.Generar_Totales()
            txtSubtotal.Text = Me.factura.P_subtotal
            txtIva.Text = Me.factura.P_ivatotal
            txtDescuento.Text = Me.factura.P_descuento
            txtTotal.Text = Me.factura.P_total

        End If
    End Sub

    Private Sub cbxTipoPago_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbxTipoPago.SelectionChanged
        Me.factura.P_formaPago = cbxTipoPago.SelectedItem
        factura.Generar_Totales()
        txtSubtotal.Text = Me.factura.P_subtotal
        txtIva.Text = Me.factura.P_ivatotal
        txtDescuento.Text = Me.factura.P_descuento
        txtTotal.Text = Me.factura.P_total
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As RoutedEventArgs) Handles btnGuardar.Click
        Me.factura.Guardar()
        Me.Close()
        Dim frmDatos As Datos_Factura = New Datos_Factura()
        frmDatos.Show()
    End Sub

    Private Sub frmVenta1_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles frmVenta1.Closing
        Dim frmLogin As MainWindow = New MainWindow()
        frmLogin.Show()
    End Sub
End Class
