Imports System.Data
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class frmProducto
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Private dsProducto As DataSet
    Private producto As Producto

    Private Sub frmProducto1_Loaded(sender As Object, e As RoutedEventArgs) Handles frmProducto1.Loaded
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            consulta = "SELECT NOMBRE FROM CATEGORIA"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsProducto, "CATEGORIA")

            For Each categ As DataRow In dsProducto.Tables("CATEGORIA").Rows
                cbxCategoria.Items.Add(categ(0))
                'MessageBox.Show(categ(0))
            Next

            consulta = "SELECT MAX(IDPRODUCTO) FROM PRODUCTO"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsProducto, "MAX_ID")
            producto = New Producto()
            For Each max As DataRow In dsProducto.Tables("MAX_ID").Rows
                producto.P_idProducto = max(0) + 1
            Next

            txtId.Text = producto.P_idProducto
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

    Public Sub New()
        InitializeComponent()
        dsProducto = New DataSet()
        txtPrecio.Text = String.Format("{0:0.0}", 0.0)

    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As RoutedEventArgs) Handles btnGuardar.Click
        Try
            producto.P_nombre = txtNombre.Text.ToUpper
            producto.P_idCategoria = cbxCategoria.SelectedIndex + 1
            producto.P_precio = Convert.ToDouble(txtPrecio.Text)
            producto.P_stock = txtStock.Text

            Using dbConexion As New OleDbConnection(strConexion)
                Dim consulta As String
                Dim Adapter As New OleDbDataAdapter

                consulta = "SELECT * FROM PRODUCTO"
                Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
                Adapter.Fill(dsProducto, "PRODUCTO")
                Dim nuevo_producto = New OleDbCommandBuilder(Adapter)

                dsProducto.Tables(2).Rows.Add(producto.P_idProducto, producto.P_nombre, producto.P_idCategoria, producto.P_precio, producto.P_stock)

                Try
                    Adapter.Update(dsProducto.Tables("PRODUCTO"))
                    MessageBox.Show("PRODUCTO AGREGADO")
                Catch ex As Exception
                    MessageBox.Show("ERROR AL AGREGAR")
                End Try

            End Using

            Dim frmprod As frmProducto = New frmProducto()
            frmprod.Show()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("PRECIO MAL ESCRITO")
        End Try


    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs) Handles btnCancelar.Click
        Dim frmAdmin = New frmAdmin()
        frmAdmin.Show()
        Me.Close()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As RoutedEventArgs) Handles btnActualizar.Click
        Using dbConexion As New OleDbConnection(strConexion)
            Dim sentencia As String
            Dim Adapter As New OleDbDataAdapter
            Dim actualizacion = New OleDbCommandBuilder(Adapter)
            sentencia = "UPDATE PRODUCTO SET NOMBRE = 'MODIFICADO EXITOSO' WHERE IDPRODUCTO = 1"
            Adapter = New OleDbDataAdapter(New OleDbCommand(sentencia, dbConexion))
            Adapter.Fill(dsProducto, "PROD_MODIF")

            'dsProducto.Tables(3).Rows.Add(producto.P_idProducto, producto.P_nombre, producto.P_idCategoria, producto.P_precio, producto.P_stock)

            Try
                Adapter.Update(dsProducto.Tables("PROD_MODIF"))
                MessageBox.Show("PRODUCTO MODIFICADO")
            Catch ex As Exception
                MessageBox.Show("ERROR AL MODIFICAR")
            End Try

        End Using
    End Sub
End Class
