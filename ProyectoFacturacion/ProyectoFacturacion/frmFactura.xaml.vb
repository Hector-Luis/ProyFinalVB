Imports System.Data
Imports System.Data.OleDb
Imports System.Printing
Imports System.Globalization

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

            consulta = "SELECT CANTIDAD, PRODUCTO, PRECIO_U, PRECIO_T FROM DETALLE WHERE NUMFACTURA = '" + factura.P_numero + "'"
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


    'Private Sub btnImprimir_Click(sender As Object, e As RoutedEventArgs) Handles btnImprimir.Click
    '    Dim imprimirDlg As PrintDialog = New PrintDialog()
    '    'imprimirDlg.PrintVisual(Me.label, "")
    '    'imprimirDlg.PrintVisual(Me.txtNombre, "")
    '    'imprimirDlg.PrintVisual(Me.label1, "")
    '    'imprimirDlg.PrintVisual(Me.txtIdentificacion, "")
    '    'imprimirDlg.PrintVisual(Me.dtgDetalles, "")

    '    Dim dialog As PrintDialog = New PrintDialog()
    '    Dim respuesta As MessageBoxResult = MessageBox.Show("Desea imprimir el texto?, si no, se imprimira la aplicación", "Impresión", MessageBoxButton.YesNoCancel)
    '    If respuesta = MessageBoxResult.Yes Then
    '        'Imprimir el texto
    '        If dialog.ShowDialog() = True Then
    '            Dim texto As String = txtNombre.Text
    '            Dim r As Run = New Run(texto)
    '            Dim parrafo As Paragraph = New Paragraph()
    '            parrafo.Inlines.Add(r)
    '            Dim doc As FlowDocument = New FlowDocument(parrafo)
    '            doc.PagePadding = New Thickness(100)
    '            Dim dcpagin As DocumentPaginator = 
    '            dialog.PrintDocument(IDocumentPaginatorSource(doc, texto)
    '        End If
    '    ElseIf respuesta = MessageBoxResult.No Then
    '        If dialog.ShowDialog() = True Then
    '            dialog.PrintVisual(Me.frmFactura1, "Impresión")
    '        End If
    '    End If


    'End Sub

   
    Sub Print(ele As FrameworkElement)
        Dim margin As Double = 90
        'Dim titlePadding As Double = 10

        Dim printDlg As PrintDialog = New PrintDialog()
        printDlg.PrintTicket.PageOrientation = PageOrientation.Portrait
        If (printDlg.ShowDialog() <> True) Then Return

        Dim formattedText As FormattedText = New FormattedText(Name, CultureInfo.GetCultureInfo("en-us"),
                                                        FlowDirection.LeftToRight, New Typeface("Arial"), 25, Brushes.Black)

        formattedText.MaxTextWidth = printDlg.PrintableAreaWidth

        Dim scale As Double = 0.6
        Dim visual As DrawingVisual = New DrawingVisual()
        Using context As DrawingContext = visual.RenderOpen()

            Dim brush As VisualBrush = New VisualBrush(ele)
            context.DrawRectangle(brush, Nothing, New Rect(New Point(margin, margin), New Size(ele.ActualWidth, ele.ActualHeight)))

            'context.DrawText(formattedText, New Point(margin, margin))
        End Using
        visual.Transform = New ScaleTransform(scale, scale)
        printDlg.PrintVisual(visual, "")
    End Sub

    Private Sub btn_Imprimir_Click(sender As Object, e As RoutedEventArgs) Handles btn_Imprimir.Click
        Print(Me.grdContenido)
    End Sub
End Class
