Imports System.Data
Imports System.Data.OleDb
Public Class frmAdmin
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath

    Private Sub frmAdmin1_Loaded(sender As Object, e As RoutedEventArgs) Handles frmAdmin1.Loaded
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            consulta = "SELECT * FROM USUARIO"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Dim dsUsuarios = New DataSet()
            Adapter.Fill(dsUsuarios, "USUARIO")
            dtgUsuario.DataContext = dsUsuarios


            consulta = "SELECT * FROM PRODUCTO"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Dim dsProductos = New DataSet()
            Adapter.Fill(dsProductos, "PRODUCTO")
            dtgProducto.DataContext = dsProductos

            'consulta = "SELECT * FROM FACTURA"
            consulta = "SELECT FACTURA.NUMERO, CLIENTE.NOMBRE as NOMBRE_CLIENTE, FACTURA.FECHA, FACTURA.TOTAL FROM FACTURA, CLIENTE WHERE (FACTURA.IDCLIENTE = CLIENTE.IDCLIENTE)"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Dim dsFacturas = New DataSet()
            Adapter.Fill(dsFacturas, "FACTURA")
            'MessageBox.Show(dsFacturas.Tables("FACTURA").Rows.Count)
            'For Each fila As DataRow In dsFacturas.Tables("FACTURA").Rows
            '    MessageBox.Show(fila(1))
            'Next
            dtgFactura.DataContext = dsFacturas
        End Using

    End Sub

    Private Sub btnNuevoProd_Click(sender As Object, e As RoutedEventArgs) Handles btnNuevoProd.Click
        Dim newProducto = New frmProducto
        Me.Close()
        newProducto.Show()
    End Sub

    Private Sub dtgFactura_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dtgFactura.SelectionChanged
        Dim fila As DataRowView = sender.selectedItem
        If (fila IsNot Nothing) Then
            Dim factura As New frmFactura(fila(0))
            factura.Owner = Me
            factura.Show()
            Me.Hide()
        End If
    End Sub
End Class
