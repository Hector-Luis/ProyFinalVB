﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class Datos_Factura
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Private cliente As Cliente
    Private dsDatos As DataSet

    Public Sub New()

        InitializeComponent()
        dsDatos = New DataSet()


    End Sub
    Private Sub frmDatosFactura_Loaded(sender As Object, e As RoutedEventArgs) Handles frmDatosFactura.Loaded
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            consulta = "SELECT * FROM PROVINCIA"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            'Dim dsProvincias = New DataSet()
            Adapter.Fill(dsDatos, "PROVINCIA")

            For Each prov As DataRow In dsDatos.Tables("PROVINCIA").Rows
                cbxProvincia.Items.Add(prov(1))
            Next

        End Using
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs) Handles btnAceptar.Click
        cliente = New Cliente()
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            consulta = "SELECT * FROM CLIENTE WHERE IDENTIFICACION = '" + txtIdentificacion.Text + "'"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            'Dim dsClientes = New DataSet()
            Adapter.Fill(dsDatos, "CLIENTE")
            For Each client As DataRow In dsDatos.Tables("CLIENTE").Rows
                cliente.P_idCliente = client(0)
                cliente.P_nombre = client(1)
                cliente.P_identificacion = client(2)
                cliente.P_telefono = client(3)
                cliente.P_direccion = client(4)
            Next
            If cliente.P_nombre = "" Then
                MessageBox.Show("CLIENTE NUEVO")
                cliente.P_identificacion = txtIdentificacion.Text
                Dim cli As frmCliente = New frmCliente(cliente)
                cli.Show()
            Else
                consulta = "SELECT * FROM PROVINCIA WHERE NOMBRE = '" + cbxProvincia.SelectedItem + "'"
                Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
                Dim dsProv As DataSet = New DataSet()
                Adapter.Fill(dsProv, "PROVINCIA")
                Dim aux As String = ""
                Dim baseIva As Byte = 0
                For Each row As DataRow In dsProv.Tables("PROVINCIA").Rows
                    aux = row(2)
                    baseIva = row(3)
                    'MessageBox.Show(aux)
                Next

                consulta = "SELECT * FROM FACTURA WHERE PROVINCIA = '" + cbxProvincia.SelectedItem + "'"
                Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
                Dim dsCont As DataSet = New DataSet()
                Adapter.Fill(dsCont, "REGISTRO")
                Dim sec As Integer = dsCont.Tables("REGISTRO").Rows.Count
                Dim numFact As String = aux + "-001-00000" + CStr(sec + 1)
                Dim factura As Factura = New Factura(cbxProvincia.SelectedItem, baseIva, cliente, numFact)
                'MessageBox.Show(numFact)
                Dim venta As frmVenta = New frmVenta(factura)
                venta.Show()
                Me.Close()
            End If
        End Using

    End Sub

    Private Sub txtIdentificacion_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtIdentificacion.PreviewTextInput
        Dim regex As Regex = New Regex("[^0-9]+")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub frmDatosFactura_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles frmDatosFactura.Closing
        Dim frmLogin As MainWindow = New MainWindow()
        frmLogin.Show()
    End Sub
End Class
