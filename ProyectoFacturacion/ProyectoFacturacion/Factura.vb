Imports System.Data
Imports System.Data.OleDb
Public Class Factura
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath

    Private provincia As String
    Private baseIva As Byte
    Private cliente As Cliente
    Private numero As String
    Private fecha As Date
    Private detalles As ArrayList
    Private subTotal As Double
    Private ivaTotal As Double
    Private descuento As Double
    Private total As Double
    Private forma_pago As String
    Private idfactura As Integer
    Private estado As String

    Public Property P_id() As Integer
        Get
            Return Me.idfactura
        End Get
        Set(ByVal value As Integer)
            Me.idfactura = value
        End Set
    End Property



    Public Property P_cliente() As Cliente
        Get
            Return Me.cliente
        End Get
        Set(ByVal value As Cliente)
            Me.cliente = value
        End Set
    End Property

    Public Property P_baseIva() As Byte
        Get
            Return Me.baseIva
        End Get
        Set(ByVal value As Byte)
            Me.baseIva = value
        End Set
    End Property

    Public Property P_Provincia() As String
        Get
            Return Me.provincia
        End Get
        Set(ByVal value As String)
            Me.provincia = value
        End Set
    End Property

    Public Property P_estado() As String
        Get
            Return Me.estado
        End Get
        Set(ByVal value As String)
            Me.estado = value
        End Set
    End Property

    Public Property P_numero() As String
        Get
            Return Me.numero
        End Get
        Set(ByVal value As String)
            Me.numero = value
        End Set
    End Property

    Public Property P_formaPago() As String
        Get
            Return Me.forma_pago
        End Get
        Set(ByVal value As String)
            Me.forma_pago = value
        End Set
    End Property

    Public Property P_detalles() As ArrayList
        Get
            Return Me.detalles
        End Get
        Set(ByVal value As ArrayList)
            Me.detalles = value
        End Set
    End Property

    Public Property P_subtotal() As Double
        Get
            Return Me.subTotal
        End Get
        Set(ByVal value As Double)
            Me.subTotal = value
        End Set
    End Property

    Public Property P_ivatotal() As Double
        Get
            Return Me.ivaTotal
        End Get
        Set(ByVal value As Double)
            Me.ivaTotal = value
        End Set
    End Property

    Public Property P_descuento() As Double
        Get
            Return Me.descuento
        End Get
        Set(ByVal value As Double)
            Me.descuento = value
        End Set
    End Property

    Public Property P_total() As Double
        Get
            Return Me.total
        End Get
        Set(ByVal value As Double)
            Me.total = value
        End Set
    End Property
    Public Sub CalcularSubTotal()
        For Each detalle As Detalle In Me.P_detalles
            Me.subTotal = Me.subTotal + detalle.P_PrecioFinal
        Next
    End Sub

    Public Sub CalcularIvaTotal()
        For Each detalle As Detalle In Me.P_detalles
            Me.ivaTotal = Me.ivaTotal + detalle.P_IvaCausado
        Next
    End Sub

    Public Sub CalcularDescuento(forma_pago As String)
        Me.forma_pago = forma_pago
        Select Case Me.forma_pago
            Case "EFECTIVO"
                P_descuento = P_ivatotal * 0.0
            Case "TARJETA"
                If P_baseIva = 12 Then
                    P_descuento = P_ivatotal * 0.0833333
                Else
                    P_descuento = P_ivatotal * 0.0714286
                End If
            Case "ELECTRONICO"
                If P_baseIva = 12 Then
                    P_descuento = P_ivatotal * 0.1666667
                Else
                    P_descuento = P_ivatotal * 0.1428571
                End If
        End Select
        Me.descuento = Math.Round(Me.descuento, 2)
    End Sub

    Public Sub New()
        detalles = New ArrayList
    End Sub

    Public Sub New(prov As String, baseIva As Byte, cliente As Cliente, numFact As String)
        Me.P_Provincia = prov
        Me.baseIva = baseIva
        Me.P_cliente = cliente
        Me.P_numero = numFact
        Me.P_estado = "GENERADA"
        detalles = New ArrayList
    End Sub

    Public Sub Agregar_Detalle(detalle As Detalle)
        detalles.Add(detalle)
    End Sub



    Public Sub Generar_Totales()
        Me.subTotal = 0.0
        Me.ivaTotal = 0.0
        Me.descuento = 0.0
        For Each detalle As Detalle In Me.P_detalles
            Me.subTotal = Me.subTotal + detalle.P_PrecioFinal
            Me.subTotal = Math.Round(Me.subTotal, 2)
            Me.ivaTotal = Me.ivaTotal + detalle.P_IvaCausado
            Me.ivaTotal = Math.Round(Me.ivaTotal, 2)
        Next
        CalcularDescuento(Me.forma_pago)
        Me.total = Me.P_subtotal + Me.P_ivatotal - Me.P_descuento
        Me.total = Math.Round(Me.total, 2)
    End Sub

    Public Sub Guardar()
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String
            Dim Adapter As New OleDbDataAdapter
            Dim dsFactura As DataSet = New DataSet()

            ''consulta = "SELECT MAX(IDCLIENTE) FROM CLIENTE"
            ''Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            ''Dim dsMax As DataSet = New DataSet()
            ''Adapter.Fill(dsMax, "MAX_ID")
            ''For Each max As DataRow In dsMax.Tables("MAX_ID").Rows
            ''    cliente.P_idCliente = CInt(max(0)) + 1
            ''    MessageBox.Show(cliente.P_idCliente)
            ''Next
            Dim sec As Integer = 0
            

            consulta = "SELECT * FROM DETALLE"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsFactura, "DETALLE")
            sec = dsFactura.Tables("DETALLE").Rows.Count
            Dim nuevo_detalle = New OleDbCommandBuilder(Adapter)

            'Dim idc As Integer = cliente.P_idCliente
            For Each detalle As Detalle In P_detalles
                sec = sec + 1
                dsFactura.Tables(0).Rows.Add(sec, detalle.P_Cantidad, detalle.P_Producto.P_nombre, detalle.P_PrecioUnit, detalle.P_PrecioFinal, detalle.P_IvaCausado, Me.P_numero)
            Next


            Try
                Adapter.Update(dsFactura.Tables("DETALLE"))
                'MessageBox.Show("DETALLES AGREGADOS")
            Catch ex As Exception
                MessageBox.Show("ERROR AL AGREGAR")
            End Try

            consulta = "SELECT * FROM FACTURA"
            Adapter = New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))
            Adapter.Fill(dsFactura, "FACTURA")
            sec = dsFactura.Tables("FACTURA").Rows.Count
            Dim nueva_factura = New OleDbCommandBuilder(Adapter)
            sec = sec + 1
            dsFactura.Tables(1).Rows.Add(sec, Me.P_Provincia, Me.P_numero, Me.P_cliente.P_idCliente, System.DateTime.Today,
                                         Me.P_formaPago, Me.P_subtotal, Me.P_ivatotal, Me.P_descuento, Me.P_total, Me.P_estado)


            Try
                Adapter.Update(dsFactura.Tables("FACTURA"))
                MessageBox.Show("FACTURA GUARDADA")
            Catch ex As Exception
                MessageBox.Show("ERROR AL AGREGAR")
            End Try

        End Using

    End Sub
End Class
