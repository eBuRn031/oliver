﻿Imports BE_LAVANDERIA
Imports DA_LAVANDERIA
Public Class FRM_SERVICIOS
    Dim CODIGO As String = Nothing
    Dim dts As New DataTable
    Private Sub BTN_NUEVO_Click(sender As Object, e As EventArgs) Handles BTN_NUEVO.Click
        FRM_INSERTAR_SERVICIOS.Show()
        Me.Close()
    End Sub

    Private Sub BTN_SALIR_Click(sender As Object, e As EventArgs) Handles BTN_SALIR.Click
        Me.Close()
    End Sub

    Private Sub DGV_SERVICIOS_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_SERVICIOS.CellEnter
        CODIGO = Me.DGV_SERVICIOS.CurrentRow.Cells.Item("CODIGO").Value.ToString()
    End Sub

    Sub CARGARDATAGRIDVIEW()
        Try
            Dim DACargarDataGridView As New DACargarDataGridView
            dts = DACargarDataGridView.Mostrar_Servicios()
            If dts.Rows.Count <> 0 Then
                DGV_SERVICIOS.DataSource = dts
            Else
                DGV_SERVICIOS.DataSource = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BTN_EDITAR_Click(sender As Object, e As EventArgs) Handles BTN_EDITAR.Click
        If CODIGO <> Nothing Then
            VARIABLES_GLOBALES.ID_SERVICIO = CODIGO
            FRM_INSERTAR_SERVICIOS.Show()
            Me.Close()
        End If
    End Sub

    Private Sub FRM_SERVICIOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CARGARDATAGRIDVIEW()
        DGV_SERVICIOS.Columns.Item(0).Visible = False
    End Sub

    Private Sub BTN_VER_Click(sender As Object, e As EventArgs) Handles BTN_VER.Click
        If CODIGO <> Nothing Then
            Dim FRM_INSERTAR_SERVICIOS As New FRM_INSERTAR_SERVICIOS
            VARIABLES_GLOBALES.ID_SERVICIO = CODIGO
            FRM_INSERTAR_SERVICIOS.BTN_NUEVO.Enabled = False
            FRM_INSERTAR_SERVICIOS.BTN_NUEVO.Visible = False
            FRM_INSERTAR_SERVICIOS.TXT_CODIGO.Enabled = False
            FRM_INSERTAR_SERVICIOS.TXT_NOMBRE.Enabled = False
            FRM_INSERTAR_SERVICIOS.TXT_PRECIO1.Enabled = False
            FRM_INSERTAR_SERVICIOS.TXT_PRECIO2.Enabled = False
            FRM_INSERTAR_SERVICIOS.Show()
            Me.Close()
        End If
    End Sub

    Private Sub BTN_ELIMINAR_Click(sender As Object, e As EventArgs) Handles BTN_ELIMINAR.Click
        If CODIGO <> Nothing Then
            If MsgBox("¿REALEMENTE QUIERE ELIMINAR EL SERVICIO?", MsgBoxStyle.YesNo) = vbYes Then
                Dim BEServicios As New BEServicios
                Dim DAServicios As New DAServicios

                BEServicios.gcod_serv = CODIGO
                BEServicios.gusu_eli = VARIABLES_GLOBALES.ID_USUARIO

                DAServicios.eliminar_Servicios(BEServicios)
                MsgBox("SERVICIO ELIMINADO CORRECTAMENTE...!")
                CARGARDATAGRIDVIEW()
            End If
        End If
    End Sub

    Sub CARGAR_SERVICIOS_NOMBRE()
        Try
            Dim DACargarDataGridView As New DACargarDataGridView
            dts = DACargarDataGridView.Mostrar_Servicios_Busqueda(TXT_NOMBRE.Text)
            If dts.Rows.Count <> 0 Then
                DGV_SERVICIOS.DataSource = dts
            Else
                DGV_SERVICIOS.DataSource = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub CARGAR_SERVICIOS_CODIGO()
        Try
            Dim DACargarDataGridView As New DACargarDataGridView
            dts = DACargarDataGridView.Mostrar_Servicios_CODIGO_Busqueda(TXT_NOMBRE.Text)
            If dts.Rows.Count <> 0 Then
                DGV_SERVICIOS.DataSource = dts
            Else
                DGV_SERVICIOS.DataSource = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BTN_BUSCAR_Click(sender As Object, e As EventArgs) Handles BTN_BUSCAR.Click

        If RB_CODIGO.Checked = True Then
            CARGAR_SERVICIOS_CODIGO()
        End If

        If RB_NOMBRE.Checked = True Then
            CARGAR_SERVICIOS_NOMBRE()
        End If

    End Sub

End Class