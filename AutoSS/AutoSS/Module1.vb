﻿Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Media.Imaging

Module Module1
    Private Prefix As String = ""
    Private Start As Boolean = False

    Sub Main()
        Console.WriteLine("Option...")
        Console.WriteLine("1. Setting Prefix")
        Console.WriteLine("2. Start")
Start:
        Console.Write("Choose Option : ")
        Select Case Console.ReadLine()
            Case "1"
                Console.Write("Insert Prefix : ")
                Prefix = Console.ReadLine()
                GoTo Start
            Case Else
                Start = True
                Console.Clear()
                CoreSS()
        End Select
        Dim debug = ""
    End Sub

    Private Sub CoreSS()
        While Start
            Try
                If DateTime.Now.Second = 0 OrElse DateTime.Now.Second = 30 Then
                    Dim fileName As String = Prefix & " " & DateTime.Now.Date.ToString("dd-MM-yyyy") & " Jam " & DateTime.Now.Hour & "." & DateTime.Now.Minute & ".png"
                    Dim ss As BitmapImage = ConvertBitmapImage(TakeScreenShot())
                    SavePngImage(ss, fileName)
                    Console.WriteLine(fileName)
                    Threading.Thread.Sleep(1000)
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End While
    End Sub

    Private Function TakeScreenShot() As Bitmap
        Dim screenSize As Size = New Size(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Dim screenGrab As New Bitmap(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Dim g As Graphics = Graphics.FromImage(screenGrab)
        g.CopyFromScreen(New Point(0, 0), New Point(0, 0), screenSize)
        Return screenGrab
    End Function

    Private Function ConvertBitmapImage(ByVal bitmap As Bitmap) As BitmapImage
        Dim bitmapImage = New BitmapImage()
        Using memory As MemoryStream = New MemoryStream()
            bitmap.Save(memory, ImageFormat.Png)
            memory.Position = 0
            bitmapImage.BeginInit()
            bitmapImage.StreamSource = memory
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad
            bitmapImage.EndInit()
            bitmapImage.Freeze()
        End Using
        Return bitmapImage
    End Function

    Private Sub SavePngImage(source As BitmapImage, photoLocation As String)
        SaveImage(source, photoLocation, New PngBitmapEncoder())
    End Sub

    Private Sub SaveImage(source As BitmapImage, photoLocation As String, encoder As BitmapEncoder)
        encoder.Frames.Add(BitmapFrame.Create(source))
        Using filestream = New FileStream(photoLocation, FileMode.Create)
            encoder.Save(filestream)
        End Using
    End Sub

End Module
