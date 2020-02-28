Namespace CalendarDemoVB

    Partial Class DemoForm
        Private components As System.ComponentModel.IContainer = Nothing

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim CalendarHighlightRange1 As System.Windows.Forms.Calendar.CalendarHighlightRange = New System.Windows.Forms.Calendar.CalendarHighlightRange
            Dim CalendarHighlightRange2 As System.Windows.Forms.Calendar.CalendarHighlightRange = New System.Windows.Forms.Calendar.CalendarHighlightRange
            Dim CalendarHighlightRange3 As System.Windows.Forms.Calendar.CalendarHighlightRange = New System.Windows.Forms.Calendar.CalendarHighlightRange
            Dim CalendarHighlightRange4 As System.Windows.Forms.Calendar.CalendarHighlightRange = New System.Windows.Forms.Calendar.CalendarHighlightRange
            Dim CalendarHighlightRange5 As System.Windows.Forms.Calendar.CalendarHighlightRange = New System.Windows.Forms.Calendar.CalendarHighlightRange
            Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.redTagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.yellowTagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.greenTagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.blueTagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.otherColorTagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
            Me.patternToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.diagonalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.verticalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.horizontalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.hatchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
            Me.noneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.timescaleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.hourToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.minutesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem
            Me.minutesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
            Me.minutesToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
            Me.minutesToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
            Me.selectImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.imageAlignmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.northToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.eastToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.southToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.westToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator
            Me.editItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.splitter1 = New System.Windows.Forms.Splitter
            Me.calendar1 = New System.Windows.Forms.Calendar.Calendar
            Me.monthView1 = New System.Windows.Forms.Calendar.MonthView
            Me.contextMenuStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'contextMenuStrip1
            '
            Me.contextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.redTagToolStripMenuItem, Me.yellowTagToolStripMenuItem, Me.greenTagToolStripMenuItem, Me.blueTagToolStripMenuItem, Me.otherColorTagToolStripMenuItem, Me.toolStripMenuItem1, Me.patternToolStripMenuItem, Me.timescaleToolStripMenuItem, Me.toolStripMenuItem2, Me.selectImageToolStripMenuItem, Me.imageAlignmentToolStripMenuItem, Me.toolStripMenuItem5, Me.editItemToolStripMenuItem})
            Me.contextMenuStrip1.Name = "contextMenuStrip1"
            Me.contextMenuStrip1.Size = New System.Drawing.Size(160, 242)
            '
            'redTagToolStripMenuItem
            '
            Me.redTagToolStripMenuItem.Name = "redTagToolStripMenuItem"
            Me.redTagToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.redTagToolStripMenuItem.Text = "Red tag"
            '
            'yellowTagToolStripMenuItem
            '
            Me.yellowTagToolStripMenuItem.Name = "yellowTagToolStripMenuItem"
            Me.yellowTagToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.yellowTagToolStripMenuItem.Text = "Yellow tag"
            '
            'greenTagToolStripMenuItem
            '
            Me.greenTagToolStripMenuItem.Name = "greenTagToolStripMenuItem"
            Me.greenTagToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.greenTagToolStripMenuItem.Text = "Green tag"
            '
            'blueTagToolStripMenuItem
            '
            Me.blueTagToolStripMenuItem.Name = "blueTagToolStripMenuItem"
            Me.blueTagToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.blueTagToolStripMenuItem.Text = "Blue tag"
            '
            'otherColorTagToolStripMenuItem
            '
            Me.otherColorTagToolStripMenuItem.Name = "otherColorTagToolStripMenuItem"
            Me.otherColorTagToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.otherColorTagToolStripMenuItem.Text = "Other color tag..."
            '
            'toolStripMenuItem1
            '
            Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
            Me.toolStripMenuItem1.Size = New System.Drawing.Size(156, 6)
            '
            'patternToolStripMenuItem
            '
            Me.patternToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.diagonalToolStripMenuItem, Me.verticalToolStripMenuItem, Me.horizontalToolStripMenuItem, Me.hatchToolStripMenuItem, Me.toolStripMenuItem3, Me.noneToolStripMenuItem})
            Me.patternToolStripMenuItem.Name = "patternToolStripMenuItem"
            Me.patternToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.patternToolStripMenuItem.Text = "Pattern"
            '
            'diagonalToolStripMenuItem
            '
            Me.diagonalToolStripMenuItem.Name = "diagonalToolStripMenuItem"
            Me.diagonalToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.diagonalToolStripMenuItem.Text = "Diagonal"
            '
            'verticalToolStripMenuItem
            '
            Me.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem"
            Me.verticalToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.verticalToolStripMenuItem.Text = "Vertical"
            '
            'horizontalToolStripMenuItem
            '
            Me.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem"
            Me.horizontalToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.horizontalToolStripMenuItem.Text = "Horizontal"
            '
            'hatchToolStripMenuItem
            '
            Me.hatchToolStripMenuItem.Name = "hatchToolStripMenuItem"
            Me.hatchToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.hatchToolStripMenuItem.Text = "Cross"
            '
            'toolStripMenuItem3
            '
            Me.toolStripMenuItem3.Name = "toolStripMenuItem3"
            Me.toolStripMenuItem3.Size = New System.Drawing.Size(119, 6)
            '
            'noneToolStripMenuItem
            '
            Me.noneToolStripMenuItem.Name = "noneToolStripMenuItem"
            Me.noneToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.noneToolStripMenuItem.Text = "None"
            '
            'timescaleToolStripMenuItem
            '
            Me.timescaleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.hourToolStripMenuItem, Me.minutesToolStripMenuItem, Me.toolStripMenuItem4, Me.minutesToolStripMenuItem1, Me.minutesToolStripMenuItem2, Me.minutesToolStripMenuItem3})
            Me.timescaleToolStripMenuItem.Name = "timescaleToolStripMenuItem"
            Me.timescaleToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.timescaleToolStripMenuItem.Text = "Timescale"
            '
            'hourToolStripMenuItem
            '
            Me.hourToolStripMenuItem.Name = "hourToolStripMenuItem"
            Me.hourToolStripMenuItem.Size = New System.Drawing.Size(126, 22)
            Me.hourToolStripMenuItem.Text = "1 hour"
            '
            'minutesToolStripMenuItem
            '
            Me.minutesToolStripMenuItem.Name = "minutesToolStripMenuItem"
            Me.minutesToolStripMenuItem.Size = New System.Drawing.Size(126, 22)
            Me.minutesToolStripMenuItem.Text = "30 minutes"
            '
            'toolStripMenuItem4
            '
            Me.toolStripMenuItem4.Name = "toolStripMenuItem4"
            Me.toolStripMenuItem4.Size = New System.Drawing.Size(126, 22)
            Me.toolStripMenuItem4.Text = "15 minutes"
            '
            'minutesToolStripMenuItem1
            '
            Me.minutesToolStripMenuItem1.Name = "minutesToolStripMenuItem1"
            Me.minutesToolStripMenuItem1.Size = New System.Drawing.Size(126, 22)
            Me.minutesToolStripMenuItem1.Text = "10 minutes"
            '
            'minutesToolStripMenuItem2
            '
            Me.minutesToolStripMenuItem2.Name = "minutesToolStripMenuItem2"
            Me.minutesToolStripMenuItem2.Size = New System.Drawing.Size(126, 22)
            Me.minutesToolStripMenuItem2.Text = "6 minutes"
            '
            'minutesToolStripMenuItem3
            '
            Me.minutesToolStripMenuItem3.Name = "minutesToolStripMenuItem3"
            Me.minutesToolStripMenuItem3.Size = New System.Drawing.Size(126, 22)
            Me.minutesToolStripMenuItem3.Text = "5 minutes"
            '
            'toolStripMenuItem2
            '
            Me.toolStripMenuItem2.Name = "toolStripMenuItem2"
            Me.toolStripMenuItem2.Size = New System.Drawing.Size(156, 6)
            '
            'selectImageToolStripMenuItem
            '
            Me.selectImageToolStripMenuItem.Name = "selectImageToolStripMenuItem"
            Me.selectImageToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.selectImageToolStripMenuItem.Text = "Select Image..."
            '
            'imageAlignmentToolStripMenuItem
            '
            Me.imageAlignmentToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.northToolStripMenuItem, Me.eastToolStripMenuItem, Me.southToolStripMenuItem, Me.westToolStripMenuItem})
            Me.imageAlignmentToolStripMenuItem.Name = "imageAlignmentToolStripMenuItem"
            Me.imageAlignmentToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.imageAlignmentToolStripMenuItem.Text = "Image Alignment"
            '
            'northToolStripMenuItem
            '
            Me.northToolStripMenuItem.Name = "northToolStripMenuItem"
            Me.northToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
            Me.northToolStripMenuItem.Text = "North"
            '
            'eastToolStripMenuItem
            '
            Me.eastToolStripMenuItem.Name = "eastToolStripMenuItem"
            Me.eastToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
            Me.eastToolStripMenuItem.Text = "East"
            '
            'southToolStripMenuItem
            '
            Me.southToolStripMenuItem.Name = "southToolStripMenuItem"
            Me.southToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
            Me.southToolStripMenuItem.Text = "South"
            '
            'westToolStripMenuItem
            '
            Me.westToolStripMenuItem.Name = "westToolStripMenuItem"
            Me.westToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
            Me.westToolStripMenuItem.Text = "West"
            '
            'toolStripMenuItem5
            '
            Me.toolStripMenuItem5.Name = "toolStripMenuItem5"
            Me.toolStripMenuItem5.Size = New System.Drawing.Size(156, 6)
            '
            'editItemToolStripMenuItem
            '
            Me.editItemToolStripMenuItem.Name = "editItemToolStripMenuItem"
            Me.editItemToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.editItemToolStripMenuItem.Text = "Edit item's text"
            '
            'splitter1
            '
            Me.splitter1.Location = New System.Drawing.Point(196, 0)
            Me.splitter1.Name = "splitter1"
            Me.splitter1.Size = New System.Drawing.Size(6, 368)
            Me.splitter1.TabIndex = 4
            Me.splitter1.TabStop = False
            '
            'calendar1
            '
            Me.calendar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
            Me.calendar1.ContextMenuStrip = Me.contextMenuStrip1
            Me.calendar1.DefaultStartTime = System.TimeSpan.Parse("08:30:00")
            Me.calendar1.Dock = System.Windows.Forms.DockStyle.Fill
            CalendarHighlightRange1.DayOfWeek = System.DayOfWeek.Monday
            CalendarHighlightRange1.EndTime = System.TimeSpan.Parse("17:00:00")
            CalendarHighlightRange1.StartTime = System.TimeSpan.Parse("08:00:00")
            CalendarHighlightRange2.DayOfWeek = System.DayOfWeek.Tuesday
            CalendarHighlightRange2.EndTime = System.TimeSpan.Parse("17:00:00")
            CalendarHighlightRange2.StartTime = System.TimeSpan.Parse("08:00:00")
            CalendarHighlightRange3.DayOfWeek = System.DayOfWeek.Wednesday
            CalendarHighlightRange3.EndTime = System.TimeSpan.Parse("17:00:00")
            CalendarHighlightRange3.StartTime = System.TimeSpan.Parse("08:00:00")
            CalendarHighlightRange4.DayOfWeek = System.DayOfWeek.Thursday
            CalendarHighlightRange4.EndTime = System.TimeSpan.Parse("17:00:00")
            CalendarHighlightRange4.StartTime = System.TimeSpan.Parse("08:00:00")
            CalendarHighlightRange5.DayOfWeek = System.DayOfWeek.Friday
            CalendarHighlightRange5.EndTime = System.TimeSpan.Parse("17:00:00")
            CalendarHighlightRange5.StartTime = System.TimeSpan.Parse("08:00:00")
            Me.calendar1.HighlightRanges = New System.Windows.Forms.Calendar.CalendarHighlightRange() {CalendarHighlightRange1, CalendarHighlightRange2, CalendarHighlightRange3, CalendarHighlightRange4, CalendarHighlightRange5}
            Me.calendar1.Location = New System.Drawing.Point(202, 0)
            Me.calendar1.Name = "calendar1"
            Me.calendar1.Size = New System.Drawing.Size(581, 368)
            Me.calendar1.TabIndex = 2
            Me.calendar1.Text = "calendar1"
            '
            'monthView1
            '
            Me.monthView1.Dock = System.Windows.Forms.DockStyle.Left
            Me.monthView1.ItemPadding = New System.Windows.Forms.Padding(1, 2, 1, 2)
            Me.monthView1.Location = New System.Drawing.Point(0, 0)
            Me.monthView1.MaxSelectionCount = 35
            Me.monthView1.Name = "monthView1"
            Me.monthView1.Size = New System.Drawing.Size(196, 368)
            Me.monthView1.TabIndex = 3
            Me.monthView1.Text = "monthView1"
            '
            'DemoForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(783, 368)
            Me.Controls.Add(Me.calendar1)
            Me.Controls.Add(Me.splitter1)
            Me.Controls.Add(Me.monthView1)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
            Me.Name = "DemoForm"
            Me.Text = "Calendar Demo"
            Me.contextMenuStrip1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents calendar1 As System.Windows.Forms.Calendar.Calendar
        Private WithEvents contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
        Private WithEvents redTagToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents yellowTagToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents greenTagToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents blueTagToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents toolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Private WithEvents timescaleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents hourToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents minutesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents toolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents toolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
        Private WithEvents editItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents minutesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents minutesToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents minutesToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents otherColorTagToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents patternToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents diagonalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents verticalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents horizontalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents hatchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents toolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
        Private WithEvents noneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents monthView1 As System.Windows.Forms.Calendar.MonthView
        Private WithEvents splitter1 As System.Windows.Forms.Splitter
        Private WithEvents toolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
        Private WithEvents selectImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents imageAlignmentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents northToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents eastToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents southToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents westToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    End Class
End Namespace
