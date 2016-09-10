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

            'For Each prod As DataRow In dsProductos.Tables("PRODUCTO").Rows
            '    MessageBox.Show(prod(1))
            'Next
            dtgProducto.DataContext = dsProductos

        End Using

    End Sub

    Private Sub btnNuevoProd_Click(sender As Object, e As RoutedEventArgs) Handles btnNuevoProd.Click
        Dim newProducto = New frmProducto
        newProducto.Show()
    End Sub
End Class
