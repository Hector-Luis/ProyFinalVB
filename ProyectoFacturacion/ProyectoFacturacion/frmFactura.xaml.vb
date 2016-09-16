Imports System.Data
Imports System.Data.OleDb
Public Class frmFactura
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Private factura As Factura
    Private dsFactura As DataSet
    Public Sub New(numfactura As String)
        InitializeComponent()
        MessageBox.Show(numfactura)
        factura = New Factura()
        factura.P_numero = numfactura
        dsFactura = New DataSet()

    End Sub

    Private Sub frmFactura1_Loaded(sender As Object, e As RoutedEventArgs) Handles frmFactura1.Loaded
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter

            consulta = "SELECT * FROM DETALLE WHERE NUMFACTURA = '" + factura.P_numero + "'"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsFactura, "DETALLE")
            dtgDetalles.DataContext = dsFactura


            consulta = "SELECT * FROM FACTURA WHERE NUMERO = '" + factura.P_numero + "'"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsFactura, "DATOS")
            'MessageBox.Show(dsFactura.Tables("DATOS").Columns.Count & "DATOS")

            Dim idcliente As Integer = 0
            For Each dato As DataRow In dsFactura.Tables("DATOS").Rows
                factura.P_id = dato(0)
                factura.P_numero = dato(2)
                factura.P_Provincia = dato(1)
                idcliente = dato(3)
                factura.P_subtotal = dato(6)
                factura.P_ivatotal = dato(7)
                factura.P_descuento = dato(8)
                factura.P_total = dato(9)
            Next

            txtSubtotal.Text = factura.P_subtotal
            txtIva.Text = factura.P_ivatotal
            txtDescuento.Text = factura.P_descuento
            txtTotal.Text = factura.P_total

            consulta = "SELECT * FROM CLIENTE WHERE IDCLIENTE = " & idcliente
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsFactura, "CLIENTE")
            'MessageBox.Show(dsFactura.Tables("CLIENTE").Rows.Count & "CLIENTE")
            For Each cli As DataRow In dsFactura.Tables("cliente").Rows
                txtNombre.Text = cli(1)
                txtIdentificacion.Text = cli(2)
            Next

            'consulta = "SELECT * FROM FACTURA"
            'Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            'Dim dsFacturas = New DataSet()
            'Adapter.Fill(dsFacturas, "FACTURA")
            'dtgFactura.DataContext = dsFacturas
        End Using
    End Sub


End Class
