Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace CalendarDemoVB

    Public Class ItemInfo
        Public StartTime As DateTime
        Public EndTime As DateTime
        Public Text As String
        Public A As Integer
        Public R As Integer
        Public G As Integer
        Public B As Integer

        Private pattern As HatchStyle
        Private patternColor As Color


        Public Sub New(ByVal startTime As DateTime, ByVal endTime As DateTime, ByVal text As String, ByVal color As Color)
            StartTime = startTime
            EndTime = endTime
            Text = text
            A = color.A
            R = color.R
            G = color.G
            B = color.B
        End Sub

        Public Sub New()
        End Sub

    End Class
End Namespace
