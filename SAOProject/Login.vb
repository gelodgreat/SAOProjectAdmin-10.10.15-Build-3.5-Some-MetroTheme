﻿Imports MySql.Data.MySqlClient
Imports MetroFramework
Public Class Login

    Dim mysqlconn As MySqlConnection
    Dim Command As MySqlCommand
    Dim a As Boolean
    Public usertype As String


    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Timer
        Timer_Login.Enabled = True

        'Online/Offline Status
        a = New Boolean
        a = False
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = connstring

        Try
            mysqlconn.Open()
            a = True
            mysqlconn.Close()
        Catch ex As Exception

        Finally
            mysqlconn.Dispose()
            If a = True Then
                label_status.Text = "Online"
                label_status.ForeColor = Color.Green
            Else
                label_status.Text = "Offline"
                label_status.ForeColor = Color.Red
            End If

        End Try
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
        'Automatic Enter
        AcceptButton = MetroButton1

    End Sub



    Private Sub Timer_Login_Tick(sender As Object, e As EventArgs) Handles Timer_Login.Tick
        lbl_time_login.Text = Date.Now.ToString("MMMM dd yyyy     hh:mm:ss tt")

    End Sub

    Private Sub MetroButton1_Click(sender As Object, e As EventArgs) Handles MetroButton1.Click

        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = connstring
        Command = New MySqlCommand
        Dim reader As MySqlDataReader


        Try
                mysqlconn.Open()
                Dim query As String
                query = "SELECT * FROM saoinfo.saouserinfo where UName='" & tb_username.Text & "' and Password='" & tb_password.Text & "' or id='" & tb_username.Text & "' "
                Command = New MySqlCommand(query, mysqlconn)
                reader = Command.ExecuteReader

                Dim count As Integer
                count = 0

                While reader.Read
                    count = count + 1
                End While


                If count = 1 Then

                If reader.GetString("usertype") = "Admin" Then
                        usertype = "Admin"

                        Me.Hide()
                        MetroMessageBox.Show(Me, "Entering as " & reader.GetString("fname"), "CEU Student Organization Record and Rating Forms Management System", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        TabMain.welcomeadmin.Text = "Welcome Admin, " & reader.GetString("fname") + " " + reader.GetString("lname")

                        TabMain.Show()
                        tb_password.Text = ""
                        tb_username.Text = ""

                        'Conditions
                        TabMain.GroupBoxEvent.Visible = False

                        SetAccess(usertype)
                        SetAccount(reader.GetString("lname") + ", " + reader.GetString("fname"))
                        'TabMain.Panel_Accounts.Enabled = False


                    ElseIf reader.GetString("usertype") = "SuperAdmin" Then
                        usertype = "SuperAdmin"
                        Me.Hide()
                        MetroMessageBox.Show(Me, "Entering as " & reader.GetString("fname"), "CEU Student Organization Record and Rating Forms Management System", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        TabMain.welcomeadmin.Text = "Welcome Super Admin, " & reader.GetString("fname") + " " + reader.GetString("lname")
                        SetAccess(usertype)
                        SetAccount(reader.GetString("lname") + ", " + reader.GetString("fname"))
                        TabMain.Show()
                        tb_password.Text = ""
                        tb_username.Text = ""

                        'Conditions
                        TabMain.GroupBoxEvent.Visible = False

                    ElseIf reader.GetString("usertype") = "Guest" Then
                        usertype = "Guest"
                        Me.Hide()
                        MetroMessageBox.Show(Me, "Entering as " & reader.GetString("fname"), "CEU Student Organization Record and Rating Forms Management System", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        GuestOnly.welcomeguest.Text = "Welcome Guest, " & reader.GetString("fname") + " " + reader.GetString("lname")

                        GuestOnly.Show()
                        ChangeSemester.btn_createSCYS.Visible = False
                        ChangeSemester.btn_deleteSCYS.Visible = False

                        SetAccess(usertype)
                        SetAccount(reader.GetString("lname") + ", " + reader.GetString("fname"))
                        tb_password.Text = ""
                        tb_username.Text = ""

                    End If

                Else

                    MetroMessageBox.Show(Me, "The username/password does not exist", "CEU Student Organization Record and Rating Forms Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop)

                    tb_password.Text = ""
                    tb_username.Text = ""

                End If

                mysqlconn.Close()

            Catch ex As MySqlException
                MessageBox.Show(ex.Message)

            Finally
                mysqlconn.Dispose()

            End Try




    End Sub

    Private Sub MetroLink1_Click(sender As Object, e As EventArgs) Handles MetroLink1.Click
        Application.Restart()
    End Sub

    Private Sub ml_exit_Click(sender As Object, e As EventArgs) Handles ml_exit.Click
        'EXIT CONTROL
        Dim a As Integer
        Me.TopMost = False
        a = MetroMessageBox.Show(Me, "Are you sure you want to exit?", "CEU Student Organization Record and Rating Forms Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Stop)

        If a = vbYes Then
            Application.ExitThread()

        Else
            Me.TopMost = True

        End If
    End Sub

    Private Sub ml_minimize_Click(sender As Object, e As EventArgs) Handles ml_minimize.Click
        Me.WindowState = FormWindowState.Minimized

    End Sub

    Private Sub mbtn_bypass_Click(sender As Object, e As EventArgs) Handles mbtn_bypass.Click
        usertype = "Admin"
        SetAccess(usertype)
        Hide()
        TabMain.Show()


    End Sub
End Class