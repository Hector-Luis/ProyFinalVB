Public Class Factura
    Private aux_provincia As String
    Private baseIva As Byte
    Private fecha As Date
    Private cliente As Cliente
    Private numero As String
    Private detalles As ArrayList
    Private subTotal As Double
    Private ivaTotal As Double
    Private descuento As Double
    Private total As Double
    Private forma_pago As String

    Public Property P_cliente() As Cliente
        Get
            Return Me.cliente
        End Get
        Set(ByVal value As Cliente)
            Me.cliente = value
        End Set
    End Property

    Public Property P_auxProvincia() As String
        Get
            Return Me.aux_provincia
        End Get
        Set(ByVal value As String)
            Me.aux_provincia = value
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

    Public Property P_detalles() As ArrayList
        Get
            Return Me.detalles
        End Get
        Set(ByVal value As ArrayList)
            Me.detalles = value
        End Set
    End Property

    Public ReadOnly Property P_subTotal() As Double
        Get
            Return Me.subTotal
        End Get
    End Property

    Public ReadOnly Property P_ivaTotal() As Double
        Get
            Return Me.ivaTotal
        End Get
    End Property

    Public ReadOnly Property P_total() As Double
        Get
            Return Me.total
        End Get
    End Property

    Public Sub CalcularSubTotal()
        For Each detalle As Detalle In Me.P_detalles
            Me.subTotal = Me.subTotal + detalle.P_PrecioFinal
        Next
    End Sub

    Public Sub CalcularIvaTotal()
        For Each detalle As Detalle In Me.P_detalles
            Me.ivaTotal = Me.ivaTotal + detalle.P_ivaCausado
        Next
    End Sub

    Public Sub CalcularTotal()
        Me.total = Me.P_subTotal + Me.P_ivaTotal - Me.descuento
    End Sub

    'Public Sub New()
    '    Me.P_cliente = New Cliente(1)
    '    Me.P_detalles = New ArrayList
    '    Me.subTotal = 0.0
    '    Me.ivaTotal = 0.0
    '    Me.total = 0.0
    'End Sub

    Public Sub New()
        detalles = New ArrayList
    End Sub

    Public Sub Agregar_Detalle(detalle As Detalle)
        detalles.Add(detalle)
    End Sub

   

    Public Sub Generar_Totales()
        Me.subTotal = 0.0
        Me.ivaTotal = 0.0
        For Each detalle As Detalle In Me.P_detalles
            Me.subTotal = Me.subTotal + detalle.P_PrecioFinal
            Me.ivaTotal = Me.ivaTotal + detalle.P_IvaCausado
        Next
        Me.total = Me.P_subTotal + Me.P_ivaTotal
    End Sub

    Public Sub Guardar()

    End Sub
End Class
