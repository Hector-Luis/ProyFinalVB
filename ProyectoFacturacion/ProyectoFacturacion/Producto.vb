Public Class Producto
    Private idProducto As Integer
    Private nombre As String
    Private precio As Double
    Private pagoIva As Boolean
    Private stock As Integer
    Private idCategoria As Integer

    Public Property P_idCategoria() As Integer
        Get
            Return Me.idCategoria
        End Get
        Set(ByVal value As Integer)
            Me.idCategoria = value
        End Set
    End Property

    Public Property P_stock() As Integer
        Get
            Return Me.stock
        End Get
        Set(ByVal value As Integer)
            Me.stock = value
        End Set
    End Property

    Public Property P_idProducto() As Integer
        Get
            Return Me.idProducto
        End Get
        Set(ByVal value As Integer)
            Me.idProducto = value
        End Set
    End Property

    Public Property P_nombre() As String
        Get
            Return Me.nombre
        End Get
        Set(ByVal value As String)
            Me.nombre = value
        End Set
    End Property

    Public Property P_precio() As Double
        Get
            Return Me.precio
        End Get
        Set(ByVal value As Double)
            Me.precio = value
        End Set
    End Property

    Public Property P_pagoIva() As Boolean
        Get
            Return Me.pagoIva
        End Get
        Set(ByVal value As Boolean)
            Me.pagoIva = value
        End Set
    End Property


    Public Sub New(idproducto As Integer)
        Me.P_idProducto = idproducto
        
    End Sub

    Public Sub New()

    End Sub
End Class
