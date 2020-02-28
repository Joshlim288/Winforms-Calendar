Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Windows.Forms.Calendar
Imports System.Xml.Serialization
Imports System.IO

Namespace CalendarDemoVB

    Partial Public Class DemoForm
        Inherits Form
        Private _items As New List(Of CalendarItem)()
        Private contextItem As CalendarItem = Nothing
        Public Sub New()
            InitializeComponent()
            If Me.calendar1.RendererMode = RendererModes.Professional Then
                monthView1.MonthTitleTextColor = Color.Navy
                monthView1.MonthTitleColor = CalendarColorTable.FromHex("#C2DAFC")
                monthView1.ArrowsColor = CalendarColorTable.FromHex("#77A1D3")
                monthView1.DaySelectedBackgroundColor = CalendarColorTable.FromHex("#F4CC52")
                monthView1.DaySelectedTextColor = monthView1.ForeColor
            End If
        End Sub

        Public ReadOnly Property ItemsFile() As FileInfo
            Get
                Return New FileInfo(Path.Combine(Application.StartupPath, "..\\..\\..\\items.xml"))
            End Get
        End Property

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            If ItemsFile.Exists Then

                Dim lst As New List(Of ItemInfo)
                Dim xml As New XmlSerializer(lst.GetType())
                Using s As Stream = ItemsFile.OpenRead()
                    lst = TryCast(xml.Deserialize(s), List(Of ItemInfo))
                End Using

                For Each item As ItemInfo In lst
                    Dim cal As New CalendarItem(calendar1, item.StartTime, item.EndTime, item.Text)
                    cal.BackColor = Color.FromArgb(item.A, item.R, item.G, item.B)
                    _items.Add(cal)
                Next
                PlaceItems()
            End If
        End Sub
        Private Sub calendar1_LoadItems(ByVal sender As Object, ByVal e As CalendarLoadEventArgs) Handles calendar1.LoadItems
            PlaceItems()
        End Sub
        Private Sub PlaceItems()
            For Each item As CalendarItem In _items
                If calendar1.ViewIntersects(item) Then
                    calendar1.Items.Add(item)
                End If
            Next
        End Sub
        Private Sub calendar1_ItemCreated(ByVal sender As Object, ByVal e As CalendarItemCancelEventArgs) Handles calendar1.ItemCreated
            _items.Add(e.Item)
        End Sub
        Private Sub calendar1_ItemMouseHover(ByVal sender As Object, ByVal e As CalendarItemEventArgs) Handles calendar1.ItemMouseHover
            Text = e.Item.Subject
        End Sub
        Private Sub calendar1_ItemClick(ByVal sender As Object, ByVal e As CalendarItemEventArgs) Handles calendar1.ItemClick
        End Sub
        Private Sub hourToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hourToolStripMenuItem.Click
            calendar1.TimeScale = TimeScales.SixtyMinutes
        End Sub
        Private Sub minutesToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles minutesToolStripMenuItem1.Click, minutesToolStripMenuItem.Click
            calendar1.TimeScale = TimeScales.ThirtyMinutes
        End Sub
        Private Sub toolStripMenuItem4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles toolStripMenuItem4.Click
            calendar1.TimeScale = TimeScales.FifteenMinutes
        End Sub
        Private Sub minutesToolStripMenuItem2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles minutesToolStripMenuItem2.Click
            calendar1.TimeScale = TimeScales.SixMinutes
        End Sub
        Private Sub minutesToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As EventArgs)
            calendar1.TimeScale = TimeScales.TenMinutes
        End Sub
        Private Sub minutesToolStripMenuItem3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles minutesToolStripMenuItem3.Click
            calendar1.TimeScale = TimeScales.FiveMinutes
        End Sub
        Private Sub contextMenuStrip1_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles contextMenuStrip1.Opening
            contextItem = calendar1.ItemAt(contextMenuStrip1.Bounds.Location)
        End Sub
        Private Sub redTagToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles redTagToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.BackColor = Color.Red
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub yellowTagToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles yellowTagToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.BackColor = Color.Gold
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub greenTagToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles greenTagToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.BackColor = Color.Green
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub blueTagToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles blueTagToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.BackColor = Color.DarkBlue
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub editItemToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editItemToolStripMenuItem.Click
            calendar1.ActivateEditMode()
        End Sub

        Private Sub DemoForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            Dim lst As New List(Of ItemInfo)

            For Each item As CalendarItem In _items
                Dim ifo As New ItemInfo()
                ifo.StartTime = item.StartDate
                ifo.EndTime = item.EndDate
                ifo.Text = item.Subject
                ifo.A = item.BackColor.A
                ifo.R = item.BackColor.R
                ifo.G = item.BackColor.G
                ifo.B = item.BackColor.B
                lst.Add(ifo)
            Next

            Dim xml As New XmlSerializer(lst.GetType())
            If ItemsFile.Exists Then
                ItemsFile.Delete()
            End If

            Using s As FileStream = ItemsFile.OpenWrite()
                xml.Serialize(s, lst)
                s.Close()
            End Using

        End Sub

        Private Sub otherColorTagToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles otherColorTagToolStripMenuItem.Click
            Using dlg As New ColorDialog()
                If dlg.ShowDialog() = DialogResult.OK Then
                    For Each item As CalendarItem In calendar1.SelectedItems
                        item.BackColor = dlg.Color
                        calendar1.Invalidate(item)
                    Next
                End If
            End Using
        End Sub
        Private Sub calendar1_ItemDoubleClick(ByVal sender As Object, ByVal e As CalendarItemEventArgs) Handles calendar1.ItemDoubleClick
            MessageBox.Show("Double click: " + e.Item.Subject)
        End Sub
        Private Sub calendar1_ItemDeleted(ByVal sender As Object, ByVal e As CalendarItemEventArgs) Handles calendar1.ItemDeleted
            _items.Remove(e.Item)
        End Sub
        Private Sub calendar1_DayHeaderClick(ByVal sender As Object, ByVal e As CalendarDayEventArgs) Handles calendar1.DayHeaderClick
            calendar1.SetViewRange(e.CalendarDay.[Date], e.CalendarDay.[Date])
        End Sub
        Private Sub diagonalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles diagonalToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal
                item.PatternColor = Color.Red
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub verticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles verticalToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.Vertical
                item.PatternColor = Color.Red
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub horizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles horizontalToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.Horizontal
                item.PatternColor = Color.Red
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub hatchToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hatchToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.DiagonalCross
                item.PatternColor = Color.Red
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub noneToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles noneToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.DiagonalCross
                item.PatternColor = Color.Empty
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub monthView1_SelectionChanged(ByVal sender As Object, ByVal e As DateRangeChangedEventArgs) Handles monthView1.SelectionChanged
            calendar1.SetViewRange(e.Start, e.[End])
        End Sub
        Private Sub northToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles northToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.ImageAlign = ItemImageAligns.North
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub eastToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles eastToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.ImageAlign = ItemImageAligns.East
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub southToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles southToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.ImageAlign = ItemImageAligns.South
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub westToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles westToolStripMenuItem.Click
            For Each item As CalendarItem In calendar1.SelectedItems
                item.ImageAlign = ItemImageAligns.West
                calendar1.Invalidate(item)
            Next
        End Sub
        Private Sub selectImageToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles selectImageToolStripMenuItem.Click
            Using dlg As New OpenFileDialog()
                dlg.Filter = "*.gif|*.gif|*.png|*.png|*.jpg|*.jpg"
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    Dim img As Image = Image.FromFile(dlg.FileName)
                    For Each item As CalendarItem In calendar1.SelectedItems
                        item.Image = img
                        calendar1.Invalidate(item)
                    Next
                End If
            End Using
        End Sub
        Private Sub calendar1_ItemDatesChanged(ByVal sender As Object, ByVal e As CalendarItemEventArgs) Handles calendar1.ItemDatesChanged
        End Sub
    End Class
End Namespace
