using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Calendar;
using System.Xml.Serialization;
using System.IO;

namespace CalendarDemo
{
    public partial class DemoForm : Form
    {
        List<CalendarItem> _items = new List<CalendarItem>();
        CalendarItem contextItem = null;

        public DemoForm()
        {
            InitializeComponent();

            //Monthview colors
            if (this.calendar1.RendererMode == RendererModes.Professional)
            {
                monthView1.MonthTitleTextColor = Color.Navy;
                monthView1.MonthTitleColor = CalendarColorTable.FromHex("#C2DAFC");
                monthView1.ArrowsColor = CalendarColorTable.FromHex("#77A1D3");
                monthView1.DaySelectedBackgroundColor = CalendarColorTable.FromHex("#F4CC52");
                monthView1.DaySelectedTextColor = monthView1.ForeColor;
            }
        }

        public FileInfo ItemsFile
        {
            get 
            {
                return new FileInfo(Path.Combine(Application.StartupPath, "..\\..\\..\\items.xml"));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ItemsFile.Exists)
            {
                List<ItemInfo> lst = new List<ItemInfo>();
                XmlSerializer xml = new XmlSerializer(lst.GetType());

                using (Stream s = ItemsFile.OpenRead())
                {
                    lst = xml.Deserialize(s) as List<ItemInfo>;
                }

                foreach (ItemInfo item in lst)
                {
                    CalendarItem cal = new CalendarItem(calendar1, item.StartTime, item.EndTime, item.Text);

                    cal.BackColor = Color.FromArgb(item.A, item.R, item.G, item.B);
                    //if (!(item.R == 0 && item.G == 0 && item.B == 0))
                    //{
                    //    cal.BackColor = Color.FromArgb(item.A, item.R, item.G, item.B));
                    //}

                    _items.Add(cal);
                }

                PlaceItems();
            }
        }

        private void calendar1_LoadItems(object sender, CalendarLoadEventArgs e)
        {
            PlaceItems();
        }

        private void PlaceItems()
        {
            foreach (CalendarItem item in _items)
            {
                if (calendar1.ViewIntersects(item))
                {
                    calendar1.Items.Add(item);
                }
            }
        }

        private void calendar1_ItemCreated(object sender, CalendarItemCancelEventArgs e)
        {
            _items.Add(e.Item);
        }

        private void calendar1_ItemMouseHover(object sender, CalendarItemEventArgs e)
        {
            Text = e.Item.Subject;
        }

        private void calendar1_ItemClick(object sender, CalendarItemEventArgs e)
        {
            //MessageBox.Show(e.Item.Text);
        }

        private void hourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = TimeScales.SixtyMinutes;
        }

        private void minutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = TimeScales.ThirtyMinutes;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = TimeScales.FifteenMinutes;
        }

        private void minutesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = TimeScales.SixMinutes;
        }

        private void minutesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = TimeScales.TenMinutes;
        }

        private void minutesToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = TimeScales.FiveMinutes;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextItem = calendar1.ItemAt(contextMenuStrip1.Bounds.Location);
        }

        private void redTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (CalendarItem item in calendar1.SelectedItems)
            //{
            //    item.BackColor = Color.Red);
            //    calendar1.Invalidate(item);
            //}

            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.BackColor = Color.Red;
                calendar1.Invalidate(item);
            }
        }

        private void yellowTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.BackColor = Color.Gold;
                calendar1.Invalidate(item);
            }
        }

        private void greenTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.BackColor = Color.Green;
                calendar1.Invalidate(item);
            }
        }

        private void blueTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.BackColor = Color.DarkBlue;
                calendar1.Invalidate(item);
            }
        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.ActivateEditMode();
        }

        private void DemoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<ItemInfo> lst = new List<ItemInfo>();
            
            foreach (CalendarItem item in _items)
            {
                lst.Add(new ItemInfo(item.StartDate, item.EndDate, item.Subject, item.BackColor));
            }

            XmlSerializer xmls = new XmlSerializer(lst.GetType());

            if (ItemsFile.Exists)
            {
                ItemsFile.Delete();
            }

            using (Stream s = ItemsFile.OpenWrite())
            {
                xmls.Serialize(s, lst);
                s.Close();
            }
        }

        private void otherColorTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach (CalendarItem item in calendar1.SelectedItems)
                    {
                        item.BackColor = dlg.Color;
                        calendar1.Invalidate(item);
                    }
                }
            }
        }

        private void calendar1_ItemDoubleClick(object sender, CalendarItemEventArgs e)
        {
            MessageBox.Show("Double click: " + e.Item.Subject);
        }

        private void calendar1_ItemDeleted(object sender, CalendarItemEventArgs e)
        {
            _items.Remove(e.Item);
        }

        private void calendar1_DayHeaderClick(object sender, CalendarDayEventArgs e)
        {
            //calendar1.SetViewRange(e.CalendarDay.Date, e.CalendarDay.Date);
            string[] headers = {"test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8", "test9"};
            int[] testPIDs = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            calendar1.startDailyView(9, headers, testPIDs, e.CalendarDay.Date);
            DateTime test = new DateTime(2021, 6, 30, 12, 30, 0);
            CalendarItem test1 = new CalendarItem(calendar1, test, test.AddMinutes(15), "test");
            calendar1.addItemByPhysicianID(3, test1);
        }

        private void diagonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal;
                item.PatternColor = Color.Red;
                calendar1.Invalidate(item);
            }
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.Vertical;
                item.PatternColor = Color.Red;
                calendar1.Invalidate(item);
            }
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.Horizontal;
                item.PatternColor = Color.Red;
                calendar1.Invalidate(item);
            }
        }

        private void hatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.DiagonalCross;
                item.PatternColor = Color.Red;
                calendar1.Invalidate(item);
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.DiagonalCross;
                item.PatternColor = Color.Empty;
                calendar1.Invalidate(item);
            }
        }

        private void monthView1_SelectionChanged(object sender, DateRangeChangedEventArgs e)
        {
            calendar1.SetViewRange(e.Start, e.End);
        }

        private void northToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.ImageAlign = ItemImageAligns.North;
                calendar1.Invalidate(item);
            }
        }

        private void eastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.ImageAlign = ItemImageAligns.East;
                calendar1.Invalidate(item);
            }
        }

        private void southToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.ImageAlign = ItemImageAligns.South;
                calendar1.Invalidate(item);
            }
        }

        private void westToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.SelectedItems)
            {
                item.ImageAlign = ItemImageAligns.West;
                calendar1.Invalidate(item);
            }
        }

        private void selectImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "*.gif|*.gif|*.png|*.png|*.jpg|*.jpg";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Image img = Image.FromFile(dlg.FileName);

                    foreach (CalendarItem item in calendar1.SelectedItems)
                    {
                        item.Image = img;
                        calendar1.Invalidate(item);
                    }
                }
            }

            
        }

        private void calendar1_ItemDatesChanged(object sender, CalendarItemEventArgs e)
        {
        }

        private void calendar1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(calendar1.SelectedElementStart.Date);
            Console.WriteLine(calendar1.SelectedElementEnd.Date);
            Console.WriteLine(calendar1.getMappedSelectionStart());
            Console.WriteLine(calendar1.getMappedSelectionEnd());
        }
    }
}