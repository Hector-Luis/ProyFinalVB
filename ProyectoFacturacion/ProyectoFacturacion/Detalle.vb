Public Class Detalle
    Private idDetalle As Integer
    Private idFactura As Integer
    Private cantidad As Byte
    Private producto As String
    Private precio_unit As Double
    Private precio_final As Double = 0.0
    Private iva_causado As Double = 0.0

    Public Property P_IdDetalle As Integer
        Get
            Return idDetalle
        End Get
        Set(ByVal value As Integer)
            idDetalle = value
        End Set
    End Property

    Public Property P_IdFactura As Integer
        Get
            Return idFactura
        End Get
        Set(ByVal value As Integer)
            idFactura = value
        End Set
    End Property

    Public Property P_Cantidad As Byte
        Get
            Return cantidad
        End Get
        Set(ByVal value As Byte)
            cantidad = value
        End Set
    End Property

    Public Property P_Producto As String
        Get
            Return producto
        End Get
        Set(ByVal value As String)
            producto = value
        End Set
    End Property

    Public Property P_PrecioUnit As Double
        Get
            Return precio_unit
        End Get
        Set(ByVal value As Double)
            precio_unit = value
        End Set
    End Property

    Public ReadOnly Property P_PrecioFinal() As Double
        Get
            Return precio_final
        End Get
    End Property

    Public ReadOnly Property P_IvaCausado() As Double
        Get
            Return iva_causado
        End Get
    End Property


    Public Sub Calcular_Precio_Final()
        precio_final = precio_unit * cantidad
    End Sub

    Public Sub Calcular_Iva(iva As Integer)
        iva_causado = precio_final * iva / 100
    End Sub

    'Public Sub New(cant As Byte, prod As String, p_unit As Double)
    '    Me.cantidad = cant
    '    Me.producto = prod
    '    Me.precio_unit = p_unit
    'End Sub
End Class
