Imports System.Data
Imports System.Data.OleDb
Public Class frmCliente
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Private cliente As Cliente

    Public Sub New(cli As Cliente)

        InitializeComponent()
        Me.cliente = cli
        txtDireccion.Text = cliente.P_direccion
        txtTelefono.Text = cliente.P_telefono
        txtNombre.Text = cliente.P_nombre
        txtIdentificacion.Text = cliente.P_identificacion

    End Sub


    Private Sub btnGuardar_Click(sender As Object, e As RoutedEventArgs) Handles btnGuardar.Click

        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            Dim dsClientes As DataSet = New DataSet()

            consulta = "SELECT MAX(IDCLIENTE) FROM CLIENTE"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Dim dsMax As DataSet = New DataSet()
            Adapter.Fill(dsMax, "MAX_ID")
            For Each max As DataRow In dsMax.Tables("MAX_ID").Rows
                cliente.P_idCliente = CInt(max(0)) + 1
                MessageBox.Show(cliente.P_idCliente)
            Next

            consulta = "SELECT * FROM CLIENTE"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsClientes, "CLIENTE")
            Dim nuevo_cliente = New OleDbCommandBuilder(Adapter)

            'Dim idc As Integer = cliente.P_idCliente
            dsClientes.Tables(0).Rows.Add(cliente.P_idCliente, txtNombre.Text, txtIdentificacion.Text, txtTelefono.Text, txtDireccion.Text)

            Try
                Adapter.Update(dsClientes.Tables("CLIENTE"))
                MessageBox.Show("CLIENTE AGREGADO")
            Catch ex As Exception
                MessageBox.Show("ERROR AL AGREGAR")
            End Try

            'MessageBox.Show("CLIENTE AGREGADO")
            'dsClientes.Tables("CLIENTE").AcceptChanges()

        End Using
    End Sub
End Class
