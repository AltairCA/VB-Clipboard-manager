Imports System.IO
Public Class Form1
    Dim check As Boolean
    Dim check1 As Boolean
    Private Declare Function GetKeyPress Lib "user32" Alias "GetAsyncKeyState" (ByVal key As Integer) As Integer

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        
        'If GetKeyPress(Keys.ControlKey) And GetKeyPress(Keys.C) Then
        If Clipboard.ContainsFileDropList() Then
            Dim FileList As System.Collections.Specialized.StringCollection = Clipboard.GetFileDropList()
            Dim x As Integer = 0
            Dim y As Integer = System.Convert.ToInt32(FileList.Count)
            While (x < y)
                If Not ListBox1.Items.Contains(FileList(x)) Then

                    ListBox1.Items.Add(FileList(x))
                    ListBox2.Items.Add("Files")


                End If
                x = x + 1
            End While
        Else
            If Not ListBox1.Items.Contains(Clipboard.GetText) Then
                If Not (Clipboard.GetText = "") Then
                    ListBox2.Items.Add("Text")
                    ListBox1.Items.Add(Clipboard.GetText)
                End If

            End If

        End If
        'MsgBox("Enter was pressed yo")
        'End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            ListBox2.SetSelected(ListBox1.SelectedIndex, True)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()

    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        NotifyIcon1.Visible = True
        check = False
        check1 = False
    End Sub
    Public Function deletings()
        Try
            Dim index As Integer = ListBox1.SelectedIndex
            ListBox1.Items.RemoveAt(index)
            ListBox2.Items.RemoveAt(index)
            Clipboard.Clear()
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

        Return Nothing
    End Function
    Private Sub ListBox1_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyDown
        If e.KeyCode = Keys.Delete Then
            deletings()
        End If
        If e.KeyCode = Keys.Enter Then
            runs()
        End If
    End Sub

    Private Sub Form1_Resize(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize

    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As System.Object, e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        check1 = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        check = True
        Me.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        check1 = False
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If check = False Then
            e.Cancel = True
            Me.Hide()
            If check1 = False Then
                NotifyIcon1.BalloonTipText = "Clip Board Manger Running BackGround"
                NotifyIcon1.ShowBalloonTip(500)
            End If

            
        End If

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Clipboard.Clear()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
    End Sub

    Private Sub Form1_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        Me.Hide()
        NotifyIcon1.BalloonTipText = "Clip Board Manger Running BackGround"
        NotifyIcon1.ShowBalloonTip(500)
    End Sub
    Public Function copying()
        Try
            If Not ListBox1.Items.Count = 0 Or ListBox1.SelectedIndex < 0 Then
                ListBox2.SetSelected(ListBox1.SelectedIndex, True)
                If (ListBox2.SelectedItem.ToString = "Text") Then
                    Clipboard.Clear()
                    Clipboard.SetText(ListBox1.SelectedItem.ToString)
                Else

                    If My.Computer.FileSystem.FileExists(ListBox1.SelectedItem.ToString) Or My.Computer.FileSystem.DirectoryExists(ListBox1.SelectedItem.ToString) Then
                        Dim dataobject As New DataObject
                        Dim file1(0) As String
                        file1(0) = ListBox1.SelectedItem.ToString
                        dataobject.SetData(DataFormats.FileDrop, True, file1)
                        Clipboard.Clear()
                        Clipboard.SetDataObject(dataobject)
                    Else
                        MessageBox.Show("This file or Directory is not Exists", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Dim index As Integer = ListBox1.SelectedIndex
                        ListBox1.Items.RemoveAt(index)
                        ListBox2.Items.RemoveAt(index)
                        Clipboard.Clear()
                    End If


                End If
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
        Return Nothing
    End Function
    Private Sub ListBox1_DoubleClick(sender As System.Object, e As System.EventArgs) Handles ListBox1.DoubleClick
        copying()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Form2.ShowDialog()
    End Sub

    Private Sub ListBox1_Click(sender As System.Object, e As System.EventArgs) Handles ListBox1.Click


    End Sub
    Public Function checkcontext() As Boolean
        Try
            Dim sss As String
            sss = ListBox1.SelectedItem.ToString
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Sub ContextMenuStrip2_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening
        If Not (checkcontext()) Then
            e.Cancel = True
            Exit Sub
        End If
        If ListBox2.SelectedItem.ToString = "Text" Then
            ContextMenuStrip2.Items.Item(1).Enabled = False
            ContextMenuStrip2.Items.Item(3).Enabled = False
        Else
            ContextMenuStrip2.Items.Item(1).Enabled = True
            ContextMenuStrip2.Items.Item(3).Enabled = True
        End If


    End Sub
    Public Function opens()
        Try
            ListBox2.SetSelected(ListBox1.SelectedIndex, True)
            If ListBox2.SelectedItem.ToString = "Files" Then
                If My.Computer.FileSystem.FileExists(ListBox1.SelectedItem.ToString) Or My.Computer.FileSystem.DirectoryExists(ListBox1.SelectedItem.ToString) Then
                    Dim fullpath As String

                    fullpath = Path.GetDirectoryName(ListBox1.SelectedItem.ToString)

                    Process.Start("explorer.exe", fullpath)
                Else
                    MessageBox.Show("This file or Directory is not Exists", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Dim index As Integer = ListBox1.SelectedIndex
                    Dim str As String = ListBox1.SelectedItem.ToString
                    ListBox1.Items.RemoveAt(index)
                    ListBox2.Items.RemoveAt(index)
                    If Clipboard.ContainsFileDropList() Then
                        Dim FileList As System.Collections.Specialized.StringCollection = Clipboard.GetFileDropList()
                        Dim x As Integer = 0
                        Dim y As Integer = System.Convert.ToInt32(FileList.Count)
                        While (x < y)
                            If str = FileList(x) Then
                                Clipboard.Clear()
                            End If

                            x = x + 1
                        End While

                    End If
                End If
            Else
                MessageBox.Show("This is not a file or Directory", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'Clipboard.Clear()
            End If
        Catch ex As Exception

        End Try
        Return Nothing
    End Function
    Public Function runs()
        Try
            If Not ListBox1.Items.Count = 0 Or ListBox1.SelectedIndex < 0 Then
                ListBox2.SetSelected(ListBox1.SelectedIndex, True)
                If (ListBox2.SelectedItem.ToString = "Text") Then
                    Form3.TextBox1.Text = ListBox1.SelectedItem.ToString
                    Form3.ShowDialog()
                Else

                    If My.Computer.FileSystem.FileExists(ListBox1.SelectedItem.ToString) Or My.Computer.FileSystem.DirectoryExists(ListBox1.SelectedItem.ToString) Then
                        Dim p As New System.Diagnostics.Process
                        Dim full As String = Path.GetFullPath(ListBox1.SelectedItem.ToString)
                        Dim s As New System.Diagnostics.ProcessStartInfo(full)
                        s.UseShellExecute = True
                        s.WindowStyle = ProcessWindowStyle.Normal
                        p.StartInfo = s
                        p.Start()
                    Else
                        MessageBox.Show("This file or Directory is not Exists", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Dim index As Integer = ListBox1.SelectedIndex
                        ListBox1.Items.RemoveAt(index)
                        ListBox2.Items.RemoveAt(index)
                        Clipboard.Clear()
                    End If


                End If
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

        Return Nothing
    End Function
    Private Sub OpenToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem1.Click
        runs()
    End Sub

    Private Sub OpenLocationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenLocationToolStripMenuItem.Click
        opens()
    End Sub

    Private Sub CopyToClipboardToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CopyToClipboardToolStripMenuItem.Click
        copying()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        deletings()
    End Sub

    Private Sub CopyPathToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CopyPathToolStripMenuItem.Click


        Try
            If Not ListBox1.Items.Count = 0 Or ListBox1.SelectedIndex < 0 Then
                ListBox2.SetSelected(ListBox1.SelectedIndex, True)
                

                    If My.Computer.FileSystem.FileExists(ListBox1.SelectedItem.ToString) Or My.Computer.FileSystem.DirectoryExists(ListBox1.SelectedItem.ToString) Then
                       Clipboard.SetText(ListBox1.SelectedItem.ToString)
                    Else
                        MessageBox.Show("This file or Directory is not Exists", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Dim index As Integer = ListBox1.SelectedIndex
                        ListBox1.Items.RemoveAt(index)
                        ListBox2.Items.RemoveAt(index)
                        Clipboard.Clear()
                    End If


                End If



        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        If GetKeyPress(Keys.LControlKey) And GetKeyPress(Keys.LShiftKey) And GetKeyPress(Keys.D5) Then

            Me.Close()
            check1 = True
        End If
        If GetKeyPress(Keys.LControlKey) And GetKeyPress(Keys.LShiftKey) And GetKeyPress(Keys.D4) Then
            Me.Show()
            check1 = False
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub
End Class
