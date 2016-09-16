Public Class Cliente
    Private idcliente As Integer
    Private identificacion As String
    Private nombre As String
    Private telefono As String
    Private direccion As String

    Public Property P_idCliente() As Integer
        Get
            Return Me.idcliente
        End Get
        Set(ByVal value As Integer)
            Me.idcliente = value
        End Set
    End Property

    Public Property P_identificacion() As String
        Get
            Return Me.identificacion
        End Get
        Set(ByVal value As String)
            Me.identificacion = value
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

    Public Property P_telefono() As String
        Get
            Return Me.telefono
        End Get
        Set(ByVal value As String)
            Me.telefono = value
        End Set
    End Property

    Public Property P_direccion() As String
        Get
            Return Me.direccion
        End Get
        Set(ByVal value As String)
            Me.direccion = value
        End Set
    End Property


    Public Sub New()
        Me.P_identificacion = ""
        Me.P_nombre = ""
        Me.P_telefono = ""
        Me.P_direccion = ""
    End Sub


End Class
