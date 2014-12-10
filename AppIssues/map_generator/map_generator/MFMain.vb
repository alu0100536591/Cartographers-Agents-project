﻿Imports System.Reflection

Public Class MFMain


    Private Const ROWSIZE = 30

    Private SelectedColor As Color
    Private SelectedValue As String
    Private Painting As Boolean = False


#Region "Procedures"

    Private Sub MapCreate(Optional ByVal Height As Integer = 30, Optional ByVal Width As Integer = 30)
        Cursor = System.Windows.Forms.Cursors.WaitCursor
        Try
            Me.grid.Visible = False
            Me.grid.Rows.Clear()

            Me.grid.ColumnCount = Width
            For index = 1 To Width
                Me.grid.Columns(index - 1).Name = index.ToString
                Me.grid.Columns(index - 1).Width = ROWSIZE
            Next index

            For index = 0 To Height - 1
                Dim row = New DataGridViewRow
                row.Height = ROWSIZE
                Me.grid.Rows.Add(row)
            Next index

            Me.grid.Dock = DockStyle.Fill
            Me.grid.BringToFront()
            Me.grid.Visible = True
            Me.grid.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub MakeGridViewDoubleBuffered(ByVal dgv As DataGridView)
        ' sirve simplemente para cambiar la propiedad de DoubleBuffered a True
        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, True, Nothing)
    End Sub

    Private Sub Save(ByVal PathFile As String)
        Cursor = System.Windows.Forms.Cursors.WaitCursor
        Try
            Dim Dimension As String = (Me.grid.RowCount) & "x" & (Me.grid.ColumnCount) & Chr(13)
            Dim Agents As String = "Agents" & Chr(13)
            Dim Blocks As String = "Blocks" & Chr(13)

            For row As Integer = 0 To Me.grid.RowCount - 1
                For column As Integer = 0 To Me.grid.ColumnCount - 1

                    Select Case Me.grid.Item(column, row).Value
                        Case 1
                            Blocks += row.ToString & "," & column.ToString & ";" & "1" & Chr(13)
                        Case -1
                            Agents += row.ToString & "," & column.ToString & Chr(13)
                    End Select

                Next column
            Next row

            Dim Content As String = Dimension & Agents & Blocks

            Dim sw As New System.IO.StreamWriter(PathFile)
            sw.WriteLine(Content.Substring(0, Content.Length - 1))
            sw.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub CellPaint(ByVal row As Integer, ByVal col As Integer, ByVal color As Color, ByVal value As Integer)
        Me.grid.Rows(row).Cells(col).Style.BackColor = color
        Me.grid.Rows(row).Cells(col).Style.ForeColor = color
        Me.grid.Rows(row).Cells(col).Value = value
    End Sub


#End Region


#Region "Events"

    Private Sub MFMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SelectedColor = New Color
        SelectedColor = btnPared.BackColor
        SelectedValue = 1

        MakeGridViewDoubleBuffered(Me.grid)
    End Sub

    Private Sub btnMapCreate_Click(sender As Object, e As EventArgs) Handles btnMapCreate.Click
        Me.pnlMapCreate.Size = New Size(147, 176)
        Me.pnlMapCreate.BringToFront()
        Me.pnlMapCreate.Visible = Not Me.pnlMapCreate.Visible
    End Sub

    Private Sub btnMapCreate_OK_Click(sender As Object, e As EventArgs) Handles btnMapCreate_OK.Click
        Me.pnlMapCreate.Visible = False
        MapCreate(Me.txtHeight.Value, Me.txtWidth.Value)
    End Sub

    Private Sub btnBlock_Click(sender As Object, e As EventArgs) Handles btnSuelo.Click, btnPared.Click, btnAgente.Click
        SelectedColor = CType(sender, Button).BackColor
        SelectedValue = CType(sender, Button).Tag
        Me.grid.DefaultCellStyle.SelectionBackColor = SelectedColor
        Me.grid.DefaultCellStyle.SelectionForeColor = SelectedColor
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If (Me.grid.RowCount <= 0) Then
            Exit Sub
        End If

        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.Filter = "Map file|*.map"
        saveFileDialog1.Title = "Guardar fichero de mapa"
        saveFileDialog1.ShowDialog()
        If saveFileDialog1.FileName <> String.Empty Then
            Save(saveFileDialog1.FileName)
        End If
    End Sub

#Region "   Paint"

    Private Sub grid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles grid.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Painting = True

            If (e.RowIndex > -1) And (e.ColumnIndex > -1) Then
                CellPaint(e.RowIndex, e.ColumnIndex, SelectedColor, SelectedValue)
            End If
        End If

        If e.Button = Windows.Forms.MouseButtons.Right Then
            If (e.RowIndex > -1) And (e.ColumnIndex > -1) Then
                CellPaint(e.RowIndex, e.ColumnIndex, Me.btnSuelo.BackColor, Me.btnSuelo.Tag)
            End If
        End If
    End Sub

    Private Sub grid_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellMouseEnter
        If (Not Painting) Then
            Exit Sub
        End If

        If (e.RowIndex > -1) And (e.ColumnIndex > -1) Then
            CellPaint(e.RowIndex, e.ColumnIndex, SelectedColor, SelectedValue)
        End If
    End Sub

    'Private Sub grid_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellMouseLeave
    '    If (Not Painting) Then
    '        Exit Sub
    '    End If

    '    If (e.RowIndex > -1) And (e.ColumnIndex > -1) Then
    '        CellPaint(e.RowIndex, e.ColumnIndex, SelectedColor, SelectedValue)
    '    End If
    'End Sub

    Private Sub grid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles grid.CellMouseUp
        Painting = False
    End Sub

#End Region

#End Region






End Class
