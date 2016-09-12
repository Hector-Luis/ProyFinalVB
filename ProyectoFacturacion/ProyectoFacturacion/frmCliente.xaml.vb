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

        'Using dbConexion As New OleDbConnection(strConexion)

        '    Dim guardar As String
        '    Dim Adapter As New OleDbDataAdapter
        '    guardar = "INSERT INTO CLIENTE VALUES ('" + cliente.P_nombre + "','" + cliente.P_identificacion + "','" + cliente.P_telefono + "','" + cliente.P_direccion + "'"
        '    Adapter = New OleDbDataAdapter(New OleDbCommand(guardar, dbConexion))

        '    Adapter.Update()

        'End Using
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            Dim dsClientes As DataSet = New DataSet()
            consulta = "SELECT * FROM CLIENTE"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            'Dim dsProvincias = New DataSet()
            Adapter.Fill(dsClientes, "CLIENTE")
            Dim nuevo As DataRow = dsClientes.Tables("CLIENTE").NewRow()
            nuevo("NOMBRE") = txtNombre.Text
            nuevo("IDENTIFICACION") = txtIdentificacion.Text
            nuevo("TELEFONO") = txtTelefono
            nuevo("DIRECCION") = txtDireccion

            Adapter.Update(dsClientes.Tables("CLIENTE"))
            dsClientes.Tables("CLIENTE").AcceptChanges()

        End Using
    End Sub
End Class
