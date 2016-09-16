Imports System.Data
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class frmProducto
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath

    Private Sub frmProducto1_Loaded(sender As Object, e As RoutedEventArgs) Handles frmProducto1.Loaded
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            consulta = "SELECT NOMBRE FROM CATEGORIA"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Dim dsCategorias = New DataSet()
            Adapter.Fill(dsCategorias, "CATEGORIA")

            For Each categ As DataRow In dsCategorias.Tables("CATEGORIA").Rows
                cbxCategoria.Items.Add(categ(0))
                'MessageBox.Show(categ(0))
            Next


        End Using
    End Sub

    Private Sub txtStock_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtStock.PreviewTextInput
        Dim regex As Regex = New Regex("[^0-9]+")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub txtPrecio_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtPrecio.PreviewTextInput
        'Dim regex As Regex = New Regex("[^0-9]+")
        'e.Handled = regex.IsMatch(e.Text)
    End Sub
End Class
