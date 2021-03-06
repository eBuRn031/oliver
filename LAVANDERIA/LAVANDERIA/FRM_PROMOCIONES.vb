﻿Imports BE_LAVANDERIA
Imports DA_LAVANDERIA
Public Class FRM_PROMOCIONES
    Dim dts As New DataTable
    Dim CODIGO As Integer = Nothing
    Dim i As Integer = Nothing

    Private Sub PROMOCIONES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim DAConsultas_Base As New DAConsultas_Base
        Dim DACargar_Combobox As New DACargar_Combobox
        Dim DAConsultas_Completas_Base As New DAConsultas_Completas_Base
        DACargar_Combobox.cargarServicio(CB_SERVICIO)
        DACargar_Combobox.cargarServicio(CB_SERVICIO_BONO)
        DACargar_Combobox.cargarSucursal(CB_SUCURSAL)


        If VARIABLES_GLOBALES.ID_PROMOCION = Nothing Then
            Dim VARIABLE1 As String
            VARIABLE1 = DAConsultas_Base.mostrarCod_Promocion_Ultima()
            VARIABLE1 = Microsoft.VisualBasic.Right(VARIABLE1, 3)
            VARIABLE1 = Integer.Parse(VARIABLE1) + 1
            If VARIABLE1.Length = 1 Then VARIABLE1 = "00" & VARIABLE1
            If VARIABLE1.Length = 2 Then VARIABLE1 = "0" & VARIABLE1
            VARIABLE1 = "P" & VARIABLE1
            TXT_CODIGO.Text = VARIABLE1
        Else
            DGVPROMOCIONES.Columns.Clear()
            DAConsultas_Completas_Base.cargarPromocion(VARIABLES_GLOBALES.ID_PROMOCION, TXT_CODIGO, TXT_DESCRIPCION, CB_SUCURSAL, DTP_FECHA_INI, DTP_FECHA_FIN)
            CARGARDATAGRIDVIEW()
        End If

        If DGVPROMOCIONES.Columns.Count <> 0 Then
            DGVPROMOCIONES.Columns.Item(0).Visible = False
        End If

    End Sub


    Private Sub BTN_AGREGAR_Click(sender As Object, e As EventArgs) Handles BTN_AGREGAR.Click
        If DGVPROMOCIONES.Columns.Count <> Nothing Then
            Dim dt As DataTable = DirectCast(DGVPROMOCIONES.DataSource, DataTable)

            ' Creamos una nueva fila u objeto DataRow.
            '
            Dim row As DataRow = dt.NewRow()

            ' Especificamos los valores de las columnas.

            i += 1
            row(DGVPROMOCIONES.Columns.Item(0).Name) = i
            row(DGVPROMOCIONES.Columns.Item(1).Name) = CB_SERVICIO.SelectedValue
            row(DGVPROMOCIONES.Columns.Item(2).Name) = CB_SERVICIO.Text
            row(DGVPROMOCIONES.Columns.Item(3).Name) = NUD_CANTIDAD_SERVICIO.Value
            row(DGVPROMOCIONES.Columns.Item(4).Name) = CB_SERVICIO_BONO.SelectedValue
            row(DGVPROMOCIONES.Columns.Item(5).Name) = CB_SERVICIO_BONO.Text
            row(DGVPROMOCIONES.Columns.Item(6).Name) = NUD_CANTIDAD_SERVICIO_BONO.Value

            ' Añadimos la fila al objeto DataTable y automáticamente
            ' se añadirá al control DataGridView enlazado.
            '
            dt.Rows.Add(row)
            DGVPROMOCIONES.DataSource = dt

            ' DGVPROMOCIONES.Rows.Add(CB_SERVICIO.SelectedValue, CB_SERVICIO.Text, NUD_CANTIDAD_SERVICIO.Value, CB_SERVICIO_BONO.SelectedValue, CB_SERVICIO_BONO.Text, NUD_CANTIDAD_SERVICIO_BONO.Value)
            CB_SERVICIO.SelectedIndex = 0
            CB_SERVICIO_BONO.SelectedIndex = 0
            NUD_CANTIDAD_SERVICIO.Value = 0
            NUD_CANTIDAD_SERVICIO_BONO.Value = 0
        Else
            DGVPROMOCIONES2.Enabled = True
            DGVPROMOCIONES2.Visible = True
            DGVPROMOCIONES.Enabled = False
            DGVPROMOCIONES.Visible = False
            DGVPROMOCIONES2.Rows.Add(CB_SERVICIO.SelectedValue, CB_SERVICIO.Text, NUD_CANTIDAD_SERVICIO.Value, CB_SERVICIO_BONO.SelectedValue, CB_SERVICIO_BONO.Text, NUD_CANTIDAD_SERVICIO_BONO.Value)
            CB_SERVICIO.SelectedIndex = 0
            CB_SERVICIO_BONO.SelectedIndex = 0
            NUD_CANTIDAD_SERVICIO.Value = 0
            NUD_CANTIDAD_SERVICIO_BONO.Value = 0
        End If

    End Sub

    Private Sub DGVPROMOCIONES_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVPROMOCIONES.CellEnter
        If VARIABLES_GLOBALES.ID_PROMOCION <> Nothing Then
            If DGVPROMOCIONES.Columns.Count <> Nothing Then
                CODIGO = Me.DGVPROMOCIONES.CurrentRow.Cells.Item("CODIGO").Value.ToString
            End If
        End If
    End Sub

    Sub CARGARDATAGRIDVIEW()
        Try
            Dim DACargarDataGridView As New DACargarDataGridView
            dts = DACargarDataGridView.Mostrar_Promocion_Detalle(VARIABLES_GLOBALES.ID_PROMOCION)
            If dts.Rows.Count <> 0 Then
                DGVPROMOCIONES.DataSource = dts
            Else
                DGVPROMOCIONES.DataSource = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BTN_SALIR_Click(sender As Object, e As EventArgs) Handles BTN_SALIR.Click
        VARIABLES_GLOBALES.ID_PROMOCION = Nothing
        FRM_PROMOCIONES_D.Show()
        Me.Close()
    End Sub

    Private Sub BTN_ACEPTAR_Click(sender As Object, e As EventArgs) Handles BTN_ACEPTAR.Click
        Dim BEPromocion As New BEPromocion
        Dim BEPromocion_Det As New BEPromocion_Det
        Dim DAPromociones As New DAPromociones
        Dim DAPromocion_Det As New DAPromocion_Det
        Dim DAConsultas_Base As New DAConsultas_Base

        If VARIABLES_GLOBALES.ID_PROMOCION = Nothing Then
            BEPromocion.gcod_promo = TXT_CODIGO.Text
            BEPromocion.gdes_promo = TXT_DESCRIPCION.Text
            BEPromocion.gid_suculsal = CB_SUCURSAL.SelectedValue
            BEPromocion.gfec_ini = Convert.ToDateTime(DTP_FECHA_INI.Value).ToString("yyyy-MM-dd HH:mm:ss")
            BEPromocion.gfec_fin = Convert.ToDateTime(DTP_FECHA_FIN.Value).ToString("yyyy-MM-dd HH:mm:ss")
            BEPromocion.gusu_reg = VARIABLES_GLOBALES.ID_USUARIO

            If DAPromociones.insertar_Promociones(BEPromocion) Then
            Else
                MsgBox("ERROR AL INSERTAR LA PROMOCION...")
            End If

            Dim ID_PROMOCION_CONSULTA As Integer = DAConsultas_Base.mostrarId_Promocion_Ultima()

            For n = 0 To DGVPROMOCIONES.Rows.Count - 1
                BEPromocion_Det.gid_serv = DGVPROMOCIONES.Item(0, n).Value
                BEPromocion_Det.gcantserv = DGVPROMOCIONES.Item(2, n).Value
                BEPromocion_Det.gid_servbono = DGVPROMOCIONES.Item(3, n).Value
                BEPromocion_Det.gcantbono = DGVPROMOCIONES.Item(5, n).Value
                BEPromocion_Det.gid_promocion = ID_PROMOCION_CONSULTA
                If DAPromocion_Det.insertar_Promociones_Det(BEPromocion_Det) Then
                Else
                    MsgBox("ERROR AL INSERTAR LA PROMOCION...")
                End If
            Next

            MsgBox("PROMOCION REGISTRADO CORRECTAMENTE...!!")
        Else

            BEPromocion.gid_promocion = VARIABLES_GLOBALES.ID_PROMOCION
            BEPromocion.gcod_promo = TXT_CODIGO.Text
            BEPromocion.gdes_promo = TXT_DESCRIPCION.Text
            BEPromocion.gid_suculsal = CB_SUCURSAL.SelectedValue
            BEPromocion.gfec_ini = Convert.ToDateTime(DTP_FECHA_INI.Value).ToString("yyyy-MM-dd HH:mm:ss")
            BEPromocion.gfec_fin = Convert.ToDateTime(DTP_FECHA_FIN.Value).ToString("yyyy-MM-dd HH:mm:ss")
            BEPromocion.gusu_mod = VARIABLES_GLOBALES.ID_USUARIO

            If DAPromociones.actualizar_Promociones(BEPromocion) Then
            Else
                MsgBox("ERROR AL ACTUALIZAR LA PROMOCION...")
            End If

            For n = 0 To DGVPROMOCIONES.Rows.Count - 1
                If DGVPROMOCIONES.Item(0, n).Value <> Nothing Then
                    If DAConsultas_Base.mostrarId_Promocion_Detalle(DGVPROMOCIONES.Item(0, n).Value) <> Nothing Then
                        BEPromocion_Det.gid_serv = DGVPROMOCIONES.Item(0, n).Value
                        BEPromocion_Det.gcantserv = DGVPROMOCIONES.Item(2, n).Value
                        BEPromocion_Det.gid_servbono = DGVPROMOCIONES.Item(3, n).Value
                        BEPromocion_Det.gcantbono = DGVPROMOCIONES.Item(5, n).Value
                        BEPromocion_Det.gid_promocion = VARIABLES_GLOBALES.ID_PROMOCION
                        If DAPromocion_Det.insertar_Promociones_Det(BEPromocion_Det) Then
                        Else
                            MsgBox("ERROR AL ACTUALIZAR LA PROMOCION...")
                        End If
                    Else
                        If DGVPROMOCIONES2.Enabled = True Then
                            BEPromocion_Det.gid_serv = DGVPROMOCIONES2.Item(0, n).Value
                            BEPromocion_Det.gcantserv = DGVPROMOCIONES2.Item(2, n).Value
                            BEPromocion_Det.gid_servbono = DGVPROMOCIONES2.Item(3, n).Value
                            BEPromocion_Det.gcantbono = DGVPROMOCIONES2.Item(5, n).Value
                            BEPromocion_Det.gid_promocion = VARIABLES_GLOBALES.ID_PROMOCION
                            If DAPromocion_Det.insertar_Promociones_Det(BEPromocion_Det) Then
                            Else
                                MsgBox("ERROR AL ACTUALIZAR LA PROMOCION...")
                            End If
                        End If
                    End If
                End If
            Next
            MsgBox("PROMOCION ACTUALIZADO CORRECTAMENTE...!!")
        End If

        VARIABLES_GLOBALES.ID_PROMOCION = Nothing
        FRM_PROMOCIONES_D.Show()
        Me.Close()

    End Sub


    Private Sub BTN_ELIMINAR_Click(sender As Object, e As EventArgs) Handles BTN_ELIMINAR.Click
        If CODIGO <> Nothing Then
            If MsgBox("¿REALMENTE DESEAS ELIMINAR ESTA PROMOCION?", MsgBoxStyle.YesNo) = vbYes Then

                Dim BEPromocion_Det As New BEPromocion_Det
                Dim DAPromocion_Det As New DAPromocion_Det

                BEPromocion_Det.gid_promodet = CODIGO
                DAPromocion_Det.eliminar_Promociones_Det(BEPromocion_Det)
                MsgBox("PROMOCION ELIMINADO CORRECTAMENTE...!!")
                CARGARDATAGRIDVIEW()
            End If
        End If
    End Sub

    Sub LIMPIAR()
        TXT_CODIGO.Text = ""
        TXT_DESCRIPCION.Text = ""
        CB_SERVICIO.SelectedIndex = 0
        CB_SERVICIO_BONO.SelectedIndex = 0
        CB_SUCURSAL.SelectedIndex = 0
        NUD_CANTIDAD_SERVICIO.Value = 0
        NUD_CANTIDAD_SERVICIO_BONO.Value = 0
        DTP_FECHA_INI.Value = Now
        DTP_FECHA_FIN.Value = Now
        DGVPROMOCIONES.Rows.Clear()
    End Sub

End Class