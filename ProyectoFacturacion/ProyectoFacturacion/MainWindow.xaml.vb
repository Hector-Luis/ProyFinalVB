Imports System.Data
Imports System.Data.OleDb
Class MainWindow
    Private dbPath = "Facturacion.mdb"
    Public strConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & dbPath
    Dim dsUsuarios As DataSet


    Private Sub frmLogin_Loaded(sender As Object, e As RoutedEventArgs) Handles frmLogin.Loaded
        Using dbConexion As New OleDbConnection(strConexion)

            Dim consulta As String = "SELECT * FROM USUARIO"
            Dim Adapter As New OleDbDataAdapter(New OleDbCommand(consulta, dbConexion))

            dsUsuarios = New DataSet("USUARIO")
            Adapter.Fill(dsUsuarios, "USUARIO")


        End Using
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs) Handles btnLogin.Click
        For Each user As DataRow In dsUsuarios.Tables("USUARIO").Rows
            If txtUser.Text = user(1) Then
                If pwdPass.Password = user(2) Then
                    MessageBox.Show("BIENVENIDO")
                    Select Case user(5)
                        Case 1
                            Dim admin As New frmAdmin
                            admin.Show()
                            Me.Hide()
                        Case 2
                            Dim dat_fact As New Datos_Factura
                            dat_fact.Show()
                            Me.Hide()
                    End Select
                Else
                    MessageBox.Show("CLAVE INVÁLIDA")
                End If
            End If
        Next
    End Sub

    Private Sub frmLogin_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles frmLogin.Closing

    End Sub
End Class
